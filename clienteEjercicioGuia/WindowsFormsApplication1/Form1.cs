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
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Socket server;
        Thread atender;
        int connected = 0;

        Form2 form2;
        Form4 form4;

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

       

        public void Form1_Load(object sender, EventArgs e)
        {
         
     

        }

        public void AtenderServidor ()
        {
                      
            while (true)
            {
                //Recibimos mensaje del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                string[] trozos = Encoding.ASCII.GetString(msg2).Split('?');
                int codigo = Convert.ToInt32(trozos[0]);
                string mensaje = trozos[1].Split('\0')[0];
            
                switch (codigo)
                {
                    case 1:
                        mensaje = trozos[1];
                        if (mensaje == "-2")
                            MessageBox.Show("User already exists");
                        if (mensaje == "-1")
                            MessageBox.Show("Error!");
                        if (mensaje == "0")
                        {
                            MessageBox.Show("Sucessful registration");
                            this.form2.Close();
                           
                        }
                        break;

                    case 2:
                        if (mensaje == "0")
                            MessageBox.Show("Username or password are wrong");
                        if (mensaje == "1")
                        {
                            ThreadStart ts = delegate { PonerEnMarchaFormulario4(); };
                            Thread T = new Thread(ts);
                            T.Start();

                        }
                        break;

                    case 3:
                        string[] parts = mensaje.Split('/');
                        if (parts[3] == "0")
                            parts[3] = "No";

                        string[] row0 = { parts[0], parts[2], parts[1], parts[3], parts[4] };

                        form4.dataGridView1.Rows.Add(row0);
                        break;

                    case 4:
                        if (mensaje != "-1")
                        {
                            parts = mensaje.Split('$');
                            int i = 0;
                            while (i + 1 < parts.Length)
                            {
                                string[] parts1 = parts[i].Split('/');
                                if (parts1[3] == "0")
                                {
                                    parts1[3] = "No";
                                }

                                string [] row1 = { parts1[0], parts1[2], parts1[1], parts1[3], parts1[4] };
                                form4.dataGridView1.Rows.Add(row1);
                                i = i + 1;

                            }
                        }
                        else
                        {
                            MessageBox.Show("No games");
                            
                        }
                        break;
                        

                    case 5:
                        parts = mensaje.Split('/');
                        if (mensaje != "0/null")
                        {
                            int i = 1;
                            while (i < parts.Length)
                            {
                                OnlineUsers.Rows.Add(parts[i]);
                                i = i + 1;
                            }

                        }
                        break;


                    case 6:
                        if (mensaje == "0")
                        {
                            ThreadStart ts = delegate { PonerEnMarchaFormulario1(); };
                            Thread Th = new Thread(ts);
                            Th.Start();
                        }

                        else
                            MessageBox.Show("Error logging out");

                        break;


                }
             

            }


            
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
                this.form2 = new Form2();
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
            IPEndPoint ipep = new IPEndPoint(direc, 9098);


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

            //Start thread to receive messages from server
        
            ThreadStart ts = delegate { AtenderServidor(); };
            atender = new Thread(ts);
            atender.Start();


        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Se terminó el servicio. 
            // Nos desconectamos
            string mensaje = "0/" + "Bye";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            atender.Abort();
            server.Shutdown(SocketShutdown.Both);
            server.Close();
            this.BackColor = Color.Gray;
            button3.Enabled = true;
            button4.Enabled = false;
            connected = 0;

        }



        private void button5_Click(object sender, EventArgs e)
        {
            if (connected == 1)
            {
                this.OnlineUsers.Rows.Clear();
                string mensaje = "5/Petition";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                
            }
            else
                MessageBox.Show("Press connect to start");
        }

        private void PonerEnMarchaFormulario4 () 
        {
            this.form4 = new Form4();
            this.form4.username = this.username.Text;
            this.form4.server1 = this.server;
            this.form4.ShowDialog();
            this.Hide();

            
        }

        private void PonerEnMarchaFormulario1 ()
        {
            this.form4.Close();
            Form1 form1 = new Form1();
            form1.server = this.form4.server1;
            form1.BackColor = Color.Green;
            form1.ShowDialog();
            

        }
        
    }


}
