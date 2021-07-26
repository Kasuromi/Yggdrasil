using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Yggdrasil.Models;

namespace Yggdrasil.Resources {
    public static class SoundBanks {
        public static XmlDocument SoundBanksInfo = new XmlDocument();
        public static List<SoundBankInfo> BankData { get; private set; } = new List<SoundBankInfo>();

        static SoundBanks() {
            SoundBanksInfo.LoadXml(File.ReadAllText(YggdrasilPaths.SoundBanksInfo));
            foreach (XmlNode node in SoundBanksInfo.DocumentElement.SelectSingleNode("SoundBanks").ChildNodes) {
                SoundBankInfo bankInfo = SoundBankInfo.FromNode(node);
                if (bankInfo == null) continue;
                BankData.Add(bankInfo);
            }
            YggdrasilLogger.Info($"Discovered {BankData.Count} sound banks: [" +
                string.Join(", ", BankData.Select((x) => x.ShortName)) +
                $"]"
             );
        }
    }
}
