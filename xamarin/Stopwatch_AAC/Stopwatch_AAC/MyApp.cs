using System;
using Android.App;
using Android.Arch.Lifecycle;
using Android.Util;
using Java.Interop;

namespace Stopwatch_AAC
{
    [Application]
    public class MyApp : Application, ILifecycleObserver
    {
        const string TAG = "MyApp";

        public MyApp(IntPtr handle, Android.Runtime.JniHandleOwnership ownerShip) : base(handle, ownerShip)
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
            Log.Debug(TAG, "App entered background state.");
        }

        [Lifecycle.Event.OnStart]
        [Export]
        public void Started()
        {
            Log.Debug(TAG, "App entered foreground state.");
        }
    }
}
