using SixLabors.ImageSharp.PixelFormats;

namespace TftAnimationGenerator.Formatters
{
    public static class PixelFormatter
    {
        public static string[] ToHexR5G6B5_Packed(Rgba32 pixel)
        {
            ushort hexOrder = (ushort)(((pixel.R & 0xf8) << 8) | ((pixel.G & 0xfc) << 3) | (pixel.B >> 3));
            return new[] { $"0x{hexOrder:X}" };
        }
    }
}
