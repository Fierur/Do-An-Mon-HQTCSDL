using QLQB_ChucNang_QLNhanVien_va_LichLamViec;
using QLQB_ChucNang_QLNhanVien_va_LichLamViec.Database;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace QLQB_ChucNang_QLNhanVien_va_LichLamViec
{
    public partial class frmThongTinCaNhan : Form
    {
        private Label lblTitle;
        private GroupBox grpThongTin;
        private Label lblMaNV, lblTenNV, lblChucVu, lblNgaySinh;
        private Label lblGioiTinh, lblLuongMoiGio, lblMaCa, lblQuyen;
        private TextBox txtMaNV, txtTenNV, txtChucVu, txtNgaySinh;
        private TextBox txtGioiTinh, txtLuongMoiGio, txtMaCa, txtQuyen;
        private Label lblGioBD, lblGioKT;
        private TextBox txtGioBD, txtGioKT;
        private Button btnDong;

        public frmThongTinCaNhan()
        {
            InitializeComponent();
        }

        

        private Label CreateLabel(string text, int x, int y)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Location = new Point(x, y);
            lbl.AutoSize = true;
            lbl.Font = new Font("Segoe UI", 10);
            return lbl;
        }

        private TextBox CreateTextBox(int x, int y, bool readOnly)
        {
            TextBox txt = new TextBox();
            txt.Location = new Point(x, y);
            txt.Width = 200;
            txt.Font = new Font("Segoe UI", 10);
            txt.ReadOnly = readOnly;
            txt.BackColor = readOnly ? Color.WhiteSmoke : Color.White;
            return txt;
        }

        private void frmThongTinCaNhan_Load(object sender, EventArgs e)
        {
            LoadThongTinCaNhan();
        }

        private void LoadThongTinCaNhan()
        {
            try
            {
                // Sử dụng VIEW để load thông tin
                string query = "SELECT * FROM vw_ThongTinCaNhan";

                using (SqlConnection conn = DatabaseConnection.OpenConnection())
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        txtMaNV.Text = reader["MaNV"].ToString();
                        txtTenNV.Text = reader["TenNV"].ToString();
                        txtChucVu.Text = reader["ChucVu"].ToString();
                        txtNgaySinh.Text = Convert.ToDateTime(reader["NgaySinh"]).ToString("dd/MM/yyyy");
                        txtGioiTinh.Text = reader["GioiTinh"].ToString();

                        // Format lương
                        decimal luong = Convert.ToDecimal(reader["LuongMoiGio"]);
                        txtLuongMoiGio.Text = luong.ToString("N0") + " VNĐ";

                        txtMaCa.Text = reader["MaCa"].ToString();

                        // Format giờ
                        TimeSpan gioBD = (TimeSpan)reader["GioBD"];
                        TimeSpan gioKT = (TimeSpan)reader["GioKT"];
                        txtGioBD.Text = gioBD.ToString(@"hh\:mm");
                        txtGioKT.Text = gioKT.ToString(@"hh\:mm");

                        txtQuyen.Text = reader["TenQuyen"].ToString();

                        // Cập nhật title với tên
                        lblTitle.Text = $"THÔNG TIN CÁ NHÂN - {reader["TenNV"]}";
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy thông tin nhân viên!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}