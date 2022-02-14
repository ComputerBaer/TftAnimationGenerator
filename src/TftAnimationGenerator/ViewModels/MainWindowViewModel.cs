using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using SixLabors.ImageSharp.PixelFormats;
using TftAnimationGenerator.Models;
using Image = SixLabors.ImageSharp.Image;

namespace TftAnimationGenerator.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<QueueEntryViewModel> QueueEntries { get; set; } = new();
    public ObservableCollection<QueueEntryViewModel> SelectedQueueEntries { get; set; } = new();

    public List<TftPixelFormat> PixelFormats { get; } = new(TftPixelFormat.PixelFormats);

    public List<TftCodeFormat> CodeFormats { get; } = new(TftCodeFormat.CodeFormats);

    public int SelectedPixelFormat { get; set; } = 0;
    public int SelectedCodeFormat { get; set; } = 0;
    public string CodePrefix { get; set; } = "Anim_";

    private string _outputFile = "animation.h";
    private int _exportProgressMax;
    private int _exportProgress;
    private string _exportCurrentFile = "";

    public string OutputFile
    {
        get => _outputFile;
        set => this.RaiseAndSetIfChanged(ref _outputFile, value);
    }

    public int ExportProgressMax
    {
        get => _exportProgressMax;
        set
        {
            this.RaiseAndSetIfChanged(ref _exportProgressMax, value);
            this.RaisePropertyChanged(nameof(ExportProgressText));
        }
    }

    public int ExportProgress
    {
        get => _exportProgress;
        set
        {
            this.RaiseAndSetIfChanged(ref _exportProgress, value);
            this.RaisePropertyChanged(nameof(ExportProgressText));
        }
    }

    public string ExportProgressText => $"{ExportProgress} / {ExportProgressMax}";

    public string ExportCurrentFile
    {
        get => _exportCurrentFile;
        set => this.RaiseAndSetIfChanged(ref _exportCurrentFile, value);
    }

    public ReactiveCommand<Unit, Unit> AddImagesCmd { get; set; }
    public ReactiveCommand<Unit, Unit> RemoveImagesCmd { get; set; }
    public ReactiveCommand<Unit, Unit> MoveImagesUpCmd { get; set; }
    public ReactiveCommand<Unit, Unit> MoveImagesDownCmd { get; set; }
    public ReactiveCommand<Unit, Unit> SelectOutputFileCmd { get; set; }
    public ReactiveCommand<Unit, Unit> ExportAnimationCmd { get; set; }

    public MainWindowViewModel()
    {
        AddImagesCmd = ReactiveCommand.CreateFromTask(RunAddImages);
        RemoveImagesCmd = ReactiveCommand.Create(RunRemoveImages);
        MoveImagesUpCmd = ReactiveCommand.Create(RunMoveImagesUp);
        MoveImagesDownCmd = ReactiveCommand.Create(RunMoveImagesDown);
        SelectOutputFileCmd = ReactiveCommand.CreateFromTask(RunSelectOutputFile);
        ExportAnimationCmd = ReactiveCommand.CreateFromTask(RunExportAnimation);
    }

    private async Task RunAddImages()
    {
        var openFileDialog = new OpenFileDialog
        {
            AllowMultiple = true,
            Filters = new List<FileDialogFilter>
            {
                new() { Name = "Image Files", Extensions = new() { "bmp", "jpg", "png", "gif" }},
                new() { Name = "All files", Extensions = new() { "*" }},
            },
            Title = "Add images"
        };

        var mainWindow = GetMainWindow();
        string[]? files = await openFileDialog.ShowAsync(mainWindow);
        if (files == null || files.Length == 0)
        {
            return;
        }

        foreach (string file in files)
        {
            var fileInfo = new FileInfo(file);
            if (!fileInfo.Exists)
            {
                continue;
            }

            var imageInfo = await Image.IdentifyAsync(fileInfo.FullName);
            if (imageInfo == null)
            {
                continue;
            }

            QueueEntries.Add(new QueueEntryViewModel(new ExportQueueEntry
            {
                Filename = fileInfo.FullName,
                Name = fileInfo.Name,
                Width = imageInfo.Width,
                Height = imageInfo.Height,
            }, this));
        }
    }

    private void RunRemoveImages()
    {
        RemoveImages(new List<QueueEntryViewModel>(SelectedQueueEntries));
    }

    public void RemoveImages(IEnumerable<QueueEntryViewModel> entries)
    {
        foreach (var entry in entries)
        {
            SelectedQueueEntries.Remove(entry);
            QueueEntries.Remove(entry);
        }
    }

    private void RunMoveImagesUp() => MoveImagesUp(SelectedQueueEntries);
    private void RunMoveImagesDown() => MoveImagesDown(SelectedQueueEntries);

    public void MoveImagesUp(IEnumerable<QueueEntryViewModel> entries) => MoveImages(entries, -1);
    public void MoveImagesDown(IEnumerable<QueueEntryViewModel> entries) => MoveImages(entries, 1);

    private void MoveImages(IEnumerable<QueueEntryViewModel> entries, int direction)
    {
        bool moveNegative = direction < 0;

        var entriesEnumerable = entries.Select(e => QueueEntries.IndexOf(e));
        entriesEnumerable = moveNegative ? entriesEnumerable.OrderBy(i => i) : entriesEnumerable.OrderByDescending(i => i);

        List<int> sortedEntries = entriesEnumerable.ToList();
        HashSet<int> selectedEntries = SelectedQueueEntries.Select(e => QueueEntries.IndexOf(e)).OrderBy(i => i).ToHashSet();

        SelectedQueueEntries.Clear();

        int lastIndex = QueueEntries.Count - 1;
        for (var i = 0; i < sortedEntries.Count; i++)
        {
            int index = sortedEntries[i];
            if ((moveNegative && index == i) || (!moveNegative && index == lastIndex - i))
            {
                SelectEntry(index, index);
                continue;
            }

            int newIndex = index + direction;
            QueueEntries.Move(index, newIndex);

            SelectEntry(index, newIndex);
            SelectEntry(newIndex, index);
        }

        foreach (var selectedEntry in selectedEntries)
        {
            SelectedQueueEntries.Add(QueueEntries[selectedEntry]);
        }

        void SelectEntry(int oldIndex, int newIndex)
        {
            if (!selectedEntries.Contains(oldIndex)) return;

            SelectedQueueEntries.Add(QueueEntries[newIndex]);
            selectedEntries.Remove(oldIndex);
        }
    }

    private async Task RunSelectOutputFile()
    {
        var saveFileDialog = new SaveFileDialog
        {
            DefaultExtension = "h",
            Filters = new List<FileDialogFilter>
            {
                new() { Name = "C / C++ Header", Extensions = new() { "h", "hpp" }},
                new() { Name = "All files", Extensions = new() { "*" }},
            },
            Title = "Select Output File"
        };

        var mainWindow = GetMainWindow();
        string? file = await saveFileDialog.ShowAsync(mainWindow);
        if (string.IsNullOrEmpty(file))
        {
            return;
        }
            
        OutputFile = file;
    }

    private async Task RunExportAnimation()
    {
        if (QueueEntries.Count == 0)
        {
            return;
        }

        // copy all settings and data
        var pixelFormat = PixelFormats[SelectedPixelFormat];
        var codeFormat = CodeFormats[SelectedCodeFormat];
        string prefix = CodePrefix;

        ExportQueueEntry[] queue = QueueEntries.Select(e => e.Model.Clone()).ToArray();

        // update UI
        ExportProgress = 0;
        ExportProgressMax = queue.Length;

        // open output file
        await using var fileStream = new FileStream(OutputFile, FileMode.Create, FileAccess.Write);
        await using var writer = new StreamWriter(fileStream, Encoding.UTF8);

        // header
        int width = queue[0].Width;
        int height = queue[0].Height;
        await codeFormat.CodeFormatter.WriteHeaderAsync(writer, pixelFormat, prefix, queue.Length, width, height);

        // write frames
        for (var i = 0; i < queue.Length; i++)
        {
            var queueEntry = queue[i];
            ExportCurrentFile = queueEntry.Name;

            var image = await Image.LoadAsync<Rgba32>(queueEntry.Filename);
            var pixelBuffer = image.Frames[0].PixelBuffer;
            await codeFormat.CodeFormatter.WriteFrameAsync(writer, pixelFormat, pixelBuffer, width, height, i == queue.Length - 1);

            ExportProgress = i + 1;
        }

        // footer
        await codeFormat.CodeFormatter.WriteFooterAsync(writer);
        await writer.FlushAsync();
    }

    private Window GetMainWindow()
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
        {
            throw new InvalidOperationException("Application is not running on desktop?!");
        }

        return desktop.MainWindow;
    }
}
