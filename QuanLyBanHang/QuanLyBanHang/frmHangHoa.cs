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
    public partial class frmHangHoa : Form
    {
        DataTable tblHangHoa;
        public frmHangHoa()
        {
            InitializeComponent();
        }
        private bool IsValidInputHangHoa()
        {
            if (string.IsNullOrWhiteSpace(txtMaHang.Text) ||
                string.IsNullOrWhiteSpace(txtTenHang.Text) ||
                string.IsNullOrWhiteSpace(txtDVT.Text) ||
                string.IsNullOrWhiteSpace(txtDonGia.Text) ||
                string.IsNullOrWhiteSpace(txtSoLuong.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        private void ResetValues()
        {
            txtMaHang.Text = "";
            txtTenHang.Text = "";
            txtDVT.Text = "";
            txtDonGia.Text = "";
            txtSoLuong.Text = "";
            cboMaKho.Text = "";
        }

        private void frmHangHoa_Load(object sender, EventArgs e)
        {
            Function.Connect();
            string k;
            k = "SELECT * from tblKhoHang";
           
           
            LoadDataGridViewHangHoa();
            Function.FillCombo(k, cboMaKho, "MaKho", "TenKho");
            cboMaKho.SelectedIndex = -1;
            txtMaHang.Enabled = false;
            btnLuu.Enabled = false;
            ResetValues();

        }
        private bool ThemMoiHangHoa = false;
        private void LoadDataGridViewHangHoa()
        {
            string sql = "SELECT * FROM tblHangHoa";
            tblHangHoa = Function.GetDataToTable(sql);
            dgvHangHoa.DataSource = tblHangHoa;
            dgvHangHoa.AllowUserToAddRows = false;
            dgvHangHoa.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void dgvHangHoa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaHang.Focus();
                return;
            }

            if (tblHangHoa.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            txtMaHang.Text = dgvHangHoa.CurrentRow.Cells["MaHang"].Value.ToString();
            txtTenHang.Text = dgvHangHoa.CurrentRow.Cells["TenHang"].Value.ToString();
            txtSoLuong.Text = dgvHangHoa.CurrentRow.Cells["SoLuong"].Value.ToString();
            txtDonGia.Text = dgvHangHoa.CurrentRow.Cells["DonGia"].Value.ToString();
            txtDVT.Text = dgvHangHoa.CurrentRow.Cells["DVT"].Value.ToString();
           
           
            cboMaKho.SelectedValue = dgvHangHoa.CurrentRow.Cells["MaKho"].Value.ToString();

            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnLuu.Enabled = false;

            ThemMoiHangHoa = false; // Đang ở chế độ sửa
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ThemMoiHangHoa = true;
            ResetValues();
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            txtMaHang.Enabled = true;
            txtMaHang.Focus();
        }

        private void btnBoqua_Click(object sender, EventArgs e)
        {
            ResetValues();
            btnBoqua.Enabled = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = false;
            txtMaHang.Enabled = false;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!IsValidInputHangHoa())
                return;

            string mahang = txtMaHang.Text;
            string tenhang = txtTenHang.Text;
            string donvitinh = txtDVT.Text;
            decimal giaban = decimal.Parse(txtDonGia.Text);
            string soluong = txtSoLuong.Text;
            string makho = cboMaKho.SelectedValue.ToString();



            
                // Thực hiện câu truy vấn SQL
                string sql = $@"UPDATE tblHangHoa 
                SET TenHang=N'{tenhang.Trim()}',
                     SoLuong='{soluong}',
                     DonGia='{giaban}',
                    DVT=N'{donvitinh.Trim()}',
                    MaKho='{makho}'                                             
                WHERE MaHang=N'{mahang}'";

                Function.RunSQL(sql);
                LoadDataGridViewHangHoa();
                //ResetValues();
           



           
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (!IsValidInputHangHoa())
                return;

            if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                string sql = $"DELETE tblHangHoa WHERE MaHang=N'{txtMaHang.Text}'";
                Function.RunSQL(sql);
                LoadDataGridViewHangHoa();
                ResetValues();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!IsValidInputHangHoa())
                return;

            string mahang = txtMaHang.Text;
            string tenhang = txtTenHang.Text;
            string donvitinh = txtDVT.Text;
            decimal giaban = decimal.Parse(txtDonGia.Text);
            string soluong = txtSoLuong.Text;
            string makho = cboMaKho.SelectedValue.ToString();

            if (ThemMoiHangHoa)
            {
                string sql = $@"INSERT INTO tblHangHoa (MaHang, TenHang, SoLuong, DonGia, DVT,MaKho) 
                        VALUES (N'{mahang.Trim()}', N'{tenhang.Trim()}', '{soluong}', '{giaban}', N'{donvitinh.Trim()}',N'{makho}')";

                Function.RunSQL(sql);
            }
            else
            {
                string sql = $@"UPDATE tblHangHoa 
                    SET TenHang=N'{tenhang.Trim()}',
                         SoLuong='{soluong}',
                         DonGia='{giaban}',
                        DVT=N'{donvitinh.Trim()}',
                        MaKho=N'{makho}'                                              
                    WHERE MaHang=N'{mahang}'";
                Function.RunSQL(sql);
            }

            LoadDataGridViewHangHoa();
            ResetValues();
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = false;
            txtMaHang.Enabled = false;
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
