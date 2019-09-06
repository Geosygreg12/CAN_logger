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
            this.singleRadio = new System.Windows.Forms.RadioButton();
            this.tillEndRadio = new System.Windows.Forms.RadioButton();
            this.radioPanel = new System.Windows.Forms.Panel();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.timeText = new System.Windows.Forms.TextBox();
            this.timeSearchButton = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressLabel = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.radioPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // DirText
            // 
            this.DirText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DirText.Location = new System.Drawing.Point(29, 45);
            this.DirText.Name = "DirText";
            this.DirText.Size = new System.Drawing.Size(627, 27);
            this.DirText.TabIndex = 0;
            this.DirText.Text = "Enter the directory of the log file here";
            this.DirText.TextChanged += new System.EventHandler(this.DirText_TextChanged);
            this.DirText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DirText_KeyDown);
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(682, 36);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(98, 39);
            this.BrowseButton.TabIndex = 1;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
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
            this.tillEndRadio.Checked = true;
            this.tillEndRadio.Location = new System.Drawing.Point(3, 72);
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
            this.radioPanel.Location = new System.Drawing.Point(564, 123);
            this.radioPanel.Name = "radioPanel";
            this.radioPanel.Size = new System.Drawing.Size(196, 129);
            this.radioPanel.TabIndex = 7;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(33, 337);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(123, 62);
            this.startButton.TabIndex = 8;
            this.startButton.Text = "START";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(214, 337);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(123, 62);
            this.stopButton.TabIndex = 9;
            this.stopButton.Text = "STOP";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // timeText
            // 
            this.timeText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeText.Location = new System.Drawing.Point(29, 166);
            this.timeText.Name = "timeText";
            this.timeText.Size = new System.Drawing.Size(284, 27);
            this.timeText.TabIndex = 10;
            this.timeText.Text = "Enter the time in seconds";
            this.timeText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TimeText_KeyDown);
            // 
            // timeSearchButton
            // 
            this.timeSearchButton.Location = new System.Drawing.Point(319, 162);
            this.timeSearchButton.Name = "timeSearchButton";
            this.timeSearchButton.Size = new System.Drawing.Size(75, 37);
            this.timeSearchButton.TabIndex = 11;
            this.timeSearchButton.Text = "Search";
            this.timeSearchButton.UseVisualStyleBackColor = true;
            this.timeSearchButton.Click += new System.EventHandler(this.TimeSearchButton_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker1_RunWorkerCompleted);
            // 
            // progressLabel
            // 
            this.progressLabel.AutoSize = true;
            this.progressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressLabel.Location = new System.Drawing.Point(144, 232);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(143, 20);
            this.progressLabel.TabIndex = 12;
            this.progressLabel.Text = "Transmitting... 0%";
            this.progressLabel.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(33, 263);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(361, 23);
            this.progressBar.TabIndex = 13;
            this.progressBar.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 686);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.timeSearchButton);
            this.Controls.Add(this.timeText);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.radioPanel);
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
        private System.Windows.Forms.RadioButton singleRadio;
        private System.Windows.Forms.RadioButton tillEndRadio;
        private System.Windows.Forms.Panel radioPanel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Collections.Generic.List<string> listOfLoggedValues = new System.Collections.Generic.List<string>();
        private System.Windows.Forms.TextBox timeText;
        private System.Windows.Forms.Button timeSearchButton;
        private System.String startTime = string.Empty;
        private System.Text.RegularExpressions.Regex regexTime = new System.Text.RegularExpressions.Regex(@"^\d.\d\d\d\d\d\d");
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Boolean isRead;
        private System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.ProgressBar progressBar;
        private DataParameters data;
        private const System.Int32 TIME_INDEX = 0;
        private const System.Int32 CHANNEL_ID_INDEX = 2;
        private const System.Int32 LENGTH_BIT_INDEX = 5;
        private const System.Int32 MESSAGE_INDEX = 6;

        private struct DataParameters
        {
            public int Message_Length;
            public float Message_Time;
            public string CAN_Message;
            public string Channel_ID;
        }
    }
}

