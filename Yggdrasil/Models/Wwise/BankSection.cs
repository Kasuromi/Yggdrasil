using System.Runtime.InteropServices;

namespace Yggdrasil.Models.Wwise {
    [StructLayout(LayoutKind.Sequential)]
    public class BankSection {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] MagicHeader;
        public uint SectionSize;
    }
}
