﻿using System;
using System.Timers;

namespace V2RayGCon.Lib.Sys
{
    class CancelableTimeout
    {
        Timer timer;
        int TIMEOUT;
        Action worker;

        public CancelableTimeout(Action worker, int timeout)
        {
            if (timeout <= 0 || worker == null)
            {
                throw new ArgumentException();
            }

            this.TIMEOUT = timeout;
            this.worker = worker;

            timer = new Timer();
            timer.Enabled = false;
            timer.AutoReset = false;
            timer.Elapsed += OnTimeout;
        }

        private void OnTimeout(object sender, EventArgs e)
        {
            this.worker();
        }

        public void Timeout()
        {
            Cancel();
            this.worker();
        }

        public void Start()
        {
            timer.Interval = this.TIMEOUT;
            timer.Enabled = true;
        }

        public void Cancel()
        {
            timer.Enabled = false;
        }

        public void Release()
        {
            Cancel();
            timer.Elapsed -= OnTimeout;
            this.worker = null;
            timer.Close();
        }
    }
}
