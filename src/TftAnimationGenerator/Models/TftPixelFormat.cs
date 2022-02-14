using System;
using SixLabors.ImageSharp.PixelFormats;

namespace TftAnimationGenerator.Models;

public partial class TftPixelFormat
{
    public string Name { get; init; }

    public int PixelComponentCount { get; set; }
    public int PixelComponentBitSize { get; set; }

    public Func<Rgba32, string[]> HexFormatter { get; set; }
}
