using QLQB_ChucNang_QLNhanVien_va_LichLamViec.Database;
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

namespace QLQB_ChucNang_QLNhanVien_va_LichLamViec
{
    public partial class frmTongHopLuong : Form
    {
        private Panel pnlFilter;
        private Label lblThang, lblNam, lblTitle;
        private NumericUpDown nudThang, nudNam;
        private Button btnLoc, btnDong;
        private DataGridView dgvTongHop;
        private Panel pnlThongKe;
        private Label lblTongNV, lblLuongTB;
        public frmTongHopLuong()
        {
            InitializeComponent();
        }

        private void frmTongHopLuong_Load(object sender, EventArgs e)
        {
            LoadTongHopLuong((int)nudThang.Value, (int)nudNam.Value);
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            LoadTongHopLuong((int)nudThang.Value, (int)nudNam.Value);
        }

        private void LoadTongHopLuong(int thang, int nam)
        {
            try
            {
                string query = @"SELECT 
                                    nv.MaNV,
                                    nv.TenNV,
                                    nv.ChucVu,
                                    bl.TongNgayLamMotThang AS [Số Ngày],
                                    nv.LuongMoiGio AS [Lương/Giờ],
                                    bl.ThuongPhat AS [Thưởng/Phạt],
                                    bl.TongLuong AS [Tổng Lương],
                                    bl.NgayTinhLuong AS [Ngày Tính]
                                FROM NhanVien nv
                                LEFT JOIN BangLuong bl ON nv.MaNV = bl.MaNV 
                                    AND bl.Thang = @Thang 
                                    AND bl.Nam = @Nam
                                WHERE nv.TrangThai = N'Đang làm'
                                ORDER BY bl.TongLuong DESC, nv.MaNV";

                using (SqlConnection conn = DatabaseConnection.OpenConnection())
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvTongHop.DataSource = dt;

                    // Format columns
                    FormatColumns();

                    // Tính thống kê
                    CalculateStatistics(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatColumns()
        {
            if (dgvTongHop.Columns["Lương/Giờ"] != null)
            {
                dgvTongHop.Columns["Lương/Giờ"].DefaultCellStyle.Format = "N0";
                dgvTongHop.Columns["Lương/Giờ"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (dgvTongHop.Columns["Thưởng/Phạt"] != null)
            {
                dgvTongHop.Columns["Thưởng/Phạt"].DefaultCellStyle.Format = "N0";
                dgvTongHop.Columns["Thưởng/Phạt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (dgvTongHop.Columns["Tổng Lương"] != null)
            {
                dgvTongHop.Columns["Tổng Lương"].DefaultCellStyle.Format = "N0";
                dgvTongHop.Columns["Tổng Lương"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvTongHop.Columns["Tổng Lương"].DefaultCellStyle.Font = new Font(dgvTongHop.Font, FontStyle.Bold);
            }
            if (dgvTongHop.Columns["Ngày Tính"] != null)
            {
                dgvTongHop.Columns["Ngày Tính"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }

            // Color rows
            foreach (DataGridViewRow row in dgvTongHop.Rows)
            {
                if (row.Cells["Tổng Lương"].Value != DBNull.Value)
                {
                    decimal tongLuong = Convert.ToDecimal(row.Cells["Tổng Lương"].Value);
                    if (tongLuong > 0)
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(230, 247, 255);
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 245, 230);
                    }
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
                }
            }
        }

        private void CalculateStatistics(DataTable dt)
        {
            int tongNV = 0;
            decimal tongLuong = 0;
            decimal luongTB = 0;

            foreach (DataRow row in dt.Rows)
            {
                if (row["Tổng Lương"] != DBNull.Value)
                {
                    tongNV++;
                    tongLuong += Convert.ToDecimal(row["Tổng Lương"]);
                }
            }

            if (tongNV > 0)
                luongTB = tongLuong / tongNV;

            var lblTongNV = pnlThongKe.Controls.Find("lblTongNV", false)[0] as Label;
            var lblTongLuong = pnlThongKe.Controls.Find("lblTongLuong", false)[0] as Label;
            var lblLuongTB = pnlThongKe.Controls.Find("lblLuongTB", false)[0] as Label;

            if (lblTongNV != null)
                lblTongNV.Text = $"👥 Tổng NV đã tính lương: {tongNV}";

            if (lblTongLuong != null)
                lblTongLuong.Text = $"💰 Tổng lương: {tongLuong:N0} VNĐ";

            if (lblLuongTB != null)
                lblLuongTB.Text = $"📊 Lương TB: {luongTB:N0} VNĐ";
        }


    }
}
