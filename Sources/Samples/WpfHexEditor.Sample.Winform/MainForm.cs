using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media;
using WpfHexaEditor.Core;

namespace WpfHexEditor.Winform.Sample
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);

        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            public IntPtr lpData;
        }

        const int WM_COPYDATA = 0x004A;

        public MainForm(string title)
        {
            InitializeComponent();

            hexEditor.PreloadByteInEditorMode = PreloadByteInEditor.MaxScreenVisibleLineAtDataLoad;
            hexEditor.ForegroundSecondColor = Brushes.Blue;

            if(!string.IsNullOrEmpty(title))
            {
                this.Text = title;
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch(m.Msg)
            {
                case WM_COPYDATA:
                    {
                        COPYDATASTRUCT cds = new COPYDATASTRUCT();
                        cds = (COPYDATASTRUCT)m.GetLParam(cds.GetType());

                        byte[] bytes = new byte[cds.cbData];
                        Marshal.Copy(cds.lpData, bytes, 0, cds.cbData);

                        this.hexEditor.Stream = new MemoryStream(bytes);
                    }
                    break;
               }
            base.WndProc(ref m);
        }

        private void OpenFileButton_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();

            if (fileDialog.ShowDialog() == DialogResult.OK && File.Exists(fileDialog.FileName))
            {                
                hexEditor.FileName = fileDialog.FileName;
            }
                
        }

        private void OpenTBLButton_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();

            if (fileDialog.ShowDialog() == DialogResult.OK && File.Exists(fileDialog.FileName))
            {
                hexEditor.LoadTblFile(fileDialog.FileName);
                hexEditor.TypeOfCharacterTable = CharacterTableType.TblFile;
            }
        }
    }
}
