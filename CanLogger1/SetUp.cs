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
        CAN_Channel channel;



        public SetUp()
        {
            InitializeComponent();
            this.FormClosing +=                         SetUp_FormClosing;
        }


        private void Continue_Button(object sender, EventArgs e)
        {
            int numChan =                               0;
            this.Hide();
            int num =                                   (int) numberOfCANChannels.Value;

            switch (num) {

                case 1:
                    
                    numChan =                           1;

                    goto                                default;

                case 2:

                    numChan =                           2;

                    goto                                default;

                default:

                    using (channel = new CAN_Channel(numChan))
                    {
                        channel.ShowDialog();
                        this.Close();
                    }

                    break;
            }
        }


        private void SetUp_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (channel != null)                        channel.Close();

        }

    }
}
