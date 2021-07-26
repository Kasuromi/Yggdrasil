using System;

namespace Yggdrasil.Models {
    public abstract class YggdrasilPatch {
        public abstract Type DeclaringType { get; }
        public abstract string MethodName { get; }
        public abstract Type[] ParameterTypes { get; }
        public virtual string PrefixFuncName { get; } = "Prefix";
        public virtual string PostfixFuncName { get; } = "Postfix";
    }
}
