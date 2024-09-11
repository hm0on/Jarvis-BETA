using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Jarvis.Project.Settings.SystemUtilities;

internal class PowerManagerClass
{
    [DllImport("Powrprof.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

    public void Shutdown()
    {
        Process.Start("shutdown", "/s /t 0");
    }

    public void Restart()
    {
        Process.Start("shutdown", "/r /t 0");
    }

    public void Hibernate()
    {
        SetSuspendState(true, true, true);
    }
}