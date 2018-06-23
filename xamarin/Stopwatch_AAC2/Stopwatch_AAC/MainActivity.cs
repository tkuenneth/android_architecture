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

        StopwatchViewModel model;
        TextView time;
        Button startStop, reset;

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

            model = (StopwatchViewModel)ViewModelProviders.Of(this).Get(Java.Lang.Class.FromType(typeof(StopwatchViewModel)));
            model.running.Observe(this, new OnChangedHandler(() =>
            {
                startStop.Text = GetString(model.Running ? Resource.String.stop : Resource.String.start);
                reset.Enabled = !model.Running;
            }));
            model.diff.Observe(this, new OnChangedHandler(() =>
            {
                time.Text = F.Format(new Date(model.Diff));
            }));
            StopwatchLifecycleObserver observer = new StopwatchLifecycleObserver(model);

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

    public class OnChangedHandler : Java.Lang.Object, Android.Arch.Lifecycle.IObserver
    {
        readonly Action a;

        public OnChangedHandler(Action a)
        {
            this.a = a;
        }

        public void OnChanged(Java.Lang.Object p0)
        {
            a();
        }
    }
}