using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace Vector_Space_Model
{
    public partial class Form1 : Form
    {
        VectorSpace V = new VectorSpace();
        bool Status = false;
        //Text T = new Text();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
            this.AcceptButton = button5;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(Status==false)
            {
                MessageBox.Show("Please wait for 10 seconds", "Working", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);

                //V.ConstructVectorSpace();
                //V.WriteTable();
                V.ReadTable();

                //V.CreateIDFTable();
                //V.WriteIDFTable();
                V.ReadIDFTable();

                //V.CreateVectorTable();
                //V.WriteVectorTable();
                V.ReadVectorTable();

                //V.CreateDocMagnitudeTable();
                //V.WriteMagnitudeTable();
                V.ReadMagnitudeTable();

                //V.PrintTable();

                richTextBox2.Text = V.LexiconSize;
                richTextBox3.Text = V.VectorSpaceString;
                //richTextBox3.Text = V.Result;
                Status = true;
            }
            MessageBox.Show("Vector Space Created", "Succeeded", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            dataGridView1.Rows.Clear();
            V.Query = textBox1.Text;
            V.QueryProcessing();
            richTextBox1.Text = V.Result;
            //richTextBox3.Text = V.Temp;
            for (int i = 1; i < 51; i++)
            {
                if (V.CosineFinal[i]>0.0)   // > 0.005 && V.CosineFinal[i] <= 1.0)
                {
                    dataGridView1.Rows.Add(i, V.CosineFinal[i]);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            richTextBox1.Text = "";
            dataGridView1.Rows.Clear();
        }
    }
}