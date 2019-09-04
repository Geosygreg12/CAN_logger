using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


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
    }
}
