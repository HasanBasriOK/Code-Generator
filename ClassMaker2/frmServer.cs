using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ClassMaker2
{
    public partial class frmServer : DevExpress.XtraEditors.XtraForm
    {
        public frmServer()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (textEdit1.Text != string.Empty && txtUserName.Text != string.Empty && txtPassword.Text != string.Empty)
            {

                Program.Server = textEdit1.Text;
                Program.Username = txtUserName.Text;
                Program.Password = txtPassword.Text;

                Form1 frm = new Form1();
                this.Hide();
                frm.ShowDialog();


            }
            else
            {
                XtraMessageBox.Show("Server Adı Boş Olamaz", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}