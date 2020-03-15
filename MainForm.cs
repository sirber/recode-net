using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;
using recode.net.lib;
using System.Linq;
using System.ComponentModel;

namespace recode.net
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private string version = "2020-03-15 broken";

        Encoder encoder;
        BindingList<QueuedFile> queuedFiles = new BindingList<QueuedFile>();
        QueuedFile queuedFile;

        public MainForm()
		{
			InitializeComponent();

            // Bind list with grid
            lstFiles.DataSource = queuedFiles;
            foreach (DataGridViewColumn col in lstFiles.Columns)
            {
                col.Visible = false;
            }
            lstFiles.Columns["Status"].Visible = true;
            lstFiles.Columns["FileSource"].Visible = true;
            lstFiles.Columns["FileSource"].Width = 600;

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
                if (System.IO.File.Exists(file))
                {
                    QueuedFile queuedFile = new QueuedFile(file);
                    queuedFiles.Add(queuedFile);
                }
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
            // Already encoding?
            if (encoder != null && encoder.isEncoding)
            {
                encoder.Stop();
                return;
            }

            // Find a file to encode
            this.queuedFile = queuedFiles.FirstOrDefault(s => s.Status == EncodingStatus.Ready);
            if (this.queuedFile == null)
            {
                setStatus("Nothing to encode...?");
                return;
            }

            // Set encoding parameters
            queuedFile.Status = EncodingStatus.Encoding;
            queuedFile.VideoCodec = cboVCodec.Text;
            queuedFile.VideoQuality = cboVPreset.SelectedIndex;
            queuedFile.VideoBitrate = int.Parse(txtVBitrate.Text);
            queuedFile.AudioCodec = cboACodec.Text;
            queuedFile.AudioTrack = cboATrack.SelectedIndex;
            queuedFile.AudioBitrate = int.Parse(txtABitrate.Text);

            lstFiles.Refresh();

            // Start and listen to events!
            encoder = new Encoder(queuedFile);
            encoder.EncoderStopped += EncoderOnStop;
            // encoder.Start(); 
        }

        private void EncoderOnLog(string msg)
        {
            setStatus(msg);
        }

        private void EncoderOnStop(object sender, EncoderStoppedEventArgs e)
        {
            if (e.Status == EncodingStatus.Done)
            {
                // TODO: update 
            }
            else
            {

            }

            queuedFile.Status = e.Status;
            lstFiles.Refresh();
        }

	
		void setStatus(string msg) {
			toolStripStatusLabel1.Text = msg;	
		}

        void ResetToolStripMenuItemClick(object sender, EventArgs e)
		{
			for (int icpt = 0; icpt < lstFiles.Rows.Count; icpt ++) {
            	if (lstFiles.Rows[icpt].Cells[0].Value.ToString() == "error") {
					lstFiles.Rows[icpt].Cells[0].Value = "ready";
            	}            	
            }
		}

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (encoder != null && encoder.isEncoding)
            {
                encoder.Stop();
            }
        }
    }
}
