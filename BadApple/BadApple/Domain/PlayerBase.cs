namespace BadApple.Domain
{
    internal abstract class PlayerBase : IPlayable
    {
        public string PlayFilePath { get; } = string.Empty;

        public PlayerBase(string playFilePath)
        {
            if (!Directory.Exists(playFilePath))
                throw new ArgumentException("There is no file to play in this directory", nameof(playFilePath));

            PlayFilePath = playFilePath;
        }

        public abstract void Play();
    }
}
