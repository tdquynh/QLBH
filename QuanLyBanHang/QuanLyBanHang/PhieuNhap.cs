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
    public partial class PhieuNhap : Form
    {
        DataTable tblPNH;
        public PhieuNhap()
        {
            InitializeComponent();
        }

        private void txtThanhTien_TextChanged(object sender, EventArgs e)
        {

        }
        private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT a.MaHang, b.TenHang, a.SoLuong, b.DonGia,a.ThanhTien FROM tblPhieuNhapChiTiet AS a, tblHangHoa AS b WHERE a.MaPhieu = N'" + txtMaPhieu.Text + "' AND a.MaHang=b.MaHang";
            tblPNH = Function.GetDataToTable(sql);
            dgvPhieuNhap.DataSource = tblPNH;
            dgvPhieuNhap.Columns[0].HeaderText = "Mã hàng";
            dgvPhieuNhap.Columns[1].HeaderText = "Tên hàng";
            dgvPhieuNhap.Columns[2].HeaderText = "Số lượng";
            dgvPhieuNhap.Columns[3].HeaderText = "Đơn giá";
           
            dgvPhieuNhap.Columns[4].HeaderText = "Thành tiền";
            dgvPhieuNhap.Columns[0].Width = 80;
            dgvPhieuNhap.Columns[1].Width = 130;
            dgvPhieuNhap.Columns[2].Width = 80;
            dgvPhieuNhap.Columns[3].Width = 90;
            dgvPhieuNhap.Columns[4].Width = 90;
            dgvPhieuNhap.AllowUserToAddRows = false;
            dgvPhieuNhap.EditMode = DataGridViewEditMode.EditProgrammatically;
        }
        private void ResetValues()
        {
            txtMaPhieu.Text = "";
            dtNgayNhap.Value = DateTime.Now;
            cboMaNhanVien.Text = "";
            txtTongTien.Text = "0";
            lblBangChu.Text = "Bằng chữ: ";
            cboMaHang.Text = "";
            txtSoLuong.Text = "";
            txtThanhTien.Text = "0";
        }
        private void LoadInfoHoaDon()
        {
            string str;
            str = "SELECT NgayNhap FROM tblPhieuNhap WHERE MaPhieu = N'" + txtMaPhieu.Text + "'";
            dtNgayNhap.Value = DateTime.Parse(Function.GetFieldValues(str));
            str = "SELECT MaNV FROM tblPhieuNhap WHERE MaPhieu = N'" + txtMaPhieu.Text + "'";
            cboMaNhanVien.SelectedValue = Function.GetFieldValues(str);
            str = "SELECT TongTien FROM tblPhieuNhap WHERE MaPhieu = N'" + txtMaPhieu.Text + "'";
            txtTongTien.Text = Function.GetFieldValues(str);
            lblBangChu.Text = "Bằng chữ: " + Function.ChuyenSoSangChuoi(Double.Parse(txtTongTien.Text));


        }

        private void PhieuNhap_Load(object sender, EventArgs e)
        {
            btnThem.Enabled = true;
            btnLuu.Enabled = false;
            btnXoa.Enabled = false;
            // btnInHoaDon.Enabled = false;
            txtMaPhieu.ReadOnly = true;
            txtTenNhanVien.ReadOnly = true;
            txtTenHang.ReadOnly = true;
            txtDonGiaNhap.ReadOnly = true;
            txtThanhTien.ReadOnly = true;
            txtTongTien.ReadOnly = true;
           
            txtTongTien.Text = "0";
            
            Function.FillCombo("SELECT MaNV, HoTen FROM tblNhanVien", cboMaNhanVien, "MaNV", "TenKhach");
            cboMaNhanVien.SelectedIndex = -1;
            Function.FillCombo("SELECT MaHang, TenHang FROM tblHangHoa", cboMaHang, "MaHang", "MaHang");
            cboMaHang.SelectedIndex = -1;
            //Hiển thị thông tin của một hóa đơn được gọi từ form tìm kiếm
            if (txtMaPhieu.Text != "")
            {
                LoadInfoHoaDon();
                btnXoa.Enabled = true;
               
            }
            LoadDataGridView();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            
            btnThem.Enabled = false;
            ResetValues();
            txtMaPhieu.Text = Function.CreateKey("PN");
            LoadDataGridView();
        }
        private void ResetValuesHang()
        {
            cboMaHang.Text = "";
            txtSoLuong.Text = "";
            txtThanhTien.Text = "0";
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            double sl, SLcon, tong, Tongmoi;
            sql = "SELECT MaPhieu FROM tblPhieuNhap WHERE MaPhieu=N'" + txtMaPhieu.Text + "'";
            if (!Function.CheckKey(sql))
            {
               
                if (cboMaNhanVien.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboMaNhanVien.Focus();
                    return;
                }
                sql = "INSERT INTO tblPhieuNhap(MaPhieu, MaNV, NgayNhap, TongTien) " +
                    "VALUES (N'" + txtMaPhieu.Text.Trim() + "',N'" + cboMaNhanVien.SelectedValue + "','" + dtNgayNhap.Value.ToString("yyyy-MM-dd") + "'," + txtTongTien.Text + ")";


                Function.RunSQL(sql);
            }
            // Lưu thông tin của các mặt hàng
            if (cboMaHang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMaHang.Focus();
                return;
            }
            if ((txtSoLuong.Text.Trim().Length == 0) || (txtSoLuong.Text == "0"))
            {
                MessageBox.Show("Bạn phải nhập số lượng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoLuong.Text = "";
                txtSoLuong.Focus();
                return;
            }
           
            sql = "SELECT MaHang FROM tblPhieuNhapChiTiet WHERE MaHang=N'" + cboMaHang.SelectedValue + "' AND MaPhieu = N'" + txtMaPhieu.Text.Trim() + "'";
            if (Function.CheckKey(sql))
            {
                MessageBox.Show("Mã hàng này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetValuesHang();
                cboMaHang.Focus();
                return;
            }
            // Kiểm tra xem số lượng hàng trong kho còn đủ để cung cấp không?
            sl = Convert.ToDouble(Function.GetFieldValues("SELECT SoLuong FROM tblHangHoa WHERE MaHang = N'" + cboMaHang.SelectedValue + "'"));
           
            sql = "INSERT INTO tblPhieuNhapChiTiet(MaPhieu, MaHang, SoLuong, DonGia, ThanhTien) " +
      "VALUES(N'" + txtMaPhieu.Text.Trim() + "', N'" + cboMaHang.SelectedValue + "', " +
      txtSoLuong.Text + ", " + txtDonGiaNhap.Text + ", " + txtThanhTien.Text + ")";

            Function.RunSQL(sql);
            LoadDataGridView();
            // Cập nhật lại số lượng của mặt hàng vào bảng tblHang
            SLcon = sl + Convert.ToDouble(txtSoLuong.Text);
            sql = "UPDATE tblHangHoa SET SoLuong =" + SLcon + " WHERE MaHang= N'" + cboMaHang.SelectedValue + "'";
            Function.RunSQL(sql);
            // Cập nhật lại tổng tiền cho hóa đơn bán
            tong = Convert.ToDouble(Function.GetFieldValues("SELECT TongTien FROM tblPhieuNhap WHERE MaPhieu = N'" + txtMaPhieu.Text + "'"));
            Tongmoi = tong + Convert.ToDouble(txtThanhTien.Text);
            sql = "UPDATE tblPhieuNhap SET TongTien =" + Tongmoi + " WHERE MaPhieu = N'" + txtMaPhieu.Text + "'";
            Function.RunSQL(sql);
            txtTongTien.Text = Tongmoi.ToString();
            lblBangChu.Text = "Bằng chữ: " + Function.ChuyenSoSangChuoi(Double.Parse(Tongmoi.ToString()));
            ResetValuesHang();
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            // btnInHoaDon.Enabled = true;
        }

        private void cboMaHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (cboMaHang.Text == "")
            {
                txtTenHang.Text = "";
                txtDonGiaNhap.Text = "";
            }
            // Khi chọn mã hàng thì các thông tin về hàng hiện ra
            str = "SELECT TenHang FROM tblHangHoa WHERE MaHang =N'" + cboMaHang.SelectedValue + "'";
            txtTenHang.Text = Function.GetFieldValues(str);
            str = "SELECT DonGia FROM tblHangHoa WHERE MaHang =N'" + cboMaHang.SelectedValue + "'";
            txtDonGiaNhap.Text = Function.GetFieldValues(str);
        }

        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(txtSoLuong.Text, out double sl) &&
      
       double.TryParse(txtDonGiaNhap.Text, out double dg))
            {
                double tt = sl * dg - sl * dg  / 100;
                txtThanhTien.Text = tt.ToString("0.##"); // Định dạng kết quả với tối đa hai chữ số thập phân
            }
            else
            {
                if (txtSoLuong.Text == "")
                    sl = 0;
                else
                    sl = Convert.ToDouble(txtSoLuong.Text);
               
                if (txtDonGiaNhap.Text == "")
                    dg = 0;
                else
                    dg = Convert.ToDouble(txtDonGiaNhap.Text);
                double tt = sl * dg - sl * dg  / 100;
                txtThanhTien.Text = tt.ToString();
                // Xử lý trường hợp khi đầu vào không phải là một số hợp lệ
                txtThanhTien.Text = "0";
            }
        }

        private void cboMaNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {

            string str;
            if (cboMaNhanVien.Text == "")
                txtTenNhanVien.Text = "";
            // Khi chọn Mã nhân viên thì tên nhân viên tự động hiện ra
            str = "Select HoTen from tblNhanVien where MaNV =N'" + cboMaNhanVien.SelectedValue + "'";
            txtTenNhanVien.Text = Function.GetFieldValues(str);
        }

        private void cboMaPhieu_DropDown(object sender, EventArgs e)
        {
            Function.FillCombo("SELECT MaPhieu FROM tblPhieuNhap", cboMaPhieu, "MaPhieu", "MaPhieu");
            cboMaPhieu.SelectedIndex = -1;
        }

        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            if (cboMaPhieu.Text == "")
            {
                MessageBox.Show("Bạn phải chọn một mã hóa đơn để tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMaPhieu.Focus();
                return;
            }
            txtMaPhieu.Text = cboMaPhieu.Text;
            LoadInfoHoaDon();
            LoadDataGridView();
            btnXoa.Enabled = true;
            btnLuu.Enabled = true;
            //btnInHoaDon.Enabled = true;
            cboMaPhieu.SelectedIndex = -1;
        }

        private void txtSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9')) || (Convert.ToInt32(e.KeyChar) == 8))
                e.Handled = false;
            else e.Handled = true;
        }

        private void dgvPhieuNhap_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string MaHangxoa, sql;
            Double ThanhTienxoa, SoLuongxoa, sl, slcon, tong, tongmoi;
            if (tblPNH.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if ((MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                //Xóa hàng và cập nhật lại số lượng hàng 
                MaHangxoa = dgvPhieuNhap.CurrentRow.Cells["MaHang"].Value.ToString();
                SoLuongxoa = Convert.ToDouble(dgvPhieuNhap.CurrentRow.Cells["SoLuong"].Value.ToString());
                ThanhTienxoa = Convert.ToDouble(dgvPhieuNhap.CurrentRow.Cells["ThanhTien"].Value.ToString());
                sql = "DELETE tblPhieuNhapChiTiet WHERE MaPhieu=N'" + txtMaPhieu.Text + "' AND MaHang = N'" + MaHangxoa + "'";
                Function.RunSQL(sql);
                // Cập nhật lại số lượng cho các mặt hàng
                sl = Convert.ToDouble(Function.GetFieldValues("SELECT SoLuong FROM tblHangHoa WHERE MaHang = N'" + MaHangxoa + "'"));
                slcon = sl + SoLuongxoa;
                sql = "UPDATE tblHangHoa SET SoLuong =" + slcon + " WHERE MaHang= N'" + MaHangxoa + "'";
                Function.RunSQL(sql);
                // Cập nhật lại tổng tiền cho hóa đơn bán
                tong = Convert.ToDouble(Function.GetFieldValues("SELECT TongTien FROM tblPhieuNhap WHERE MaHD = N'" + txtMaPhieu.Text + "'"));
                tongmoi = tong - ThanhTienxoa;
                sql = "UPDATE tblPhieuNhap SET TongTien =" + tongmoi + " WHERE MaHD = N'" + txtMaPhieu.Text + "'";
                Function.RunSQL(sql);
                txtTongTien.Text = tongmoi.ToString();
                lblBangChu.Text = "Bằng chữ: " + Function.ChuyenSoSangChuoi(tongmoi);
                LoadDataGridView();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            double sl, slcon, slxoa;
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "SELECT MaHang,SoLuong FROM tblPhieuNhapChiTiet WHERE MaPhieu = N'" + txtMaPhieu.Text + "'";
                DataTable tblHang = Function.GetDataToTable(sql);
                for (int hang = 0; hang <= tblHang.Rows.Count - 1; hang++)
                {
                    // Cập nhật lại số lượng cho các mặt hàng
                    sl = Convert.ToDouble(Function.GetFieldValues("SELECT SoLuong FROM tblHangHoa WHERE MaHang = N'" + tblHang.Rows[hang][0].ToString() + "'"));
                    slxoa = Convert.ToDouble(tblHang.Rows[hang][1].ToString());
                    slcon = sl - slxoa;
                    sql = "UPDATE tblHangHoa SET SoLuong =" + slcon + " WHERE MaHang= N'" + tblHang.Rows[hang][0].ToString() + "'";
                    Function.RunSQL(sql);
                }

                //Xóa chi tiết hóa đơn
                sql = "DELETE tblPhieuNhapChiTiet WHERE MaPhieu=N'" + txtMaPhieu.Text + "'";
                Function.RunSQL(sql);

                //Xóa hóa đơn
                sql = "DELETE tblPhieuNhap WHERE MaPhieu=N'" + txtMaPhieu.Text + "'";
                Function.RunSQL(sql);
                ResetValues();
                LoadDataGridView();
                btnXoa.Enabled = false;
                //btnInHoaDon.Enabled = false;
            }
        }
    }
}
