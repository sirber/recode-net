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
		private string version = "2015-05-07 dev";
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
			string sCmd = "";
			
			if (isEncoding) {
				return;
			}
			
			// Find a file to encode
			iEncoding = 0; // todo		
			
			string sFile = lstFiles.Rows[iEncoding].Cells[2].Value.ToString();
			
			// Config process
			startInfo.CreateNoWindow = false;
			startInfo.WindowStyle = ProcessWindowStyle.Minimized;
			startInfo.UseShellExecute = false;
			if (Environment.Is64BitOperatingSystem) {
				startInfo.FileName = "bin64/ffmpeg.exe";
			}
			else {
				startInfo.FileName = "bin32/ffmpeg.exe";
			}			
			startInfo.WindowStyle = ProcessWindowStyle.Hidden;
			
			// CmdLine (base)
			sCmd = "-i \"" + sFile + "\" -b:v " + txtVBitrate.Text + "K" +
				" -b:a " + txtABitrate.Text + "K" +
				" -qmin 4 -rc_lookahead 0 -g 8 -" + 
				" -threads " + Environment.ProcessorCount.ToString();
			
			// Codecs
			switch (cboPreset.SelectedIndex) {
				case 0: // vp8/vorbis
					sCmd += " -c:v libvpx -c:a libvorbis";
					break;
				case 1: // vp9/opus
					sCmd += " -c:v libvpx-vp9 -c:a libopus";
					break;					
			}
			
			// Quality
			switch (cboVPreset.SelectedIndex) {
				case 0:
					sCmd += " -quality best";		
					break;
				case 1:
					sCmd += " -quality good";		
					break;
				case 2:
					sCmd += " -quality realtime";		
					break;					
			}
			
			// Output
			sCmd += " -y \"" + sFile + ".webm\"";
			
			// *** Start
			startInfo.Arguments = sCmd;					
	    	exeProcess.EnableRaisingEvents = true;
    		exeProcess.Exited += new EventHandler(myProcess_Exited);
    		exeProcess.StartInfo = startInfo;
			isEncoding = true;
    		exeProcess.Start();
    		exeProcess.PriorityClass = ProcessPriorityClass.Idle;

			setStatus("encoding...");
			lstFiles.Rows[iEncoding].Cells[0].Value = "encoding";
		}
	
		private void myProcess_Exited(object sender, System.EventArgs e)
	    {
	        isEncoding = false;
	        
	        switch (exeProcess.ExitCode) {
	        	case 0: // success
	        		setStatus("success!");	     
	        		lstFiles.Rows[iEncoding].Cells[0].Value = "done";
	        		break;
	        	case 1: // error 
	        		setStatus("error!");	        		
	        		lstFiles.Rows[iEncoding].Cells[0].Value = "error";
	        		break;	        
	        }
	        
	        
	        //Console.WriteLine("Exit time:    {0}\r\n" + "Exit code:    {1}\r\nElapsed time: {2}", exeProcess.ExitTime, exeProcess.ExitCode, exeProcess);
	        
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
