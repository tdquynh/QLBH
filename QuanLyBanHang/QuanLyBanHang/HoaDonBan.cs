using QuanLyBanHang.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBanHang
{
    public partial class HoaDonBan : Form
    {
        DataTable tblCTHDB;
        public HoaDonBan()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void HoaDonBan_Load(object sender, EventArgs e)
        {
            btnThem.Enabled = true;
            btnLuu.Enabled = false;
            btnXoa.Enabled = false;
            // btnInHoaDon.Enabled = false;
            txtMaHDBan.ReadOnly = true;
            txtTenNhanVien.ReadOnly = true;
            txtTenKhach.ReadOnly = true;
            txtDiaChi.ReadOnly = true;
            txtDienThoai.ReadOnly = true;
            txtTenHang.ReadOnly = true;
            txtDonGiaBan.ReadOnly = true;
            txtThanhTien.ReadOnly = true;
            txtTongTien.ReadOnly = true;
            txtGiamGia.Text = "0";
            txtTongTien.Text = "0";
            Function.FillCombo("SELECT MaKhach, HoTen FROM tblKhachHang", cboMaKhach, "MaKhach", "MaKhach");
            cboMaKhach.SelectedIndex = -1;
            Function.FillCombo("SELECT MaNV, HoTen FROM tblNhanVien", cboMaNhanVien, "MaNV", "TenKhach");
            cboMaNhanVien.SelectedIndex = -1;
            Function.FillCombo("SELECT MaHang, TenHang FROM tblHangHoa", cboMaHang, "MaHang", "MaHang");
            cboMaHang.SelectedIndex = -1;
            //Hiển thị thông tin của một hóa đơn được gọi từ form tìm kiếm
            if (txtMaHDBan.Text != "")
            {
                LoadInfoHoaDon();
                btnXoa.Enabled = true;
                //btnInHoaDon.Enabled = true;
            }
            LoadDataGridView();
        }
        private void LoadInfoHoaDon()
        {
            string str;
            str = "SELECT NgayMua FROM tblHoaDon WHERE MaHD = N'" + txtMaHDBan.Text + "'";
            dtNgayBan.Value = DateTime.Parse(Function.GetFieldValues(str));
            str = "SELECT MaNV FROM tblHoaDon WHERE MaHD = N'" + txtMaHDBan.Text + "'";
            cboMaNhanVien.SelectedValue = Function.GetFieldValues(str);
            str = "SELECT MaKhach FROM tblHoaDon WHERE MaHD = N'" + txtMaHDBan.Text + "'";
            cboMaKhach.SelectedValue = Function.GetFieldValues(str);
            str = "SELECT TongTien FROM tblHoaDon WHERE MaHD = N'" + txtMaHDBan.Text + "'";
            txtTongTien.Text = Function.GetFieldValues(str);
            lblBangChu.Text = "Bằng chữ: " + Function.ChuyenSoSangChuoi(Double.Parse(txtTongTien.Text));

            
        }
        private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT a.MaHang, b.TenHang, a.SoLuong, b.DonGia, a.GiamGia,a.ThanhTien FROM tblHoaDonChiTiet AS a, tblHangHoa AS b WHERE a.MaHD = N'" + txtMaHDBan.Text + "' AND a.MaHang=b.MaHang";
            tblCTHDB = Function.GetDataToTable(sql);
            dgvHDBanHang.DataSource = tblCTHDB;
            dgvHDBanHang.Columns[0].HeaderText = "Mã hàng";
            dgvHDBanHang.Columns[1].HeaderText = "Tên hàng";
            dgvHDBanHang.Columns[2].HeaderText = "Số lượng";
            dgvHDBanHang.Columns[3].HeaderText = "Đơn giá";
            dgvHDBanHang.Columns[4].HeaderText = "Giảm giá %";
            dgvHDBanHang.Columns[5].HeaderText = "Thành tiền";
            dgvHDBanHang.Columns[0].Width = 80;
            dgvHDBanHang.Columns[1].Width = 130;
            dgvHDBanHang.Columns[2].Width = 80;
            dgvHDBanHang.Columns[3].Width = 90;
            dgvHDBanHang.Columns[4].Width = 90;
            dgvHDBanHang.Columns[5].Width = 90;
            dgvHDBanHang.AllowUserToAddRows = false;
            dgvHDBanHang.EditMode = DataGridViewEditMode.EditProgrammatically;
        }
        private void ResetValues()
        {
            txtMaHDBan.Text = "";
            dtNgayBan.Value = DateTime.Now;
            cboMaNhanVien.Text = "";
            cboMaKhach.Text = "";
            txtTongTien.Text = "0";
            lblBangChu.Text = "Bằng chữ: ";
            cboMaHang.Text = "";
            txtSoLuong.Text = "";
            txtGiamGia.Text = "0";
            txtThanhTien.Text = "0";
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            //btnInHoaDon.Enabled = false;
            btnThem.Enabled = false;
            ResetValues();
            txtMaHDBan.Text = Function.CreateKey("HDB");

            // txtMaHDBan.Enabled = true;

            LoadDataGridView();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            double sl, SLcon, tong, Tongmoi;
            sql = "SELECT MaHD FROM tblHoaDon WHERE MaHD=N'" + txtMaHDBan.Text + "'";
            if (!Function.CheckKey(sql))
            {
                // Mã hóa đơn chưa có, tiến hành lưu các thông tin chung
                // Mã HDBan được sinh tự động do đó không có trường hợp trùng khóa
                /* if (dtNgayBan.Value.Length == 0)
                 {
                     MessageBox.Show("Bạn phải nhập ngày bán", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                     txtNgayBan.Focus();
                     return;
                 }*/
                if (cboMaNhanVien.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboMaNhanVien.Focus();
                    return;
                }
                if (cboMaKhach.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboMaKhach.Focus();
                    return;
                }
                sql = "INSERT INTO tblHoaDon(MaHD, MaNV, MaKhach, NgayMua, TongTien) " +
                    "VALUES (N'" + txtMaHDBan.Text.Trim() + "',N'" + cboMaNhanVien.SelectedValue + "',N'" +
                        cboMaKhach.SelectedValue + "','"+dtNgayBan.Value.ToString("yyyy-MM-dd") + "'," + txtTongTien.Text + ")";
                
                
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
            if (txtGiamGia.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập giảm giá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtGiamGia.Focus();
                return;
            }
            sql = "SELECT MaHang FROM tblHoaDonChiTiet WHERE MaHang=N'" + cboMaHang.SelectedValue + "' AND MaHD = N'" + txtMaHDBan.Text.Trim() + "'";
            if (Function.CheckKey(sql))
            {
                MessageBox.Show("Mã hàng này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetValuesHang();
                cboMaHang.Focus();
                return;
            }
            // Kiểm tra xem số lượng hàng trong kho còn đủ để cung cấp không?
            sl = Convert.ToDouble(Function.GetFieldValues("SELECT SoLuong FROM tblHangHoa WHERE MaHang = N'" + cboMaHang.SelectedValue + "'"));
            if (Convert.ToDouble(txtSoLuong.Text) > sl)
            {
                MessageBox.Show("Số lượng mặt hàng này chỉ còn " + sl, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoLuong.Text = "";
                txtSoLuong.Focus();
                return;
            }
            sql = "INSERT INTO tblHoaDonChiTiet(MaHD, MaHang, SoLuong, DonGia, GiamGia, ThanhTien) " +
      "VALUES(N'" + txtMaHDBan.Text.Trim() + "', N'" + cboMaHang.SelectedValue + "', " +
      txtSoLuong.Text + ", " + txtDonGiaBan.Text + ", " + txtGiamGia.Text + ", " + txtThanhTien.Text + ")";
           
            Function.RunSQL(sql);
            LoadDataGridView();
            // Cập nhật lại số lượng của mặt hàng vào bảng tblHang
            SLcon = sl - Convert.ToDouble(txtSoLuong.Text);
            sql = "UPDATE tblHangHoa SET SoLuong =" + SLcon + " WHERE MaHang= N'" + cboMaHang.SelectedValue + "'";
            Function.RunSQL(sql);
            // Cập nhật lại tổng tiền cho hóa đơn bán
            tong = Convert.ToDouble(Function.GetFieldValues("SELECT TongTien FROM tblHoaDon WHERE MaHD = N'" + txtMaHDBan.Text + "'"));
            Tongmoi = tong + Convert.ToDouble(txtThanhTien.Text);
            sql = "UPDATE tblHoaDon SET TongTien =" + Tongmoi + " WHERE MaHD = N'" + txtMaHDBan.Text + "'";
            Function.RunSQL(sql);
            txtTongTien.Text = Tongmoi.ToString();
            lblBangChu.Text = "Bằng chữ: " + Function.ChuyenSoSangChuoi(Double.Parse(Tongmoi.ToString()));
            ResetValuesHang();
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            // btnInHoaDon.Enabled = true;
        }
        private void ResetValuesHang()
        {
            cboMaHang.Text = "";
            txtSoLuong.Text = "";
            txtGiamGia.Text = "0";
            txtThanhTien.Text = "0";
        }

        private void cboMaHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (cboMaHang.Text == "")
            {
                txtTenHang.Text = "";
                txtDonGiaBan.Text = "";
            }
            // Khi chọn mã hàng thì các thông tin về hàng hiện ra
            str = "SELECT TenHang FROM tblHangHoa WHERE MaHang =N'" + cboMaHang.SelectedValue + "'";
            txtTenHang.Text = Function.GetFieldValues(str);
            str = "SELECT DonGia FROM tblHangHoa WHERE MaHang =N'" + cboMaHang.SelectedValue + "'";
            txtDonGiaBan.Text = Function.GetFieldValues(str);
        }

        private void cboMaKhach_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (cboMaKhach.Text == "")
            {
                txtTenKhach.Text = "";
                txtDiaChi.Text = "";
                txtDienThoai.Text = "";
            }
            //Khi chọn Mã khách hàng thì các thông tin của khách hàng sẽ hiện ra
            str = "Select HoTen from tblKhachHang where MaKhach = N'" + cboMaKhach.SelectedValue + "'";
            txtTenKhach.Text = Function.GetFieldValues(str);
            str = "Select DiaChi from tblKhachHang where MaKhach = N'" + cboMaKhach.SelectedValue + "'";
            txtDiaChi.Text = Function.GetFieldValues(str);
            str = "Select SDT from tblKhachHang where MaKhach= N'" + cboMaKhach.SelectedValue + "'";
            txtDienThoai.Text = Function.GetFieldValues(str);
        }

        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
           
            if (double.TryParse(txtSoLuong.Text, out double sl) &&
        double.TryParse(txtGiamGia.Text, out double gg) &&
        double.TryParse(txtDonGiaBan.Text, out double dg))
            {
                double tt = sl * dg - sl * dg * gg / 100;
                txtThanhTien.Text = tt.ToString("0.##"); // Định dạng kết quả với tối đa hai chữ số thập phân
            }
            else
            {
                if (txtSoLuong.Text == "")
                    sl = 0;
                else
                    sl = Convert.ToDouble(txtSoLuong.Text);
                if (txtGiamGia.Text == "")
                    gg = 0;
                else
                    gg = Convert.ToDouble(txtGiamGia.Text);
                if (txtDonGiaBan.Text == "")
                    dg = 0;
                else
                    dg = Convert.ToDouble(txtDonGiaBan.Text);
                double tt = sl * dg - sl * dg * gg / 100;
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

        private void txtGiamGia_TextChanged(object sender, EventArgs e)
        {
            //Khi thay đổi giảm giá thì tính lại thành tiền
            double tt, sl, dg, gg;
            if (txtSoLuong.Text == "")
                sl = 0;
            else
                sl = Convert.ToDouble(txtSoLuong.Text);
            if (txtGiamGia.Text == "")
                gg = 0;
            else
                gg = Convert.ToDouble(txtGiamGia.Text);
            if (txtDonGiaBan.Text == "")
                dg = 0;
            else
                dg = Convert.ToDouble(txtDonGiaBan.Text);
            tt = sl * dg - sl * dg * gg / 100;
            txtThanhTien.Text = tt.ToString();
        }

        private void cboMaHDBan_DropDown(object sender, EventArgs e)
        {
           
            Function.FillCombo("SELECT MaHD FROM tblHoaDon", cboMaHDBan, "MaHD", "MaHD");
            cboMaHDBan.SelectedIndex = -1;
        }

        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            if (cboMaHDBan.Text == "")
            {
                MessageBox.Show("Bạn phải chọn một mã hóa đơn để tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMaHDBan.Focus();
                return;
            }
            txtMaHDBan.Text = cboMaHDBan.Text;
            LoadInfoHoaDon();
            LoadDataGridView();
            btnXoa.Enabled = true;
            btnLuu.Enabled = true;
            //btnInHoaDon.Enabled = true;
            cboMaHDBan.SelectedIndex = -1; 
        }
        

        private void btnXoa_Click(object sender, EventArgs e)
        {
            double sl, slcon, slxoa;
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "SELECT MaHang,SoLuong FROM tblHoaDonChiTiet WHERE MaHD = N'" + txtMaHDBan.Text + "'";
                DataTable tblHang = Function.GetDataToTable(sql);
                for (int hang = 0; hang <= tblHang.Rows.Count - 1; hang++)
                {
                    // Cập nhật lại số lượng cho các mặt hàng
                    sl = Convert.ToDouble(Function.GetFieldValues("SELECT SoLuong FROM tblHangHoa WHERE MaHang = N'" + tblHang.Rows[hang][0].ToString() + "'"));
                    slxoa = Convert.ToDouble(tblHang.Rows[hang][1].ToString());
                    slcon = sl + slxoa;
                    sql = "UPDATE tblHangHoa SET SoLuong =" + slcon + " WHERE MaHang= N'" + tblHang.Rows[hang][0].ToString() + "'";
                    Function.RunSQL(sql);
                }

                //Xóa chi tiết hóa đơn
                sql = "DELETE tblHoaDonChiTiet WHERE MaHD=N'" + txtMaHDBan.Text + "'";
                Function.RunSQL(sql);

                //Xóa hóa đơn
                sql = "DELETE tblHoaDon WHERE MaHD=N'" + txtMaHDBan.Text + "'";
                Function.RunSQL(sql);
                ResetValues();
                LoadDataGridView();
                btnXoa.Enabled = false;
                //btnInHoaDon.Enabled = false;
            }
        }

        private void txtSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9')) || (Convert.ToInt32(e.KeyChar) == 8))
                e.Handled = false;
            else e.Handled = true;

        }

        private void dgvHDBanHang_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string MaHangxoa, sql;
            Double ThanhTienxoa, SoLuongxoa, sl, slcon, tong, tongmoi;
            if (tblCTHDB.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if ((MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                //Xóa hàng và cập nhật lại số lượng hàng 
                MaHangxoa = dgvHDBanHang.CurrentRow.Cells["MaHang"].Value.ToString();
                SoLuongxoa = Convert.ToDouble(dgvHDBanHang.CurrentRow.Cells["SoLuong"].Value.ToString());
                ThanhTienxoa = Convert.ToDouble(dgvHDBanHang.CurrentRow.Cells["ThanhTien"].Value.ToString());
                sql = "DELETE tblHoaDonChiTiet WHERE MaHD=N'" + txtMaHDBan.Text + "' AND MaHang = N'" + MaHangxoa + "'";
                Function.RunSQL(sql);
                // Cập nhật lại số lượng cho các mặt hàng
                sl = Convert.ToDouble(Function.GetFieldValues("SELECT SoLuong FROM tblHangHoa WHERE MaHang = N'" + MaHangxoa + "'"));
                slcon = sl + SoLuongxoa;
                sql = "UPDATE tblHangHoa SET SoLuong =" + slcon + " WHERE MaHang= N'" + MaHangxoa + "'";
                Function.RunSQL(sql);
                // Cập nhật lại tổng tiền cho hóa đơn bán
                tong = Convert.ToDouble(Function.GetFieldValues("SELECT TongTien FROM tblHoaDon WHERE MaHD = N'" + txtMaHDBan.Text + "'"));
                tongmoi = tong - ThanhTienxoa;
                sql = "UPDATE tblHoaDon SET TongTien =" + tongmoi + " WHERE MaHD = N'" + txtMaHDBan.Text + "'";
                Function.RunSQL(sql);
                txtTongTien.Text = tongmoi.ToString();
                lblBangChu.Text = "Bằng chữ: " + Function.ChuyenSoSangChuoi(tongmoi);
                LoadDataGridView();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dtNgayBan_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void txtThanhTien_TextChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void txtTenKhach_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void txtDiaChi_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void txtTenNhanVien_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDienThoai_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtDonGiaBan_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblBangChu_Click(object sender, EventArgs e)
        {

        }

        private void txtTenHang_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTongTien_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboMaHDBan_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dgvHDBanHang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtMaHDBan_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
