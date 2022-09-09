using BadApple.Domain;
using NAudio.Wave;

namespace BadApple.Captures
{
    internal class AudioCapture : CaptureBase
    {
        public AudioCapture(string recordFilePath, string extractFilesFolderName) : base(recordFilePath, extractFilesFolderName)
        {
        }

        public override void Capture()
        {
            Console.WriteLine("Extracting audio...");

            using (MemoryStream audioStream = new MemoryStream())
            {
                using (MediaFoundationReader mediaReader = new MediaFoundationReader(RecordFilePath))
                {
                    if (mediaReader.CanRead)
                    {
                        mediaReader.Seek(0, SeekOrigin.Begin);

                        WaveFileWriter.WriteWavFileToStream(audioStream, mediaReader);

                        audioStream.Seek(0, SeekOrigin.Begin);
                    }
                }

                string path = Path.Combine(ExtractFilesFolderPath, "audio.wav");               

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    using (var binaryWriter = new BinaryWriter(fileStream))
                    {
                        binaryWriter.Write(audioStream.ToArray());
                    }
                }
            }

            Console.Clear();
        }
    }
}
