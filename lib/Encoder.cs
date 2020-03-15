using System.Diagnostics;
using System;
using System.Windows.Forms;

namespace recode.net.lib
{
    public class EncoderStoppedEventArgs : EventArgs
    {
        public EncodingStatus Status { get; set; }
    }

    public class EncoderMessageEventArgs : EventArgs
    {
        public string Message { get; set; }
    }

    class Encoder
    {
        private ProcessStartInfo startInfo;
        private Process exeProcess;
        private QueuedFile queuedFile;

        public event EventHandler<EncoderStoppedEventArgs> EncoderStopped;
        public event EventHandler<EncoderMessageEventArgs> EncoderMessage;

        public bool isEncoding { get; set; } = false; // FIXME: should be private. Form can have it's own variable.

        public Encoder(QueuedFile queuedFile)
        {
            this.queuedFile = queuedFile;

            exeProcess = new Process();

            startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.FileName = "ffmpeg.exe";
        }

        public void Start()
        {
            if (isEncoding)
            {
                throw new Exception("Already encoding?");
            }

            isEncoding = true;
            startInfo.Arguments = this.getCommandLine();

            try
            {
                exeProcess = new Process();
                exeProcess.StartInfo = startInfo;
                exeProcess.EnableRaisingEvents = true;
                exeProcess.Exited += new EventHandler(myProcess_Exited);
                exeProcess.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
                exeProcess.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);

                bool started = exeProcess.Start();
                if (started)
                {
                    exeProcess.PriorityClass = ProcessPriorityClass.Idle;
                    exeProcess.BeginOutputReadLine();
                    exeProcess.BeginErrorReadLine();
                } 
                else
                {
                    throw new Exception("Encoder did not start!");
                }
            } 
            catch (Exception ex)
            {
                isEncoding = false;
                queuedFile.Status = EncodingStatus.Failed;

                RaiseMessage(ex.Message);
                Stop();
            }
        }
        private void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (String.IsNullOrEmpty(outLine.Data))
            {
                return;
            }

            // Raise stop event
            RaiseMessage(outLine.Data);
        }

        private void RaiseMessage(string message)
        {
            EncoderMessageEventArgs args = new EncoderMessageEventArgs
            {
                Message = message,
            };
            OnMessage(args);
        }

        private void myProcess_Exited(object sender, System.EventArgs e)
        {
            isEncoding = false;
            if (exeProcess.ExitCode == 0) // success
            {
                queuedFile.Status = EncodingStatus.Done;
            } 
            else
            {
                queuedFile.Status = EncodingStatus.Failed;
            }

            Stop();
        }

        protected virtual void OnEncoderStop(EncoderStoppedEventArgs e)
        {
            EventHandler<EncoderStoppedEventArgs> handler = EncoderStopped;
            handler?.Invoke(this, e);
        }

        protected virtual void OnMessage(EncoderMessageEventArgs e)
        {
            EventHandler<EncoderMessageEventArgs> handler = EncoderMessage;
            handler?.Invoke(this, e);
        }

        public void Stop()
        {
            if (!exeProcess.HasExited)
            {
                isEncoding = false;
                queuedFile.Status = EncodingStatus.Failed;
                exeProcess.Kill();
            }

            // Raise stop event
            EncoderStoppedEventArgs args = new EncoderStoppedEventArgs {
                Status = queuedFile.Status,  
            };
            OnEncoderStop(args);
        }

        private string getCommandLine()
        {
            string crf = "-v:crf";
            string sCmd = $"-hide_banner -hwaccel dxva2 -i \"{queuedFile.FileSource}\""; // TODO: use an array, join with " "

            // Video
            sCmd += $" -map 0:v:0 -c:v {queuedFile.VideoCodec}";
            switch (queuedFile.VideoCodec)
            {
                case "libx264":
                case "libx265":
                    switch (queuedFile.VideoQuality)
                    {
                        case 0: // best
                            sCmd += " -preset slow";
                            break;
                        case 1: // good
                            sCmd += " -preset medium";
                            break;
                        case 2: // fast
                            sCmd += " -preset fast";
                            break;
                    }
                    break;
                case "h264_qsv": // h264 (intel)
                    crf = "-q:v";
                    sCmd += " -preset slow";
                    break;
                case "hevc_qsv": // h265 (intel)
                    crf = "-q:v";
                    sCmd += " -preset slow";
                    break;
                case "libvpx": // VP8
                    break;
                case "libvpx-vp9": // VP9
                    crf = "b:v 0 -v:crf";
                    sCmd += " -row-mt 1";
                    break;
                case "h264_amf": // H264 (amd)
                    crf = "-rc 0 -qp_i ";
                    sCmd += " -quality 2";
                    break;
                case "hevc_amf": // H265 (amd)
                    crf = "-rc 0 -qp_i ";
                    sCmd += " -quality 0";
                    break;
                case "h264_nvenc": // H264 (nvidia)
                case "hevc_nvenc": // H265 (nvidia)
                    crf = "-v:cq";
                    break;
            }

            if (queuedFile.VideoBitrate > 100)
            {
                sCmd += $" -b:v {queuedFile.VideoBitrate}K";
            }
            else
            {
                sCmd += $" {crf} {queuedFile.VideoBitrate}";
            }

            // Audio
            sCmd += $" -map 0:a:{queuedFile.AudioTrack}? -b:a {queuedFile.AudioBitrate}K -ac 2 ";
            sCmd += $" -c:a {queuedFile.AudioCodec}";

            // Resize
            switch (queuedFile.FilteringResizeWidth)
            {
                case 1080:
                    sCmd += " -vf scale=1920:1080:force_original_aspect_ratio=decrease";
                    break;
                case 720:
                    sCmd += " -vf scale=1280:720:force_original_aspect_ratio=decrease";
                    break;
                case 480:
                    sCmd += " -vf scale=-2:480:force_original_aspect_ratio=decrease";
                    break;
                case 240:
                    sCmd += " -vf scale=-2:240:force_original_aspect_ratio=decrease";
                    break;
            }

            // Subtitles (all)
            sCmd += " -map 0:s? -c:s copy";

            // Output
            sCmd += $" -y \"{queuedFile.FileDestination}\"";

            // Debug!
            try
            {
                Clipboard.SetText(sCmd);
            }
            catch (Exception ex)
            {

            }

            return sCmd;
        }
    }
}
