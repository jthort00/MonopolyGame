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
            server1.Send(msg);

            //Recibimos la respuesta del servidor
            byte[] msg2 = new byte[800];
            server1.Receive(msg2);
            mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
         
            if (mensaje != "-1")
            {
                string[] parts = mensaje.Split('$');
                int i = 0;
                while (i+1 < parts.Length)
                {
                    string[] parts1 = parts[i].Split('/');
                    if (parts1[3] == "0")
                        parts1[3] = "No";

                    string[] row0 = { parts1[0], parts1[2], parts1[1], parts1[3], parts1[4] };
                    dataGridView1.Rows.Add(row0);
                    i = i + 1;
                  
                }
            }
            else
            {
                MessageBox.Show("No games");
            }
                

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

            //Recibimos la respuesta del servidor
            byte[] msg2 = new byte[80];
            server1.Receive(msg2);
            mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
            string[] parts = mensaje.Split('/');
            if (parts[3] == "0")
                parts[3] = "No";

            string[] row0 = { parts[0], parts[2], parts[1], parts[3], parts[4]};
            dataGridView1.Rows.Add(row0); 
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("**Work in progress**");
        }

        private void button3_Click(object sender, EventArgs e)
        {
       
            Form1 form1 = new Form1();
            form1.server = this.server1;
            form1.BackColor = Color.Green;
            form1.Show();
            this.Close();
           

        }
    }
}
