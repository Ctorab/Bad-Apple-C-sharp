using BadApple.ASCII;
using BadApple.Domain;
using OpenCvSharp;
using System.Drawing;
using System.Text.RegularExpressions;

namespace BadApple.Players
{
    internal class VideoFramesPlayer : PlayerBase
    {
        private static List<string> _videFramesNames = new();

        public VideoFramesPlayer(string playFilePath) : base(playFilePath) { }

        public override void Play()
        {
            LoadDownloadedFrames();
            WriteFrames();
        }

        private void LoadDownloadedFrames()
        {
            _videFramesNames = new();

            var frames = Directory.GetFiles(PlayFilePath, "*.png");

            Array.Sort(frames, (a, b) => int.Parse(Regex.Replace(a, "[^0-9]", "")) - int.Parse(Regex.Replace(b, "[^0-9]", "")));

            frames.ToList().ForEach(frame => _videFramesNames.Add(Path.GetFileName(frame)));
        }

        private void WriteFrames()
        {
            var fps = GetVideoFPS();
            var startTime = DateTime.Now;

            var delay = ((1 / fps) * 1000);

            int i = 0;
            while (i < _videFramesNames.Count)
            {
                var currentTime = DateTime.Now;

                if (currentTime - startTime < DateTime.Now.AddMilliseconds(delay) - DateTime.Now) continue;

                Console.Title = $"{i}/{_videFramesNames.Count}";

                var filePath = Path.Combine(PlayFilePath, _videFramesNames[i]);

                var frame = GetBitmapFromImage(filePath);
                var asciiChars = BitmapToASCIIConverter.ConvertBitmapToASCII(frame);

                NonBlockingConsole.Write(asciiChars);

                i++;

                startTime = currentTime;
            }

            Bitmap GetBitmapFromImage(string imagePath)
            {
                using (var ms = new MemoryStream(File.ReadAllBytes(imagePath)))
                    return new Bitmap(ms);
            }
        }

        private double GetVideoFPS()
        {
            using (var videoCapture = new VideoCapture(Program.VideFilePath))
            {
                return videoCapture.Get(VideoCaptureProperties.Fps);
            }
        }
    }
}