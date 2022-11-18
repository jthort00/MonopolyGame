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
  
        
        public Form4()
        {
            InitializeComponent();
        }



        private void Form4_Load(object sender, EventArgs e)
        {
         
            label1.Text = "Welcome " + this.username;
            string mensaje = "4/" + username;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            //server1.Send(msg);
            
            

        }

        
        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            int gameid = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
            MessageBox.Show("Game selected:" + gameid + " **Work in progress**");

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            string mensaje = "3/" + username;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server1.Send(msg);
                     
            
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

        }
    }
}
