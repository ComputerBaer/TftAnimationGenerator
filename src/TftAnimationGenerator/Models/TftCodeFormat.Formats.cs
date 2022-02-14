using System.Collections.Generic;
using TftAnimationGenerator.Formatters;

namespace TftAnimationGenerator.Models;

public partial class TftCodeFormat
{
    public static IReadOnlyCollection<TftCodeFormat> CodeFormats { get; } = new List<TftCodeFormat>()
    {
        new()
        {
            Name = "Arduino",
            CodeFormatter = new ArduinoCodeFormatter()
        },
    };
}
