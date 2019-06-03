using System.Runtime.InteropServices;

namespace NModbus.UI
{
    [StructLayout(LayoutKind.Explicit)]
    struct ShortConverter
    {
        [FieldOffset(0)] short ShortValue;
        [FieldOffset(0)] ushort UShortValue;

        public static short ToShort(ushort source)
        {
            var converter = new ShortConverter();
            converter.UShortValue = source;
            return converter.ShortValue;
        }
    }
}
