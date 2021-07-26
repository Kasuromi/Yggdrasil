using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Yggdrasil.Models;
using Yggdrasil.Models.Custom;
using Yggdrasil.Models.Wwise;
using Yggdrasil.Resources;
using Yggdrasil.Utilities;

namespace Yggdrasil.Patches {
    public class AkSoundEnginePatches {
        public class LoadBank : YggdrasilPatch {
            public override Type DeclaringType => typeof(AkSoundEngine);
            public override string MethodName => nameof(AkSoundEngine.LoadBank);
            public override Type[] ParameterTypes => new Type[] {
                typeof(string),
                typeof(int),
                typeof(uint).MakeByRefType()
            };
            public static bool Prefix(string in_pszString, int in_memPoolId, ref uint out_bankID) {
                string bankName = Path.GetFileNameWithoutExtension(in_pszString);
                List<CustomSoundReplacement> newBankReplacements = Configs.SoundReplacementConfig.Replacements
                    .Where((x) => x.BankName == bankName && File.Exists(x.ReplacementFilePath))
                    .ToList();
                if (newBankReplacements.Count == 0) return true;
                GCHandle hBank = BankUtils.AllocBankByName(in_pszString);
                long oldBankBaseAddr = hBank.AddrOfPinnedObject().ToInt64();
                long readerAddr = oldBankBaseAddr;
                BankUtils.BankData bankData = BankUtils.ReadBankData(ref readerAddr);
                int oldBankSize = ((byte[])hBank.Target).Length;
                int bankSizeInc = Enumerable.Sum(
                    newBankReplacements.Select((x) =>
                        File.ReadAllBytes(x.ReplacementFilePath).Length - (bankData.DataIndexElements.FirstOrDefault((y) => x.SfxId == y.FileId)?.DataSize ?? 0)
                    )
                );
                YggdrasilLogger.Debug($"Changing '{bankName}' size by {bankSizeInc}");
                long newBankSize = oldBankSize + bankSizeInc;
                IntPtr newBankBaseAddr = Marshal.AllocHGlobal((int)newBankSize);
                long writerAddr = newBankBaseAddr.ToInt64();
                MarshalUtils.WriteStruct(bankData.Header, ref writerAddr, false);
                MarshalUtils.WriteStruct(bankData.DataIndex, ref writerAddr, false);
                long didxWriterAddr = writerAddr;
                long dataAddr = didxWriterAddr + bankData.DataIndex.SectionSize;
                long dataOffset = dataAddr + Marshal.SizeOf<BankDataSection>() - newBankBaseAddr.ToInt64();
                int newOffset = 0;
                for (int i = 0; i < bankData.DataIndexElements.Length; i++) {
                    DataIndexElement currElem = bankData.DataIndexElements[i];
                    int origOffset = currElem.DataOffset;
                    currElem.DataOffset += newOffset;
                    var replacement = newBankReplacements.FirstOrDefault((x) => x.SfxId == currElem.FileId);
                    if (replacement != null) {
                        byte[] replacementBytes = File.ReadAllBytes(replacement.ReplacementFilePath);
                        Marshal.Copy(replacementBytes, 0, new IntPtr(newBankBaseAddr.ToInt64() + dataOffset + currElem.DataOffset), replacementBytes.Length);
                        newOffset += replacementBytes.Length - currElem.DataSize;
                        currElem.DataSize = replacementBytes.Length;
                    } else {
                        MarshalUtils.Copy(
                            new IntPtr(oldBankBaseAddr + dataOffset + origOffset),
                            new IntPtr(newBankBaseAddr.ToInt64() + dataOffset + currElem.DataOffset),
                            currElem.DataSize
                        );
                    }
                    MarshalUtils.WriteStruct(currElem, ref didxWriterAddr, false);
                }
                bankData.Data.SectionSize += (uint)newOffset;
                MarshalUtils.WriteStruct(bankData.Data, ref dataAddr, false);
                writerAddr = dataAddr + bankData.Data.SectionSize;
                long dataWritten = writerAddr - newBankBaseAddr.ToInt64() - newOffset;
                MarshalUtils.Copy(
                    new IntPtr(oldBankBaseAddr + dataWritten),
                    new IntPtr(writerAddr),
                    (int)(oldBankSize - dataWritten)
                );
                hBank.Free();
                AKRESULT loadResult = AkSoundEngine.LoadBank(newBankBaseAddr, (uint)newBankSize, out out_bankID);
                if (loadResult != AKRESULT.AK_Success) {
                    YggdrasilLogger.Error($"Failed to load modified bank for {bankName}. Load result: {loadResult}. Loading original bank");
                    Marshal.FreeHGlobal(newBankBaseAddr);
                    return true;
                }
                YggdrasilLogger.Info($"Loaded '{bankName}' with replaced sounds successfully.");
                return false;
            }
        }
    }
}
