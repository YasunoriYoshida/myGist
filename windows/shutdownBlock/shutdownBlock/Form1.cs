using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace shutdownBlock
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private bool blocked = false;

        protected override void WndProc(ref Message aMessage)
        {
            const int WM_QUERYENDSESSION = 0x0011;
            const int WM_ENDSESSION = 0x0016;

            if (blocked && (aMessage.Msg == WM_QUERYENDSESSION || aMessage.Msg == WM_ENDSESSION))
                return;

            base.WndProc(ref aMessage);
        }

        /// <summary>
        /// Shutdown Blcok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (ShutdownBlockReasonCreate(this.Handle, textBox1.Text))
            {
                blocked = true;
                MessageBox.Show("Shutdown blocking succeeded");
            }
            else
                MessageBox.Show("Shutdown blocking failed");
        }

        /// <summary>
        /// Shutdown UnBlcok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (ShutdownBlockReasonDestroy(this.Handle))
            {
                blocked = false;
                MessageBox.Show("Shutdown unblocking succeeded");
            }
            else
                MessageBox.Show("Shutdown unblocking failed");
        }

        // DLL Import
        [DllImport("user32.dll")]
        public extern static bool ShutdownBlockReasonCreate(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string pwszReason);

        [DllImport("user32.dll")]
        public extern static bool ShutdownBlockReasonDestroy(IntPtr hWnd);
    }
}
