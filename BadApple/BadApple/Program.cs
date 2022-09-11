using BadApple.Captures;
using BadApple.Domain;
using BadApple.Players;

namespace BadApple
{
    internal class Program
    {
        #region Settings
        //videos: Saul Goodman 3d.mp4,Bad Apple.mp4
        public readonly static string VideFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Saul Goodman 3d.mp4");

        public readonly static string VideFramesFolderName = "Frames";
        public readonly static string VideFramesFolderPath = Path.Combine(Directory.GetCurrentDirectory(), VideFramesFolderName);

        public readonly static string VideAudioFolderName = "Audio";
        public readonly static string VideAudioFolderPath = Path.Combine(Directory.GetCurrentDirectory(), VideAudioFolderName);

        private const string ASCII_TABLE = " .:;+=xX$&";

        private static int _consoleWindowWidth = Console.LargestWindowWidth;
        private static int _consoleWindowHeight = Console.LargestWindowHeight;
        #endregion

        private static void Main(string[] args)
        {
            SetupConsole();

            CaptureFiles();
            Play();
        }

        private static void SetupConsole()
        {
            Console.SetWindowSize(_consoleWindowWidth, _consoleWindowHeight);
            Console.SetBufferSize(_consoleWindowWidth, _consoleWindowHeight);
            Console.CursorVisible = false;
        }

        private static void CaptureFiles()
        {
            ICapturable videoFramesCapture = new VideoFramesCapture(VideFilePath, VideFramesFolderPath, ASCII_TABLE);
            ICapturable audioCaprute = new AudioCapture(VideFilePath, VideAudioFolderPath);

            videoFramesCapture.Capture();
            audioCaprute.Capture();
        }                       

        private static void Play()
        {
            IPlayable videoFramesPlayer = new VideoFramesPlayer(VideFramesFolderPath);
            IPlayable videoAuidoPlayer = new AudioPlayer(VideAudioFolderPath);

            Thread audioThread = new(videoAuidoPlayer.Play) { IsBackground = true };
            Thread videoPlayerThread = new(videoFramesPlayer.Play);

            videoPlayerThread.Start();
            audioThread.Start();
        }
    }
}