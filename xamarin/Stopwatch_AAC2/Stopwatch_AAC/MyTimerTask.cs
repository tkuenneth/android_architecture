using Java.Lang;
using Java.Util;

namespace Stopwatch_AAC
{
    public class MyTimerTask : TimerTask
    {
        readonly StopwatchViewModel model;

        public MyTimerTask(StopwatchViewModel model)
        {
            this.model = model;
        }

        public override void Run()
        {
            model.Diff = JavaSystem.CurrentTimeMillis() - model.Started;
        }
    }
}
