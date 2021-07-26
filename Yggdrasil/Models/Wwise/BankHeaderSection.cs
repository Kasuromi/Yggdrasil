using System.Runtime.InteropServices;

namespace Yggdrasil.Models.Wwise {
    [StructLayout(LayoutKind.Sequential)]
    public class BankHeaderSection : BankSection {
        public uint BankVersion;
        public uint BankId;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x18)]
        private byte[] _IGNORE_0_;
    }
}
