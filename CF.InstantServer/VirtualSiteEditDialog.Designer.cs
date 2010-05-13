namespace CF.InstantServer
{
    partial class VirtualSiteEditDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.addressListBox_ = new System.Windows.Forms.TextBox();
            this.bindingGroup_ = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.siteNameGroup_ = new System.Windows.Forms.GroupBox();
            this.serverNameBox_ = new System.Windows.Forms.TextBox();
            this.rootDirGroup_ = new System.Windows.Forms.GroupBox();
            this.doSelectDocumentRootButton_ = new System.Windows.Forms.Button();
            this.documentRootBox_ = new System.Windows.Forms.TextBox();
            this.buttonPanel_ = new System.Windows.Forms.Panel();
            this.validateMessageLabel_ = new System.Windows.Forms.Label();
            this.cancelButton_ = new System.Windows.Forms.Button();
            this.okButton_ = new System.Windows.Forms.Button();
            this.bindingGroup_.SuspendLayout();
            this.siteNameGroup_.SuspendLayout();
            this.rootDirGroup_.SuspendLayout();
            this.buttonPanel_.SuspendLayout();
            this.SuspendLayout();
            // 
            // addressListBox_
            // 
            this.addressListBox_.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.addressListBox_.Location = new System.Drawing.Point(3, 47);
            this.addressListBox_.Multiline = true;
            this.addressListBox_.Name = "addressListBox_";
            this.addressListBox_.Size = new System.Drawing.Size(446, 100);
            this.addressListBox_.TabIndex = 0;
            // 
            // bindingGroup_
            // 
            this.bindingGroup_.Controls.Add(this.label1);
            this.bindingGroup_.Controls.Add(this.addressListBox_);
            this.bindingGroup_.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bindingGroup_.Location = new System.Drawing.Point(0, 103);
            this.bindingGroup_.Name = "bindingGroup_";
            this.bindingGroup_.Size = new System.Drawing.Size(452, 150);
            this.bindingGroup_.TabIndex = 1;
            this.bindingGroup_.TabStop = false;
            this.bindingGroup_.Text = "IP Binding";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(307, 26);
            this.label1.TabIndex = 1;
            this.label1.Text = "One entry perline. Use format like \"127.0.0.1:8080\" or \"*.8080\".\r\n Must end with " +
                "a port number";
            // 
            // siteNameGroup_
            // 
            this.siteNameGroup_.Controls.Add(this.serverNameBox_);
            this.siteNameGroup_.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.siteNameGroup_.Location = new System.Drawing.Point(0, 13);
            this.siteNameGroup_.Name = "siteNameGroup_";
            this.siteNameGroup_.Size = new System.Drawing.Size(452, 45);
            this.siteNameGroup_.TabIndex = 2;
            this.siteNameGroup_.TabStop = false;
            this.siteNameGroup_.Text = "Host Name";
            // 
            // serverNameBox_
            // 
            this.serverNameBox_.Location = new System.Drawing.Point(6, 19);
            this.serverNameBox_.Name = "serverNameBox_";
            this.serverNameBox_.Size = new System.Drawing.Size(440, 20);
            this.serverNameBox_.TabIndex = 0;
            // 
            // rootDirGroup_
            // 
            this.rootDirGroup_.Controls.Add(this.doSelectDocumentRootButton_);
            this.rootDirGroup_.Controls.Add(this.documentRootBox_);
            this.rootDirGroup_.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rootDirGroup_.Location = new System.Drawing.Point(0, 58);
            this.rootDirGroup_.Name = "rootDirGroup_";
            this.rootDirGroup_.Size = new System.Drawing.Size(452, 45);
            this.rootDirGroup_.TabIndex = 3;
            this.rootDirGroup_.TabStop = false;
            this.rootDirGroup_.Text = "Root Directory";
            // 
            // doSelectDocumentRootButton_
            // 
            this.doSelectDocumentRootButton_.Location = new System.Drawing.Point(371, 16);
            this.doSelectDocumentRootButton_.Name = "doSelectDocumentRootButton_";
            this.doSelectDocumentRootButton_.Size = new System.Drawing.Size(75, 23);
            this.doSelectDocumentRootButton_.TabIndex = 1;
            this.doSelectDocumentRootButton_.Text = "&Select";
            this.doSelectDocumentRootButton_.UseVisualStyleBackColor = true;
            this.doSelectDocumentRootButton_.Click += new System.EventHandler(this.doSelectDocumentRootButton_Click);
            // 
            // documentRootBox_
            // 
            this.documentRootBox_.Location = new System.Drawing.Point(6, 19);
            this.documentRootBox_.Name = "documentRootBox_";
            this.documentRootBox_.ReadOnly = true;
            this.documentRootBox_.Size = new System.Drawing.Size(359, 20);
            this.documentRootBox_.TabIndex = 0;
            // 
            // buttonPanel_
            // 
            this.buttonPanel_.Controls.Add(this.validateMessageLabel_);
            this.buttonPanel_.Controls.Add(this.cancelButton_);
            this.buttonPanel_.Controls.Add(this.okButton_);
            this.buttonPanel_.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonPanel_.Location = new System.Drawing.Point(0, 253);
            this.buttonPanel_.Name = "buttonPanel_";
            this.buttonPanel_.Size = new System.Drawing.Size(452, 50);
            this.buttonPanel_.TabIndex = 4;
            // 
            // validateMessageLabel_
            // 
            this.validateMessageLabel_.Dock = System.Windows.Forms.DockStyle.Top;
            this.validateMessageLabel_.ForeColor = System.Drawing.Color.Red;
            this.validateMessageLabel_.Location = new System.Drawing.Point(0, 0);
            this.validateMessageLabel_.Name = "validateMessageLabel_";
            this.validateMessageLabel_.Size = new System.Drawing.Size(452, 21);
            this.validateMessageLabel_.TabIndex = 1;
            this.validateMessageLabel_.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cancelButton_
            // 
            this.cancelButton_.Location = new System.Drawing.Point(229, 24);
            this.cancelButton_.Name = "cancelButton_";
            this.cancelButton_.Size = new System.Drawing.Size(75, 23);
            this.cancelButton_.TabIndex = 0;
            this.cancelButton_.Text = "&Cancel";
            this.cancelButton_.UseVisualStyleBackColor = true;
            this.cancelButton_.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton_
            // 
            this.okButton_.Location = new System.Drawing.Point(148, 24);
            this.okButton_.Name = "okButton_";
            this.okButton_.Size = new System.Drawing.Size(75, 23);
            this.okButton_.TabIndex = 0;
            this.okButton_.Text = "&OK";
            this.okButton_.UseVisualStyleBackColor = true;
            this.okButton_.Click += new System.EventHandler(this.okButton_Click);
            // 
            // VirtualSiteEditDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 303);
            this.Controls.Add(this.siteNameGroup_);
            this.Controls.Add(this.rootDirGroup_);
            this.Controls.Add(this.bindingGroup_);
            this.Controls.Add(this.buttonPanel_);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VirtualSiteEditDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "VirtualSiteEditDialog";
            this.bindingGroup_.ResumeLayout(false);
            this.bindingGroup_.PerformLayout();
            this.siteNameGroup_.ResumeLayout(false);
            this.siteNameGroup_.PerformLayout();
            this.rootDirGroup_.ResumeLayout(false);
            this.rootDirGroup_.PerformLayout();
            this.buttonPanel_.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox addressListBox_;
        private System.Windows.Forms.GroupBox bindingGroup_;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox siteNameGroup_;
        private System.Windows.Forms.TextBox serverNameBox_;
        private System.Windows.Forms.GroupBox rootDirGroup_;
        private System.Windows.Forms.TextBox documentRootBox_;
        private System.Windows.Forms.Panel buttonPanel_;
        private System.Windows.Forms.Button cancelButton_;
        private System.Windows.Forms.Button okButton_;
        private System.Windows.Forms.Label validateMessageLabel_;
        private System.Windows.Forms.Button doSelectDocumentRootButton_;
    }
}