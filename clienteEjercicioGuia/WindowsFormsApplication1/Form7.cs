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
    public partial class Form7 : Form
    {
        string player;
        public int allow_invite;
        public Socket server;
        public int gameid;
        public string username;

        public Form7()
        {
            InitializeComponent();
        }

      

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            player = dataGridView1.Rows[index].Cells[0].Value.ToString();
            if (allow_invite == 1)
            {
                if (string.Equals(username, player) == false)
                {
                    string mensaje = "7/" + player + "/" + player + "/" + gameid;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                else
                    MessageBox.Show("You can't invite yourself");
            }

        }
    }
}
