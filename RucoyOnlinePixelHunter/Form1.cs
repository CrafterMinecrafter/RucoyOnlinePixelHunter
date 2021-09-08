using RucoyOnline.PixelHunter.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RucoyOnline.PixelHunter
{
    public partial class Form1 : Form
    {
        KeyboardHook hook = new KeyboardHook();
        public static Process emulator;
        public Bot bot;
        public Form1()
        {
            InitializeComponent();
            bot = new Bot();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.Cast<Form>().Where(t => t is ProcessDialog).Count() == 0)
            {
                new ProcessDialog().Show();
            }

            hook.KeyPressed +=
            new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            // register the control + alt + F12 combination as hot key.
            hook.RegisterHotKey(Input.ModifierKeys.Control, Keys.NumPad1);
        }
        void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            bot.Update();
            if (bot.application_source != null)
                pictureBox1.Image = bot.application_source;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            timer1.Interval = Convert.ToInt32(textBox1.Text);
        }
    }
}