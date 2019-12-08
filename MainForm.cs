using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace recode.net
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private string version = "2019-12-03 dev";
		private bool isEncoding = false;
		private int iEncoding = 0;
		private ProcessStartInfo startInfo = new ProcessStartInfo();
		private Process exeProcess = new Process();

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
			cboVCodec.SelectedIndex = 0;
            cboACodec.SelectedIndex = 2; // Vorbis 2.0
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
            sCmd += " -map 0:v:0";
            string crf = "crf";
            switch (cboVCodec.SelectedIndex) {
				case 0: // h264
					sCmd += " -c:v libx264";
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
                case 2: // h264 (intel)
                    crf = "global_quality";
                    sCmd += " -c:v h264_qsv -preset slow";
                    break;
				case 4: // h265
					sCmd += " -c:v libx265";
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
                case 6: // h265 (intel)
                    crf = "global_quality";
                    sCmd += " -c:v hevc_qsv -preset slow";
                    break;
                case 8: // VP8
                    sCmd += " -c:v libvpx";
                    break;
                case 9: // VP9
                    sCmd += " -c:v libvpx-vp9 -row-mt 1";
                    break;
                case 1: // H264 (amd)
                    crf = "rc 0 -qp_i ";
                    sCmd += " -c:v h264_amf -quality 2";
                    break;
                case 5: // H265 (amd)
                    crf = "rc 0 -qp_i ";
                    sCmd += " -c:v hevc_amf -quality 0";
                    break;
                case 3: // H264 (nvidia)
                    crf = "cq";
                    sCmd += " -c:v h264_nvenc";
                    break;
                case 7: // H265 (nvidia)
                    crf = "cq";
                    sCmd += " -c:v hevc_nvenc"; // -cq
                    break;
            }

            if (Int32.Parse(txtVBitrate.Text) > 100)
            {
                sCmd += " -b:v " + txtVBitrate.Text + "K";
            }
            else
            {
                sCmd += " -" + crf + " " + txtVBitrate.Text;
            }

            // Audio
            sCmd += " -map 0:a:" + cboATrack.SelectedIndex + "? -b:a " + txtABitrate.Text + "K";
            switch(cboACodec.SelectedIndex)
            {
                case 0: 
                case 1:
                    sCmd += " -c:a libopus";
                    break;

                case 2:
                case 3:
                    sCmd += " -c:a libvorbis";
                    break;

                case 4:
                case 5:
                    sCmd += " -c:a aac";
                    break;
            }

            switch(cboACodec.SelectedIndex)
            {
                case 0:
                case 2:
                case 4:
                    sCmd += " -ac 2";
                    break;
            }

			// Resize
			switch (cboFResize.SelectedIndex) {
                case 1: // 1080
                    sCmd += " -vf scale=out_h:1080";
                    break;
                case 2: // 720
					sCmd += " -vf scale=out_h:720";
					break;
				case 3: // 480
					sCmd += " -vf scale=out_h:480";
					break;
				case 4: // 240
					sCmd += " -vf scale=out_h:240";
					break;				
			}

            // Subtitles (all)
            sCmd += " -map 0:s? -c:s copy";
			
			// Output
			sCmd += " -y \"" + sFile + ".out.mkv\"";
			
			// Debug!
			Clipboard.SetText(sCmd);

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

        private void cboACodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboACodec.SelectedIndex)
            {
                case 0: txtABitrate.Text = "32"; break;
                case 1: txtABitrate.Text = "128"; break;
                case 2: txtABitrate.Text = "64"; break;
                case 3: txtABitrate.Text = "256"; break;
                case 4: txtABitrate.Text = "128"; break;
                case 5: txtABitrate.Text = "320"; break;
            }
        }

        private void cboVCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboVCodec.SelectedIndex)
            {
                case 0:
                case 1:
                case 4:
                    txtVBitrate.Text = "4096"; 
                    break;
                case 2:
                case 3:
                case 5:
                    txtVBitrate.Text = "2048"; 
                    break;
            }
        }
    }
}
