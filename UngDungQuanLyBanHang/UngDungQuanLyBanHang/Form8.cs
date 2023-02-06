using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace UngDungQuanLyBanHang
{
    public partial class Form8 : Form
    {
        // Chuỗi kết nối
        string strConnectionString = @"Data Source=.\SQLEXPRESS2008;Initial Catalog=QuanLyBanHang;Integrated Security = True";
        // Đối tượng kết nối
        SqlConnection conn = null;
        // Đối tượng đưa dữ liệu vào DataTable dtHoaDon
        SqlDataAdapter daHoaDon = null;
        // Đối tượng hiển thị dữ liệu lên Form
        DataTable dtHoaDon = null;
        // Đối tượng đưa dữ liệu vào DataTable dtKhachHang
        SqlDataAdapter daKhachHang = null;
        // Đối tượng hiển thị dữ liệu lên Form
        DataTable dtKhachHang = null;
        // Đối tượng đưa dữ liệu vào DataTable dtNhanVien
        SqlDataAdapter daNhanVien = null;
        // Đối tượng hiển thị dữ liệu lên Form
        DataTable dtNhanVien = null;
        // Khai báo biến kiểm tra việc Thêm hay Sửa dữ liệu
        bool Them;

        void LoadData()
        {
            try
            {
                // Khởi động connection
                conn = new SqlConnection(strConnectionString);

                // Vận chuyển dữ liệu vào DataTable dtNhanVien
                daNhanVien = new SqlDataAdapter("SELECT MaNV, Ho, Ten FROM NHANVIEN", conn);
                dtNhanVien = new DataTable();
                dtNhanVien.Clear();
                daNhanVien.Fill(dtNhanVien);
                // Đưa dữ liệu lên ComboBox trong DataGridView
                (dgvHOADON.Columns["MaNV"] as DataGridViewComboBoxColumn).DataSource = dtNhanVien;
                (dgvHOADON.Columns["MaNV"] as DataGridViewComboBoxColumn).DisplayMember = "Ten";
                (dgvHOADON.Columns["MaNV"] as DataGridViewComboBoxColumn).ValueMember = "MaNV";
                // Vận chuyển dữ liệu vào DataTable dtKhachHang
                daKhachHang = new SqlDataAdapter("SELECT MaKH, TenCty FROM KHACHHANG", conn);
                dtKhachHang = new DataTable();
                dtKhachHang.Clear();
                daKhachHang.Fill(dtKhachHang);
                // Đưa dữ liệu lên ComboBox trong DataGridView
                (dgvHOADON.Columns["TenCty"] as DataGridViewComboBoxColumn).DataSource = dtKhachHang;
                (dgvHOADON.Columns["TenCty"] as DataGridViewComboBoxColumn).DisplayMember = "TenCty";
                (dgvHOADON.Columns["TenCty"] as DataGridViewComboBoxColumn).ValueMember = "MaKH";
                // Vận chuyển dữ liệu vào DataTable dtHoaDon
                daHoaDon = new SqlDataAdapter("SELECT * FROM HoaDon", conn);
                dtHoaDon = new DataTable();
                dtHoaDon.Clear();
                daHoaDon.Fill(dtHoaDon);
                // Đưa dữ liệu lên DataGridView 
                dgvHOADON.DataSource = dtHoaDon;
                // Xóa trống các đối tượng trong Panel
                this.txtMaHD.ResetText();
                this.txtNgayLapHD.ResetText();
                this.txtNgayNhanHang.ResetText();
                // Không cho thao tác trên các nút Lưu / Hủy
                this.btnLuu.Enabled = false;
                this.btnHuyBo.Enabled = false;
                this.panel.Enabled = false;
                // Cho thao tác trên các nút Thêm / Sửa / Xóa / Thoát
                this.btnThem.Enabled = true;
                this.btnSua.Enabled = true;
                this.btnXoa.Enabled = true;
                this.btnTroVe.Enabled = true;

            }
            catch (SqlException)
            {
                MessageBox.Show("Không lấy được nội dung trong table KHACHHANG.Lỗi rồi!!!");
            }
        }
        public Form8()
        {
            InitializeComponent();
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void Form8_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Giải phóng tài nguyên
            dtNhanVien.Dispose();
            dtNhanVien = null;
            dtKhachHang.Dispose();
            dtKhachHang = null;
            dtHoaDon.Dispose();
            dtHoaDon = null;
            // Hủy kết nối
            conn = null;
        }

        private void btnReLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnTroVe_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // Kich hoạt biến Them
            Them = true;
            // Xóa trống các đối tượng trong Panel
            this.txtMaHD.ResetText();
            this.txtNgayLapHD.ResetText();
            this.txtNgayNhanHang.ResetText();            
            // Cho thao tác trên các nút Lưu / Hủy / Panel
            this.btnLuu.Enabled = true;
            this.btnHuyBo.Enabled = true;
            this.panel.Enabled = true;
            // Không cho thao tác trên các nút Thêm / Xóa / Thoát
            this.btnThem.Enabled = false;
            this.btnSua.Enabled = false;
            this.btnXoa.Enabled = false;
            this.btnTroVe.Enabled = false;
            // Đưa dữ liệu lên ComboBox
            this.cbMaKH.DataSource = dtKhachHang;
            this.cbMaKH.DisplayMember = "TenCty";
            this.cbMaKH.ValueMember = "MaKH";
            // Đưa dữ liệu lên ComboBox
            this.cbMaNV.DataSource = dtNhanVien;
            this.cbMaNV.DisplayMember = "Ten";
            this.cbMaNV.ValueMember = "MaNV";
            // Đưa con trỏ đến TextField txtMaKH
            this.txtMaHD.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Kích hoạt biến Sửa
            Them = false;
            // Đưa dữ liệu lên ComboBox
            this.cbMaKH.DataSource = dtKhachHang;
            this.cbMaKH.DisplayMember = "TenCty";
            this.cbMaKH.ValueMember = "MaKH";
            // Đưa dữ liệu lên ComboBox
            this.cbMaNV.DataSource = dtNhanVien;
            this.cbMaNV.DisplayMember = "Ten";
            this.cbMaNV.ValueMember = "MaNV";
            // Cho phép thao tác trên Panel
            this.panel.Enabled = true;
            // Thứ tự dòng hiện hành
            int r = dgvHOADON.CurrentCell.RowIndex;
            // Chuyển thông tin lên panel
            this.txtMaHD.Text =
            dgvHOADON.Rows[r].Cells[0].Value.ToString();
            this.cbMaKH.SelectedValue =
            dgvHOADON.Rows[r].Cells[1].Value.ToString();
            this.cbMaNV.SelectedValue =
            dgvHOADON.Rows[r].Cells[2].Value.ToString();
            this.txtNgayLapHD.Text =
            dgvHOADON.Rows[r].Cells[3].Value.ToString();
            this.txtNgayNhanHang.Text =
            dgvHOADON.Rows[r].Cells[4].Value.ToString();
            // Cho thao tác trên các nút Lưu / Hủy / Panel
            this.btnLuu.Enabled = true;
            this.btnHuyBo.Enabled = true;
            this.panel.Enabled = true;
            // Không cho thao tác trên các nút Thêm / Xóa / Thoát
            this.btnThem.Enabled = false;
            this.btnSua.Enabled = false;
            this.btnXoa.Enabled = false;
            this.btnTroVe.Enabled = false;
            // Đưa con trỏ đến TextField txtMaKH
            this.txtMaHD.Focus();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Mở kết nối
            conn.Open();
            try
            {
                // Thực hiện lệnh
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                // Lấy thứ tự record hiện hành
                int r = dgvHOADON.CurrentCell.RowIndex;
                // Lấy MaHD của record hiện hành
                string strMAHD =
                dgvHOADON.Rows[r].Cells[0].Value.ToString();
                // Viết câu lệnh SQL
                cmd.CommandText = System.String.Concat("Delete From HoaDon Where MaHD = '" + strMAHD + "'");
                cmd.CommandType = CommandType.Text;
                // Thực hiện câu lệnh SQL
                cmd.ExecuteNonQuery();
                // Cập nhật lại DataGridView
                LoadData();
                // Thông báo
                MessageBox.Show("Đã xóa xong!");
            }
            catch (SqlException)
            {
                MessageBox.Show("Không xóa được. Lỗi rồi!!!");
            }
            // Đóng kết nối
            conn.Close();
        }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            // Xóa trống các đối tượng trong Panel
            this.txtMaHD.ResetText();
            this.txtNgayLapHD.ResetText();
            this.txtNgayNhanHang.ResetText();
            // Không cho thao tác trên các nút Lưu / Hủy
            this.btnLuu.Enabled = false;
            this.btnHuyBo.Enabled = false;
            this.panel.Enabled = false;
            // Cho thao tác trên các nút Thêm / Sửa / Xóa / Thoát
            this.btnThem.Enabled = true;
            this.btnSua.Enabled = true;
            this.btnXoa.Enabled = true;
            this.btnTroVe.Enabled = true;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Mở kết nối
            conn.Open();
            // Thêm dữ liệu
            if (Them)
            {
                try
                {
                    // Thực hiện lệnh
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    // Lệnh Insert InTo
                    cmd.CommandText = System.String.Concat("Insert Into HoaDon Values(" + "'" +
                    this.txtMaHD.Text.ToString() + "','" +
                    this.cbMaKH.SelectedValue.ToString() + "','" +
                    this.cbMaNV.SelectedValue.ToString() + "','" +
                    this.txtNgayLapHD.Text.ToString() + "','" +
                    this.txtNgayNhanHang.Text.ToString() +
                    "')");
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    // Load lại dữ liệu trên DataGridView
                    LoadData();
                    // Thông báo
                    MessageBox.Show("Đã thêm xong!");
                }
                catch (SqlException)
                {
                    MessageBox.Show("Không thêm được. Lỗi rồi!");
                }
            }
            if (!Them)
            {
                try
                {
                    // Thực hiện lệnh
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    // Thứ tự dòng hiện hành
                    int r = dgvHOADON.CurrentCell.RowIndex;
                    // MaHD hiện hành
                    string strMAHD =
                    dgvHOADON.Rows[r].Cells[0].Value.ToString();
                    // Câu lệnh SQL
                    cmd.CommandText = System.String.Concat("Update HoaDon Set MaKH = '" +
                    this.cbMaKH.SelectedValue.ToString() + "', MaNV='"
                    + this.cbMaNV.SelectedValue.ToString() + "', NgayLapHD = '" +
                    this.txtNgayLapHD.Text.ToString() + "', NgayNhanHang = '" +
                    this.txtNgayNhanHang.Text.ToString() + "' Where MaHD = '" + strMAHD + "'");
                    // Cập nhật
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    // Load lại dữ liệu trên DataGridView
                    LoadData();
                    // Thông báo
                    MessageBox.Show("Đã sửa xong!");
                }
                catch (SqlException)
                {
                    MessageBox.Show("Không sửa được. Lỗi rồi!");
                }
            }
            // Đóng kết nối
            conn.Close();
        }
    }
}
