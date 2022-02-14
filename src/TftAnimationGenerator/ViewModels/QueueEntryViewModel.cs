using System;
using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;
using TftAnimationGenerator.Models;

namespace TftAnimationGenerator.ViewModels
{
    public class QueueEntryViewModel : ViewModelBase
    {
        public ExportQueueEntry Model { get; }
        public MainWindowViewModel? ParentWindow { get; set; }

        public string Name => Model.Name;

        private bool _actionsVisible;

        public bool ActionsVisible
        {
            get => _actionsVisible;
            set => this.RaiseAndSetIfChanged(ref _actionsVisible, value);
        }

        public ReactiveCommand<Unit, Unit> RemoveCmd { get; set; }
        public ReactiveCommand<Unit, Unit> MoveUpCmd { get; set; }
        public ReactiveCommand<Unit, Unit> MoveDownCmd { get; set; }

        public QueueEntryViewModel() : this(new ExportQueueEntry { Name = "Example File" })
        {
            _actionsVisible = true;
        }

        public QueueEntryViewModel(ExportQueueEntry model, MainWindowViewModel? parentWindow = null)
        {
            ParentWindow = parentWindow;
            Model = model;

            RemoveCmd = ReactiveCommand.Create(RunRemove);
            MoveUpCmd = ReactiveCommand.Create(RunMoveUp);
            MoveDownCmd = ReactiveCommand.Create(RunMoveDown);
        }

        private void RunRemove()
        {
            if (ParentWindow == null)
            {
                return;
            }

            int index = ParentWindow.QueueEntries.IndexOf(this);
            ParentWindow.RemoveImages(new List<QueueEntryViewModel> { this });

            // update next element action buttons
            if (index < ParentWindow.QueueEntries.Count)
            {
                ParentWindow.QueueEntries[index].ActionsVisible = true;
            }
        }

        private void RunMoveUp() => Move(window => window.MoveImagesUp(new[] { this }));

        private void RunMoveDown() => Move(window => window.MoveImagesDown(new[] { this }));

        private void Move(Action<MainWindowViewModel> moveAction)
        {
            if (ParentWindow == null)
            {
                return;
            }

            int index = ParentWindow.QueueEntries.IndexOf(this);
            moveAction(ParentWindow);

            // update action buttons
            ActionsVisible = false;
            ParentWindow.QueueEntries[index].ActionsVisible = true;
        }
    }
}
