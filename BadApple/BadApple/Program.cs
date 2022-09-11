using BadApple.Captures;
using BadApple.Domain;
using BadApple.Players;

namespace BadApple
{
    internal class Program
    {
        //videos: Saul Goodman 3d.mp4,Bad Apple.mp4
        public readonly static string VideFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Bad Apple.mp4");

        public readonly static string VideFramesFolderName = "Frames";
        public readonly static string VideFramesFolderPath = Path.Combine(Directory.GetCurrentDirectory(), VideFramesFolderName);

        public readonly static string VideAudioFolderName = "Audio";
        public readonly static string VideAudioFolderPath = Path.Combine(Directory.GetCurrentDirectory(), VideAudioFolderName);

        //large console size may cause performance loss
        private const int _consoleWindowWidth = 140;
        private const int _consoleWindowHeight = 80;

        private static void Main(string[] args)
        {
            Console.SetWindowSize(_consoleWindowWidth, _consoleWindowHeight);
            Console.SetBufferSize(_consoleWindowWidth, _consoleWindowHeight);
            Console.CursorVisible = false;

            CaptureFiles();
            Play();
        }

        private static void CaptureFiles()
        {
            ICapturable videoFramesCapture = new VideoFramesCapture(VideFilePath, VideFramesFolderPath);
            ICapturable audioCaprute = new AudioCapture(VideFilePath, VideAudioFolderPath);

            videoFramesCapture.Capture();
            audioCaprute.Capture();
        }                       

        private static void Play()
        {
            IPlayable videoFramesPlayer = new VideoFramesPlayer(VideFramesFolderPath);
            IPlayable videoAuidoPlayer = new AudioPlayer(VideAudioFolderPath);

            Thread audioThread = new Thread(videoAuidoPlayer.Play) { IsBackground = true };
            Thread videoPlayerThread = new Thread(videoFramesPlayer.Play);

            videoPlayerThread.Start();
            audioThread.Start();
        }
    }
}