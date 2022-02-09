using System;
using SixLabors.ImageSharp.PixelFormats;

namespace TftAnimationGenerator.Models
{
    public class TftPixelFormat
    {
        public string Name { get; init; }

        public Func<Rgba32, string[]> HexFormatter { get; set; }
    }
}
