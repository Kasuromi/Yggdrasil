using System.Text.Json.Serialization;

namespace Yggdrasil.Config {
    public abstract class YggdrasilConfig {
        [JsonIgnore] public abstract string ConfigName { get; }
        public abstract YggdrasilConfig GetDefault();
    }
}
