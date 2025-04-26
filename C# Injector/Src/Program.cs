using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Injector.Native;

namespace Injector;

public class Program
{
    public static void Main(string[] Parameters)
    {
        Console.Title = "Injector";

        Process Proc = Process.GetProcessesByName("RobloxPlayerBeta").FirstOrDefault();

        IntPtr hWindow = IntPtr.Zero;
        EnumWindows(delegate (IntPtr hWnd, IntPtr lParam)
        {
            GetWindowThreadProcessId(hWnd, out uint ProcessId);

            if (ProcessId == Proc.Id)
            {
                hWindow = hWnd;
                return false;
            }
            return true;
        }, IntPtr.Zero);

        if (Proc == null || hWindow == IntPtr.Zero)
        {
            Console.WriteLine($"[{DateTime.Now}]: Failed To Get Process.");
            Console.ReadKey();
        }

        Console.WriteLine($"[{DateTime.Now}]: Window Handle - 0x{(long)hWindow:x}.");

        uint ThreadId = GetWindowThreadProcessId(hWindow, out uint ProcessId);

        if (ThreadId == 0 || ProcessId == 0)
        {
            Console.WriteLine($"[{DateTime.Now}]: Failed To Get Thread Id.");
            Console.ReadKey();
        }

        Console.WriteLine($"[{DateTime.Now}]: Thread Id - {ThreadId}.");

        IntPtr hModule = LoadLibraryEx("amdxx64.dll", IntPtr.Zero, LoadLibraryFlags.DontResolveReferences);

        if (hModule == IntPtr.Zero)
        {
            Console.WriteLine($"[{DateTime.Now}]: Failed To Load Module.");
            Console.ReadKey();
        }

        Console.WriteLine($"[{DateTime.Now}]: Module Handle - 0x{(long)hModule:x}.");

        WindowsHookDelegate HookProc = Marshal.GetDelegateForFunctionPointer<WindowsHookDelegate>(GetProcAddress(hModule, "run"));

        if (HookProc == null)
        {
            Console.WriteLine($"[{DateTime.Now}]: Failed To Get Hook Procedure.");
            Console.ReadKey();
        }

        IntPtr hHook = SetWindowsHookEx(HookType.GetMessage, HookProc, hModule, ThreadId);

        if (hHook == IntPtr.Zero)
        {
            Console.WriteLine($"[{DateTime.Now}]: Failed To Set Hook.");
            Console.ReadKey();
        }

        Console.WriteLine($"[{DateTime.Now}]: Hook Handle - 0x{(long)hHook:x}.");

        PostThreadMessage(ThreadId, 0x0000, IntPtr.Zero, IntPtr.Zero);

        Console.WriteLine($"[{DateTime.Now}]: Posted Message.");
        Console.ReadKey();
    }
}
