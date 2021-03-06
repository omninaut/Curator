﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Curator.Utils
{
    /// <summary>
    /// Timer wrapper class, implemented as a singleton object
    /// </summary>
    
    public sealed class SlideshowTimer : System.Windows.Forms.Timer
    {
        private static SlideshowTimer _timerInstance;
        private static readonly object _timerInstanceSync = new object(); // In case we want to multithread later

        private SlideshowTimer() : base()
        {
            this.Enabled = true;
            this.Tick += new EventHandler(timer_Tick);
        }

        public static SlideshowTimer GetInstance
        {
            get
            {
                if (_timerInstance == null)
                {
                    lock (_timerInstanceSync)
                    {
                        if (_timerInstance == null)
                        {
                            _timerInstance = new SlideshowTimer();
                        }
                    }
                }

                return _timerInstance;
            }
        }

        // Set the wallpaper to the next image when the timer ticks
        // Add any other processing performed at the same time to this event handler
        private void timer_Tick(object sender, EventArgs e)
        {
            WallpaperChanger.GetInstance.SetNextWallpaper();
        }

    }
}