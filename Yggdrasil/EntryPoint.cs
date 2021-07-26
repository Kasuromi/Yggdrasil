using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using System.Reflection;
using Yggdrasil.Extensions;
using Yggdrasil.Resources;
using static BepInEx.BepInDependency;

namespace Yggdrasil {
    [BepInPlugin("com.kasuromi.yggdrasil", "Yggdrasil", VersionInfo.SemVer)]
    [BepInDependency("com.dak.MTFO", DependencyFlags.SoftDependency)]
    public class EntryPoint : BasePlugin {
        public override void Load() {
            YggdrasilLogger.Info($"Using {YggdrasilPaths.ConfigPath} as a path to configuration");
            var harmony = new Harmony("com.kasuromi.yggdrasil");
            harmony.YggdrasilPatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
