using CommunityToolkit.Mvvm.ComponentModel;
using MyDesktopCards.View;
using System;
using System.Windows.Threading;

namespace MyDesktopCards.ViewModel
{
    public abstract class ViewModelBase : ObservableObject
    {
        public DispatcherTimer? _Timer { get; set; }

        public event EventHandler<bool>? OnActiveChanged;

        private bool _active;

        public bool Active
        {
            get { return _active; }
            set
            {
                _active = value;

                OnActiveChanged?.Invoke(this, value);

            }
        }

        private ControlBase view { get; set; }

        public ViewModelBase(ControlBase control)
        {
            view= control;
        }

        public T GetView<T>()
        {
            if (view is T v)
            {
                return v;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

    }
}
