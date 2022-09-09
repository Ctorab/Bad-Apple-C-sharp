namespace BadApple.Domain
{
    internal interface ICapturable
    {
        string RecordFilePath { get; }
        string ExtractFilesFolderPath { get; }

        public void Capture();
    }
}
