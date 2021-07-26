using System;
using System.Runtime.InteropServices;
using Yggdrasil.Models.Wwise;

namespace Yggdrasil.Utilities {
    public static class MarshalUtils {
        public static Byte ReadU8(ref long addr, bool incrAddr = true) {
            Byte val = (Byte)Marshal.ReadByte(new IntPtr(addr));
            if (incrAddr) addr += Marshal.SizeOf<Byte>();
            return val;
        }
        public static UInt32 ReadU32(ref long addr, bool incrAddr = true) {
            UInt32 val = (UInt32)Marshal.ReadInt32(new IntPtr(addr));
            if (incrAddr) addr += Marshal.SizeOf<UInt32>();
            return val;
        }
        public static void WriteStruct<T>(T _struct, ref long addr, bool fDeleteOld, bool incrAddr = true) {
            Marshal.StructureToPtr(_struct, new IntPtr(addr), fDeleteOld);
            if (incrAddr) addr += Marshal.SizeOf<T>();
        }
        public static T ReadStruct<T>(ref long addr, bool incrAddr = true) {
            T _struct = Marshal.PtrToStructure<T>(new IntPtr(addr));
            if (incrAddr) addr += Marshal.SizeOf<T>();
            return _struct;
        }
        public static T ReadSection<T>(ref long addr, bool incrAddr = true) where T : BankSection {
            BankSection section = Marshal.PtrToStructure<T>(new IntPtr(addr));
            addr += Marshal.SizeOf<BankSection>();
            if (incrAddr) addr += section.SectionSize;
            return (T)section;
        }
        public static void Copy(IntPtr from, IntPtr to, int size) {
            byte[] bytes = new byte[size];
            Marshal.Copy(from, bytes, 0, bytes.Length);
            Marshal.Copy(bytes, 0, to, bytes.Length);
        }
    }
}
