using Android.App;
using Android.Arch.Lifecycle;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Java.Interop;

namespace Stopwatch_AAC
{

    [Activity(Label = "Minimal", Exported = true, MainLauncher = true)]
    public class Minimal : AppCompatActivity, ILifecycleObserver
    {
        const string TAG = "Stopwatch_AAC";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Lifecycle.AddObserver(this);
            Log.Debug(TAG, Lifecycle.CurrentState.ToString());
        }

        [Lifecycle.Event.OnAny]
        [Export]
        public void Hello()
        {
            Log.Debug(TAG, Lifecycle.CurrentState.ToString());
        }
    }
}
