/*
 * Created by SharpDevelop.
 * User: stber214
 * Date: 14/04/2015
 * Time: 11:24 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace recode.net
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.lstFiles = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.resetfailed = new System.Windows.Forms.ToolStripMenuItem();
            this.cleanList = new System.Windows.Forms.ToolStripMenuItem();
            this.removeSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.cboVCodec = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.cboATrack = new System.Windows.Forms.ComboBox();
            this.cboVPreset = new System.Windows.Forms.ComboBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.cboFResize = new System.Windows.Forms.ComboBox();
            this.cboACodec = new System.Windows.Forms.ComboBox();
            this.txtVBitrate = new System.Windows.Forms.ComboBox();
            this.txtABitrate = new System.Windows.Forms.ComboBox();
            this.cboOutput = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.lstFiles)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstFiles
            // 
            this.lstFiles.AllowUserToAddRows = false;
            this.lstFiles.AllowUserToDeleteRows = false;
            this.lstFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lstFiles.ContextMenuStrip = this.contextMenuStrip1;
            this.lstFiles.Location = new System.Drawing.Point(12, 12);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.ReadOnly = true;
            this.lstFiles.RowHeadersVisible = false;
            this.lstFiles.Size = new System.Drawing.Size(762, 229);
            this.lstFiles.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetfailed,
            this.cleanList,
            this.removeSelected});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(164, 70);
            // 
            // resetfailed
            // 
            this.resetfailed.Name = "resetfailed";
            this.resetfailed.Size = new System.Drawing.Size(163, 22);
            this.resetfailed.Text = "Reset";
            this.resetfailed.Click += new System.EventHandler(this.ResetClick);
            // 
            // cleanList
            // 
            this.cleanList.Name = "cleanList";
            this.cleanList.Size = new System.Drawing.Size(163, 22);
            this.cleanList.Text = "Clean";
            this.cleanList.Click += new System.EventHandler(this.CleanClick);
            // 
            // removeSelected
            // 
            this.removeSelected.Name = "removeSelected";
            this.removeSelected.Size = new System.Drawing.Size(163, 22);
            this.removeSelected.Text = "Remove selected";
            // 
            // cboVCodec
            // 
            this.cboVCodec.FormattingEnabled = true;
            this.cboVCodec.Items.AddRange(new object[] {
            "libx264",
            "libx265",
            "h264_qsv",
            "h264_nvenc",
            "h264_amf",
            "hevc_qsv",
            "hevc_nvenc",
            "hevc_amf",
            "libvpx",
            "libvpx-vp9"});
            this.cboVCodec.Location = new System.Drawing.Point(12, 247);
            this.cboVCodec.Name = "cboVCodec";
            this.cboVCodec.Size = new System.Drawing.Size(212, 21);
            this.cboVCodec.TabIndex = 1;
            this.cboVCodec.Text = "libx264";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(699, 248);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 47);
            this.button1.TabIndex = 2;
            this.button1.Text = "Start / Stop";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1Click);
            // 
            // cboATrack
            // 
            this.cboATrack.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboATrack.FormattingEnabled = true;
            this.cboATrack.Items.AddRange(new object[] {
            "audio track 1",
            "audio track 2",
            "audio track 3",
            "audio track 4"});
            this.cboATrack.Location = new System.Drawing.Point(364, 274);
            this.cboATrack.Name = "cboATrack";
            this.cboATrack.Size = new System.Drawing.Size(121, 21);
            this.cboATrack.TabIndex = 3;
            // 
            // cboVPreset
            // 
            this.cboVPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVPreset.FormattingEnabled = true;
            this.cboVPreset.Items.AddRange(new object[] {
            "slow",
            "medium",
            "fast"});
            this.cboVPreset.Location = new System.Drawing.Point(364, 248);
            this.cboVPreset.Name = "cboVPreset";
            this.cboVPreset.Size = new System.Drawing.Size(121, 21);
            this.cboVPreset.TabIndex = 6;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 308);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(792, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(38, 17);
            this.toolStripStatusLabel1.Text = "status";
            // 
            // cboFResize
            // 
            this.cboFResize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFResize.FormattingEnabled = true;
            this.cboFResize.Items.AddRange(new object[] {
            "no resize",
            "1080p",
            "720p",
            "480p",
            "240p"});
            this.cboFResize.Location = new System.Drawing.Point(491, 248);
            this.cboFResize.Name = "cboFResize";
            this.cboFResize.Size = new System.Drawing.Size(121, 21);
            this.cboFResize.TabIndex = 12;
            // 
            // cboACodec
            // 
            this.cboACodec.FormattingEnabled = true;
            this.cboACodec.Items.AddRange(new object[] {
            "libopus",
            "libvorbis",
            "aac"});
            this.cboACodec.Location = new System.Drawing.Point(12, 274);
            this.cboACodec.Name = "cboACodec";
            this.cboACodec.Size = new System.Drawing.Size(212, 21);
            this.cboACodec.TabIndex = 13;
            this.cboACodec.Text = "libopus";
            // 
            // txtVBitrate
            // 
            this.txtVBitrate.FormattingEnabled = true;
            this.txtVBitrate.Items.AddRange(new object[] {
            "18",
            "20",
            "22",
            "24",
            "30",
            "1024",
            "2048",
            "4096"});
            this.txtVBitrate.Location = new System.Drawing.Point(230, 247);
            this.txtVBitrate.Name = "txtVBitrate";
            this.txtVBitrate.Size = new System.Drawing.Size(128, 21);
            this.txtVBitrate.TabIndex = 14;
            this.txtVBitrate.Text = "4096";
            // 
            // txtABitrate
            // 
            this.txtABitrate.FormattingEnabled = true;
            this.txtABitrate.Items.AddRange(new object[] {
            "32",
            "64",
            "96",
            "128",
            "192",
            "256",
            "320"});
            this.txtABitrate.Location = new System.Drawing.Point(231, 274);
            this.txtABitrate.Name = "txtABitrate";
            this.txtABitrate.Size = new System.Drawing.Size(127, 21);
            this.txtABitrate.TabIndex = 15;
            this.txtABitrate.Text = "32";
            // 
            // cboOutput
            // 
            this.cboOutput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOutput.FormattingEnabled = true;
            this.cboOutput.Items.AddRange(new object[] {
            "mkv",
            "mp4",
            "webm"});
            this.cboOutput.Location = new System.Drawing.Point(491, 274);
            this.cboOutput.Name = "cboOutput";
            this.cboOutput.Size = new System.Drawing.Size(121, 21);
            this.cboOutput.TabIndex = 16;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 330);
            this.Controls.Add(this.cboOutput);
            this.Controls.Add(this.txtABitrate);
            this.Controls.Add(this.txtVBitrate);
            this.Controls.Add(this.cboACodec);
            this.Controls.Add(this.cboFResize);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.cboVPreset);
            this.Controls.Add(this.cboATrack);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cboVCodec);
            this.Controls.Add(this.lstFiles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Recode.NET";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.lstFiles)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ComboBox cboVPreset;
		private System.Windows.Forms.ComboBox cboATrack;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ComboBox cboVCodec;
		private System.Windows.Forms.ComboBox cboFResize;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem resetfailed;
		private System.Windows.Forms.ToolStripMenuItem cleanList;
        private System.Windows.Forms.ComboBox cboACodec;
        private System.Windows.Forms.ComboBox txtVBitrate;
        private System.Windows.Forms.ComboBox txtABitrate;
        private System.Windows.Forms.DataGridView lstFiles;
        private System.Windows.Forms.ToolStripMenuItem removeSelected;
        private System.Windows.Forms.ComboBox cboOutput;
    }
}
