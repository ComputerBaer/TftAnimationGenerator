using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.PixelFormats;
using TftAnimationGenerator.Models;

namespace TftAnimationGenerator.Formatters
{
    public class ArduinoCodeFormatter : ICodeFormatter
    {
        public async Task WriteHeaderAsync(StreamWriter writer, string prefix, int count, int width, int height)
        {
            await writer.WriteLineAsync("//----------------------------------------------------------------");
            await writer.WriteLineAsync("// This file was generated by TTF Animation Generator");
            await writer.WriteLineAsync("// https://github.com/ComputerBaer/TftAnimationGenerator");
            await writer.WriteLineAsync("//");
            await writer.WriteLineAsync("// Changes to this file may cause incorrect behavior");
            await writer.WriteLineAsync("// and will be lost if the animation is regenerated.");
            await writer.WriteLineAsync("//----------------------------------------------------------------\n");

            await writer.WriteLineAsync($"const int {prefix}FrameCount = {count};");
            await writer.WriteLineAsync($"const int {prefix}FrameWidth = {width};");
            await writer.WriteLineAsync($"const int {prefix}FrameHeight = {height};");

            await writer.WriteLineAsync($"const unsigned short PROGMEM {prefix}Frames[{count}][{width * height}] = {{");
        }

        public async Task WriteFrameAsync(StreamWriter writer, TftPixelFormat pixelFormat, Buffer2D<Rgba32> pixelBuffer, int width, int height, bool isLast)
        {
            await writer.WriteAsync("  { ");

            int maxX = width - 1;
            int maxY = height - 1;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    string[] hexPixel = pixelFormat.HexFormatter(pixelBuffer[x, y]);
                    await writer.WriteAsync(string.Join(", ", hexPixel));

                    if (y < maxY || x < maxX) // only exclude last pixel
                    {
                        await writer.WriteAsync(", ");
                    }
                }
            }

            if (isLast)
            {
                await writer.WriteLineAsync(" }");
            }
            else
            {
                await writer.WriteLineAsync(" },");
            }
        }

        public async Task WriteFooterAsync(StreamWriter writer)
        {
            await writer.WriteLineAsync("};");
        }
    }
}
