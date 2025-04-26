#include <Windows.h>

extern "C" __declspec(dllexport) void run() {

}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD dwCallReason, LPVOID lpReserved) {
	if (dwCallReason == DLL_PROCESS_ATTACH) {
		MessageBoxA(NULL, "Module Injected!", "Success", MB_OK);
	}
}

