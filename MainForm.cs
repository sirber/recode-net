using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;
using recode.net.lib;

namespace recode.net
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private string version = "2020-12-08 dev";
		private bool isEncoding = false;
		private int iEncoding = 0;
		private ProcessStartInfo startInfo = new ProcessStartInfo();
		private Process exeProcess = new Process();

        private FfmpegHandler ffmpeg = new FfmpegHandler();

        public MainForm()
		{
			InitializeComponent();
			
			// Enable dragdrop
			this.AllowDrop = true;
			this.DragEnter += new DragEventHandler(Form1_DragEnter);
			this.DragDrop += new DragEventHandler(Form1_DragDrop);
		}
		
		void Form1_DragEnter(object sender, DragEventArgs e) {
			if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
		}
		
		void Form1_DragDrop(object sender, DragEventArgs e) {
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			foreach (string file in files) {
                if (!File.Exists(file))
                {
                    continue;
                }
				DataGridViewRow row = (DataGridViewRow) lstFiles.RowTemplate.Clone();
				row.CreateCells(lstFiles, "ready", Path.GetFileName(file), file);
				lstFiles.Rows.Add(row);
			}
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
			setStatus("Recode.NET v/" + version);
			
			// UI Defaults
			cboVPreset.SelectedIndex = 0;
			cboATrack.SelectedIndex = 0;
			cboFResize.SelectedIndex = 0;

            // Tooltips
            ToolTip toolTip1 = new ToolTip();
			toolTip1.ShowAlways = true;
			toolTip1.SetToolTip(txtABitrate, "Opus: 32-320kbps, Vorbis: 64-320kbps, AAC: 128-320kbps");
		}
		
		void Button1Click(object sender, EventArgs e)
		{
            // TODO: fill FFmpegHandler

			encode();
        }
		
		private void encode() {
			if (isEncoding) {
				isEncoding = false;
				setStatus("canceled.");
				lstFiles.Rows[iEncoding].Cells[0].Value = "ready";
                if (!exeProcess.HasExited)
                {
				    exeProcess.Kill();
                }
                return;
			}

            // Find a file to encode
            string sFile = "";
            for (int icpt = 0; icpt < lstFiles.Rows.Count; icpt ++) {
            	if (lstFiles.Rows[icpt].Cells[0].Value.ToString() == "ready") {
            		iEncoding = icpt;
            		sFile = lstFiles.Rows[iEncoding].Cells[2].Value.ToString();	
            		break;
            	}            	
            }
            if (String.IsNullOrEmpty(sFile)) {
            	setStatus("Nothing to encode...?");
            	return;
            }

            // Get file information
            /* todo */


			
            // CmdLine (base)
            string sCmd = "-hide_banner -hwaccel dxva2 -i \"" + sFile + "\"";

            // Video
            sCmd += " -map 0:v:0 -c:v " + cboVCodec.Text;
            string crf = "-v:crf";
            switch (cboVCodec.Text) {
				case "libx264":
                case "libx265":
                    switch (cboVPreset.SelectedIndex)
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

            if (Int32.Parse(txtVBitrate.Text) > 100)
            {
                sCmd += " -b:v " + txtVBitrate.Text + "K";
            }
            else
            {
                sCmd += " " + crf + " " + txtVBitrate.Text;
            }

            // Audio
            sCmd += " -map 0:a:" + cboATrack.SelectedIndex + "? -b:a " + txtABitrate.Text + "K -ac 2 ";
            sCmd += " -c:a " + cboACodec.Text;

			// Resize
			switch (cboFResize.SelectedIndex) {
                // 0: no resize
                case 1: // 1080
                    sCmd += " -vf scale=1920:1080:force_original_aspect_ratio=decrease";
                    break;
                case 2: // 720
					sCmd += " -vf scale=1280:720:force_original_aspect_ratio=decrease";
					break;
				case 3: // 480
					sCmd += " -vf scale=-2:480:force_original_aspect_ratio=decrease";
					break;
				case 4: // 240
					sCmd += " -vf scale=-2:240:force_original_aspect_ratio=decrease";
					break;				
			}

            // Subtitles (all)
            sCmd += " -map 0:s? -c:s copy";
			
			// Output
			sCmd += " -y \"" + sFile + ".out.mkv\""; // FIXME: create better scheme
			
			// Debug!
            try
            {
			    Clipboard.SetText(sCmd);
            } catch(Exception ex)
            {

            }

            // GUI
            isEncoding = true;
            setStatus("encoding...");
            lstFiles.Rows[iEncoding].Cells[0].Value = "encoding";

            // *** Start
            try {
                // Config process
                startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.FileName = "ffmpeg.exe";
                startInfo.Arguments = sCmd;

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
            }
            catch (Exception ex)
	        {
	            setStatus("Error: " + ex.Message);
                lstFiles.Rows[iEncoding].Cells[0].Value = "error";
                isEncoding = false;
	        }   
		}
		
		private void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine) 
        {
			if (!String.IsNullOrEmpty(outLine.Data)) {
                Invoke(new SetStatusDelegate(setStatus), outLine.Data);
                // setStatus(outLine.Data);	
			}		    
		}
        
        private void myProcess_Exited(object sender, System.EventArgs e)
	    {
            if (isEncoding == false)
            {
                return;
            }

	        isEncoding = false;
            switch (exeProcess.ExitCode)
            {
                case 0: // success
                    setStatus("success!");
                    lstFiles.Rows[iEncoding].Cells[0].Value = "done";
                    
                    // next!
                    this.Invoke((MethodInvoker) delegate {encode();});
                    break;
                default: // error 
                    setStatus("error!");
                    lstFiles.Rows[iEncoding].Cells[0].Value = "error";
                    break;
            }	        
	    }
		
		void setStatus(string msg) {
			toolStripStatusLabel1.Text = msg;	
		}

        private delegate void SetStatusDelegate(string msg);

        void ResetToolStripMenuItemClick(object sender, EventArgs e)
		{
			for (int icpt = 0; icpt < lstFiles.Rows.Count; icpt ++) {
            	if (lstFiles.Rows[icpt].Cells[0].Value.ToString() == "error") {
					lstFiles.Rows[icpt].Cells[0].Value = "ready";
            	}            	
            }
		}
		void CboHWSelectedIndexChanged(object sender, EventArgs e)
		{
			cboFResize.Enabled = true;
		}

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isEncoding && !exeProcess.HasExited)
            {
                exeProcess.Kill();
            }
        }
        
    }
}
