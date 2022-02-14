using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.PixelFormats;
using TftAnimationGenerator.Models;

namespace TftAnimationGenerator.Formatters;

public interface ICodeFormatter
{
    Task WriteHeaderAsync(StreamWriter writer, TftPixelFormat pixelFormat, string prefix, int count, int width, int height);

    Task WriteFrameAsync(StreamWriter writer, TftPixelFormat pixelFormat, Buffer2D<Rgba32> pixelBuffer, int width, int height, bool isLast);

    Task WriteFooterAsync(StreamWriter writer);
}
