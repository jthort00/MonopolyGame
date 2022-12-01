using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace WindowsFormsApplication1
{
    public partial class Form4 : Form
    {
        public Socket server1;
        public string username;
        public List<string> onlineList = new List<string>();
        public int lastgameid;
        public Form5 form5;
        



        public Form4()
        {
            InitializeComponent();
        }



        private void Form4_Load(object sender, EventArgs e)
        {
         
            label1.Text = "Welcome " + this.username;
            string mensaje = "4/" + username;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server1.Send(msg);
            
            

        }

        
        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            int gameid = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
            //MessageBox.Show("Game selected:" + gameid + " **Work in progress**");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string mensaje = "3/" + username;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server1.Send(msg);


            form5 = new Form5();
            form5.server = this.server1;
            form5.username = this.username;
            form5.label1.Refresh();
            form5.Show();
            

            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("**Work in progress**");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string mensaje = "6/" + username;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server1.Send(msg);

            mensaje = "5/N";
            msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server1.Send(msg); 

        }

        public void AddToTheGame(string añadir)
        {
            form5.dataGridView2.Rows.Add(añadir);
            form5.dataGridView2.Refresh();
        }

        public void GiveGameForm5(int gameid)
        {
            form5.gameid = gameid;
            form5.label1.Text = "Game " + gameid + " waiting room";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //int index = dataGridView1.CurrentCell.RowIndex;
            //int gameid = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);

            //string mensaje = "9/" + username + "/" + gameid;
            //byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            //server1.Send(msg);
            //dataGridView1.Rows.Clear();

            //mensaje = "4/" + username;
            //msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            //server1.Send(msg);

        }
    }
}
