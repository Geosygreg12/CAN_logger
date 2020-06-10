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
            if (disposing && (components != null) && !isStarted)
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
            this.BrowseButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.SpeedComboBox = new System.Windows.Forms.ComboBox();
            this.SpeedLabel = new System.Windows.Forms.Label();
            this.canChanNum = new System.Windows.Forms.NumericUpDown();
            this.BaudrateLabel = new System.Windows.Forms.Label();
            this.interfaceLabel = new System.Windows.Forms.Label();
            this.baudRateComboBox = new System.Windows.Forms.ComboBox();
            this.InterfaceComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DirText = new System.Windows.Forms.TextBox();
            this.SetButton = new System.Windows.Forms.Button();
            this.AppSetting = new System.Windows.Forms.GroupBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.PauseButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.canChanNum)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.AppSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // BrowseButton
            // 
            this.BrowseButton.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.BrowseButton.Location = new System.Drawing.Point(463, 101);
            this.BrowseButton.Margin = new System.Windows.Forms.Padding(2);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(92, 22);
            this.BrowseButton.TabIndex = 1;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = false;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // startButton
            // 
            this.startButton.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.startButton.Enabled = false;
            this.startButton.Location = new System.Drawing.Point(11, 273);
            this.startButton.Margin = new System.Windows.Forms.Padding(2);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(92, 50);
            this.startButton.TabIndex = 8;
            this.startButton.Text = "START";
            this.startButton.UseVisualStyleBackColor = false;
            this.startButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.stopButton.Location = new System.Drawing.Point(474, 273);
            this.stopButton.Margin = new System.Windows.Forms.Padding(2);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(92, 50);
            this.stopButton.TabIndex = 9;
            this.stopButton.Text = "STOP";
            this.stopButton.UseVisualStyleBackColor = false;
            this.stopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // SpeedComboBox
            // 
            this.SpeedComboBox.BackColor = System.Drawing.SystemColors.HighlightText;
            this.SpeedComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SpeedComboBox.FormattingEnabled = true;
            this.SpeedComboBox.Items.AddRange(new object[] {
            "RealTime",
            "AutoTime"});
            this.SpeedComboBox.Location = new System.Drawing.Point(47, 43);
            this.SpeedComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.SpeedComboBox.Name = "SpeedComboBox";
            this.SpeedComboBox.Size = new System.Drawing.Size(92, 21);
            this.SpeedComboBox.TabIndex = 19;
            // 
            // SpeedLabel
            // 
            this.SpeedLabel.AutoSize = true;
            this.SpeedLabel.Location = new System.Drawing.Point(5, 46);
            this.SpeedLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SpeedLabel.Name = "SpeedLabel";
            this.SpeedLabel.Size = new System.Drawing.Size(38, 13);
            this.SpeedLabel.TabIndex = 20;
            this.SpeedLabel.Text = "Speed";
            // 
            // canChanNum
            // 
            this.canChanNum.Location = new System.Drawing.Point(131, 0);
            this.canChanNum.Margin = new System.Windows.Forms.Padding(2);
            this.canChanNum.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.canChanNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.canChanNum.Name = "canChanNum";
            this.canChanNum.Size = new System.Drawing.Size(44, 20);
            this.canChanNum.TabIndex = 21;
            this.canChanNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.canChanNum.ValueChanged += new System.EventHandler(this.canChanNum_ValueChanged);
            // 
            // BaudrateLabel
            // 
            this.BaudrateLabel.AutoSize = true;
            this.BaudrateLabel.Location = new System.Drawing.Point(175, 43);
            this.BaudrateLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.BaudrateLabel.Name = "BaudrateLabel";
            this.BaudrateLabel.Size = new System.Drawing.Size(83, 13);
            this.BaudrateLabel.TabIndex = 18;
            this.BaudrateLabel.Text = "Select Baudrate";
            // 
            // interfaceLabel
            // 
            this.interfaceLabel.AutoSize = true;
            this.interfaceLabel.Location = new System.Drawing.Point(24, 43);
            this.interfaceLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.interfaceLabel.Name = "interfaceLabel";
            this.interfaceLabel.Size = new System.Drawing.Size(82, 13);
            this.interfaceLabel.TabIndex = 18;
            this.interfaceLabel.Text = "Select Interface";
            // 
            // baudRateComboBox
            // 
            this.baudRateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.baudRateComboBox.FormattingEnabled = true;
            this.baudRateComboBox.Items.AddRange(new object[] {
            "250K",
            "500K",
            "1M"});
            this.baudRateComboBox.Location = new System.Drawing.Point(164, 58);
            this.baudRateComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.baudRateComboBox.Name = "baudRateComboBox";
            this.baudRateComboBox.Size = new System.Drawing.Size(114, 21);
            this.baudRateComboBox.TabIndex = 17;
            // 
            // InterfaceComboBox
            // 
            this.InterfaceComboBox.BackColor = System.Drawing.SystemColors.Window;
            this.InterfaceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InterfaceComboBox.FormattingEnabled = true;
            this.InterfaceComboBox.Items.AddRange(new object[] {
            "KVASER",
            "PEAK"});
            this.InterfaceComboBox.Location = new System.Drawing.Point(4, 58);
            this.InterfaceComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.InterfaceComboBox.Name = "InterfaceComboBox";
            this.InterfaceComboBox.Size = new System.Drawing.Size(114, 21);
            this.InterfaceComboBox.TabIndex = 17;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DirText);
            this.groupBox1.Controls.Add(this.SetButton);
            this.groupBox1.Controls.Add(this.InterfaceComboBox);
            this.groupBox1.Controls.Add(this.canChanNum);
            this.groupBox1.Controls.Add(this.baudRateComboBox);
            this.groupBox1.Controls.Add(this.interfaceLabel);
            this.groupBox1.Controls.Add(this.BaudrateLabel);
            this.groupBox1.Controls.Add(this.BrowseButton);
            this.groupBox1.Location = new System.Drawing.Point(11, 11);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(560, 128);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CAN Channel Settings";
            // 
            // DirText
            // 
            this.DirText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DirText.Location = new System.Drawing.Point(4, 101);
            this.DirText.Margin = new System.Windows.Forms.Padding(2);
            this.DirText.Name = "DirText";
            this.DirText.Size = new System.Drawing.Size(410, 23);
            this.DirText.TabIndex = 24;
            this.DirText.Text = "Enter the directory of the log file here";
            // 
            // SetButton
            // 
            this.SetButton.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.SetButton.Location = new System.Drawing.Point(463, -1);
            this.SetButton.Margin = new System.Windows.Forms.Padding(2);
            this.SetButton.Name = "SetButton";
            this.SetButton.Size = new System.Drawing.Size(92, 19);
            this.SetButton.TabIndex = 23;
            this.SetButton.Text = "SET";
            this.SetButton.UseVisualStyleBackColor = false;
            this.SetButton.Click += new System.EventHandler(this.SetButton_Click);
            // 
            // AppSetting
            // 
            this.AppSetting.Controls.Add(this.progressBar1);
            this.AppSetting.Controls.Add(this.SpeedLabel);
            this.AppSetting.Controls.Add(this.SpeedComboBox);
            this.AppSetting.Controls.Add(this.PauseButton);
            this.AppSetting.Location = new System.Drawing.Point(12, 144);
            this.AppSetting.Name = "AppSetting";
            this.AppSetting.Size = new System.Drawing.Size(559, 69);
            this.AppSetting.TabIndex = 23;
            this.AppSetting.TabStop = false;
            this.AppSetting.Text = "App Settings";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(6, 16);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(547, 23);
            this.progressBar1.TabIndex = 21;
            // 
            // PauseButton
            // 
            this.PauseButton.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.PauseButton.Location = new System.Drawing.Point(462, 41);
            this.PauseButton.Margin = new System.Windows.Forms.Padding(2);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(91, 22);
            this.PauseButton.TabIndex = 14;
            this.PauseButton.Text = "Pause";
            this.PauseButton.UseVisualStyleBackColor = false;
            this.PauseButton.Visible = false;
            this.PauseButton.Click += new System.EventHandler(this.PauseButton_Click);
            // 
            // CAN_Channel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(592, 334);
            this.Controls.Add(this.AppSetting);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximumSize = new System.Drawing.Size(608, 373);
            this.MinimumSize = new System.Drawing.Size(608, 373);
            this.Name = "CAN_Channel";
            this.Text = "CAN Channel Settings";
            ((System.ComponentModel.ISupportInitialize)(this.canChanNum)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.AppSetting.ResumeLayout(false);
            this.AppSetting.PerformLayout();
            this.ResumeLayout(false);

        }


        #endregion
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private const System.Int32 TIME_INDEX = 0;
        private const System.Int32 CAN_CHANNEL_INDEX = 1;
        private const System.Int32 MESSAGE_ID_INDEX = 2;
        private const System.Int32 LENGTH_BIT_INDEX = 5;
        private const System.Int32 MESSAGE_INDEX = 6;
        private System.IO.StreamReader streamReader;
        private string loggedMessage = string.Empty;

        private System.Threading.Thread transmitLogthread;
        private static  System.Boolean isStarted = false;
        System.Windows.Forms.RadioButton trans_modeRadio;
        private System.Windows.Forms.ComboBox SpeedComboBox;
        private System.Windows.Forms.Label SpeedLabel;
        private System.Int16 TransmissionSpeed;
        private System.Windows.Forms.NumericUpDown canChanNum;
        private System.Windows.Forms.Label BaudrateLabel;
        private System.Windows.Forms.Label interfaceLabel;
        private System.Windows.Forms.ComboBox baudRateComboBox;
        private System.Windows.Forms.ComboBox InterfaceComboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button SetButton;
        private System.Windows.Forms.TextBox DirText;
        private System.Windows.Forms.GroupBox AppSetting;
        private System.Windows.Forms.Button PauseButton;
        private System.Windows.Forms.ProgressBar progressBar1;

        public class DataParameters
        {
            public int Message_Length;
            public float Message_Time;
            public System.Byte[] CAN_Message;
            public string Message_ID;
            public bool Extended;
            public int CAN_Channel_ID;
        }
        
       
    }

    
}

