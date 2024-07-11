﻿using System;
using System.Runtime.InteropServices;

namespace Jarvis.Project.Settings.SystemUtilities
{
    internal class RecycleBinManagerClass
    {

        enum RecycleFlags : uint
        {
            SHERB_NOCONFIRMATION = 0x00000001,
            SHERB_NOPROGRESSUI = 0x00000002,
            SHERB_NOSOUND = 0x00000004
        }


        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        static extern uint SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlags dwFlags);

        public void CleanRecycleBin()
        {
            try
            {
                uint result = SHEmptyRecycleBin(IntPtr.Zero, null, 0);
            }
            catch (Exception e)
            {

            }
        }

    }
}
