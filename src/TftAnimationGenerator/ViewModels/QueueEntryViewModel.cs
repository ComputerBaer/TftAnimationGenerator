using TftAnimationGenerator.Models;

namespace TftAnimationGenerator.ViewModels
{
    public class QueueEntryViewModel : ViewModelBase
    {
        public ExportQueueEntry Model { get; }
        private MainWindowViewModel _window;

        public QueueEntryViewModel(MainWindowViewModel window, ExportQueueEntry model)
        {
            _window = window;
            Model = model;
        }

        public string Name => Model.Name;
    }
}
