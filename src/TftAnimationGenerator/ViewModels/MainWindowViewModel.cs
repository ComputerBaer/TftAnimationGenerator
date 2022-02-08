using System.Collections.Generic;
using TftAnimationGenerator.Models;

namespace TftAnimationGenerator.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        public List<TftPixelFormat> PixelFormats { get; } = new()
        {
            new() { Name = "R5G6R5 / RGB565" },
        };

        public int SelectedPixelFormat { get; set; } = 0;

        public List<TftCodeFormat> CodeFormats { get; } = new()
        {
            new() { Name = "Arduino" },
        };

        public int SelectedCodeFormat { get; set; } = 0;

        public string CodePrefix { get; set; } = "Anim_";

        public string OutputFile { get; set; } = "animation.h";

        public int ExportProgressMax { get; set; } = 0;
        public int ExportProgress { get; set; } = 0;
        public string ExportProgressText => $"{ExportProgress} / {ExportProgressMax}";
        public string ExportCurrentFile { get; set; } = "";
    }
}
