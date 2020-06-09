namespace CanLogger1
{
    partial class SetUp
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
            this.questionLabel = new System.Windows.Forms.Label();
            this.numberOfCANChannels = new System.Windows.Forms.NumericUpDown();
            this.ContinueButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfCANChannels)).BeginInit();
            this.SuspendLayout();
            // 
            // questionLabel
            // 
            this.questionLabel.AutoSize = true;
            this.questionLabel.Location = new System.Drawing.Point(80, 33);
            this.questionLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.questionLabel.Name = "questionLabel";
            this.questionLabel.Size = new System.Drawing.Size(196, 13);
            this.questionLabel.TabIndex = 0;
            this.questionLabel.Text = "How many CAN channels do you need?";
            // 
            // numberOfCANChannels
            // 
            this.numberOfCANChannels.Location = new System.Drawing.Point(300, 33);
            this.numberOfCANChannels.Margin = new System.Windows.Forms.Padding(2);
            this.numberOfCANChannels.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numberOfCANChannels.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numberOfCANChannels.Name = "numberOfCANChannels";
            this.numberOfCANChannels.Size = new System.Drawing.Size(58, 20);
            this.numberOfCANChannels.TabIndex = 1;
            this.numberOfCANChannels.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ContinueButton
            // 
            this.ContinueButton.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ContinueButton.Location = new System.Drawing.Point(155, 76);
            this.ContinueButton.Margin = new System.Windows.Forms.Padding(2);
            this.ContinueButton.Name = "ContinueButton";
            this.ContinueButton.Size = new System.Drawing.Size(76, 25);
            this.ContinueButton.TabIndex = 2;
            this.ContinueButton.Text = "Continue";
            this.ContinueButton.UseVisualStyleBackColor = false;
            this.ContinueButton.Click += new System.EventHandler(this.Continue_Button);
            // 
            // SetUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(390, 133);
            this.Controls.Add(this.ContinueButton);
            this.Controls.Add(this.numberOfCANChannels);
            this.Controls.Add(this.questionLabel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SetUp";
            this.Text = "SetUp";
            ((System.ComponentModel.ISupportInitialize)(this.numberOfCANChannels)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label questionLabel;
        private System.Windows.Forms.NumericUpDown numberOfCANChannels;
        private System.Windows.Forms.Button ContinueButton;
    }
}