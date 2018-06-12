using Android.App;
using Android.OS;
using Java.Text;
using Java.Util;
using Android.Arch.Lifecycle;
using System;
using Android.Support.V7.App;

namespace Stopwatch_AAC
{
    [Activity(Label = "Stopwatch", Exported = true, MainLauncher = false)]
    public class MainActivity : AppCompatActivity
    {
        readonly DateFormat F;
        readonly StopwatchViewModel model;

        public MainActivity()
        {
            F = new SimpleDateFormat("HH:mm:ss:SSS",
                    Locale.Us)
            {
                TimeZone = Java.Util.TimeZone.GetTimeZone(("UTC"))
            };
            model = new StopwatchViewModel();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);
            var time = FindViewById(Resource.Id.time);
            var startStop = FindViewById(Resource.Id.start_stop);
            var reset = FindViewById(Resource.Id.reset);

            StopwatchLifecycleObserver observer = new StopwatchLifecycleObserver(model, new Handler());

            // LiefeData
            //model.getIsRunning().observe(this, isRunning -> {
            //    final boolean running = StopwatchViewModel.getBoolean(isRunning);
            //    startStop.setText(running ? R.string.stop : R.string.start);
            //    reset.setEnabled(!running);
            //});
            //model.getDiff().observe(this, diff
            //-> time.setText(F.format(new Date(StopwatchViewModel.getLong(diff)))));

            startStop.Click += (object sender, EventArgs e) =>
            {
                model.Running = !model.Running;
                if (model.Running)
                {
                    observer.ScheduleAtFixedRate();
                }
                else
                {
                    observer.Stop();
                }
            };
            reset.Click += (object sender, EventArgs e) => { model.Diff = 0; };
            Lifecycle.AddObserver(observer);
        }
    }
}