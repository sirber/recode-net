using System.Diagnostics;

namespace recode.net.lib
{
    class FfmpegHandler
    {
        private ProcessStartInfo startInfo = new ProcessStartInfo();
        private Process exeProcess = new Process();

        private string CmdLine;

        public void SetSource(string FileName)
        {

        }

        public void SetDestination(string FileName)
        {

        }

        public void SetVideo(string Codec, int Bitrate, ushort Quality)
        {

        }

        public void SetAudio(string Codec, int Bitrate)
        {

        }

        public void SetResize(int Height)
        {

        }

        public void Encode()
        {

        }

        public string GetStatus()
        {
            return "idle";
        }

        public void Stop()
        {

        }
    }
}
