using Yggdrasil.Models.Custom;

namespace Yggdrasil.Config {
    public class SoundReplacementConfig : YggdrasilConfig {
        public override string ConfigName => "SoundReplacement.json";
        public CustomSoundReplacement[] Replacements { get; set; }

        public override YggdrasilConfig GetDefault() {
            return new SoundReplacementConfig {
                Replacements = new CustomSoundReplacement[] {
                    new CustomSoundReplacement {
                        BankName = "simon_sfx",
                        SfxId = 0,
                        ReplacementFilePath = "PATH_TO_REPLACEMENT_WEM_FILE"
                    }
                }
            };
        }
    }
}
