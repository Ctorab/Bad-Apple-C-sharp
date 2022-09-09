using BadApple.ASCII;
using BadApple.Domain;
using OpenCvSharp;
using System.Diagnostics;
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

            var frameTimer = new Stopwatch();

            for (int i = 0; i < _videFramesNames.Count; i++)
            {
                frameTimer.Start();
                Console.Title = $"{i}/{_videFramesNames.Count}";

                var filePath = $@"{PlayFilePath}\{_videFramesNames[i]}";

                var frame = GetBitmapFromImage(filePath);
                var asciiChars = BitmapToASCIIConverter.ConvertBitmapToASCII(frame);

                Console.SetCursorPosition(0, 0);
                Console.Write(asciiChars);

                frameTimer.Restart();

                try
                {
                    Thread.Sleep((int)(fps / (frameTimer.ElapsedTicks / 1000)));
                }
                catch { }

                frameTimer.Restart();
            }

            frameTimer.Stop();

            Bitmap GetBitmapFromImage(string imagePath)
            {
                using (var ms = new MemoryStream(File.ReadAllBytes(imagePath)))
                    return new Bitmap(ms);
            }
        }

        private double GetVideoFPS()
        {
            var videoCapture = new VideoCapture(Program.VideFilePath);

            return videoCapture.Get(VideoCaptureProperties.Fps);
        }
    }
}