using BadApple.Domain;
using NAudio.Wave;

namespace BadApple.Players
{
    internal class AudioPlayer : PlayerBase
    {
        public AudioPlayer(string playFilePath) : base(playFilePath){}

        public override void Play()
        {
            var audioFiles = Directory.GetFiles(PlayFilePath, "*.wav");

            if (audioFiles.Length == 0)
                throw new Exception("There is no audio files in directory");

            if (audioFiles.Length > 1)
                throw new Exception("There are more then 1 audio file in directory");

            var filePath = Path.Combine(PlayFilePath, Path.GetFileName(audioFiles[0]));

            using (var audioFile = new AudioFileReader(filePath))
            {
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();

                    while (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        Thread.Sleep(1000);
                    }
                }
            }            
        }
    }
}
