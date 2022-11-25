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
        public List<string> onlineList = new List<string>();
        public List<string> ingameList = new List<string>();

        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            int i = 0;
            while (i<onlineList.Count)
            {
                dataGridView1.Rows.Add(onlineList[i]);
                i = i + 1;
            }

        }

     

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            string player = Convert.ToString(dataGridView1.Rows[index].Cells[0].Value);
            MessageBox.Show("Game selected:" + player + " **Work in progress**");

            string mensaje = "7/" + username + "/" + player + "/"+gameid;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        public void ReturnThings()
        {

        }
    }
}
