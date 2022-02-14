using System.Collections.Generic;
using TftAnimationGenerator.Formatters;

namespace TftAnimationGenerator.Models;

public partial class TftPixelFormat
{
    public static IReadOnlyCollection<TftPixelFormat> PixelFormats { get; } = new List<TftPixelFormat>
    {
        new ()
        {
            Name = "R5G6B5 / RGB565 (16 Bit)",
            HexFormatter = PixelFormatter.ToHexR5G6B5_Packed,
            PixelComponentCount = 1,
            PixelComponentBitSize = 16,
        },
        new ()
        {
            Name = "R8G8B8 / RGB (24 Bit, 3x8 Bit)",
            HexFormatter = PixelFormatter.ToHexR8G8B8,
            PixelComponentCount = 3,
            PixelComponentBitSize = 8,
        },
        new()
        {
            Name = "R8G8B8A8 / RGBA (32 Bit)",
            HexFormatter = PixelFormatter.ToHexR8G8B8A8_Packed,
            PixelComponentCount = 1,
            PixelComponentBitSize = 32,
        },
        new()
        {
            Name = "R8G8B8A8 / RGBA (32 Bit, 4x8 Bit)",
            HexFormatter = PixelFormatter.ToHexR8G8B8A8,
            PixelComponentCount = 4,
            PixelComponentBitSize = 8,
        },
    };
}
