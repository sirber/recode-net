/*
 * Created by SharpDevelop.
 * User: stber214
 * Date: 14/04/2015
 * Time: 11:24 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using BjSTools.MultiMedia;

namespace recode.net
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private string version = "2016-11-21 dev";
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
				row.CreateCells(lstFiles, "idle", Path.GetFileName(file), file);
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

            // Process events
            exeProcess.Exited += new EventHandler(myProcess_Exited);

            // Tooltips
            ToolTip toolTip1 = new ToolTip();
			toolTip1.ShowAlways = true;
			toolTip1.SetToolTip(txtABitrate, "Vorbis: 48-500, Opus: 8-256"); 
		}
		
		private void saveConfig() {
			// todo
		}
		
		private void loadConfig() {
			// todo
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			if (isEncoding) {
				return;
			}


            // Find a file to encode
            iEncoding = 0; // todo		
            string sFile = lstFiles.Rows[iEncoding].Cells[2].Value.ToString();

            // Get file information
            /* todo */

            // Config process
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.FileName = "ffmpeg.exe";
			
            // CmdLine (base)
            string sCmd = "-hwaccel cuvid -i \"" + sFile + "\" -b:v " + txtVBitrate.Text + "K" +
				" -b:a " + txtABitrate.Text + "K" +            	
				" -qmin 4 -g 8 "; // -threads 4
			
			// Codecs
			string sCodec = "libx264";
			switch (cboPreset.SelectedIndex) {
				case 0: // h264/he-aac
					if (chkHW.Checked) {
						sCodec = "h264_nvenc";
					}
					sCmd += " -c:v " + sCodec + " -c:a libfdk_aac -profile:v main -level 4.0 -profile:a aac_he_v2 -ac 2";
					break;
				case 1: // h265/he-aac
					sCodec = "libx265";
					if (chkHW.Checked) {
						sCodec = "hevc_nvenc";
					}
					sCmd += " -c:v " + sCodec + " -c:a libfdk_aac -profile:v main -level 4.0 -profile:a aac_he_v2 -ac 2";
					break;	
			}
			
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
			
			// Output
			sCmd += " -y \"" + sFile + ".out.mkv\"";

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
				exeProcess.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
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
		
		private void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine) {
			if (!String.IsNullOrEmpty(outLine.Data)) {
				setStatus(outLine.Data);	
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
		
		void Button2Click(object sender, EventArgs e)
		{
			if (isEncoding) {
				setStatus("canceled.");
				lstFiles.Rows[iEncoding].Cells[0].Value = "idle";
				exeProcess.Kill();	
			}			
		}
	}
}
