using ReactiveUI;
using TftAnimationGenerator.Models;

namespace TftAnimationGenerator.ViewModels
{
    public class QueueEntryViewModel : ViewModelBase
    {
        public ExportQueueEntry Model { get; }
        public MainWindowViewModel? ParentWindow { get; set; }

        public QueueEntryViewModel()
        {
            Model = new ExportQueueEntry
            {
                Name = "Example File",
            };
            _actionsVisible = true;
        }

        public QueueEntryViewModel(MainWindowViewModel parentWindow, ExportQueueEntry model)
        {
            ParentWindow = parentWindow;
            Model = model;
        }

        public string Name => Model.Name;

        private bool _actionsVisible;

        public bool ActionsVisible
        {
            get => _actionsVisible;
            set => this.RaiseAndSetIfChanged(ref _actionsVisible, value);
        }
    }
}
