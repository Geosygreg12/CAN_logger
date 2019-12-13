using System;

namespace CanLogger1
{
    partial class CAN_Channel
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
            if (disposing && (components != null) && !play)
            {
                components.Dispose();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("STOP TRANSMISSION BEFORE CLOSING!!!", "ERROR", 
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                StopButton_Click(this, System.EventArgs.Empty);
            }

            if(!play) base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
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
            this.progressBarTimer = new System.Windows.Forms.Timer(this.components);
            this.SpeedComboBox = new System.Windows.Forms.ComboBox();
            this.SpeedLabel = new System.Windows.Forms.Label();
            this.numOfChanUpnDwn = new System.Windows.Forms.NumericUpDown();
            this.BaudrateLabel = new System.Windows.Forms.Label();
            this.interfaceLabel = new System.Windows.Forms.Label();
            this.baudRateComboBox = new System.Windows.Forms.ComboBox();
            this.InterfaceComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SetButton = new System.Windows.Forms.Button();
            this.radioPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOfChanUpnDwn)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // DirText
            // 
            this.DirText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DirText.Location = new System.Drawing.Point(29, 22);
            this.DirText.Name = "DirText";
            this.DirText.Size = new System.Drawing.Size(627, 27);
            this.DirText.TabIndex = 0;
            this.DirText.Text = "Enter the directory of the log file here";
            this.DirText.TextChanged += new System.EventHandler(this.DirText_TextChanged);
            this.DirText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DirText_KeyDown);
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(678, 12);
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
            this.radioPanel.Location = new System.Drawing.Point(483, 113);
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
            this.timeText.Location = new System.Drawing.Point(29, 195);
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
            this.progressLabel.Location = new System.Drawing.Point(25, 252);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(136, 20);
            this.progressLabel.TabIndex = 12;
            this.progressLabel.Text = "No Transmission";
            this.progressLabel.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(29, 278);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(280, 23);
            this.progressBar.TabIndex = 13;
            this.progressBar.Visible = false;
            // 
            // PauseButton
            // 
            this.PauseButton.Location = new System.Drawing.Point(315, 258);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(93, 43);
            this.PauseButton.TabIndex = 14;
            this.PauseButton.Text = "Pause";
            this.PauseButton.UseVisualStyleBackColor = true;
            this.PauseButton.Visible = false;
            this.PauseButton.Click += new System.EventHandler(this.PauseButton_Click);
            // 
            // progressBarTimer
            // 
            this.progressBarTimer.Interval = 1000;
            this.progressBarTimer.Tick += new System.EventHandler(this.ProgressBarTimer_Tick);
            // 
            // SpeedComboBox
            // 
            this.SpeedComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SpeedComboBox.FormattingEnabled = true;
            this.SpeedComboBox.Items.AddRange(new object[] {
            "1X",
            "2X",
            "2.5X",
            "inf"});
            this.SpeedComboBox.Location = new System.Drawing.Point(535, 76);
            this.SpeedComboBox.Name = "SpeedComboBox";
            this.SpeedComboBox.Size = new System.Drawing.Size(121, 24);
            this.SpeedComboBox.TabIndex = 19;
            // 
            // SpeedLabel
            // 
            this.SpeedLabel.AutoSize = true;
            this.SpeedLabel.Location = new System.Drawing.Point(480, 79);
            this.SpeedLabel.Name = "SpeedLabel";
            this.SpeedLabel.Size = new System.Drawing.Size(49, 17);
            this.SpeedLabel.TabIndex = 20;
            this.SpeedLabel.Text = "Speed";
            // 
            // numOfChanUpnDwn
            // 
            this.numOfChanUpnDwn.Location = new System.Drawing.Point(175, 0);
            this.numOfChanUpnDwn.Name = "numOfChanUpnDwn";
            this.numOfChanUpnDwn.Size = new System.Drawing.Size(59, 22);
            this.numOfChanUpnDwn.TabIndex = 21;
            // 
            // BaudrateLabel
            // 
            this.BaudrateLabel.AutoSize = true;
            this.BaudrateLabel.Location = new System.Drawing.Point(46, 85);
            this.BaudrateLabel.Name = "BaudrateLabel";
            this.BaudrateLabel.Size = new System.Drawing.Size(109, 17);
            this.BaudrateLabel.TabIndex = 18;
            this.BaudrateLabel.Text = "Select Baudrate";
            // 
            // interfaceLabel
            // 
            this.interfaceLabel.AutoSize = true;
            this.interfaceLabel.Location = new System.Drawing.Point(46, 45);
            this.interfaceLabel.Name = "interfaceLabel";
            this.interfaceLabel.Size = new System.Drawing.Size(106, 17);
            this.interfaceLabel.TabIndex = 18;
            this.interfaceLabel.Text = "Select Interface";
            // 
            // baudRateComboBox
            // 
            this.baudRateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.baudRateComboBox.FormattingEnabled = true;
            this.baudRateComboBox.Items.AddRange(new object[] {
            "250K",
            "500K"});
            this.baudRateComboBox.Location = new System.Drawing.Point(175, 85);
            this.baudRateComboBox.Name = "baudRateComboBox";
            this.baudRateComboBox.Size = new System.Drawing.Size(150, 24);
            this.baudRateComboBox.TabIndex = 17;
            // 
            // InterfaceComboBox
            // 
            this.InterfaceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InterfaceComboBox.FormattingEnabled = true;
            this.InterfaceComboBox.Items.AddRange(new object[] {
            "KVASER",
            "PEAK"});
            this.InterfaceComboBox.Location = new System.Drawing.Point(175, 42);
            this.InterfaceComboBox.Name = "InterfaceComboBox";
            this.InterfaceComboBox.Size = new System.Drawing.Size(150, 24);
            this.InterfaceComboBox.TabIndex = 17;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SetButton);
            this.groupBox1.Controls.Add(this.InterfaceComboBox);
            this.groupBox1.Controls.Add(this.numOfChanUpnDwn);
            this.groupBox1.Controls.Add(this.baudRateComboBox);
            this.groupBox1.Controls.Add(this.interfaceLabel);
            this.groupBox1.Controls.Add(this.BaudrateLabel);
            this.groupBox1.Location = new System.Drawing.Point(29, 68);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(344, 115);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CAN Channel Settings";
            // 
            // SetButton
            // 
            this.SetButton.Location = new System.Drawing.Point(265, -1);
            this.SetButton.Name = "SetButton";
            this.SetButton.Size = new System.Drawing.Size(60, 23);
            this.SetButton.TabIndex = 23;
            this.SetButton.Text = "SET";
            this.SetButton.UseVisualStyleBackColor = true;
            this.SetButton.Click += new System.EventHandler(this.SetButton_Click);
            // 
            // CAN_Channel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 403);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.SpeedLabel);
            this.Controls.Add(this.SpeedComboBox);
            this.Controls.Add(this.PauseButton);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.timeText);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.radioPanel);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.DirText);
            this.MaximumSize = new System.Drawing.Size(806, 450);
            this.MinimumSize = new System.Drawing.Size(806, 450);
            this.Name = "CAN_Channel";
            this.Text = "CAN Channel Settings";
            this.radioPanel.ResumeLayout(false);
            this.radioPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOfChanUpnDwn)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private const System.Int32 CAN_CHANNEL_INDEX = 1;
        private const System.Int32 MESSAGE_ID_INDEX = 2;
        private const System.Int32 LENGTH_BIT_INDEX = 5;
        private const System.Int32 MESSAGE_INDEX = 6;
        private System.Windows.Forms.Button PauseButton;
        private System.IO.StreamReader streamReader;
        private string loggedMessage = string.Empty;
        private System.Int32[] INTERFACE = { -1, -1};
        
        private System.Threading.Thread transmitLogthread;
        private System.Boolean play = false;
        System.Windows.Forms.RadioButton Var;
        private System.Windows.Forms.Timer progressBarTimer;
        private System.Windows.Forms.ComboBox SpeedComboBox;
        private System.Windows.Forms.Label SpeedLabel;
        private System.Int16 TransmissionSpeed;
        private System.Collections.Generic.List<DataParameters> canData = new System.Collections.Generic.List<DataParameters>();
        private System.Windows.Forms.NumericUpDown numOfChanUpnDwn;
        private System.Windows.Forms.Label BaudrateLabel;
        private System.Windows.Forms.Label interfaceLabel;
        private System.Windows.Forms.ComboBox baudRateComboBox;
        private System.Windows.Forms.ComboBox InterfaceComboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button SetButton;

        public struct DataParameters
        {
            public int Message_Length;
            public float Message_Time;
            public System.Byte[] CAN_Message;
            public string Message_ID;
            public bool Extended;
            public int CAN_Channel_ID;
        }
        
        public System.Collections.Generic.List<DataParameters> GetData { get => this.canData; }
        public System.Int32[] GetInterface { get => this.INTERFACE; }
    }
}

