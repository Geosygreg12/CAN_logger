using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CanLogger1
{
    public partial class SetUp : Form
    {
        CAN_Channel[] channels = new CAN_Channel[2];
        //Timer timer = new Timer();
        public SetUp()
        {
            InitializeComponent();
            this.FormClosing += SetUp_FormClosing;
           
            //timer.Interval = 100;
            //timer.Tick += checkIsDisposed;
        }

        private void checkIsDisposed(object sender, EventArgs e)
        {
            if (channels[0].IsDisposed && channels[1].IsDisposed) {
                this.Close();
            }
        }

        private void SetUp_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            foreach (CAN_Channel ch in channels) {
                if( ch != null )
                    ch.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int numChan = 0;
            this.Hide();
            int num = (int) numberOfCANChannels.Value;

            switch (num) {

                case 1:
                    if(numChan == 0) numChan = 1;

                    using (channels[0] = new CAN_Channel(numChan)) {
                        channels[0].ShowDialog();
                        this.Close();
                    }
                    

                    break;
                case 2:
                    numChan = 2;
                    //for (int i = 0; i < channels.Length; i++)
                    //{
                    //    channels[i] = new CAN_Channel(i + 1);
                    //    channels[i].Show();
                    //}
                    // timer.Enabled = true;
                    // timer.Start();

                    goto case 1;
                    
            }
        }
    }
}
