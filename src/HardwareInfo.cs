using System;
using System.Management;

namespace WukongBenchmarkAutomation;

public static class HardwareInfo
{
    public static string GetCpuName()
    {
        if (!OperatingSystem.IsWindows())
            return "Apple Silicon / macOS Host (Development Mode)";

        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_Processor");
            foreach (var item in searcher.Get())
            {
                return item["Name"]?.ToString()?.Trim() ?? "Неизвестный CPU";
            }
        }
        catch (Exception ex)
        {
            return $"Ошибка CPU: {ex.Message}";
        }
        
        return Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER") ?? "Неизвестный CPU";
    }

    public static string GetGpuName()
    {
        if (!OperatingSystem.IsWindows())
            return "Apple GPU / macOS Host (Development Mode)";

        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_VideoController");
            string bestGpu = "Неизвестный GPU";
            foreach (var item in searcher.Get())
            {
                string name = item["Name"]?.ToString()?.Trim() ?? "";
                if (name.Contains("NVIDIA", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("GeForce", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("Radeon RX", StringComparison.OrdinalIgnoreCase))
                {
                    return name;
                }
                if (!string.IsNullOrEmpty(name)) bestGpu = name;
            }
            return bestGpu;
        }
        catch (Exception ex)
        {
            return $"Ошибка GPU: {ex.Message}";
        }
    }
    
    public static string GetRamAmount()
    {
        if (!OperatingSystem.IsWindows())
            return "16 GB (Development Mode)";

        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory");
            long totalBytes = 0;
            foreach (var item in searcher.Get())
            {
                if (long.TryParse(item["Capacity"]?.ToString(), out long capacity))
                {
                    totalBytes += capacity;
                }
            }
            if (totalBytes > 0) return $"{totalBytes / (1024L * 1024 * 1024)} GB";
        }
        catch (Exception ex)
        {
            return $"Ошибка RAM: {ex.Message}";
        }
        
        return "16 GB";
    }
}