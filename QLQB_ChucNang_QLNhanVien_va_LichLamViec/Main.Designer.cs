namespace QLQB_ChucNang_QLNhanVien_va_LichLamViec
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabMenu;
        private System.Windows.Forms.TabPage tabQuanLyNV;
        private System.Windows.Forms.TabPage tabChamCong;
        private System.Windows.Forms.TabPage tabTinhLuong;
        private System.Windows.Forms.TabPage tabLichLamViec;

        // Menu chính
        private System.Windows.Forms.Panel pnlMenuMain;
        private System.Windows.Forms.Button btnMenuQLNV;
        private System.Windows.Forms.Button btnMenuChamCong;
        private System.Windows.Forms.Button btnMenuTinhLuong;
        private System.Windows.Forms.Button btnMenuLichLV;

        // Tab Quản lý nhân viên
        private System.Windows.Forms.DataGridView dgvNhanVien;
        private System.Windows.Forms.Panel pnlQLNV;
        private System.Windows.Forms.Button btnQLNV_Them;
        private System.Windows.Forms.Button btnQLNV_Xoa;
        private System.Windows.Forms.Button btnQLNV_Sua;
        private System.Windows.Forms.Button btnQLNV_Save;
        private System.Windows.Forms.Button btnQLNV_Cancel;
        private System.Windows.Forms.Button btnQLNV_BackMenu;

        // Tab Chấm công
        private System.Windows.Forms.DataGridView dgvChamCong;
        private System.Windows.Forms.Panel pnlChamCong;
        private System.Windows.Forms.Button btnCC_Them;
        private System.Windows.Forms.Button btnCC_Xoa;
        private System.Windows.Forms.Button btnCC_Sua;
        private System.Windows.Forms.Button btnCC_Save;
        private System.Windows.Forms.Button btnCC_Cancel;
        private System.Windows.Forms.Button btnCC_BackMenu;
        private System.Windows.Forms.DateTimePicker dtpChamCong;
        private System.Windows.Forms.ComboBox cboNhanVienCC;

        // Tab Tính lương
        private System.Windows.Forms.DataGridView dgvTinhLuong;
        private System.Windows.Forms.Panel pnlTinhLuong;
        private System.Windows.Forms.Button btnTL_TinhLuong;
        private System.Windows.Forms.Button btnTL_Xoa;
        private System.Windows.Forms.Button btnTL_Save;
        private System.Windows.Forms.Button btnTL_Cancel;
        private System.Windows.Forms.Button btnTL_BackMenu;
        private System.Windows.Forms.ComboBox cboNhanVienTL;
        private System.Windows.Forms.NumericUpDown nudThang;
        private System.Windows.Forms.NumericUpDown nudNam;
        private System.Windows.Forms.NumericUpDown nudThuongPhat;

        // Tab Lịch làm việc
        private System.Windows.Forms.DataGridView dgvLichLamViec;
        private System.Windows.Forms.Panel pnlLichLV;
        private System.Windows.Forms.Button btnLLV_Them;
        private System.Windows.Forms.Button btnLLV_Xoa;
        private System.Windows.Forms.Button btnLLV_Sua;
        private System.Windows.Forms.Button btnLLV_Save;
        private System.Windows.Forms.Button btnLLV_Cancel;
        private System.Windows.Forms.Button btnLLV_BackMenu;

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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnLogout = new System.Windows.Forms.Button();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabMenu = new System.Windows.Forms.TabPage();
            this.pnlMenuMain = new System.Windows.Forms.Panel();
            this.btnMenuQLNV = new System.Windows.Forms.Button();
            this.btnMenuChamCong = new System.Windows.Forms.Button();
            this.btnMenuTinhLuong = new System.Windows.Forms.Button();
            this.btnMenuLichLV = new System.Windows.Forms.Button();
            this.tabQuanLyNV = new System.Windows.Forms.TabPage();
            this.tabChamCong = new System.Windows.Forms.TabPage();
            this.tabTinhLuong = new System.Windows.Forms.TabPage();
            this.tabLichLamViec = new System.Windows.Forms.TabPage();
            this.pnlHeader.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabMenu.SuspendLayout();
            this.pnlMenuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.pnlHeader.Controls.Add(this.btnLogout);
            this.pnlHeader.Controls.Add(this.lblWelcome);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(780, 70);
            this.pnlHeader.TabIndex = 0;
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(638, 18);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(130, 40);
            this.btnLogout.TabIndex = 1;
            this.btnLogout.Text = "Đăng xuất";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.White;
            this.lblWelcome.Location = new System.Drawing.Point(20, 20);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(145, 30);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Chào mừng, ";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabMenu);
            this.tabControl.Controls.Add(this.tabQuanLyNV);
            this.tabControl.Controls.Add(this.tabChamCong);
            this.tabControl.Controls.Add(this.tabTinhLuong);
            this.tabControl.Controls.Add(this.tabLichLamViec);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabControl.Location = new System.Drawing.Point(0, 70);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(780, 630);
            this.tabControl.TabIndex = 1;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabMenu
            // 
            this.tabMenu.Controls.Add(this.pnlMenuMain);
            this.tabMenu.Location = new System.Drawing.Point(4, 26);
            this.tabMenu.Name = "tabMenu";
            this.tabMenu.Padding = new System.Windows.Forms.Padding(3);
            this.tabMenu.Size = new System.Drawing.Size(772, 600);
            this.tabMenu.TabIndex = 0;
            this.tabMenu.Text = "Menu Chính";
            this.tabMenu.UseVisualStyleBackColor = true;
            // 
            // pnlMenuMain
            // 
            this.pnlMenuMain.Controls.Add(this.btnMenuQLNV);
            this.pnlMenuMain.Controls.Add(this.btnMenuChamCong);
            this.pnlMenuMain.Controls.Add(this.btnMenuTinhLuong);
            this.pnlMenuMain.Controls.Add(this.btnMenuLichLV);
            this.pnlMenuMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMenuMain.Location = new System.Drawing.Point(3, 3);
            this.pnlMenuMain.Name = "pnlMenuMain";
            this.pnlMenuMain.Size = new System.Drawing.Size(766, 594);
            this.pnlMenuMain.TabIndex = 0;
            // 
            // btnMenuQLNV
            // 
            this.btnMenuQLNV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnMenuQLNV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenuQLNV.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnMenuQLNV.ForeColor = System.Drawing.Color.White;
            this.btnMenuQLNV.Location = new System.Drawing.Point(152, 80);
            this.btnMenuQLNV.Name = "btnMenuQLNV";
            this.btnMenuQLNV.Size = new System.Drawing.Size(200, 200);
            this.btnMenuQLNV.TabIndex = 0;
            this.btnMenuQLNV.Text = "👥\r\n\r\nQuản Lý\r\nNhân Viên";
            this.btnMenuQLNV.UseVisualStyleBackColor = false;
            this.btnMenuQLNV.Click += new System.EventHandler(this.btnMenuQLNV_Click);
            // 
            // btnMenuChamCong
            // 
            this.btnMenuChamCong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnMenuChamCong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenuChamCong.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnMenuChamCong.ForeColor = System.Drawing.Color.White;
            this.btnMenuChamCong.Location = new System.Drawing.Point(420, 80);
            this.btnMenuChamCong.Name = "btnMenuChamCong";
            this.btnMenuChamCong.Size = new System.Drawing.Size(200, 200);
            this.btnMenuChamCong.TabIndex = 1;
            this.btnMenuChamCong.Text = "✓\r\n\r\nChấm Công";
            this.btnMenuChamCong.UseVisualStyleBackColor = false;
            this.btnMenuChamCong.Click += new System.EventHandler(this.btnMenuChamCong_Click);
            // 
            // btnMenuTinhLuong
            // 
            this.btnMenuTinhLuong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(196)))), ((int)(((byte)(15)))));
            this.btnMenuTinhLuong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenuTinhLuong.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnMenuTinhLuong.ForeColor = System.Drawing.Color.White;
            this.btnMenuTinhLuong.Location = new System.Drawing.Point(420, 329);
            this.btnMenuTinhLuong.Name = "btnMenuTinhLuong";
            this.btnMenuTinhLuong.Size = new System.Drawing.Size(200, 200);
            this.btnMenuTinhLuong.TabIndex = 2;
            this.btnMenuTinhLuong.Text = "💰\r\n\r\nTính Lương";
            this.btnMenuTinhLuong.UseVisualStyleBackColor = false;
            this.btnMenuTinhLuong.Click += new System.EventHandler(this.btnMenuTinhLuong_Click);
            // 
            // btnMenuLichLV
            // 
            this.btnMenuLichLV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.btnMenuLichLV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenuLichLV.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnMenuLichLV.ForeColor = System.Drawing.Color.White;
            this.btnMenuLichLV.Location = new System.Drawing.Point(152, 329);
            this.btnMenuLichLV.Name = "btnMenuLichLV";
            this.btnMenuLichLV.Size = new System.Drawing.Size(200, 200);
            this.btnMenuLichLV.TabIndex = 3;
            this.btnMenuLichLV.Text = "📅\r\n\r\nLịch Làm Việc";
            this.btnMenuLichLV.UseVisualStyleBackColor = false;
            this.btnMenuLichLV.Click += new System.EventHandler(this.btnMenuLichLV_Click);
            // 
            // tabQuanLyNV
            // 
            this.tabQuanLyNV.Location = new System.Drawing.Point(4, 26);
            this.tabQuanLyNV.Name = "tabQuanLyNV";
            this.tabQuanLyNV.Padding = new System.Windows.Forms.Padding(3);
            this.tabQuanLyNV.Size = new System.Drawing.Size(1192, 600);
            this.tabQuanLyNV.TabIndex = 1;
            this.tabQuanLyNV.Text = "Quản Lý Nhân Viên";
            this.tabQuanLyNV.UseVisualStyleBackColor = true;
            // 
            // tabChamCong
            // 
            this.tabChamCong.Location = new System.Drawing.Point(4, 26);
            this.tabChamCong.Name = "tabChamCong";
            this.tabChamCong.Size = new System.Drawing.Size(1192, 600);
            this.tabChamCong.TabIndex = 2;
            this.tabChamCong.Text = "Chấm Công";
            this.tabChamCong.UseVisualStyleBackColor = true;
            // 
            // tabTinhLuong
            // 
            this.tabTinhLuong.Location = new System.Drawing.Point(4, 26);
            this.tabTinhLuong.Name = "tabTinhLuong";
            this.tabTinhLuong.Size = new System.Drawing.Size(1192, 600);
            this.tabTinhLuong.TabIndex = 3;
            this.tabTinhLuong.Text = "Tính Lương";
            this.tabTinhLuong.UseVisualStyleBackColor = true;
            // 
            // tabLichLamViec
            // 
            this.tabLichLamViec.Location = new System.Drawing.Point(4, 26);
            this.tabLichLamViec.Name = "tabLichLamViec";
            this.tabLichLamViec.Size = new System.Drawing.Size(1192, 600);
            this.tabLichLamViec.TabIndex = 4;
            this.tabLichLamViec.Text = "Lịch Làm Việc";
            this.tabLichLamViec.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 700);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.pnlHeader);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hệ Thống Quản Lý Bar";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabMenu.ResumeLayout(false);
            this.pnlMenuMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }
}