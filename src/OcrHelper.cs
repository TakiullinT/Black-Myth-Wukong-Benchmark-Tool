using System.Text.RegularExpressions;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage;

namespace WukongBenchmarkAutomation;

public static class OcrHelper
{
    public static async Task<double> ReadFpsFromScreenAsync()
    {
        string tempPath = Path.Combine(Path.GetTempPath(), "wukong_screenshot.png");

        try
        {
            var bounds = Screen.PrimaryScreen.Bounds;
            using (var bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }
                bitmap.Save(tempPath, System.Drawing.Imaging.ImageFormat.Png);
            }

            var engine = OcrEngine.TryCreateFromUserProfileLanguages() ?? 
                         OcrEngine.TryCreateFromLanguage(new Windows.Globalization.Language("en-US"));

            if (engine == null)
            {
                Console.WriteLine("[OCR Ошибка]: Не удалось инициализировать движок распознавания текста.");
                return 0;
            }

            StorageFile file = await StorageFile.GetFileFromPathAsync(tempPath);
            using var stream = await file.OpenAsync(FileAccessMode.Read);
            var decoder = await BitmapDecoder.CreateAsync(stream);
            SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();

            OcrResult ocrResult = await engine.RecognizeAsync(softwareBitmap);
            string fullText = string.Join(" ", ocrResult.Lines.Select(l => l.Text));

            var match = Regex.Match(fullText, @"Average[^\d]*([0-9]+\.?[0-9]*)", RegexOptions.IgnoreCase);
            if (match.Success && double.TryParse(match.Groups[1].Value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double fps))
            {
                return fps;
            }

            foreach (var line in ocrResult.Lines)
            {
                if (line.Text.Contains("FPS", StringComparison.OrdinalIgnoreCase) || line.Text.Contains("Average", StringComparison.OrdinalIgnoreCase))
                {
                    var numMatch = Regex.Match(line.Text, @"([0-9]+\.[0-9]+|[0-9]+)");
                    if (numMatch.Success && double.TryParse(numMatch.Value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double val))
                    {
                        if (val > 5 && val < 1000) return val;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[OCR Ошибка]: {ex.Message}");
        }
        finally
        {
            if (File.Exists(tempPath))
            {
                try { File.Delete(tempPath); } catch { }
            }
        }

        return 0;
    }
}