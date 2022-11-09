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
    public partial class Form2 : Form
    {
        public Socket server;

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == textBox3.Text)
            {
                Boolean caracteres = textBox2.Text.Contains("$");
                Boolean caracteres2 = textBox2.Text.Contains("/");
                if (caracteres == false && caracteres2 == false)
                {


                    //IPAddress direc = IPAddress.Parse("192.168.56.102");
                    //IPEndPoint ipep = new IPEndPoint(direc, 9087);

                    //server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    try
                    {
                        //server.Connect(ipep);//Intentamos conectar el socket
                        this.BackColor = Color.Green;

                        string mensaje = "1/" + textBox1.Text + "$" + textBox2.Text + "$" + numericUpDown1.Value;
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);






                        //Recibimos la respuesta del servidor
                        byte[] msg2 = new byte[80];
                        server.Receive(msg2);
                        mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                        if (mensaje == "-2")
                            MessageBox.Show("User already exists");
                        if (mensaje == "-1")
                            MessageBox.Show("Error!");
                        if (mensaje == "0")
                        {
                            MessageBox.Show("Sucessful registration");
                            Close();
                        }

                        //server.Shutdown(SocketShutdown.Both);
                        //server.Close();






                        // Se terminó el servicio. 
                        // Nos desconectamos





                    }
                    catch (SocketException)
                    {
                        //Si hay excepcion imprimimos error y salimos del programa con return 
                        MessageBox.Show("No he podido conectar con el servidor");
                        return;
                    }

                    // Enviamos al servidor el nombre tecleado
                }
                else
                    MessageBox.Show("La contraseña no puede contener los símbolos /, $");


            }

            else
                MessageBox.Show("La contraseña no coincide");
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
