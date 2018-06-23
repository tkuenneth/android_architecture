using Android.App;
using Android.OS;
using Java.Text;
using Java.Util;
using Android.Arch.Lifecycle;
using System;
using Android.Support.V7.App;
using Android.Widget;
using System.ComponentModel;

namespace Stopwatch_AAC
{
    [Activity(Label = "Stopwatch", Exported = true, MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        static readonly StopwatchViewModel model = new StopwatchViewModel();

        readonly DateFormat F;

        TextView time;
        Button startStop, reset;

        PropertyChangedEventHandler handlerModelChanged;
        EventHandler startStopClicked;
        EventHandler resetClicked;

        public MainActivity()
        {
            F = new SimpleDateFormat("HH:mm:ss:SSS",
                    Locale.Us)
            {
                TimeZone = Java.Util.TimeZone.GetTimeZone(("UTC"))
            };
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            time = FindViewById<TextView>(Resource.Id.time);
            startStop = FindViewById<Button>(Resource.Id.start_stop);
            reset = FindViewById<Button>(Resource.Id.reset);
            StopwatchLifecycleObserver observer = new StopwatchLifecycleObserver(model, new Handler());

            handlerModelChanged = (sender, e) =>
            {
                switch (e.PropertyName)
                {
                    case "Running":
                        UpdateButtons();
                        break;
                    case "Diff":
                        UpdateTime();
                        break;
                }
            };
            model.PropertyChanged += handlerModelChanged;

            startStopClicked = (object sender, EventArgs e) =>
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
            startStop.Click += startStopClicked;

            resetClicked = (object sender, EventArgs e) => { model.Diff = 0; };
            reset.Click += resetClicked;

            Lifecycle.AddObserver(observer);
            UpdateButtons();
            UpdateTime();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            model.PropertyChanged -= handlerModelChanged;
            startStop.Click -= startStopClicked;
            reset.Click -= resetClicked;
        }

        void UpdateButtons()
        {
            startStop.Text = GetString(model.Running ? Resource.String.stop : Resource.String.start);
            reset.Enabled = !model.Running;
        }

        void UpdateTime()
        {
            time.Text = F.Format(new Date(model.Diff));
        }
    }
}