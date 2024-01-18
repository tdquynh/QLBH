using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using QuanLyBanHang.Class;

namespace QuanLyBanHang
{
    public partial class NhanVien : Form

    {
        DataTable tblNV;

       
        public NhanVien()
        {
            InitializeComponent();
        }
        

        private bool IsValidInput()
        {
            if (string.IsNullOrWhiteSpace(txtManv.Text) ||
                string.IsNullOrWhiteSpace(txtTennv.Text) ||
                string.IsNullOrWhiteSpace(txtDiachi.Text) ||
                string.IsNullOrWhiteSpace(txtDienthoai.Text)||
                string.IsNullOrWhiteSpace(txtCCCD.Text)
                )
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void NhanVien_Load(object sender, EventArgs e)
        {
            Function.Connect();
            txtManv.Enabled = false;
            btnLuu.Enabled = false;
            LoadDataGridView();
        }

        private bool ThemMoi = false;

        private void ResetValues()
        {
            txtManv.Text = "";
            txtTennv.Text = "";
            rdbNam.Checked = false;
            txtDiachi.Text = "";
            dtNgaysinh.Value = DateTime.Now;
            txtDienthoai.Text = "";
            txtCCCD.Text = "";
        }

        private void LoadDataGridView()
        {
            string sql = "SELECT * FROM tblNhanVien";
            tblNV = Function.GetDataToTable(sql);
            dgvNhanvien.DataSource = tblNV;
            dgvNhanvien.AllowUserToAddRows = false;
            dgvNhanvien.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void dgvNhanvien_Click(object sender, EventArgs e)
        {
            
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ThemMoi = true;
            ResetValues();
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            txtManv.Enabled = true;
            txtManv.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvNhanvien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtManv.Focus();
                return;
            }

            if (tblNV.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            txtManv.Text = dgvNhanvien.CurrentRow.Cells["MaNV"].Value.ToString();
            txtTennv.Text = dgvNhanvien.CurrentRow.Cells["HoTen"].Value.ToString();
            dtNgaysinh.Value = (DateTime)dgvNhanvien.CurrentRow.Cells["NgaySinh"].Value;
            if (dgvNhanvien.CurrentRow.Cells["GioiTinh"].Value.ToString() == "Nam")
            {
                rdbNam.Checked = true;
            }
            else
            {
                rdbNu.Checked = true;
            }

            txtDiachi.Text = dgvNhanvien.CurrentRow.Cells["QueQuan"].Value.ToString();
            txtDienthoai.Text = dgvNhanvien.CurrentRow.Cells["SDT"].Value.ToString();
            txtCCCD.Text = dgvNhanvien.CurrentRow.Cells["CCCD"].Value.ToString();



            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnLuu.Enabled = false;

            ThemMoi = false; // Đang ở chế độ sửa
        }

        private void btnSua_Click_1(object sender, EventArgs e)
        {
            if (!IsValidInput())
                return;
            string manv = txtManv.Text;
            string hoten = txtTennv.Text;
            string quequan = txtDiachi.Text;
            string sdt = txtDienthoai.Text;
            string cccd = txtCCCD.Text;
            DateTime ngaysinh = dtNgaysinh.Value;

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

            string sql = $@"UPDATE tblNhanVien 
                    SET HoTen=N'{hoten.Trim()}', 
                        NgaySinh='{ngaysinh:yyyy-MM-dd}', 
                        GioiTinh=N'{gt}', 
                        QueQuan=N'{quequan.Trim()}', 
                        SDT='{sdt}', 
                        CCCD='{cccd}'
                       
                    WHERE MaNV=N'{manv}'";

            Function.RunSQL(sql);
            LoadDataGridView();
            ResetValues();
        }

        private void btnXoa_Click_1(object sender, EventArgs e)
        {
            if (!IsValidInput())
                return;

            if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                string sql = $"DELETE tblNhanVien WHERE MaNV=N'{txtManv.Text}'";
                Function.RunSQL(sql);
                LoadDataGridView();
                ResetValues();
            }
        }

        private void btnLuu_Click_1(object sender, EventArgs e)
        {
            string manv = txtManv.Text;
            string hoten = txtTennv.Text;
            string quequan = txtDiachi.Text;
            string sdt = txtDienthoai.Text;
            string cccd = txtCCCD.Text;
            DateTime ngaysinh = dtNgaysinh.Value;
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
                string sql = $@"INSERT INTO tblNhanVien (MaNV, HoTen, NgaySinh, GioiTinh, QueQuan, SDT,CCCD) 
                        VALUES (N'{manv.Trim()}', N'{hoten.Trim()}', '{ngaysinh:yyyy-MM-dd}', N'{gt}', N'{quequan.Trim()}', '{sdt}', '{cccd}')";

                Function.RunSQL(sql);
            }
            else
            {
                // Sửa
                string sql = $@"UPDATE tblNhanVien 
                    SET HoTen=N'{hoten.Trim()}', 
                        NgaySinh='{ngaysinh:yyyy-MM-dd}', 
                        GioiTinh=N'{gt}', 
                        QueQuan=N'{quequan.Trim()}', 
                        SDT='{sdt}', 
                        CCCD='{cccd}'                      
                    WHERE MaNV=N'{manv}'";
                Function.RunSQL(sql);
            }

            LoadDataGridView();
            ResetValues();
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = false;
            txtManv.Enabled = false;
        }

        private void btnBoqua_Click(object sender, EventArgs e)
        {
            ResetValues();
            btnBoqua.Enabled = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = false;
            txtManv.Enabled = false;
        }
    }
}
