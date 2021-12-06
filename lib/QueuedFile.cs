namespace recode.net.lib
{
    public enum EncodingStatus
    {
        Ready,
        Encoding,
        Failed,
        Done,
    }

    class QueuedFile
    {
        public EncodingStatus Status { get; set; } = EncodingStatus.Ready;

        // Input / Output
        public string FileSource { get; }
        public string FileDestination { get; set;  }

        // Video
        public string VideoCodec { get; set; }
        public int VideoQuality { get; set; }
        public int VideoBitrate { get; set; }

        // Audio
        public string AudioCodec { get; set; }
        public int AudioTrack { get; set; }
        public int AudioBitrate { get; set; }

        // Container
        public string OutputContainer { get; set; }

        // Filtering
        public int FilteringResizeWidth { get; set; }

        public QueuedFile(string sourceFileName)
        {
            this.FileSource = sourceFileName;
        }

        public string GetDestinationFile(string sourceFile)
        {
            var outputFileName = $"{sourceFile}.out.{this.OutputContainer}"; // TODO: better job here!

            return outputFileName; 
        }

        public string GetDestinationFile()
        {
            var outputFileName = $"{this.FileSource}.out.{this.OutputContainer}"; // TODO: better job here!

            return outputFileName;
        }
    }
}
