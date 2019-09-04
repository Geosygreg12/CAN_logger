namespace CanLogger1
{
    partial class Form1
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
            this.DirText = new System.Windows.Forms.TextBox();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.TimesComboBox = new System.Windows.Forms.ComboBox();
            this.singleRadio = new System.Windows.Forms.RadioButton();
            this.tillEndRadio = new System.Windows.Forms.RadioButton();
            this.radioPanel = new System.Windows.Forms.Panel();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.radioPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // DirText
            // 
            this.DirText.Location = new System.Drawing.Point(29, 45);
            this.DirText.Name = "DirText";
            this.DirText.Size = new System.Drawing.Size(596, 22);
            this.DirText.TabIndex = 0;
            this.DirText.Text = "Enter the directory of the log file here";
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(655, 38);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(98, 36);
            this.BrowseButton.TabIndex = 1;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // TimesComboBox
            // 
            this.TimesComboBox.FormattingEnabled = true;
            this.TimesComboBox.Location = new System.Drawing.Point(29, 105);
            this.TimesComboBox.Name = "TimesComboBox";
            this.TimesComboBox.Size = new System.Drawing.Size(248, 24);
            this.TimesComboBox.TabIndex = 2;
            this.TimesComboBox.Text = "Select the time to start";
            // 
            // singleRadio
            // 
            this.singleRadio.AutoSize = true;
            this.singleRadio.Location = new System.Drawing.Point(3, 14);
            this.singleRadio.Name = "singleRadio";
            this.singleRadio.Size = new System.Drawing.Size(156, 21);
            this.singleRadio.TabIndex = 3;
            this.singleRadio.TabStop = true;
            this.singleRadio.Text = "Single Transmission";
            this.singleRadio.UseVisualStyleBackColor = true;
            // 
            // tillEndRadio
            // 
            this.tillEndRadio.AutoSize = true;
            this.tillEndRadio.Location = new System.Drawing.Point(167, 14);
            this.tillEndRadio.Name = "tillEndRadio";
            this.tillEndRadio.Size = new System.Drawing.Size(167, 21);
            this.tillEndRadio.TabIndex = 4;
            this.tillEndRadio.TabStop = true;
            this.tillEndRadio.Text = "Transmit till end of file";
            this.tillEndRadio.UseVisualStyleBackColor = true;
            // 
            // radioPanel
            // 
            this.radioPanel.Controls.Add(this.singleRadio);
            this.radioPanel.Controls.Add(this.tillEndRadio);
            this.radioPanel.Location = new System.Drawing.Point(339, 94);
            this.radioPanel.Name = "radioPanel";
            this.radioPanel.Size = new System.Drawing.Size(346, 51);
            this.radioPanel.TabIndex = 7;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(29, 207);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(123, 62);
            this.startButton.TabIndex = 8;
            this.startButton.Text = "START";
            this.startButton.UseVisualStyleBackColor = true;
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(210, 207);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(123, 62);
            this.stopButton.TabIndex = 9;
            this.stopButton.Text = "STOP";
            this.stopButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 686);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.radioPanel);
            this.Controls.Add(this.TimesComboBox);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.DirText);
            this.MaximumSize = new System.Drawing.Size(962, 733);
            this.MinimumSize = new System.Drawing.Size(800, 415);
            this.Name = "Form1";
            this.Text = "Form1";
            this.radioPanel.ResumeLayout(false);
            this.radioPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox DirText;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Object obj;
        private System.Windows.Forms.ComboBox TimesComboBox;
        private System.Windows.Forms.RadioButton singleRadio;
        private System.Windows.Forms.RadioButton tillEndRadio;
        private System.Windows.Forms.Panel radioPanel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
    }
}

