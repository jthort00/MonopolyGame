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
        Socket server;
        public string username;
        public Form4()
        {
            InitializeComponent();
        }



        private void Form4_Load(object sender, EventArgs e)
        {
            label1.Text = "Welcome " + this.username;

            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9087);

            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                server.Connect(ipep);//Intentamos conectar el socket

                string mensaje = "4/" + username;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[800];
                server.Receive(msg2);
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



                server.Shutdown(SocketShutdown.Both);
                server.Close();
            }
            catch (SocketException)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
                return;
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
            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9087);

            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                server.Connect(ipep);//Intentamos conectar el socket

                string mensaje = "3/" + username;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                string[] parts = mensaje.Split('/');
                if (parts[3] == "0")
                    parts[3] = "No";

                string[] row0 = { parts[0], parts[2], parts[1], parts[3], parts[4]};
                dataGridView1.Rows.Add(row0);




                server.Shutdown(SocketShutdown.Both);
                server.Close();
            }
            catch (SocketException)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
                return;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("**Work in progress**");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Close();
           

        }
    }
}
