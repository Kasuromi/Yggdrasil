using System;

namespace Yggdrasil.Attributes {
    public enum PathType {
        Directory = 0,
        File,
        External
    }
    public class YggdrasilPathAttribute : Attribute {
        public YggdrasilPathAttribute() : base() {}
        public PathType PathType { get; set; }
        public bool CreateOnInvalid { get; set; }
        public bool ThrowOnInvalid { get; set; }
    }
}
