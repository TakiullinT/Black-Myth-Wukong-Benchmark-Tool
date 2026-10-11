namespace WukongBenchmarkAutomation;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=======================================================");
        Console.WriteLine("    Black Myth: Wukong - Benchmark Automation Tool     ");
        Console.WriteLine("=======================================================\n");

        SystemInfoPrinter.PrintSpecs();

        Console.WriteLine("\n=======================================================");
        Console.WriteLine("[СТАРТ ПРОГОНА 1: CPU-ТЕСТ]");
        Console.WriteLine("=======================================================");
        
        Console.WriteLine("Применение настроек для CPU-теста...");
        ConfigManager.ApplyCpuSettings();

        double cpuFps = await BenchmarkRunner.RunTestAndCaptureAsync();

        await Task.Delay(3000);

        Console.WriteLine("\n=======================================================");
        Console.WriteLine("[СТАРТ ПРОГОНА 2: GPU-ТЕСТ]");
        Console.WriteLine("=======================================================");

        Console.WriteLine("Применение настроек для GPU-теста...");
        ConfigManager.ApplyGpuSettings();

        double gpuFps = await BenchmarkRunner.RunTestAndCaptureAsync();

        Console.WriteLine("\n=======================================================");
        Console.WriteLine("                     ИТОГОВЫЙ ОТЧЕТ                    ");
        Console.WriteLine("=======================================================");
        SystemInfoPrinter.PrintSummary(cpuFps, gpuFps);
        Console.WriteLine("=======================================================");
        
        Console.WriteLine("\nТестирование полностью завершено. Нажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}