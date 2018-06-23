using Android.Arch.Lifecycle;

namespace Stopwatch_AAC
{
    public class StopwatchViewModel : ViewModel
    {
        public readonly MutableLiveData running = new MutableLiveData();
        public readonly MutableLiveData diff = new MutableLiveData();
        readonly MutableLiveData started = new MutableLiveData();

        public StopwatchViewModel()
        {
            running.SetValue(false);
            diff.SetValue(0);
            started.SetValue(0);
        }

        public bool Running
        {
            get
            {
                return (bool)running.Value;
            }
            set
            {
                running.SetValue(value);
            }
        }

        public long Diff
        {
            get
            {
                return (long)diff.Value;
            }
            set
            {
                diff.PostValue(value);
            }
        }

        public long Started
        {
            get
            {
                return (long)started.Value;
            }
            set
            {
                started.PostValue(value);
            }
        }
    }
}
