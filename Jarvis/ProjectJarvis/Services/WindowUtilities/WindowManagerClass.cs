using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Automation;
using System.Runtime.InteropServices;

namespace Jarvis.Project.Services.WindowUtilities
{
    public static class WindowManagerClass
    {
        private const int SW_MINIMIZE = 6;
        private const int SW_MAXIMIZE = 3;
        private const int SW_RESTORE = 9;

        // Импортируем функцию ShowWindow из User32.dll
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);


        public static void CloseWindow(string processName)
        {
            try
            {
                var processes = Process.GetProcessesByName(processName);
                foreach (var process in processes)
                {
                    process.Kill();
                    Console.WriteLine("Process {0} killed", process.ProcessName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error closing window: " + ex.Message);
            }
        }

        public static void MinimizeWindow(string processName)
        {
            try
            {
                var processes = Process.GetProcessesByName(processName);
                var mainProcess = processes.OrderBy(p => p.StartTime).FirstOrDefault();
                if (mainProcess != null)
                {
                    var window = FindWindowByProcess(mainProcess);
                    if (window != null)
                    {
                        ShowWindow(new IntPtr(window.Current.NativeWindowHandle), SW_MINIMIZE);
                        Console.WriteLine("Minimized window for process: " + mainProcess.ProcessName);
                    }
                    else
                    {
                        Console.WriteLine("Window not found for process: " + mainProcess.ProcessName);
                    }
                }
                else
                {
                    Console.WriteLine("No processes found with name: " + processName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error minimizing window: " + ex.Message);
            }
        }

        public static void MaximizeWindow(string processName)
        {
            try
            {
                var processes = Process.GetProcessesByName(processName);
                var mainProcess = processes.OrderBy(p => p.StartTime).FirstOrDefault();
                if (mainProcess != null)
                {
                    var window = FindWindowByProcess(mainProcess);
                    if (window != null)
                    {
                        ShowWindow(new IntPtr(window.Current.NativeWindowHandle), SW_MAXIMIZE);
                        Console.WriteLine("Maximized window for process: " + mainProcess.ProcessName);
                    }
                    else
                    {
                        Console.WriteLine("Window not found for process: " + mainProcess.ProcessName);
                    }
                }
                else
                {
                    Console.WriteLine("No processes found with name: " + processName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error maximizing window: " + ex.Message);
            }
        }

        public static void RestoreWindow(string processName)
        {
            try
            {
                var processes = Process.GetProcessesByName(processName);
                var mainProcess = processes.OrderBy(p => p.StartTime).FirstOrDefault();
                if (mainProcess != null)
                {
                    var window = FindWindowByProcess(mainProcess);
                    if (window != null)
                    {
                        ShowWindow(new IntPtr(window.Current.NativeWindowHandle), SW_RESTORE);
                        Console.WriteLine("Restored window for process: " + mainProcess.ProcessName);
                    }
                    else
                    {
                        Console.WriteLine("Window not found for process: " + mainProcess.ProcessName);
                    }
                }
                else
                {
                    Console.WriteLine("No processes found with name: " + processName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error restoring window: " + ex.Message);
            }
        }

        private static AutomationElement FindWindowByProcess(Process process)
        {
            var condition = new PropertyCondition(AutomationElement.ProcessIdProperty, process.Id);
            return AutomationElement.RootElement.FindFirst(TreeScope.Children, condition);
        }
    }

}