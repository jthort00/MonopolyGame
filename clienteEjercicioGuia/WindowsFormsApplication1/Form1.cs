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
        Form5 form5;
        Form6 form6; 
        Form7 form7 = new Form7();

        string player_inv;
        int gameid;
        string[] parts;
        

        delegate void DelegateForm4();
        delegate void DelegateForm4_Add(string[] row);
        delegate void DelegateForm4_Add2(string[] row);
        delegate void DelegateForm4_Close();
        delegate void DelegateForm7(string[] parts);
        delegate void DelegateForm7_Close();
        delegate void DelegateForm7_Hide();


        public Form1()
        {
            InitializeComponent();
            //CheckForIllegalCrossThreadCalls = false;
        }

       

        public void Form1_Load(object sender, EventArgs e)
        {
             //form7 = new Form7();
             

        }

        public void AtenderServidor ()
        {
                      
            while (true)
            {
                //Recibimos mensaje del servidor
                byte[] msg2 = new byte[500];
                server.Receive(msg2);
                string[] trozos = Encoding.ASCII.GetString(msg2).Split('?');
                int codigo = Convert.ToInt32(trozos[0]);
                string mensaje = trozos[1].Split('\0')[0];
            
                switch (codigo)
                {
                    case 1: // Respuesta a registrarse 
                        mensaje = trozos[1];
                        if (mensaje == "-2")
                            MessageBox.Show("User already exists");
                        if (mensaje == "-1")
                            MessageBox.Show("Error!");
                        if (mensaje == "0")
                        {
                            MessageBox.Show("Successful registration");
                           
                        }
                        break;

                    case 2: // Respuesta a iniciar sesión 
                        if (mensaje == "0")
                        {
                            MessageBox.Show("Username or password are wrong");
                            DelegateForm7_Hide delegado = new DelegateForm7_Hide(HideForm7);
                            form7.Invoke(delegado);
                        }
                        if (mensaje == "1")
                        {
 
                            ThreadStart t = delegate { PonerEnMarchaFormulario4(); };
                            Thread T = new Thread(t);
                            T.Start();
                            form7.username = this.username.Text;
                            

                        }
                        break;

                    case 3: // Respuesta a crear partida 
                        parts = mensaje.Split('/');
                        DelegateForm4_Add2 delega1 = new DelegateForm4_Add2(MoreThingsForm4);
                        form4.Invoke(delega1, new object[] { parts });
                        //if (parts[3] == "0")
                        //    parts[3] = "No";

                        //string[] row0 = { parts[0], parts[2], parts[1], parts[3], parts[4] };
                        //form4.GiveGameForm5(Convert.ToInt32(parts[0]));
                        //form4.dataGridView1.Rows.Add(row0);
                        //form7.gameid = Convert.ToInt32(parts[0]);
                        //form7.allow_invite = 1;
                        break;

                    case 4: // Conseguir todas las partidas en las que está el usuario 
                        if (mensaje != "-1")
                        {
                            parts = mensaje.Split('$');
                            DelegateForm4_Add delegad = new DelegateForm4_Add(ThingsForm4);
                            form4.Invoke(delegad, new object[] { parts });
                            //int i = 0;
                            //while (i + 1 < parts.Length)
                            //{
                            //    string[] parts1 = parts[i].Split('/');
                            //    if (parts1[3] == "0")
                            //    {
                            //        parts1[3] = "No";
                            //    }

                            //    string [] row1 = { parts1[0], parts1[2], parts1[1], parts1[3], parts1[4] };

                            //    //form4.dataGridView1.Rows.Add(row1);
                            //    i = i + 1;

                            //}
                        }
                        else
                        {
                            MessageBox.Show("No games");
                            
                        }
                        break;
                        

                    case 5: // Notificación de que otro usuario ha iniciado sesión
                        parts = mensaje.Split('/');
                        if (mensaje != "0/null")
                        {
                            DelegateForm7 delega = new DelegateForm7(ThingsForm7);
                            this.Invoke(delega, new object[] {parts});
                            //int i = 1;
                            //form7.dataGridView1.Rows.Clear();
                            //while (i < parts.Length)
                            //{
                            //    form7.dataGridView1.Rows.Add(parts[i]);
                            //    form7.dataGridView1.Refresh();
                            //    i = i + 1;
                            //}

                        }
                        break;


                    case 6: // Respuesta a desconectarse
                        if (mensaje == "0")
                        {

                            ThreadStart tss = delegate { PonerEnMarchaFormulario1(); };
                            Thread Th = new Thread(tss);
                            Th.Start();
                            //form7 = new Form7();
                        }

                        else
                            MessageBox.Show("Error logging out");

                        break;

                    case 7: // Notificación de que un jugador te ha invitado 
                        parts = mensaje.Split('/');
                        player_inv = parts[0];
                        gameid = Convert.ToInt32(parts[1]);
                        ThreadStart ts = delegate { PonerEnMarchaFormulario6(); };
                        Thread Thr = new Thread(ts);
                        Thr.Start();
                        break;

                    case 8: // Respuesta a la invitación
                        parts = mensaje.Split('/');
                        MessageBox.Show("He recibido respuesta a la invitación " + mensaje);
                        if (parts[0]=="1")
                        {
                            MessageBox.Show(parts[1] + " has accepted the request");
                            if (parts[2] != username.Text)
                            {
                                MessageBox.Show("Tu no eres el host");
                                ThreadStart thread = delegate { PonerEnMarchaFormulario5(); };
                                Thread Thread = new Thread(thread);
                                Thread.Start();
                                form5.dataGridView2.Rows.Clear();
                                int i = 1;
                                while (i < parts.Length)
                                {
                                    form5.dataGridView2.Rows.Add(parts[i]);
                                    form5.dataGridView2.Refresh();
                                    i = i + 1;
                                }    
                            }

                            else
                            {
                                form4.AddToTheGame(parts[1]);
                            }
                        }

                        if (parts[0]=="0")
                        {
                            MessageBox.Show(parts[0] + " has rejected the request");

                        }

                       


                        //La respuesta a la invitación se envia a todos. Crear form de partida desde aquí. El host va en la posición parts 2. 
                        break;
                        
                    case 9: // Notificación de que la partida se ha borrado correctamente  
                        parts = mensaje.Split('/');
                        if (parts[0] == "0")
                            MessageBox.Show("Game deleted successfully");
                        else
                            MessageBox.Show("Error deleting the game");
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
                    form7.Show();

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
            IPEndPoint ipep = new IPEndPoint(direc, 7063);


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
            form7.server = this.server;
            

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
            atender.Abort();
            this.BackColor = Color.Gray;
            button3.Enabled = true;
            button4.Enabled = false;
            connected = 0;
            form7.Hide();

        }


        private void PonerEnMarchaFormulario4 () 
        {
            this.form4 = new Form4();
            this.form4.username = this.username.Text;
            this.form4.server1 = this.server;
            this.form4.form5 = this.form5;
            //this.Hide();
            DelegateForm4 delegat = new DelegateForm4(ShowDialog4);
            this.Invoke(delegat);
            this.form4.ShowDialog();

        }

        private void PonerEnMarchaFormulario1 ()
        {
            //form4.Close();
            DelegateForm7_Close delegat = new DelegateForm7_Close(CloseForm7);
            form7.Invoke(delegat);
           
            DelegateForm4_Close delegat2 = new DelegateForm4_Close(CloseForm4);
            form4.Invoke(delegat2);
            //DelegateForm7_Close delegat3 = new DelegateForm7_Close(CloseForm7);
            //form7.Invoke(delegat3);
            //Form1 form1 = new Form1();
            //form1.server = this.form4.server1;
            //form1.BackColor = Color.Gray;
            //form1.ShowDialog();
            

        }

        private void PonerEnMarchaFormulario6()
        {
            this.form6 = new Form6();
            form6.player_inviting = this.player_inv;
            form6.server = this.server;
            form6.username = this.username.Text;
            form6.gameid = this.gameid;
            form6.ShowDialog();
        }

        private void PonerEnMarchaFormulario5()
        {
            this.form5 = new Form5();
            form5.ingameList.Add(this.username.Text);
            form5.server = this.server;
            form5.username = this.username.Text;
            form5.gameid = this.gameid;
            form5.ShowDialog();
        }

        public void ShowDialog4()
        {
           // this.Hide();
        }

        public void CloseForm4()
        {
            form4.Close();
        }

        public void CloseForm7()
        {
            form7.Close();
        }

        public void HideForm7()
        {
            form7.Hide();
        }


        public void ThingsForm7(string[] parts)
        {
            int i = 1;
            form7.dataGridView1.Rows.Clear();
            while (i < parts.Length)
            {

                form7.dataGridView1.Rows.Add(parts[i]);
                form7.dataGridView1.Refresh();
                i = i + 1;
            }
        }

        public void ThingsForm4(string[] parts)
        {
            int i = 0;
            while (i + 1 < parts.Length)
            {
                string[] parts1 = parts[i].Split('/');
                if (parts1[3] == "0")
                {
                    parts1[3] = "No";
                }

                string[] row1 = { parts1[0], parts1[2], parts1[1], parts1[3], parts1[4] };
                form4.dataGridView1.Rows.Add(row1);
                i = i + 1;

            }

        }

        public void MoreThingsForm4(string[] parts)
        {
            if (parts[3] == "0")
                parts[3] = "No";

            string[] row0 = { parts[0], parts[2], parts[1], parts[3], parts[4] };
            form4.GiveGameForm5(Convert.ToInt32(parts[0]));
            form4.dataGridView1.Rows.Add(row0);
            form7.gameid = Convert.ToInt32(parts[0]);
            form7.allow_invite = 1;

        }
    }


}
