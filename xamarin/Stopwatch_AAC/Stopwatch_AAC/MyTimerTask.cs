using Android.OS;
using Java.Lang;
using Java.Util;

namespace Stopwatch_AAC
{
    public class MyTimerTask : TimerTask
    {
        readonly Handler handler;
        readonly StopwatchViewModel model;

        public MyTimerTask(Handler handler, StopwatchViewModel model)
        {
            this.handler = handler;
            this.model = model;
        }

        public override void Run()
        {
            handler.Post(() => model.Diff = JavaSystem.CurrentTimeMillis() - model.Started);
        }
    }
}
