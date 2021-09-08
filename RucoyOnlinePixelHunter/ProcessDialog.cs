using RucoyOnline.PixelHunter;
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
    public class ProcessInList
    {
        public static ProcessInList[] GetArray(Process[] pArray)
        {
            var buffer = new List<ProcessInList>();
            foreach (var l in pArray)
            {
                buffer.Add(new ProcessInList(l));
            }

            return buffer.ToArray();
        }


        public ProcessInList(Process p)
        {
            process = p;
        }

        public override string ToString()
        {
            return $"{process.ProcessName} | {process.MainWindowTitle} | {process.Id}";
        }

        public Process process;
    }
    public partial class ProcessDialog : Form
    {
        public ProcessDialog()
        {
            InitializeComponent();
        }
        public Process[] processes = null;

        private void Init()
        {
            listBox1.Items.Clear();
            processes = Process.GetProcesses().ToList().FindAll(i => i.ProcessName.Contains(textBox1.Text)).ToArray();
            listBox1.Items.AddRange(ProcessInList.GetArray(processes));
        }

        private void ProcessDialog_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void Update_Click(object sender, EventArgs e)
        {
            Init();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                Form1.emulator = (listBox1.Items[listBox1.SelectedIndex] as ProcessInList).process;
                Close();
            }
        }
    }
}
