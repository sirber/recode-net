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
			this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cleanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cboPreset = new System.Windows.Forms.ComboBox();
			this.button1 = new System.Windows.Forms.Button();
			this.cboATrack = new System.Windows.Forms.ComboBox();
			this.cboSTrack = new System.Windows.Forms.ComboBox();
			this.cboVPreset = new System.Windows.Forms.ComboBox();
			this.txtVBitrate = new System.Windows.Forms.TextBox();
			this.txtABitrate = new System.Windows.Forms.TextBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.cboHW = new System.Windows.Forms.ComboBox();
			this.cboFResize = new System.Windows.Forms.ComboBox();
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
			this.lstFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
			this.Column1,
			this.Column2,
			this.Column3});
			this.lstFiles.ContextMenuStrip = this.contextMenuStrip1;
			this.lstFiles.Location = new System.Drawing.Point(12, 12);
			this.lstFiles.Name = "lstFiles";
			this.lstFiles.ReadOnly = true;
			this.lstFiles.RowHeadersVisible = false;
			this.lstFiles.Size = new System.Drawing.Size(762, 229);
			this.lstFiles.TabIndex = 0;
			// 
			// Column1
			// 
			this.Column1.HeaderText = "Statut";
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			// 
			// Column2
			// 
			this.Column2.HeaderText = "Filename";
			this.Column2.Name = "Column2";
			this.Column2.ReadOnly = true;
			// 
			// Column3
			// 
			this.Column3.HeaderText = "Path";
			this.Column3.Name = "Column3";
			this.Column3.ReadOnly = true;
			this.Column3.Width = 500;
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.resetToolStripMenuItem,
			this.cleanToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(105, 48);
			// 
			// resetToolStripMenuItem
			// 
			this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
			this.resetToolStripMenuItem.Size = new System.Drawing.Size(104, 22);
			this.resetToolStripMenuItem.Text = "Reset";
			this.resetToolStripMenuItem.Click += new System.EventHandler(this.ResetToolStripMenuItemClick);
			// 
			// cleanToolStripMenuItem
			// 
			this.cleanToolStripMenuItem.Name = "cleanToolStripMenuItem";
			this.cleanToolStripMenuItem.Size = new System.Drawing.Size(104, 22);
			this.cleanToolStripMenuItem.Text = "Clean";
			// 
			// cboPreset
			// 
			this.cboPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPreset.FormattingEnabled = true;
			this.cboPreset.Items.AddRange(new object[] {
			"mkv: h264/he-aac",
			"mkv: h265/he-aac"});
			this.cboPreset.Location = new System.Drawing.Point(12, 247);
			this.cboPreset.Name = "cboPreset";
			this.cboPreset.Size = new System.Drawing.Size(212, 21);
			this.cboPreset.TabIndex = 1;
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
			this.cboATrack.Location = new System.Drawing.Point(364, 247);
			this.cboATrack.Name = "cboATrack";
			this.cboATrack.Size = new System.Drawing.Size(121, 21);
			this.cboATrack.TabIndex = 3;
			// 
			// cboSTrack
			// 
			this.cboSTrack.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSTrack.FormattingEnabled = true;
			this.cboSTrack.Items.AddRange(new object[] {
			"no subtitles",
			"subtitle track 1",
			"subtitle track 2",
			"subtitle track 3",
			"subtitle track 4"});
			this.cboSTrack.Location = new System.Drawing.Point(364, 274);
			this.cboSTrack.Name = "cboSTrack";
			this.cboSTrack.Size = new System.Drawing.Size(121, 21);
			this.cboSTrack.TabIndex = 4;
			// 
			// cboVPreset
			// 
			this.cboVPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboVPreset.FormattingEnabled = true;
			this.cboVPreset.Items.AddRange(new object[] {
			"slow",
			"medium",
			"fast"});
			this.cboVPreset.Location = new System.Drawing.Point(230, 274);
			this.cboVPreset.Name = "cboVPreset";
			this.cboVPreset.Size = new System.Drawing.Size(128, 21);
			this.cboVPreset.TabIndex = 6;
			// 
			// txtVBitrate
			// 
			this.txtVBitrate.Location = new System.Drawing.Point(230, 248);
			this.txtVBitrate.Name = "txtVBitrate";
			this.txtVBitrate.Size = new System.Drawing.Size(128, 20);
			this.txtVBitrate.TabIndex = 7;
			this.txtVBitrate.Text = "368";
			// 
			// txtABitrate
			// 
			this.txtABitrate.Location = new System.Drawing.Point(491, 247);
			this.txtABitrate.Name = "txtABitrate";
			this.txtABitrate.Size = new System.Drawing.Size(121, 20);
			this.txtABitrate.TabIndex = 9;
			this.txtABitrate.Text = "64";
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.toolStripStatusLabel1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 306);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(786, 22);
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
			// cboHW
			// 
			this.cboHW.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboHW.FormattingEnabled = true;
			this.cboHW.Items.AddRange(new object[] {
			"Software Only",
			"Hardware Decode, Software encode",
			"Software Decode, Hardware encode",
			"Hardware Transcode"});
			this.cboHW.Location = new System.Drawing.Point(12, 274);
			this.cboHW.Name = "cboHW";
			this.cboHW.Size = new System.Drawing.Size(212, 21);
			this.cboHW.TabIndex = 11;
			this.cboHW.SelectedIndexChanged += new System.EventHandler(this.CboHWSelectedIndexChanged);
			// 
			// cboFResize
			// 
			this.cboFResize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboFResize.FormattingEnabled = true;
			this.cboFResize.Items.AddRange(new object[] {
			"no resize",
			"720p",
			"480p",
			"240p"});
			this.cboFResize.Location = new System.Drawing.Point(491, 274);
			this.cboFResize.Name = "cboFResize";
			this.cboFResize.Size = new System.Drawing.Size(121, 21);
			this.cboFResize.TabIndex = 12;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(786, 328);
			this.Controls.Add(this.cboFResize);
			this.Controls.Add(this.cboHW);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.txtABitrate);
			this.Controls.Add(this.txtVBitrate);
			this.Controls.Add(this.cboVPreset);
			this.Controls.Add(this.cboSTrack);
			this.Controls.Add(this.cboATrack);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.cboPreset);
			this.Controls.Add(this.lstFiles);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.Text = "Recode.NET";
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
		private System.Windows.Forms.TextBox txtABitrate;
		private System.Windows.Forms.TextBox txtVBitrate;
		private System.Windows.Forms.ComboBox cboVPreset;
		private System.Windows.Forms.ComboBox cboSTrack;
		private System.Windows.Forms.ComboBox cboATrack;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ComboBox cboPreset;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
		private System.Windows.Forms.DataGridView lstFiles;
		private System.Windows.Forms.ComboBox cboHW;
		private System.Windows.Forms.ComboBox cboFResize;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem cleanToolStripMenuItem;
    }
}
