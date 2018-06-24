using Java.Util;
using System;

namespace Stopwatch
{
    public class MyTimerTask : TimerTask
    {
        readonly Action action;

        public MyTimerTask(Action action)
        {
            this.action = action;
        }

        public override void Run()
        {
            action();
        }
    }
}
