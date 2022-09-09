using BadApple.Domain;
using OpenCvSharp;

namespace BadApple.Captures
{
    internal class VideoFramesCapture : CaptureBase
    {
        public VideoFramesCapture(string recordFilePath, string extractFilesFolderPath) : base(recordFilePath, extractFilesFolderPath) { }

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

                // Save image to disk.
                var fileName = $"frame{i}.png";
                Cv2.ImWrite($@"{ExtractFilesFolderPath}\{fileName}", image);

                Console.SetCursorPosition(0, 1);
                Console.Write($"{i}/{capture.FrameCount}");

                i++;
            }

            Console.Clear();
        }
    }
}
