using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Injector;

public class Native
{
    #region Kernel32-Module

    private const string KernelModuleName = "kernel32.dll";

    [DllImport(KernelModuleName, SetLastError = true)]
    public static extern IntPtr GetProcAddress(
        IntPtr hModule,
        string ProcName);

    [DllImport(KernelModuleName, SetLastError = true)]
    public static extern IntPtr LoadLibraryEx(
        string lpFileName,
        IntPtr hFile,
        LoadLibraryFlags dwFlags);

    [Flags]
    public enum LoadLibraryFlags : uint
    {
        DontResolveReferences = 0x00000001,
        LoadIgnoreCodeAuthzLevel = 0x00000010,
        LoadLibraryAsDatafile = 0x00000002,
        LoadLibraryAsDatafileExclusive = 0x00000040,
        LoadLibraryAsImageResource = 0x00000020,
        LoadWithAlteredSearchPath = 0x00000008,

        LoadLibrarySearchDllLoadDir = 0x00000100,
        LoadLibrarySearchApplicationDir = 0x00000200,
        LoadLibrarySearchUserDirs = 0x00000400,
        LoadLibrarySearchSystem32 = 0x00000800,
        LoadLibrarySearchDefaultDirs = 0x00001000,
        LoadLibrarySearchSystem64 = 0x00002000,
    }

    #endregion

    #region User-Module

    private const string UserModuleName = "user32.dll";

    [DllImport(UserModuleName, SetLastError = true)]
    public static extern IntPtr FindWindowA(
        string lpClassName,
        string lpWindowName);

    [DllImport(UserModuleName, SetLastError = true)]
    public static extern IntPtr SetWindowsHookEx(
        HookType HookType,
        WindowsHookDelegate lPfn,
        IntPtr hInstance,
        uint dwThreadId);

    public delegate IntPtr WindowsHookDelegate(int nCode, IntPtr wParam, IntPtr lParam);

    public enum HookType : int
    {
        JournalRecord = 0,
        JournalPlayback = 1,
        Keyboard = 2,
        GetMessage = 3,
        CallWndProc = 4,
        CBT = 5,
        SysMsgFilter = 6,
        Mouse = 7,
        Hardware = 8,
        Debug = 9,
        Shell = 10,
        ForegroundIdle = 11,
        CallWndProcRet = 12,
        KeyboardLL = 13,
        MouseLL = 14
    }

    [DllImport(UserModuleName, SetLastError = true)]
    public static extern uint GetWindowThreadProcessId(
        IntPtr hWnd,
        out uint Process);

    [DllImport(UserModuleName, SetLastError = true)]
    public static extern bool PostThreadMessage(
        uint ThreadId,
        uint Msg,
        IntPtr wParam,
        IntPtr lParam);

    [DllImport(UserModuleName, SetLastError = true)]
    public static extern bool EnumWindows(
    EnumWindowsDelegate lpEnumFunc,
    IntPtr lParam);

    public delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lParam);

    #endregion
}
