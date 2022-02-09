using TftAnimationGenerator.Formatters;

namespace TftAnimationGenerator.Models
{
    public class TftCodeFormat
    {
        public string Name { get; init; }

        public ICodeFormatter CodeFormatter { get; init; }
    }
}
