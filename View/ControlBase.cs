using Microsoft.Extensions.Logging;

using MyDesktopCards.ViewModel;
using MyWidgets.SDK;
using MyWidgets.SDK.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyDesktopCards.View
{
    public abstract class ControlBase : UserControl, ICard
    {
        public ILogger? logger { get; set; }

        public int HeightPix { get; set; }= 4;

        public int WidthPix { get; set; } =4;

        public CardInfo Info { get; protected set; }

        public Guid GUID {get;set;}

        public ControlBase(Guid gUID)
        {

            GUID = gUID;    
        }

        public UIElement GetUIElement()
        {
            return this;
        }

        public ViewModelBase? vm;

        public Type? VMType;

        public ConfigBase? Config { get; set; }

        public void InitViewModel<T> () where T : ViewModelBase
        {
            VMType= typeof (T);
        }

        public void InitViewModel()
        {
            VMType = typeof(EmptyVM);
        }

        public virtual void OnDisabled()
        {
            vm.Active = false;
            vm = null;
        }

        public virtual void OnEnabled()
        {
            if (VMType==null)
            {
                throw new NotSupportedException();
            }
            vm = (ViewModelBase)Activator.CreateInstance(VMType,this);
            DataContext = vm;
            vm.Active = true;
        }

        public abstract void ShowSetting();
    }


    public class EmptyVM : ViewModelBase
    {
        public EmptyVM(ControlBase control) : base(control)
        {
        }
    }
}
