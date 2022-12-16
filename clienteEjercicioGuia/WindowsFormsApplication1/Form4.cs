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
        delegate void DelegateForm5(int gameid);
        delegate void DelegateForm5_Add(string add);
        delegate void DelegateCerrarTodo();




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
            form5.ingameList.Add(this.username);
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
        

            //DelegateCerrarTodo delegado = new DelegateCerrarTodo(CerrarForms);
            //this.Invoke(delegado);

            //mensaje = "5/N";
            //msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            //server1.Send(msg);

        }

        public void AddToTheGame(string añadir)
        {
            MessageBox.Show("Voy a añadir a "+ añadir);
            DelegateForm5_Add delegado = new DelegateForm5_Add(AddPlayerForm5);
            form5.Invoke(delegado, new object[] { añadir });


        }

        public void ClearDataGridViewGame()
        {
            //form5.dataGridView2.Rows.Clear();
        }

        public void GiveGameForm5(int gameid)
        {
            form5.gameid = gameid;
            DelegateForm5 delega = new DelegateForm5(ThingsForm5);
            this.Invoke(delega, new object[] { gameid });
            //form5.label1.Text = "Game " + gameid + " waiting room";
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

        public void ThingsForm5(int gameid)
        {
            form5.label1.Text = "Game " + gameid + " waiting room";

        }

        public void CerrarForms()
        {
            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                if (Application.OpenForms[i].Name != "Form1")
                    MessageBox.Show(Application.OpenForms[i].Name + " has been closed");
                    Application.OpenForms[i].Close();
            }

        }

        public void AddPlayerForm5(string add)
        {
            form5.dataGridView2.Rows.Add(add);
            form5.dataGridView2.Refresh();
        }
    }
}
