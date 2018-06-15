using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Stopwatch_AAC
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetPropertyValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if ((value != null) && !value.Equals(field))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return false;
        }
    }}
