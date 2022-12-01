using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;

namespace WindowsFormsApplication1
{
    public partial class Form5 : Form
    {
        public Socket server;
        public string username;
        public int gameid;
        public List<string> ingameList = new List<string>();

        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            
            int i = 0;
            while (i<ingameList.Count)
            {
                dataGridView2.Rows.Add(ingameList[i]);
                i = i + 1;
            }
            

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
