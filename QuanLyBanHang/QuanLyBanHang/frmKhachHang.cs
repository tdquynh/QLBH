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
   
    public partial class frmKhachHang : Form
    {
        DataTable tblKhach;
        public frmKhachHang()
        {
            InitializeComponent();
        }
        private bool ThemMoi = false;
        private bool IsValidInput()
        {
            if (string.IsNullOrWhiteSpace(txtMaKhach.Text) ||
                string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                string.IsNullOrWhiteSpace(txtDiaChi.Text) ||
                string.IsNullOrWhiteSpace(txtSDT.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text)
                )
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        private void ResetValues()
        {
            txtMaKhach.Text = "";
            txtHoTen.Text = "";
            txtDiaChi.Text = "";
            dtNgaySinh.Value = DateTime.Now;
            txtSDT.Text = "";
            txtEmail.Text = "";
        }

        private void btnBoqua_Click(object sender, EventArgs e)
        {
            ResetValues();
            btnBoqua.Enabled = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = false;
            txtMaKhach.Enabled = false;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ThemMoi = true;
            ResetValues();
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            txtMaKhach.Enabled = true;           
            txtMaKhach.Focus();
        }
        private void LoadDataGridView()
        {
            string sql = "SELECT * FROM tblKhachHang";
            tblKhach = Function.GetDataToTable(sql);
            dgvKhach.DataSource = tblKhach;
            dgvKhach.AllowUserToAddRows = false;
            dgvKhach.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void frmKhachHang_Load(object sender, EventArgs e)
        {
            Function.Connect();
            txtMaKhach.Enabled = false;
            btnLuu.Enabled = false;
            LoadDataGridView();
        }
        

        private void dgvKhach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaKhach.Focus();
                return;
            }

            if (tblKhach.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            txtMaKhach.Text = dgvKhach.CurrentRow.Cells["MaKhach"].Value.ToString();
            txtHoTen.Text = dgvKhach.CurrentRow.Cells["HoTen"].Value.ToString();
            dtNgaySinh.Value = (DateTime)dgvKhach.CurrentRow.Cells["NgaySinh"].Value;
            if (dgvKhach.CurrentRow.Cells["GioiTinh"].Value.ToString() == "Nam")
            {
                rdbNam.Checked = true;
            }
            else
            {
                rdbNu.Checked = true;
            }

            txtDiaChi.Text = dgvKhach.CurrentRow.Cells["DiaChi"].Value.ToString();
            txtSDT.Text = dgvKhach.CurrentRow.Cells["SDT"].Value.ToString();
            txtEmail.Text = dgvKhach.CurrentRow.Cells["Email"].Value.ToString();



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
                string sql = $"DELETE tblKhachHang WHERE MaKhach=N'{txtMaKhach.Text}'";
                Function.RunSQL(sql);
                LoadDataGridView();
                ResetValues();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string makhach = txtMaKhach.Text;
            string hoten = txtHoTen.Text;
            string diachi = txtDiaChi.Text;
            string sdt = txtSDT.Text;
            string email = txtEmail.Text;
            DateTime ngaysinh = dtNgaySinh.Value;
            if (!IsValidInput())
                return;

            string gt;
            if (rdbNam.Checked)
            {
                gt = "Nam";
            }
            else if (rdbNu.Checked)
            {
                gt = "Nữ";
            }
            else
            {
                // Trường hợp không có RadioButton nào được chọn (nếu cần xử lý)
                gt = "Không xác định";
            }

            if (ThemMoi)
            {
                // Thêm mới
                string sql = $@"INSERT INTO tblKhachHang (MaKhach, HoTen, NgaySinh, GioiTinh, DiaChi, SDT,Email) 
                        VALUES (N'{makhach.Trim()}', N'{hoten.Trim()}', '{ngaysinh:yyyy-MM-dd}', N'{gt}', N'{diachi.Trim()}', '{sdt}', '{email}')";

                Function.RunSQL(sql);
            }
            else
            {
                // Sửa
                string sql = $@"UPDATE tblKhachHang 
                    SET HoTen=N'{hoten.Trim()}', 
                        NgaySinh='{ngaysinh:yyyy-MM-dd}', 
                        GioiTinh=N'{gt}', 
                        DiaChi=N'{diachi.Trim()}', 
                        SDT='{sdt}', 
                        Email='{email}'
                       
                    WHERE MaKhach=N'{makhach}'";

                Function.RunSQL(sql);
            }

            LoadDataGridView();
            ResetValues();
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = false;
            txtMaKhach.Enabled = false;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string makhach = txtMaKhach.Text;
            string hoten = txtHoTen.Text;
            string diachi = txtDiaChi.Text;
            string sdt = txtSDT.Text;
            string email = txtEmail.Text;
            DateTime ngaysinh = dtNgaySinh.Value;
            if (!IsValidInput())
                return;

            string gt;
            if (rdbNam.Checked)
            {
                gt = "Nam";
            }
            else if (rdbNu.Checked)
            {
                gt = "Nữ";
            }
            else
            {
                // Trường hợp không có RadioButton nào được chọn (nếu cần xử lý)
                gt = "Không xác định";
            }

            string sql = $@"UPDATE tblKhachHang 
                    SET HoTen=N'{hoten.Trim()}', 
                        NgaySinh='{ngaysinh:yyyy-MM-dd}', 
                        GioiTinh=N'{gt}', 
                        DiaChi=N'{diachi.Trim()}', 
                        SDT='{sdt}', 
                        Email='{email}'
                       
                    WHERE MaKhach=N'{makhach}'";

            Function.RunSQL(sql);
            LoadDataGridView();
            ResetValues();

        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
