using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;
using Yggdrasil.Models;

namespace Yggdrasil.Extensions {
    public static class HarmonyExtensions {
        public static void YggdrasilPatchAll(this Harmony harmony, Assembly assembly) {
            Type[] patches = assembly.GetTypes().Where((x) => x.IsSubclassOf(typeof(YggdrasilPatch))).ToArray();
            for(int i = 0; i < patches.Length; i++) {
                Type patch = patches[i];
                YggdrasilPatch patchObj = (YggdrasilPatch)Activator.CreateInstance(patch);
                MethodInfo method = patchObj.DeclaringType.GetMethod(patchObj.MethodName, patchObj.ParameterTypes);
                if(method == null) {
                    YggdrasilLogger.Error($"Couldn't find method with name {patchObj.MethodName} and parameters [" +
                        string.Join(", ", patchObj.ParameterTypes.Select((x) => x.Name)) +
                        $"] in {patchObj.DeclaringType.Name}"
                    );
                    continue;
                }
                MethodInfo prefixMethod = patch.GetMethod(patchObj.PrefixFuncName);
                MethodInfo postfixMethod = patch.GetMethod(patchObj.PostfixFuncName);
                if(prefixMethod == null && postfixMethod == null) {
                    YggdrasilLogger.Error($"Couldn't find Prefix and Postfix methods in {patch.Name}. Are the FuncName parameters valid?");
                    continue;
                }
                harmony.Patch(
                    method,
                    prefixMethod != null ? new HarmonyMethod(prefixMethod) : null,
                    postfixMethod != null ? new HarmonyMethod(postfixMethod) : null
                );
            }
        }
    }
}
