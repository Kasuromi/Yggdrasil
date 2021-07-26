using System.Runtime.InteropServices;

namespace Yggdrasil.Models.Wwise {
    [StructLayout(LayoutKind.Sequential)]
    public class DataIndexElement {
        public uint FileId;
        public int DataOffset;
        public int DataSize;
    }
}
