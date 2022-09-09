namespace BadApple.Domain
{
    internal abstract class CaptureBase : ICapturable
    {
        public string RecordFilePath { get; private set; } = string.Empty;

        public string ExtractFilesFolderPath { get; private set; } = string.Empty;

        public CaptureBase(string recordFilePath, string extractFilesFolderPath)
        {
            #region Check The Input Data For Correctness
            if (string.IsNullOrEmpty(recordFilePath))
                throw new ArgumentNullException("record file path in null or whitespace", nameof(recordFilePath));

            if (string.IsNullOrEmpty(extractFilesFolderPath))
                throw new ArgumentNullException("extract files folder path in null or whitespace", nameof(extractFilesFolderPath));
            #endregion

            #region Check Directory
            if (Directory.Exists(extractFilesFolderPath))
                Directory.Delete(extractFilesFolderPath, true);

            Directory.CreateDirectory(extractFilesFolderPath);
            #endregion

            RecordFilePath = recordFilePath;
            ExtractFilesFolderPath = extractFilesFolderPath;
        }

        public abstract void Capture();
    }
}
