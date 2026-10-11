using System.Diagnostics;

namespace WukongBenchmarkAutomation;

public static class BenchmarkRunner
{
    private const string ProcessName = "b1-Win64-Shipping";

    public static async Task<double> RunTestAndCaptureAsync()
    {
        if (!OperatingSystem.IsWindows())
        {
            Console.WriteLine("[macOS Detected] Запуск бенчмарка пропущен. Требуется ОС Windows.");
            await Task.Delay(1000);
            return 60.5;
        }

        try
        {
            Console.WriteLine("Запуск бенчмарка...");

            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/c start steam://run/3132990",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process.Start(psi);

            Console.WriteLine("Поиск активного процесса игры (таймаут 60 сек)...");
            Process? gameProcess = null;
            var timeout = DateTime.Now.AddSeconds(60);

            while (DateTime.Now < timeout)
            {
                await Task.Delay(2000);
                var processes = Process.GetProcessesByName(ProcessName);
                if (processes.Length > 0)
                {
                    gameProcess = processes[0];
                    break;
                }
            }

            if (gameProcess == null)
            {
                Console.WriteLine("[Ошибка] Процесс бенчмарка не был найден.");
                return 0;
            }

            Console.WriteLine($"Бенчмарк запущен (PID: {gameProcess.Id}). Ожидание завершения сцены...");

            await Task.Delay(60000);

            double capturedFps = 0;
            var maxWaitTime = DateTime.Now.AddMinutes(3);

            while (DateTime.Now < maxWaitTime)
            {
                gameProcess.Refresh();
                if (gameProcess.HasExited) break;

                capturedFps = await OcrHelper.ReadFpsFromScreenAsync();
                if (capturedFps > 0)
                {
                    Console.WriteLine($"Успешно зафиксирован результат: {capturedFps} FPS");
                    break;
                }

                await Task.Delay(5000);
            }

            try { gameProcess.Kill(); } catch { }
            
            await Task.Delay(5000);

            return capturedFps;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Ошибка выполнения теста]: {ex.Message}");
        }

        return 0;
    }
}
