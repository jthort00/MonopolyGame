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


                    

                    try
                    {
                        this.BackColor = Color.Green;

                        string mensaje = "1/" + textBox1.Text + "$" + textBox2.Text + "$" + numericUpDown1.Value;
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);
                      
                        //this.Close();
                        
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
            { 
                MessageBox.Show("La contraseña no coincide");
                

            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
