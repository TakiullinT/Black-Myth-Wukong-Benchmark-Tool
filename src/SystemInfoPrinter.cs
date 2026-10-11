namespace WukongBenchmarkAutomation;

public static class SystemInfoPrinter
{
    public static void PrintSpecs()
    {
        Console.WriteLine("[ХАРАКТЕРИСТИКИ КОМПЬЮТЕРА]");
        Console.WriteLine($"CPU: {HardwareInfo.GetCpuName()}");
        Console.WriteLine($"GPU: {HardwareInfo.GetGpuName()}");
        Console.WriteLine($"RAM: {HardwareInfo.GetRamAmount()} GB");
    }

    public static void PrintSummary(double cpuFps, double gpuFps)
    {
        Console.WriteLine($"ПК: {HardwareInfo.GetCpuName()} | {HardwareInfo.GetGpuName()} | {HardwareInfo.GetRamAmount()}");
        Console.WriteLine($"Результат CPU-теста (1080p Low): {cpuFps} FPS");
        Console.WriteLine($"Результат GPU-теста (1080p Medium): {gpuFps} FPS");
    }
}
