using System.IO;
using System.Threading.Tasks;

namespace TftAnimationGenerator.Formatters
{
    public interface ICodeFormatter
    {
        Task WriteHeaderAsync(StreamWriter writer, string prefix, int count, int width, int height);
    }
}
