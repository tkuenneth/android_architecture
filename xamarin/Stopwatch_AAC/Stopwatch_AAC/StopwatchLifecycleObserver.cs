using Android.Arch.Lifecycle;
using Android.OS;
using Java.Lang;
using Java.Util;

namespace Stopwatch_AAC
{
    public class StopwatchLifecycleObserver : Java.Lang.Object, ILifecycleObserver
    {
        readonly StopwatchViewModel model;
        readonly Handler handler;

        Timer timer;
        TimerTask timerTask;

        public StopwatchLifecycleObserver(StopwatchViewModel model, Handler handler)
        {
            this.model = model;
            this.handler = handler;
        }

        [Lifecycle.Event.OnResume]
        public void StartTimer(ILifecycleOwner owner, Lifecycle.Event evt)
        {
            timer = new Timer();
            if (model.Running)
            {
                ScheduleAtFixedRate();
            }
        }

        [Lifecycle.Event.OnPause]
        public void StopTimer()
        {
            timer.Cancel();
        }

        public void Stop()
        {
            timerTask.Cancel();
        }

        public void ScheduleAtFixedRate()
        {
            model.Started = JavaSystem.CurrentTimeMillis() - model.Diff;
            timerTask = new MyTimerTask(handler, model);
            timer.ScheduleAtFixedRate(timerTask, 0, 200);
        }
    }
}
