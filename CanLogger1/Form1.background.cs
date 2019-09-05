using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace CanLogger1
{
    public partial class Form1
    {
        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            readAndTransmitFile();
        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Visible = true;
            progressLabel.Visible = true;
            progressLabel.Text = String.Format("Transmitting... {0}%", e.ProgressPercentage);
            progressBar.Value = e.ProgressPercentage;
            Console.WriteLine("The percentage is: " + e.ProgressPercentage);
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Thread.Sleep(2000);
            progressBar.Visible = false;
            progressLabel.Visible = false;
            progressLabel.Text = "Transmitting... 0%";
            progressBar.Value = 0;
        }
    }
}
