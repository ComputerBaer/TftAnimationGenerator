using TftAnimationGenerator.Formatters;

namespace TftAnimationGenerator.Models
{
    public partial class TftCodeFormat
    {
        public string Name { get; init; }

        public ICodeFormatter CodeFormatter { get; init; }
    }
}
