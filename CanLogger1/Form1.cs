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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.Filter = "ASC file (*.asc) | *.asc| Text file (*.txt) | *.txt ";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        DirText.Text = openFileDialog.FileName;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("An exception occured! Exception: " + exception.Message);
            }            
        }

        private void TimesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            bool isRead = false;
            if (!(listOfLoggedValues.Count > 0))
            {
                isRead = readFile();
            } 
            if(isRead) MessageBox.Show("Transmission has started!");
        }

        private void StopButton_Click(object sender, EventArgs e)
        {

        }

        private void DirText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) timeText.Focus();
        }

        private void TimeSearchButton_Click(object sender, EventArgs e)
        {
            /**
             * TODO::
             * make the textbox show the time it started if time exist or message box for wrong time
            and textbox text show the default message. **/

            readFile();
            startTime = timeText.Text;
        }

        //if the user presses enter after inputing start time
        private void TimeText_KeyDown(object sender, KeyEventArgs e) 
        {
            if (e.KeyCode == Keys.Enter)
            {
                TimeSearchButton_Click(sender, e);
                StartButton_Click(sender, e);
            }
        }

        private bool readFile()
        {
            regexTime = new Regex(@"^\d.\d\d\d\d\d\d");
            try
            {
                if (File.Exists(DirText.Text))
                {
                    using (StreamReader streamReader = new StreamReader(DirText.Text, Encoding.ASCII))
                    {
                        while (!streamReader.EndOfStream)
                        {
                            listOfLoggedValues.Add(streamReader.ReadLine());
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Wrong Directory! Try Again");
                    return false;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("An Exception occurred!!! Exception: " + exception.Message);
                return false;
            }

            return true;
        }

        private void DirText_TextChanged(object sender, EventArgs e)
        {
            // if text change is true, delete previous list of can messages
            listOfLoggedValues.Clear();
        }
    }
}
