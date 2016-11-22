namespace DesktopStreamerDemo
{
    partial class MainWindow
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._clientsListBox = new System.Windows.Forms.ListBox();
            this._urlLabel = new System.Windows.Forms.Label();
            this._startStopButton = new System.Windows.Forms.Button();
            this._clientsLabel = new System.Windows.Forms.Label();
            this._screensListView = new System.Windows.Forms.ListView();
            this._previewUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this._previewImageList = new System.Windows.Forms.ImageList(this.components);
            this._screensLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _clientsListBox
            // 
            this._clientsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this._clientsListBox.FormattingEnabled = true;
            this._clientsListBox.Location = new System.Drawing.Point(12, 63);
            this._clientsListBox.Name = "_clientsListBox";
            this._clientsListBox.Size = new System.Drawing.Size(191, 485);
            this._clientsListBox.TabIndex = 0;
            // 
            // _urlLabel
            // 
            this._urlLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._urlLabel.Location = new System.Drawing.Point(12, 12);
            this._urlLabel.Name = "_urlLabel";
            this._urlLabel.Size = new System.Drawing.Size(595, 23);
            this._urlLabel.TabIndex = 1;
            this._urlLabel.Text = "Server not running.";
            this._urlLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _startStopButton
            // 
            this._startStopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._startStopButton.Location = new System.Drawing.Point(613, 12);
            this._startStopButton.Name = "_startStopButton";
            this._startStopButton.Size = new System.Drawing.Size(89, 23);
            this._startStopButton.TabIndex = 2;
            this._startStopButton.Text = "Start Server";
            this._startStopButton.UseVisualStyleBackColor = true;
            this._startStopButton.Click += new System.EventHandler(this._startStopButton_Click);
            // 
            // _clientsLabel
            // 
            this._clientsLabel.AutoSize = true;
            this._clientsLabel.Location = new System.Drawing.Point(12, 47);
            this._clientsLabel.Name = "_clientsLabel";
            this._clientsLabel.Size = new System.Drawing.Size(41, 13);
            this._clientsLabel.TabIndex = 3;
            this._clientsLabel.Text = "Clients:";
            // 
            // _screensListView
            // 
            this._screensListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._screensListView.LargeImageList = this._previewImageList;
            this._screensListView.Location = new System.Drawing.Point(209, 63);
            this._screensListView.Name = "_screensListView";
            this._screensListView.Size = new System.Drawing.Size(493, 484);
            this._screensListView.TabIndex = 4;
            this._screensListView.UseCompatibleStateImageBehavior = false;
            // 
            // _previewUpdateTimer
            // 
            this._previewUpdateTimer.Enabled = true;
            this._previewUpdateTimer.Interval = 500;
            this._previewUpdateTimer.Tick += new System.EventHandler(this._previewUpdateTimer_Tick);
            // 
            // _previewImageList
            // 
            this._previewImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
            this._previewImageList.ImageSize = new System.Drawing.Size(160, 160);
            this._previewImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // _screensLabel
            // 
            this._screensLabel.AutoSize = true;
            this._screensLabel.Location = new System.Drawing.Point(206, 47);
            this._screensLabel.Name = "_screensLabel";
            this._screensLabel.Size = new System.Drawing.Size(49, 13);
            this._screensLabel.TabIndex = 5;
            this._screensLabel.Text = "Screens:";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 559);
            this.Controls.Add(this._screensLabel);
            this.Controls.Add(this._screensListView);
            this.Controls.Add(this._clientsLabel);
            this.Controls.Add(this._startStopButton);
            this.Controls.Add(this._urlLabel);
            this.Controls.Add(this._clientsListBox);
            this.Name = "MainWindow";
            this.Text = "DesktopStreamerDemo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox _clientsListBox;
        private System.Windows.Forms.Label _urlLabel;
        private System.Windows.Forms.Button _startStopButton;
        private System.Windows.Forms.Label _clientsLabel;
        private System.Windows.Forms.ListView _screensListView;
        private System.Windows.Forms.Timer _previewUpdateTimer;
        private System.Windows.Forms.ImageList _previewImageList;
        private System.Windows.Forms.Label _screensLabel;
    }
}

