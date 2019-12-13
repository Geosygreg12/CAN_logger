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
            this.questionLabel.Location = new System.Drawing.Point(107, 41);
            this.questionLabel.Name = "questionLabel";
            this.questionLabel.Size = new System.Drawing.Size(321, 21);
            this.questionLabel.TabIndex = 0;
            this.questionLabel.Text = "How many CAN channels do you need?";
            // 
            // numberOfCANChannels
            // 
            this.numberOfCANChannels.Location = new System.Drawing.Point(400, 41);
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
            this.numberOfCANChannels.Size = new System.Drawing.Size(77, 22);
            this.numberOfCANChannels.TabIndex = 1;
            this.numberOfCANChannels.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ContinueButton
            // 
            this.ContinueButton.Location = new System.Drawing.Point(207, 94);
            this.ContinueButton.Name = "ContinueButton";
            this.ContinueButton.Size = new System.Drawing.Size(102, 31);
            this.ContinueButton.TabIndex = 2;
            this.ContinueButton.Text = "Continue";
            this.ContinueButton.UseVisualStyleBackColor = true;
            this.ContinueButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // SetUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 164);
            this.Controls.Add(this.ContinueButton);
            this.Controls.Add(this.numberOfCANChannels);
            this.Controls.Add(this.questionLabel);
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