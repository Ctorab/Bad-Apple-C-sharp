namespace BadApple.Domain
{
    internal interface IPlayable
    {
        string PlayFilePath { get; }

        public void Play();
    }
}
