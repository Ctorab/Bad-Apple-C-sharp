using BadApple.ASCII;
using BadApple.Domain;
using OpenCvSharp;
using System.Drawing;

namespace BadApple.Captures
{
    internal class VideoFramesCapture : CaptureBase
    {
        private readonly string _asciiTable = string.Empty;

        public VideoFramesCapture(string recordFilePath, string extractFilesFolderPath, string asciiTable) : base(recordFilePath, extractFilesFolderPath)
        {
            _asciiTable = asciiTable;
        }

        public override void Capture()
        {
            Console.Clear();

            //capture frames
            var capture = new VideoCapture(RecordFilePath);
            var image = new Mat();

            int i = 0;

            Console.WriteLine("Begin extracting frames from video file..");

            while (capture.IsOpened())
            {
                // Read next frame in video file
                capture.Read(image);

                if (image.Empty())
                    break;

                var fileName = $"frame{i}.txt";
                var filePath = Path.Combine(ExtractFilesFolderPath, fileName);

                var btmp = new Bitmap(image.ToMemoryStream());

                var asciiImage = BitmapToASCIIConverter.ConvertBitmapToASCII(btmp, _asciiTable);

                btmp.Dispose();

                File.WriteAllText(filePath, new string(asciiImage));

                Console.SetCursorPosition(0, 1);
                Console.Write($"{i}/{capture.FrameCount}");

                i++;
            }

            capture.Dispose();
            image.Dispose();

            Console.Clear();
        }
    }
}
