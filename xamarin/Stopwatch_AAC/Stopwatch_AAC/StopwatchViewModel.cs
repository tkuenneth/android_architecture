namespace Stopwatch_AAC
{
    public class StopwatchViewModel : ViewModelBase
    {
        long diff;
        public long Diff
        {
            get
            {
                return diff;
            }
            set
            {
                SetPropertyValue(ref diff, value);
            }
        }

        long started;
        public long Started
        {
            get
            {
                return started;
            }
            set
            {
                SetPropertyValue(ref started, value);
            }
        }

        bool running;
        public bool Running
        {
            get
            {
                return running;
            }
            set
            {
                SetPropertyValue(ref running, value);
            }
        }
    }
}
