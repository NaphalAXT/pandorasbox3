﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TheBox.Common
{
    //  Issue 33:  	 Bring to front if already started - Tarion
    public static class ProcessExtension
    {
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        private const int WS_SHOWNORMAL = 1;

        /// <summary>
        /// Brings an application to front
        /// </summary>
        /// <param name="procToFront"></param>
        public static void BringToFront(Process procToFront)
        {
            if (procToFront != null)
            {
                ShowWindowAsync(procToFront.MainWindowHandle, WS_SHOWNORMAL);
                SetForegroundWindow(procToFront.MainWindowHandle);
            }
        }
    }
}
