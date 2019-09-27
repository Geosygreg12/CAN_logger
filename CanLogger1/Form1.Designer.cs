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
            this.progressLabel = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.PauseButton = new System.Windows.Forms.Button();
            this.timeLabel = new System.Windows.Forms.Label();
            this.timeUpdateText = new System.Windows.Forms.TextBox();
            this.InterfaceComboBox = new System.Windows.Forms.ComboBox();
            this.interfaceLabel = new System.Windows.Forms.Label();
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
            this.timeText.Text = "Enter the time in milliseconds";
            this.timeText.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TimeText_MouseClick);
            this.timeText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TimeText_KeyDown);
            // 
            // progressLabel
            // 
            this.progressLabel.AutoSize = true;
            this.progressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressLabel.Location = new System.Drawing.Point(25, 223);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(136, 20);
            this.progressLabel.TabIndex = 12;
            this.progressLabel.Text = "No Transmission";
            this.progressLabel.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(29, 249);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(280, 23);
            this.progressBar.TabIndex = 13;
            this.progressBar.Visible = false;
            // 
            // PauseButton
            // 
            this.PauseButton.Location = new System.Drawing.Point(315, 229);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(93, 43);
            this.PauseButton.TabIndex = 14;
            this.PauseButton.Text = "Pause";
            this.PauseButton.UseVisualStyleBackColor = true;
            this.PauseButton.Visible = false;
            this.PauseButton.Click += new System.EventHandler(this.PauseButton_Click);
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(29, 288);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(43, 17);
            this.timeLabel.TabIndex = 15;
            this.timeLabel.Text = "Time:";
            this.timeLabel.Visible = false;
            // 
            // timeUpdateText
            // 
            this.timeUpdateText.Location = new System.Drawing.Point(78, 283);
            this.timeUpdateText.Name = "timeUpdateText";
            this.timeUpdateText.Size = new System.Drawing.Size(132, 22);
            this.timeUpdateText.TabIndex = 16;
            this.timeUpdateText.Visible = false;
            // 
            // InterfaceComboBox
            // 
            this.InterfaceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InterfaceComboBox.FormattingEnabled = true;
            this.InterfaceComboBox.Items.AddRange(new object[] {
            "KVASER",
            "PEAK"});
            this.InterfaceComboBox.Location = new System.Drawing.Point(142, 100);
            this.InterfaceComboBox.Name = "InterfaceComboBox";
            this.InterfaceComboBox.Size = new System.Drawing.Size(150, 24);
            this.InterfaceComboBox.TabIndex = 17;
            this.InterfaceComboBox.SelectedIndexChanged += new System.EventHandler(this.InterfaceComboBox_SelectedIndexChanged);
            // 
            // interfaceLabel
            // 
            this.interfaceLabel.AutoSize = true;
            this.interfaceLabel.Location = new System.Drawing.Point(30, 103);
            this.interfaceLabel.Name = "interfaceLabel";
            this.interfaceLabel.Size = new System.Drawing.Size(106, 17);
            this.interfaceLabel.TabIndex = 18;
            this.interfaceLabel.Text = "Select Interface";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 405);
            this.Controls.Add(this.interfaceLabel);
            this.Controls.Add(this.InterfaceComboBox);
            this.Controls.Add(this.timeUpdateText);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.PauseButton);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.timeText);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.radioPanel);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.DirText);
            this.MaximumSize = new System.Drawing.Size(807, 452);
            this.MinimumSize = new System.Drawing.Size(807, 452);
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
        private System.Int32 startTime = 0;
        private System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.ProgressBar progressBar;
        private DataParameters data;
        private const System.Int32 TIME_INDEX = 0;
        private const System.Int32 MESSAGE_ID_INDEX = 2;
        private const System.Int32 LENGTH_BIT_INDEX = 5;
        private const System.Int32 MESSAGE_INDEX = 6;
        private System.Int64 streamLength;
        private System.Int64 streamVar = 1;
        private System.Int32 progressPercent;
        private System.Int64 previousTime = 0;
        private System.Boolean status = false;
        private System.Windows.Forms.Button PauseButton;
        private System.IO.StreamReader streamReader;
        private string loggedMessage = string.Empty;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.TextBox timeUpdateText;
        private System.Windows.Forms.ComboBox InterfaceComboBox;
        private System.Windows.Forms.Label interfaceLabel;
        private System.Int32 INTERFACE = 0;
        private System.Collections.Generic.List<DataParameters> canData = new System.Collections.Generic.List<DataParameters>();
        private System.Threading.Thread thread;
        private System.Boolean pause = false;

        public struct DataParameters
        {
            public int Message_Length;
            public float Message_Time;
            public System.String[] CAN_Message;
            public string Message_ID;
        }
        private void backgroundFunction()
        {
            while (timeLabel.Visible && pause)
            {
                readLogTransmitEnable();
            }
        }

        public System.Collections.Generic.List<DataParameters> GetData { get => this.canData; }
        public System.Int32 GetInterface { get => this.INTERFACE; }
    }
}

