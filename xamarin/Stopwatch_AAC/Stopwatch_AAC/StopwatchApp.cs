using System;
using Android.App;
using Android.Arch.Lifecycle;
using Android.Util;
using Java.Interop;

namespace Stopwatch_AAC
{
    [Application]
    public class StopwatchApp : Application, ILifecycleObserver
    {
        const string TAG = "MyApp";

        public StopwatchApp(IntPtr handle, Android.Runtime.JniHandleOwnership ownerShip) : base(handle, ownerShip)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            ProcessLifecycleOwner.Get().Lifecycle.AddObserver(this);
        }

        [Lifecycle.Event.OnStop]
        [Export]
        public void Stopped()
        {
            Log.Debug(TAG, "App received OnStop");
        }

        [Lifecycle.Event.OnStart]
        [Export]
        public void Started()
        {
            Log.Debug(TAG, "App received OnStart");
        }
    }
}
