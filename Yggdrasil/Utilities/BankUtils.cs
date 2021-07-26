using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Yggdrasil.Models;
using Yggdrasil.Models.Wwise;
using Yggdrasil.Resources;

namespace Yggdrasil.Utilities {
    public static class BankUtils {
        public static GCHandle AllocBankByName(string bankName) {
            bankName = Path.GetFileNameWithoutExtension(bankName);
            SoundBankInfo bankInfo = SoundBanks.BankData.FirstOrDefault((x) => x.ShortName == bankName);
            if (bankInfo == null) throw new Exception($"Couldn't find a sound bank with the name '{bankName}'");
            return GCHandle.Alloc(File.ReadAllBytes(bankInfo.BankPath), GCHandleType.Pinned);
        }
        public struct BankData {
            public BankHeaderSection Header;
            public BankDataIndexSection DataIndex;
            public DataIndexElement[] DataIndexElements;
            public BankDataSection Data;
        }
        public static BankData ReadBankData(ref long addr) {
            BankData bankData = new BankData();
            bankData.Header = MarshalUtils.ReadSection<BankHeaderSection>(ref addr);
            bankData.DataIndex = MarshalUtils.ReadSection<BankDataIndexSection>(ref addr, false);
            bankData.DataIndexElements = new DataIndexElement[bankData.DataIndex.SectionSize / Marshal.SizeOf<DataIndexElement>()];
            for (int i = 0; i < bankData.DataIndexElements.Length; i++) {
                bankData.DataIndexElements[i] = MarshalUtils.ReadStruct<DataIndexElement>(ref addr);
            }
            bankData.Data = MarshalUtils.ReadSection<BankDataSection>(ref addr, false);
            return bankData;
        }
    }
}
