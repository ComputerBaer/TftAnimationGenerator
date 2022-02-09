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

        public static string[] ToHexR8G8B8(Rgba32 pixel)
        {
            return new[] { $"0x{pixel.R:X}", $"0x{pixel.G:X}", $"0x{pixel.B:X}" };
        }

        public static string[] ToHexR8G8B8A8(Rgba32 pixel)
        {
            return new[] { $"0x{pixel.R:X}", $"0x{pixel.G:X}", $"0x{pixel.B:X}", $"0x{pixel.A:X}" };
        }

        public static string[] ToHexR8G8B8A8_Packed(Rgba32 pixel)
        {
            uint hexOrder = (uint)((pixel.R << 24) | (pixel.G << 16) | (pixel.B << 8) | (pixel.A << 0));
            return new[] { $"0x{hexOrder:X}" };
        }
    }
}
