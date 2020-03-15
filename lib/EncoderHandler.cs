using System.Diagnostics;
using System;

namespace recode.net.lib
{
    class Job
    {
        // Input / Output
        public string FileSource { get; set; }
        public string FileDestination { get; set; }

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
    }

    class EncoderHandler
    {
        private ProcessStartInfo startInfo = new ProcessStartInfo();
        private Process exeProcess = new Process();
        private Job job;

        public EncoderHandler(Job job)
        {
            this.job = job;
        }

        public void Encode()
        {
            if (!exeProcess.HasExited)
            {
                throw new Exception("Already encoging?");
            }

            string sCmd = this.getCommandLine();
        }

        public string GetStatus()
        {
            return "idle";
        }

        public void Stop()
        {
            if (!exeProcess.HasExited)
            {
                exeProcess.Kill();
            }
        }

        private string getCommandLine()
        {
            return "";
        }
    }
}
