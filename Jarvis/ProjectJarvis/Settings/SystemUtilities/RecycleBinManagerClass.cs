using System;
using System.Runtime.InteropServices;

namespace Jarvis.ProjectJarvis.Settings.SystemUtilities;

internal class RecycleBinManagerClass
{
    [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
    private static extern uint SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlags dwFlags);

    public void CleanRecycleBin()
    {
        try
        {
            var result = SHEmptyRecycleBin(IntPtr.Zero, null, 0);
        }
        catch (Exception e)
        {
            // ignored
        }
    }

    private enum RecycleFlags : uint
    {
        SHERB_NOCONFIRMATION = 0x00000001,
        SHERB_NOPROGRESSUI = 0x00000002,
        SHERB_NOSOUND = 0x00000004
    }
}