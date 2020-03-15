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
        public string FileDestination { get; }

        // Video
        public string VideoCodec { get; set; }
        public int VideoQuality { get; set; }
        public int VideoBitrate { get; set; }

        // Audio
        public string AudioCodec { get; set; }
        public int AudioTrack { get; set; }
        public int AudioBitrate { get; set; }

        // Filtering
        public int FilteringResizeWidth { get; set; }

        public QueuedFile(string sourceFileName)
        {
            this.FileSource = sourceFileName;
            this.FileDestination = this.GetDestinationFile(sourceFileName);
        }

        private string GetDestinationFile(string SourceFile)
        {
            return $"{SourceFile}.out.mkv"; // TODO: better job here!
        }
    }
}
