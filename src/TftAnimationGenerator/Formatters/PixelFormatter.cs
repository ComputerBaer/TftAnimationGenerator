using SixLabors.ImageSharp.PixelFormats;

namespace TftAnimationGenerator.Formatters
{
    public static class PixelFormatter
    {
        public static string ToHexR5G6B5(Rgba32 pixel)
        {
            ushort rgb565 = (ushort)(((pixel.R & 0xf8) << 8) | ((pixel.G & 0xfc) << 3) | (pixel.B >> 3));
            return $"0x{rgb565:X}";
        }
    }
}
