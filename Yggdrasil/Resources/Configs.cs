using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Yggdrasil.Config;
using Yggdrasil.Utilities;

namespace Yggdrasil.Resources {
    public static class Configs {
        public static SoundReplacementConfig SoundReplacementConfig { get; private set; }

        static Configs() {
            PropertyInfo[] pathProperties = 
                typeof(Configs)
                .GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Where((x) => x.PropertyType.IsSubclassOf(typeof(YggdrasilConfig))).ToArray();
            for (int i = 0; i < pathProperties.Length; i++) {
                PropertyInfo property = pathProperties[i];
                YggdrasilConfig config = (YggdrasilConfig)Activator.CreateInstance(property.PropertyType);
                string configPath = Path.Combine(YggdrasilPaths.ConfigPath, config.ConfigName);
                if (!File.Exists(configPath)) {
                    YggdrasilLogger.Error($"Couldn't locate config {config.ConfigName}. Writing default..");
                    File.WriteAllText(configPath, JsonUtils.Serialize(config.GetDefault()));
                }
                property.SetValue(null, JsonUtils.Deserialize(property.PropertyType, File.ReadAllText(configPath)));
            }
        }
    }
}
