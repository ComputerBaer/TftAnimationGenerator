using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using TftAnimationGenerator.ViewModels;

namespace TftAnimationGenerator.Views
{
    public partial class QueueEntryView : UserControl
    {
        public QueueEntryView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            // register events
            if (Parent != null)
            {
                Parent.PointerEnter += View_OnPointerEnter;
                Parent.PointerLeave += View_OnPointerLeave;
            }
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            // clean up events
            if (Parent != null)
            {
                Parent.PointerEnter -= View_OnPointerEnter;
                Parent.PointerLeave -= View_OnPointerLeave;
            }

            // reset event based properties
            if (DataContext is QueueEntryViewModel vm)
            {
                vm.ActionsVisible = false;
            }
        }

        private void View_OnPointerEnter(object? sender, PointerEventArgs e)
        {
            if (DataContext is not QueueEntryViewModel vm)
            {
                return;
            }

            vm.ActionsVisible = true;
        }

        private void View_OnPointerLeave(object? sender, PointerEventArgs e)
        {
            if (DataContext is not QueueEntryViewModel vm)
            {
                return;
            }

            vm.ActionsVisible = false;
        }
    }
}
