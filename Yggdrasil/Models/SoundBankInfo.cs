using System.IO;
using System.Xml;
using Yggdrasil.Resources;

namespace Yggdrasil.Models {
    public class SoundBankInfo {
        private SoundBankInfo() { }
        public static SoundBankInfo FromNode(XmlNode node) {
            if (node.Attributes.Count < 2) return null;
            if (!uint.TryParse(node.Attributes.GetNamedItem("Id").Value, out uint bankId)) return null;
            string bankPath = Path.Combine(YggdrasilPaths.GeneratedSoundBanks, node.SelectSingleNode("Path").InnerText);
            if (!File.Exists(bankPath)) return null;
            return new SoundBankInfo {
                BankId = bankId,
                ObjectPath = node.SelectSingleNode("ObjectPath").InnerText,
                ShortName = node.SelectSingleNode("ShortName").InnerText,
                BankPath = bankPath,
                Language = node.Attributes.GetNamedItem("Language").Value
            };

        }
        public uint BankId { get; private set; }
        public string ObjectPath { get; private set; }
        public string ShortName { get; private set; }
        public string BankPath { get; private set; }
        public string Language { get; private set; }
    }
}
