using BepInEx;
using System.IO;
using System.Reflection;
using UnityEngine;
using Yggdrasil.Attributes;
using Yggdrasil.PluginDependency;

namespace Yggdrasil.Resources {
    public static class YggdrasilPaths {
        [YggdrasilPath(PathType = PathType.Directory, ThrowOnInvalid = true)]
        public static string GeneratedSoundBanks { get; private set; } = Path.Combine(Application.streamingAssetsPath, "GeneratedSoundBanks", "Windows");
        [YggdrasilPath(PathType = PathType.File, ThrowOnInvalid = true)]
        public static string SoundBanksInfo { get; private set; } = Path.Combine(GeneratedSoundBanks, "SoundbanksInfo.xml");
        [YggdrasilPath(PathType = PathType.Directory, CreateOnInvalid = true, ThrowOnInvalid = false)]
        public static string ConfigPath { get; private set; } = Path.Combine(
            string.IsNullOrEmpty(MTFO.CustomPath) ? Paths.PluginPath : MTFO.CustomPath,
            "Yggdrasil"
        );

        static YggdrasilPaths() {
            PropertyInfo[] pathProperties = typeof(YggdrasilPaths).GetProperties(BindingFlags.Static | BindingFlags.Public);
            for(int i = 0; i < pathProperties.Length; i++) {
                PropertyInfo property = pathProperties[i];
                YggdrasilPathAttribute attribute = property.GetCustomAttribute<YggdrasilPathAttribute>();
                if (attribute == null) continue;
                bool exists = false;
                string path = (string)property.GetValue(null);
                switch (attribute.PathType) {
                    case PathType.Directory:
                        exists = Directory.Exists(path);
                        break;
                    case PathType.File:
                        exists = File.Exists(path);
                        break;
                    default:
                        continue;
                }
                if(!exists) {
                    if (!attribute.CreateOnInvalid) {
                        YggdrasilLogger.Error($"Path '{property.Name}' doesn't exist as a valid {attribute.PathType}.");
                        if (attribute.ThrowOnInvalid)
                            throw new FileNotFoundException(property.Name);
                    } else {
                        YggdrasilLogger.Debug($"Creating directory '{property.Name}'");
                        Directory.CreateDirectory(path);
                    }
                }
            }
        }
    }
}
