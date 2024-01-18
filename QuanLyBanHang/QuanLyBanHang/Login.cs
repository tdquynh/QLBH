using QuanLyBanHang.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBanHang
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txtUser.Text;
            string matKhau = txtPass.Text;

            // Tạo câu truy vấn kiểm tra đăng nhập
            string query = $"SELECT * FROM tblTaiKhoan WHERE [User] = '{tenDangNhap}' AND Pass = '{matKhau}'";

            // Kiểm tra đăng nhập
            if (Function.CheckKey(query))
            {
               
                Main dmcl = new Main();
                this.Hide();
                dmcl.Show();
                txtUser.Text = "";
                txtPass.Text = "";
            }
            else
            {
                // Đăng nhập thất bại
                txtUser.Text = "";
                txtPass.Text = "";
                MessageBox.Show("Đăng nhập thất bại. Vui lòng kiểm tra lại tên đăng nhập và mật khẩu.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dg = MessageBox.Show("Bạn có muốn thoát ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dg == DialogResult.Yes)
                Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowMK.Checked)
            {
                txtPass.PasswordChar = (char)0;
            }
            else
            {
                txtPass.PasswordChar = '*';
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Function.Connect();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Function.Disconnect();
        }

        private void button1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string tenDangNhap = txtUser.Text;
                string matKhau = txtPass.Text;

                // Tạo câu truy vấn kiểm tra đăng nhập
                string query = $"SELECT * FROM tblTaiKhoan WHERE [User] = '{tenDangNhap}' AND Pass = '{matKhau}'";

                // Kiểm tra đăng nhập
                if (Function.CheckKey(query))
                {

                    Main dmcl = new Main();
                    this.Hide();
                    dmcl.Show();
                    txtUser.Text = "";
                    txtPass.Text = "";
                }
                else
                {
                    // Đăng nhập thất bại
                    txtUser.Text = "";
                    txtPass.Text = "";
                    MessageBox.Show("Đăng nhập thất bại. Vui lòng kiểm tra lại tên đăng nhập và mật khẩu.");
                }
                // Thực hiện hành động khi Enter được ấn
                button1.PerformClick();
                e.Handled = true; // Ngăn chặn âm thanh 'beep' khi Enter được ấn
            }
        }
    }
}
