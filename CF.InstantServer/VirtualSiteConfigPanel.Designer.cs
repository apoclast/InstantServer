namespace CF.InstantServer
{
    partial class VirtualSiteConfigPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VirtualSiteConfigPanel));
            this.virtualSiteListBox_ = new System.Windows.Forms.ListBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.doAddSiteButton_ = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.doRemoveSiteButton_ = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // virtualSiteListBox_
            // 
            this.virtualSiteListBox_.Dock = System.Windows.Forms.DockStyle.Fill;
            this.virtualSiteListBox_.FormattingEnabled = true;
            this.virtualSiteListBox_.Location = new System.Drawing.Point(0, 25);
            this.virtualSiteListBox_.Name = "virtualSiteListBox_";
            this.virtualSiteListBox_.Size = new System.Drawing.Size(227, 210);
            this.virtualSiteListBox_.TabIndex = 0;
            this.virtualSiteListBox_.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.virtualSiteListBox_MouseDoubleClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.doAddSiteButton_,
            this.toolStripSeparator1,
            this.doRemoveSiteButton_});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(227, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // doAddSiteButton_
            // 
            this.doAddSiteButton_.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.doAddSiteButton_.Image = ((System.Drawing.Image)(resources.GetObject("doAddSiteButton_.Image")));
            this.doAddSiteButton_.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.doAddSiteButton_.Name = "doAddSiteButton_";
            this.doAddSiteButton_.Size = new System.Drawing.Size(51, 22);
            this.doAddSiteButton_.Text = "Add Site";
            this.doAddSiteButton_.Click += new System.EventHandler(this.doAddSiteButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // doRemoveSiteButton_
            // 
            this.doRemoveSiteButton_.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.doRemoveSiteButton_.Image = ((System.Drawing.Image)(resources.GetObject("doRemoveSiteButton_.Image")));
            this.doRemoveSiteButton_.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.doRemoveSiteButton_.Name = "doRemoveSiteButton_";
            this.doRemoveSiteButton_.Size = new System.Drawing.Size(68, 22);
            this.doRemoveSiteButton_.Text = "RemoveSite";
            this.doRemoveSiteButton_.Click += new System.EventHandler(this.doRemoveSiteButton__Click);
            // 
            // VirtualSiteConfigPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.virtualSiteListBox_);
            this.Controls.Add(this.toolStrip1);
            this.Name = "VirtualSiteConfigPanel";
            this.Size = new System.Drawing.Size(227, 235);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox virtualSiteListBox_;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton doAddSiteButton_;
        private System.Windows.Forms.ToolStripButton doRemoveSiteButton_;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

    }
}
