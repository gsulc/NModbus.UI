using NModbus.UI.Common.Core;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Windows;

namespace NModbus.UI.ViewModels
{
    class ErrorViewModel : BindableBase
    {
        private string _latestErrorMessage;
        private Visibility _visibility = Visibility.Hidden;

        public ErrorViewModel(IEventAggregator ea)
        {
            ea.GetEvent<ExceptionEvent>().Subscribe(DisplayException);
            CloseCommand = new DelegateCommand(Close);
        }

        public DelegateCommand CloseCommand { get; private set; }

        public Visibility Visibility
        {
            get => _visibility;
            set => SetProperty(ref _visibility, value);
        }

        public string LatestErrorMessage
        {
            get => _latestErrorMessage;
            set => SetProperty(ref _latestErrorMessage, value);
        }

        private void DisplayException(Exception e)
        {
            LatestErrorMessage = e.Message;
            Visibility = Visibility.Visible;
        }

        private void Close()
        {
            Visibility = Visibility.Collapsed;
        }
    }
}
