using System.Drawing;
using System.Windows.Forms;

namespace QLQB_ChucNang_QLNhanVien_va_LichLamViec
{
    partial class frmThongTinCaNhan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Text = "Thông Tin Cá Nhân";
            this.Size = new Size(600, 550);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            // Title
            lblTitle = new Label();
            lblTitle.Text = "THÔNG TIN CÁ NHÂN";
            lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(41, 128, 185);
            lblTitle.Location = new Point(20, 20);
            lblTitle.AutoSize = true;

            // GroupBox
            grpThongTin = new GroupBox();
            grpThongTin.Text = "Chi tiết";
            grpThongTin.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            grpThongTin.Location = new Point(20, 70);
            grpThongTin.Size = new Size(540, 380);

            // Mã NV
            lblMaNV = CreateLabel("Mã NV:", 20, 30);
            txtMaNV = CreateTextBox(150, 27, true);

            // Tên NV
            lblTenNV = CreateLabel("Tên nhân viên:", 20, 70);
            txtTenNV = CreateTextBox(150, 67, true);

            // Chức vụ
            lblChucVu = CreateLabel("Chức vụ:", 20, 110);
            txtChucVu = CreateTextBox(150, 107, true);

            // Ngày sinh
            lblNgaySinh = CreateLabel("Ngày sinh:", 20, 150);
            txtNgaySinh = CreateTextBox(150, 147, true);

            // Giới tính
            lblGioiTinh = CreateLabel("Giới tính:", 20, 190);
            txtGioiTinh = CreateTextBox(150, 187, true);

            // Lương mỗi giờ
            lblLuongMoiGio = CreateLabel("Lương/giờ:", 20, 230);
            txtLuongMoiGio = CreateTextBox(150, 227, true);

            // Mã Ca
            lblMaCa = CreateLabel("Mã ca:", 20, 270);
            txtMaCa = CreateTextBox(150, 267, true);

            // Giờ bắt đầu
            lblGioBD = CreateLabel("Giờ bắt đầu:", 290, 270);
            txtGioBD = CreateTextBox(380, 267, true);
            txtGioBD.Width = 140;

            // Giờ kết thúc
            lblGioKT = CreateLabel("Giờ kết thúc:", 290, 310);
            txtGioKT = CreateTextBox(380, 307, true);
            txtGioKT.Width = 140;

            // Quyền
            lblQuyen = CreateLabel("Quyền:", 20, 310);
            txtQuyen = CreateTextBox(150, 307, true);

            // Add to GroupBox
            grpThongTin.Controls.AddRange(new Control[] {
                lblMaNV, txtMaNV, lblTenNV, txtTenNV,
                lblChucVu, txtChucVu, lblNgaySinh, txtNgaySinh,
                lblGioiTinh, txtGioiTinh, lblLuongMoiGio, txtLuongMoiGio,
                lblMaCa, txtMaCa, lblGioBD, txtGioBD, lblGioKT, txtGioKT,
                lblQuyen, txtQuyen
            });

            // Button Đóng
            btnDong = new Button();
            btnDong.Text = "Đóng";
            btnDong.Location = new Point(240, 470);
            btnDong.Size = new Size(120, 40);
            btnDong.BackColor = Color.FromArgb(52, 152, 219);
            btnDong.ForeColor = Color.White;
            btnDong.FlatStyle = FlatStyle.Flat;
            btnDong.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnDong.Cursor = Cursors.Hand;
            btnDong.Click += (s, e) => this.Close();

            // Add to Form
            this.Controls.AddRange(new Control[] {
                lblTitle, grpThongTin, btnDong
            });

            this.Load += frmThongTinCaNhan_Load;
        }

        #endregion
    }
}