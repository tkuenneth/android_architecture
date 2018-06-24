using Android.App;
using Android.OS;
using Java.Text;
using Java.Util;
using System;
using Android.Widget;
using Java.Lang;
using Android.Views;

namespace Stopwatch
{
    [Activity(Label = "@string/app_name", Exported = true, MainLauncher = true)]
    public class StopwatchActivity : Activity
    {
        const string KEY_DIFF = "diff";
        const string KEY_RUNNING = "running";

        readonly DateFormat format;
        Timer timer;
        TimerTask timerTask;
        TextView time;
        Button startStop;
        Button reset;
        long started;
        bool isRunning;
        long diff;

        public StopwatchActivity()
        {
            format = new SimpleDateFormat("HH:mm:ss:SSS",
                    Locale.Us)
            {
                TimeZone = Java.Util.TimeZone.GetTimeZone("UTC")
            };
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            time = FindViewById<TextView>(Resource.Id.time);
            startStop = FindViewById<Button>(Resource.Id.start_stop);
            startStop.SetOnClickListener(new MyOnClickListener(() =>
            {
                isRunning = !isRunning;
                if (isRunning)
                {
                    ScheduleAtFixedRate();
                }
                else
                {
                    timerTask.Cancel();
                }
                UpdateUI();
            }));
            reset = FindViewById<Button>(Resource.Id.reset);
            reset.SetOnClickListener(new MyOnClickListener(() => { ClearTime(); }));
            if (savedInstanceState != null)
            {
                GetValuesFromBundle(savedInstanceState);
                SetTime();
            }
            else
            {
                isRunning = false;
                ClearTime();
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutLong(KEY_DIFF, diff);
            outState.PutBoolean(KEY_RUNNING, isRunning);
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
            GetValuesFromBundle(savedInstanceState);
        }

        protected override void OnResume()
        {
            base.OnResume();
            timer = new Timer();
            UpdateUI();
            if (isRunning)
            {
                ScheduleAtFixedRate();
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            timer.Cancel();
        }

        private void ClearTime()
        {
            time.SetText(Resource.String.cleared);
            diff = 0;
        }

        private void UpdateUI()
        {
            startStop.SetText(isRunning ? Resource.String.stop : Resource.String.start);
            reset.Enabled = !isRunning;
        }

        private void GetValuesFromBundle(Bundle b)
        {
            diff = b.GetLong(KEY_DIFF);
            isRunning = b.GetBoolean(KEY_RUNNING);
        }

        private void SetTime()
        {
            time.Text = format.Format(new Date(diff));
        }

        private void ScheduleAtFixedRate()
        {
            started = JavaSystem.CurrentTimeMillis() - diff;
            timerTask = new MyTimerTask(() =>
            {
                diff = JavaSystem.CurrentTimeMillis() - started;
                RunOnUiThread(() =>
                {
                    SetTime();
                });
            });
            timer.ScheduleAtFixedRate(timerTask, 0, 200);
        }
    }

    public sealed class MyOnClickListener : Java.Lang.Object, View.IOnClickListener
    {
        readonly Action action;

        public MyOnClickListener(Action action)
        {
            this.action = action;
        }

        void View.IOnClickListener.OnClick(View v)
        {
            action();
        }
    }
}