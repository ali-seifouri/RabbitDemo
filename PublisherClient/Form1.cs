using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using RabbitPublisher;


namespace PublisherClient
{
    public partial class Form1 : Form
    {
        private Publisher publisher;
        public Form1()
        {
            InitializeComponent();
            publisher = new Publisher();
            cmbDataType.DataSource = Enum.GetValues(typeof(DataType));
        }

        private void cmbDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataType status;
            if (!Enum.TryParse<DataType>(cmbDataType.SelectedValue.ToString(), out status))
            {
                MessageBox.Show("Data Type not recognized.");
                return;
            }

        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            publisher.Send("test");
        }
    }
}
