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
    public partial class Form1 : Form
    {
        public Socket server;
        int connected = 0;
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            

        }


        public void button1_Click(object sender, EventArgs e)
        {
            if (connected == 1)
            {
                if (username.Text != "" && password.Text != "")
                {

                    string mensaje = "2/" + username.Text + "$" + password.Text;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    //Recibimos la respuesta del servidor
                    byte[] msg2 = new byte[80];
                    server.Receive(msg2);
                    mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];

                    if (mensaje == "0")
                        MessageBox.Show("Username or password are wrong");
                    if (mensaje == "1")
                    {

                        Form4 form4 = new Form4();
                        form4.username = this.username.Text;
                        form4.server1 = this.server;
                        form4.Show();
                        this.Hide();
                    }
                }

                else
                {
                    MessageBox.Show("Enter username and password");
                }
            }
            else
                MessageBox.Show("Press connect to start");



        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (connected == 1)
            {
                Form2 form2 = new Form2();
                form2.server = this.server;
                form2.Show();
            }
            else
                MessageBox.Show("Press connect to start");
        }

      

        private void button3_Click(object sender, EventArgs e)
        {
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9082);


            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                this.BackColor = Color.Green;
                button3.Enabled = false;
                button4.Enabled = true;
                connected = 1; 



            }
            catch (SocketException)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
                return;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Se terminó el servicio. 
            // Nos desconectamos
            string mensaje = "0/" + "Bye";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            server.Shutdown(SocketShutdown.Both);
            server.Close();
            this.BackColor = Color.Gray;
            button3.Enabled = true;
            button4.Enabled = false;
            connected = 0;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

      

        private void button5_Click(object sender, EventArgs e)
        {
            if (connected == 1)
            {
                this.OnlineUsers.Rows.Clear();
                string mensaje = "5/Petition";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];


                string[] parts = mensaje.Split('/');
                if (mensaje != "0/null")
                {
                    int i = 1;
                    while (i < parts.Length)
                    {
                        OnlineUsers.Rows.Add(parts[i]);
                        i = i + 1;
                    }

                }
            }
            else
                MessageBox.Show("Press connect to start");
        }
    }

}
