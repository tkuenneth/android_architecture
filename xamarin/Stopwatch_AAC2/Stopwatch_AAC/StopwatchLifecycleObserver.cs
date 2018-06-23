using Android.Arch.Lifecycle;
using Android.Util;
using Java.Interop;
using Java.Lang;
using Java.Util;

namespace Stopwatch_AAC
{
    public class StopwatchLifecycleObserver : Object, ILifecycleObserver
    {
        const string TAG = "StopwatchLifecycleObserver";

        readonly StopwatchViewModel model;

        Timer timer;
        TimerTask timerTask;

        public StopwatchLifecycleObserver(StopwatchViewModel model)
        {
            this.model = model;
        }

        [Lifecycle.Event.OnAny]
        [Export]
        public void Hello(ILifecycleOwner owner,
                          Lifecycle.Event evt)
        {
            Log.Debug(TAG, owner.ToString());
            Log.Debug(TAG, evt.ToString());
        }

        [Lifecycle.Event.OnResume]
        [Export]
        public void StartTimer()
        {
            timer = new Timer();
            if (model.Running)
            {
                ScheduleAtFixedRate();
            }
        }

        [Lifecycle.Event.OnPause]
        [Export]
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
            timerTask = new MyTimerTask(model);
            timer.ScheduleAtFixedRate(timerTask, 0, 200);
        }
    }
}
