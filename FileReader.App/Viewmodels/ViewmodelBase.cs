using FileReader.App.Viewmodels.Commands;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FileReader.App.Viewmodels
{
    internal abstract class ViewmodelBase : INotifyPropertyChanged
    {

        private string title;
        private ICommand commandClose;

        public ViewmodelBase(string title)
        {
            this.title = title;
        }

        public ViewmodelBase() : this(string.Empty) { }

        public event EventHandler CloseRequested;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Title
        {
            get { return title; }
            protected set
            {
                if (string.Equals(title, value)) return;
                title = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnCloseRequested()
        {
            var handle = CloseRequested;
            if (handle == null) return;
            handle.Invoke(this, new EventArgs());
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string property = null)
        {
            var handle = PropertyChanged;
            if (handle != null)
            {
                if (string.IsNullOrEmpty(property)) throw new ArgumentNullException("OnPropertyChanged is not called by a property.");
                handle.Invoke(this, new PropertyChangedEventArgs(property));
            }
        }

        public ICommand CommandClose
        {
            get { return commandClose ?? (commandClose = new RelayCommand(x => OnCloseRequested(), x => true)); }
        }
    }
}
