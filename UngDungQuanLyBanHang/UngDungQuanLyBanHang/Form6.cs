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
    public partial class Form6 : Form
    {
        // Chuỗi kết nối
        string strConnectionString = @"Data Source=.\SQLEXPRESS2008;Initial Catalog=QuanLyBanHang;Integrated Security = True";
        // Đối tượng kết nối
        SqlConnection conn = null;
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
                // Vận chuyển dữ liệu lên DataTable dtNhanVien
                daNhanVien = new SqlDataAdapter("SELECT * FROM NHANVIEN", conn);
                dtNhanVien = new DataTable();
                dtNhanVien.Clear();
                daNhanVien.Fill(dtNhanVien);
                // Đưa dữ liệu lên DataGridView 
                dgvNHANVIEN.DataSource = dtNhanVien;
                // Xóa trống các đối tượng trong Panel
                this.txtMaNV.ResetText();
                this.txtHo.ResetText();
                this.txtTen.ResetText();
                this.chkNu.Checked = false;
                this.txtNgayNV.ResetText();
                this.txtDiaChi.ResetText();
                this.txtDienThoai.ResetText();
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
                MessageBox.Show("Không lấy được nội dung trong table THANHPHO.Lỗi rồi!!!");
            }
        }

        public Form6()
        {
            InitializeComponent();
        }

        private void btnTroVe_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void Form6_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Giải phóng tài nguyên
            dtNhanVien.Dispose();
            dtNhanVien = null;
            // Hủy kết nối
            conn = null;
        }

        private void btnReLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // Kich hoạt biến Them
            Them = true;
            // Xóa trống các đối tượng trong Panel
            this.txtMaNV.ResetText();
            this.txtHo.ResetText();
            this.txtTen.ResetText();
            this.chkNu.Checked = false;
            this.txtNgayNV.ResetText();
            this.txtDiaChi.ResetText();
            this.txtDienThoai.ResetText();
            // Cho thao tác trên các nút Lưu / Hủy / Panel
            this.btnLuu.Enabled = true;
            this.btnHuyBo.Enabled = true;
            this.panel.Enabled = true;
            // Không cho thao tác trên các nút Thêm / Xóa / Thoát
            this.btnThem.Enabled = false;
            this.btnSua.Enabled = false;
            this.btnXoa.Enabled = false;
            this.btnTroVe.Enabled = false;
            // Đưa con trỏ đến TextField txtMaNV
            this.txtMaNV.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Kích hoạt biến Sửa
            Them = false;
            // Cho phép thao tác trên Panel
            this.panel.Enabled = true;
            // Thứ tự dòng hiện hành
            int r = dgvNHANVIEN.CurrentCell.RowIndex;
            //Chuyển giá kiểu int nhận được từ SQL lên thành boolean
            bool nu = false;
            if (dgvNHANVIEN.Rows[r].Cells[3].Value.ToString().Equals("1")) nu = true;
            // Chuyển thông tin lên panel
            this.txtMaNV.Text =
                dgvNHANVIEN.Rows[r].Cells[0].Value.ToString();
            this.txtHo.Text =
                dgvNHANVIEN.Rows[r].Cells[1].Value.ToString();
            this.txtTen.Text =
                dgvNHANVIEN.Rows[r].Cells[2].Value.ToString();
            this.chkNu.Checked = nu;
            this.txtNgayNV.Text =
                dgvNHANVIEN.Rows[r].Cells[4].Value.ToString();
            this.txtDiaChi.Text =
                dgvNHANVIEN.Rows[r].Cells[5].Value.ToString();
            this.txtDienThoai.Text =
                dgvNHANVIEN.Rows[r].Cells[6].Value.ToString();
            // Cho thao tác trên các nút Lưu / Hủy / Panel
            this.btnLuu.Enabled = true;
            this.btnHuyBo.Enabled = true;
            this.panel.Enabled = true;
            // Không cho thao tác trên các nút Thêm / Xóa / Thoát
            this.btnThem.Enabled = false;
            this.btnSua.Enabled = false;
            this.btnXoa.Enabled = false;
            this.btnTroVe.Enabled = false;
            // Đưa con trỏ đến TextField txtMaNV
            this.txtMaNV.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Mở kết nối
            conn.Open();
            //Chuyển dữ liệu từ kiểu bool của chkNu sang kiểu int để truyền vào SQL.
            int nu = 0;
            if (this.chkNu.Checked == true) nu = 1;
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
                    cmd.CommandText = System.String.Concat("Insert Into NhanVien (MaNV,Ho,Ten,Nu,NgayNV,DiaChi,DienThoai) Values(" + "'" +
                    this.txtMaNV.Text.ToString() + "',N'" +
                    this.txtHo.Text.ToString() + "',N'" +
                    this.txtTen.Text.ToString() + "','" +
                    nu + "','" +
                    this.txtNgayNV.Text.ToString() + "',N'" +
                    this.txtDiaChi.Text.ToString() +
                    "','" + this.txtDienThoai.Text.ToString() +
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
                    int r = dgvNHANVIEN.CurrentCell.RowIndex;
                    // MaNV hiện hành
                    string strMANV =
                    dgvNHANVIEN.Rows[r].Cells[0].Value.ToString();
                    // Câu lệnh SQL
                    cmd.CommandText = System.String.Concat("Update NhanVien Set Ho = N'" +
                    this.txtHo.Text.ToString() + "', Ten= N'"
                    + this.txtTen.Text.ToString() + "', Nu = '" +
                    nu + "', NgayNV = '"+
                    this.txtNgayNV.Text.ToString() + "', DiaChi = N'"+
                    this.txtDiaChi.Text.ToString() + "', DienThoai = '" +
                    this.txtDienThoai.Text.ToString() + "' Where MaNV = '" + strMANV + "'");
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

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            // Xóa trống các đối tượng trong Panel
            this.txtMaNV.ResetText();
            this.txtHo.ResetText();
            this.txtTen.ResetText();
            this.chkNu.Checked = false;
            this.txtNgayNV.ResetText();
            this.txtDiaChi.ResetText();
            this.txtDienThoai.ResetText();
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
                int r = dgvNHANVIEN.CurrentCell.RowIndex;
                // Lấy MaNV của record hiện hành
                string strMANV =
                dgvNHANVIEN.Rows[r].Cells[0].Value.ToString();
                // Viết câu lệnh SQL
                cmd.CommandText = System.String.Concat("Delete From NhanVien Where MaNV = '" + strMANV + "'");
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
    }
}
