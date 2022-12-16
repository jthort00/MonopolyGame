using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;

namespace WindowsFormsApplication1
{
    public partial class Form6 : Form
    {
        public string player_inviting;
        public string username;
        public Socket server;
        public int gameid;
        public List<string> ingameList = new List<string>();



        public Form6()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form6_Load(object sender, EventArgs e)
        {
            label1.Text = player_inviting + " has invited you to a game";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string mensaje = "8/" + player_inviting + "/" + username + "/"  + gameid + "/0";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string mensaje = "8/" + player_inviting + "/" + username + "/" + gameid + "/1";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            Form5 form5 = new Form5();
            //ingameList.Add(username);
            form5.button1.Visible = false;
            form5.button1.Enabled = false;
            //form5.ingameList = this.ingameList;
            form5.ShowDialog();
            this.Close();
        }
    }
}
