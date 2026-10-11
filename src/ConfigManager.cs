namespace WukongBenchmarkAutomation;

public static class ConfigManager
{
    private static string GetConfigPath()
    {
        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return Path.Combine(localAppData, "b1_benchmark", "Saved", "Config", "WindowsNoEditor");
    }

    public static void ApplyCpuSettings()
    {
        WriteSettings(1920, 1200, resolutionQuality: 50, qualityLevel: 0);
        Console.WriteLine("Применены настройки для CPU-теста (1920x1200, Low + Upscaling).");
    }

    public static void ApplyGpuSettings()
    {
        WriteSettings(1920, 1200, resolutionQuality: 100, qualityLevel: 1);
        Console.WriteLine("Применены настройки для GPU-теста (1920x1200, Medium).");
    }

    private static void WriteSettings(int resX, int resY, int resolutionQuality, int qualityLevel)
    {
        try
        {
            string configDir = GetConfigPath();
            Directory.CreateDirectory(configDir);
            string iniPath = Path.Combine(configDir, "GameUserSettings.ini");

            string iniContent = $"""
                [/Script/Engine.GameUserSettings]
                bUseDesiredScreenHeight=False
                ResolutionSizeX={resX}
                ResolutionSizeY={resY}
                LastUserConfirmedResolutionSizeX={resX}
                LastUserConfirmedResolutionSizeY={resY}
                bUseDesktopResolutionForFullscreen=False
                FullscreenMode=0
                Version=5
                AudioQualityLevel=0
                LastConfirmedFullscreenMode=0
                PreferredFullscreenMode=0

                [ScalabilityGroups]
                sg.ResolutionQuality={resolutionQuality}
                sg.ViewDistanceQuality={qualityLevel}
                sg.AntiAliasingQuality={qualityLevel}
                sg.ShadowQuality={qualityLevel}
                sg.GlobalIlluminationQuality={qualityLevel}
                sg.ReflectionQuality={qualityLevel}
                sg.PostProcessQuality={qualityLevel}
                sg.TextureQuality={qualityLevel}
                sg.EffectsQuality={qualityLevel}
                sg.FoliageQuality={qualityLevel}
                sg.ShadingQuality={qualityLevel}
                """;

            File.WriteAllText(iniPath, iniContent);
            Console.WriteLine("Параметры INI-файла успешно обновлены (стабильный полноэкранный режим).");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Ошибка записи конфига]: {ex.Message}");
        }
    }
}
