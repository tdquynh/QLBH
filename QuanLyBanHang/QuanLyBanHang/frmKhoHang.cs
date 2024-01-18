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
    public partial class frmKhoHang : Form
    {
        DataTable tblKho;
        public frmKhoHang()
        {
            InitializeComponent();
        }
        private bool IsValidInput()
        {
            if (string.IsNullOrWhiteSpace(txtMaKho.Text) ||
                string.IsNullOrWhiteSpace(txtTenKho.Text) 
               
                )
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        private bool ThemMoi = false;
        private void frmKhoHang_Load(object sender, EventArgs e)
        {
            Function.Connect();
            txtMaKho.Enabled = false;
            btnLuu.Enabled = false;
            LoadDataGridView();
        }

        private void frmKhoHang_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void frmKhoHang_FormClosed(object sender, FormClosedEventArgs e)
        {
            Function.Disconnect();
        }
        private void ResetValues()
        {
            txtMaKho.Text = "";
            txtTenKho.Text = "";
           
        }
        private void LoadDataGridView()
        {
            string sql = "SELECT * FROM tblKhoHang";
            tblKho = Function.GetDataToTable(sql);
            dgvKho.DataSource = tblKho;
            dgvKho.AllowUserToAddRows = false;
            dgvKho.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ThemMoi = true;
            ResetValues();
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            txtMaKho.Enabled = true;
            txtMaKho.Focus();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvKho_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaKho.Focus();
                return;
            }

            if (tblKho.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            txtMaKho.Text = dgvKho.CurrentRow.Cells["MaKho"].Value.ToString();
            txtTenKho.Text = dgvKho.CurrentRow.Cells["TenKho"].Value.ToString();
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnLuu.Enabled = false;

            ThemMoi = false; // Đang ở chế độ sửa
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (!IsValidInput())
                return;

            if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                string sql = $"DELETE tblKhoHang WHERE MaKho=N'{txtMaKho.Text}'";
                Function.RunSQL(sql);
                LoadDataGridView();
                ResetValues();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string makho = txtMaKho.Text;
            string tenkho = txtTenKho.Text;
            if (!IsValidInput())
                return;
            string sql = $@"UPDATE tblKhoHang
                    SET TenKho=N'{tenkho.Trim()}'                       
                    WHERE MaKho=N'{makho}'";

            Function.RunSQL(sql);
            LoadDataGridView();
            ResetValues();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string makho = txtMaKho.Text;
            string tenkho = txtTenKho.Text;
            if (!IsValidInput())
                return;
            if (ThemMoi)
            {
                // Thêm mới
                string sql = $@"INSERT INTO tblKhoHang (MaKho, TenKho) 
                        VALUES (N'{makho.Trim()}', N'{tenkho.Trim()}')";

                Function.RunSQL(sql);
            }
            else
            {
                // Sửa
                string sql = $@"UPDATE tblKhoHang
                    SET TenKho=N'{tenkho.Trim()}'                       
                    WHERE MaKho=N'{makho}'";

                Function.RunSQL(sql);
            }
            LoadDataGridView();
            ResetValues();
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = false;
            txtMaKho.Enabled = false;
        }
    }
}
