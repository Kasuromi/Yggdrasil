using BepInEx;
using BepInEx.IL2CPP;
using System;
using System.Linq;
using System.Reflection;

namespace Yggdrasil.PluginDependency {
    public static class MTFO {
        public const string GUID = "com.dak.MTFO";
        public static string CustomPath { get; private set; } = string.Empty;
        public static string Version { get; private set; } = string.Empty;
        static MTFO() {
            if (!IL2CPPChainloader.Instance.Plugins.TryGetValue(GUID, out PluginInfo info)) return;
            try {
                Assembly mtfoAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault((x) => !x.IsDynamic && x.Location == info.Location);
                if (mtfoAssembly == null) throw new Exception($"couldn't locate the MTFO assembly");
                Type[] mtfoTypes = mtfoAssembly.GetTypes();
                Type confManagerType = mtfoTypes.FirstOrDefault((x) => x.Name == "ConfigManager");
                Type mtfoType = mtfoTypes.FirstOrDefault((x) => x.Name == "MTFO");
                if (mtfoType == null) throw new Exception($"couldn't locate MTFO's EntryPoint");
                if (confManagerType == null) throw new Exception($"couldn't locate MTFO's ConfigManager");
                FieldInfo versionField = mtfoType.GetField("VERSION", BindingFlags.Public | BindingFlags.Static);
                FieldInfo customPathField = confManagerType.GetField("CustomPath", BindingFlags.Public | BindingFlags.Static);
                if (versionField == null) throw new Exception($"couldn't locate MTFO's Version");
                if (customPathField == null) throw new Exception($"couldn't locate MTFO's CustomPath");
                CustomPath = (string)customPathField.GetValue(null);
                Version = (string)versionField.GetValue(null);
            } catch (Exception ex) {
                YggdrasilLogger.Error(ex.ToString());
            }
        }
    }
}
