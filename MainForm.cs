using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;
using BjSTools.MultiMedia;

namespace recode.net
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private string version = "2019-11-18 dev";
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
				DataGridViewRow row = (DataGridViewRow) lstFiles.RowTemplate.Clone();
				row.CreateCells(lstFiles, "ready", Path.GetFileName(file), file);
				lstFiles.Rows.Add(row);
			}
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
			setStatus("Recode.NET v/" + version);
			
			// UI Defaults
			cboPreset.SelectedIndex = 0;
			cboVPreset.SelectedIndex = 0;
			cboATrack.SelectedIndex = 0;
			cboSTrack.SelectedIndex = 0;
			cboHW.SelectedIndex = 0;
			cboFResize.SelectedIndex = 0;

            // Process events
            exeProcess.Exited += new EventHandler(myProcess_Exited);

            // Tooltips
            ToolTip toolTip1 = new ToolTip();
			toolTip1.ShowAlways = true;
			toolTip1.SetToolTip(txtABitrate, "Opus: 16-320kbps");
			toolTip1.SetToolTip(txtVBitrate, "SD: 368kbps+, HD: 612kbps+");
		}
		
		private void saveConfig() {
			// todo
		}
		
		private void loadConfig() {
			// todo
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
				exeProcess.CloseMainWindow();	
				exeProcess.Kill();	
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

            // Config process
            exeProcess = new Process();
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.FileName = "ffmpeg/ffmpeg.exe";
			
            // CmdLine (base)
            string sCmd = "";
            if (cboHW.SelectedIndex == 1 || cboHW.SelectedIndex == 3) {
            	sCmd += "-hwaccel cuvid -c:v h264_cuvid"; // FIXME: add other encoers than nvidia
            }
            sCmd += " -i \"" + sFile + "\" -b:v " + txtVBitrate.Text + "K" +
				" -b:a " + txtABitrate.Text + "K" +            	
				" -qmin 4 -g 8 ";
			
			// Codecs
			string sCodec = "libx264";
			switch (cboPreset.SelectedIndex) {
				case 0: // h264/he-aac
					if (cboHW.SelectedIndex == 3) {
						sCodec = "h264_nvenc";
					}
					sCmd += " -c:v " + sCodec + " -profile:v high -level 4.1";
					break;
				case 1: // h265/he-aac
					sCodec = "libx265";
					if (cboHW.SelectedIndex == 3) {
						sCodec = "hevc_nvenc";
					}
					sCmd += " -c:v " + sCodec;
					break;	
			}

            sCmd += " -c:a libopus -ac 2";

            // Quality
            switch (cboVPreset.SelectedIndex) {
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
			
			// Resize
			switch (cboFResize.SelectedIndex) {
				case 1: // 720
					sCmd += " -vf scale=out_h:720";
					break;
				case 2: // 480
					sCmd += " -vf scale=out_h:480";
					break;
				case 3: // 240
					sCmd += " -vf scale=out_h:240";
					break;				
			}
			
			// Output
			sCmd += " -y \"" + sFile + ".out.mkv\"";
			
			// Debug!
			Clipboard.SetText(sCmd);

            // GUI
            isEncoding = true;
            setStatus("encoding...");
            lstFiles.Rows[iEncoding].Cells[0].Value = "encoding";

            // *** Start
            Clipboard.SetText(sCmd);
            try {
	            startInfo.Arguments = sCmd;					
	    		exeProcess.StartInfo = startInfo;
	    		exeProcess.EnableRaisingEvents = true;
	            exeProcess.Exited += new EventHandler(myProcess_Exited);   
				exeProcess.OutputDataReceived += new DataReceivedEventHandler(OutputHandler); // fixme: thread sync issue
    			exeProcess.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);	            
	            exeProcess.Start();   
				exeProcess.PriorityClass = ProcessPriorityClass.Idle;	            
	            exeProcess.BeginOutputReadLine();
	            exeProcess.BeginErrorReadLine();	
            }
            catch (Exception ex)
	        {
	            setStatus("Error: " + ex.Message);
	            return;
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
			if (cboHW.SelectedIndex > 0) {
				cboFResize.Enabled = false;
				cboFResize.SelectedIndex = 0;
			}
		}
	}
}
