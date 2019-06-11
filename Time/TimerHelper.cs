using System;
using System.Timers;

namespace Ji.TimeHelper
{
    public class TimerHelper
    {
        public readonly static TimerHelper Instance = new TimerHelper();

        private Timer timer = new Timer();

        private Action timerAction;

        public void Start(double interval, Action action)
        {
            timerAction = action;
            timer.Interval = interval;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (timerAction != null)
            {
                timerAction();
            }
        }

        public void Stop()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Elapsed -= timer_Elapsed;
            }
        }
    }
}