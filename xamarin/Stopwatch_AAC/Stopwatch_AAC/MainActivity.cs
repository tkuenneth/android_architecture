using Android.App;
using Android.OS;
using Java.Text;
using Java.Util;
using Android.Arch.Lifecycle;
using System;
using Android.Support.V7.App;
using Android.Widget;

namespace Stopwatch_AAC
{
    [Activity(Label = "Stopwatch", Exported = true, MainLauncher = true)]
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
            var time = FindViewById<TextView>(Resource.Id.time);
            var startStop = FindViewById<Button>(Resource.Id.start_stop);
            var reset = FindViewById<TextView>(Resource.Id.reset);

            StopwatchLifecycleObserver observer = new StopwatchLifecycleObserver(model, new Handler());

            model.PropertyChanged += (sender, e) =>
            {
                switch (e.PropertyName)
                {
                    case "Running":
                        startStop.Text = GetString(model.Running ? Resource.String.stop : Resource.String.start);
                        reset.Enabled = !model.Running;
                        break;
                    case "Diff":
                        time.Text = F.Format(new Date(model.Diff));
                        break;
                }
            };

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