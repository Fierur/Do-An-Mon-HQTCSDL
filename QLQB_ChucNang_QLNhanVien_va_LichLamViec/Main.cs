using QLQB_ChucNang_QLNhanVien_va_LichLamViec;
using QLQB_ChucNang_QLNhanVien_va_LichLamViec.Database;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QLQB_ChucNang_QLNhanVien_va_LichLamViec
{
    public partial class frmMain : Form
    {
        private DataTable dtNhanVien;
        private DataTable dtChamCong;
        private DataTable dtTinhLuong;
        private DataTable dtLichLamViec;
        private bool isDataChanged = false;

        // Thêm button xem thông tin cá nhân
        private Button btnThongTinCaNhan;

        public frmMain()
        {
            InitializeComponent();
            InitializeTabControls();
            InitializeThongTinCaNhanButton();
        }

        private void InitializeThongTinCaNhanButton()
        {
            // Thêm nút xem thông tin cá nhân ở góc phải header
            btnThongTinCaNhan = new Button();
            btnThongTinCaNhan.Text = "👤 Thông tin";
            btnThongTinCaNhan.Location = new Point(pnlHeader.Width - 280, 18);
            btnThongTinCaNhan.Size = new Size(130, 40);
            btnThongTinCaNhan.BackColor = Color.FromArgb(52, 152, 219);
            btnThongTinCaNhan.ForeColor = Color.White;
            btnThongTinCaNhan.FlatStyle = FlatStyle.Flat;
            btnThongTinCaNhan.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnThongTinCaNhan.Cursor = Cursors.Hand;
            btnThongTinCaNhan.Click += btnThongTinCaNhan_Click;
            pnlHeader.Controls.Add(btnThongTinCaNhan);
        }

        private void btnThongTinCaNhan_Click(object sender, EventArgs e)
        {
            frmThongTinCaNhan formThongTin = new frmThongTinCaNhan();
            formThongTin.ShowDialog();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"Chào mừng, {SessionInfo.TenNV} ({SessionInfo.TenQuyen})";
            tabControl.SelectedTab = tabMenu;
            CheckPermissions();
        }

        private void CheckPermissions()
        {
            if (!SessionInfo.IsAdmin)
            {
                btnMenuQLNV.Visible = false;
                btnMenuTinhLuong.Visible = false;
                btnMenuLichLV.Visible = false;
                tabControl.SelectedTab = tabChamCong;
                tabControl.TabPages.Remove(tabMenu);
                tabControl.TabPages.Remove(tabQuanLyNV);
                tabControl.TabPages.Remove(tabTinhLuong);
                tabControl.TabPages.Remove(tabLichLamViec);
            }
        }

        private void InitializeTabControls()
        {
            InitializeQLNVTab();
            InitializeChamCongTab();
            InitializeTinhLuongTab();
            InitializeLichLamViecTab();
        }

        #region Menu Navigation
        private void btnMenuQLNV_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabQuanLyNV;
        }

        private void btnMenuChamCong_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabChamCong;
        }

        private void btnMenuTinhLuong_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabTinhLuong;
        }

        private void btnMenuLichLV_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabLichLamViec;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SessionInfo.Clear();
                DatabaseConnection.ClearConnection();
                this.Close();
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabQuanLyNV)
                LoadNhanVienData();
            else if (tabControl.SelectedTab == tabChamCong)
                LoadChamCongData();
            else if (tabControl.SelectedTab == tabTinhLuong)
                LoadTinhLuongData();
            else if (tabControl.SelectedTab == tabLichLamViec)
                LoadLichLamViecData();
        }
        #endregion

        #region Quản Lý Nhân Viên
        private void InitializeQLNVTab()
        {
            pnlQLNV = new Panel();
            pnlQLNV.Dock = DockStyle.Bottom;
            pnlQLNV.Height = 60;
            pnlQLNV.BackColor = Color.WhiteSmoke;

            btnQLNV_Them = CreateButton("Thêm", 20);
            btnQLNV_Xoa = CreateButton("Xóa", 140);
            btnQLNV_Sua = CreateButton("Sửa", 260);
            btnQLNV_Save = CreateButton("Lưu", 380);
            btnQLNV_Cancel = CreateButton("Hủy", 500);
            btnQLNV_BackMenu = CreateButton("← Menu", 620);

            pnlQLNV.Controls.AddRange(new Control[] {
                btnQLNV_Them, btnQLNV_Xoa, btnQLNV_Sua,
                btnQLNV_Save, btnQLNV_Cancel, btnQLNV_BackMenu
            });

            dgvNhanVien = new DataGridView();
            dgvNhanVien.Dock = DockStyle.Fill;
            dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNhanVien.MultiSelect = false;
            dgvNhanVien.ReadOnly = true;
            dgvNhanVien.AllowUserToAddRows = false;

            tabQuanLyNV.Controls.Add(dgvNhanVien);
            tabQuanLyNV.Controls.Add(pnlQLNV);

            btnQLNV_Them.Click += btnQLNV_Them_Click;
            btnQLNV_Xoa.Click += btnQLNV_Xoa_Click;
            btnQLNV_Sua.Click += btnQLNV_Sua_Click;
            btnQLNV_Save.Click += btnQLNV_Save_Click;
            btnQLNV_Cancel.Click += btnQLNV_Cancel_Click;
            btnQLNV_BackMenu.Click += (s, ev) => tabControl.SelectedTab = tabMenu;
        }

        private void LoadNhanVienData()
        {
            try
            {
                string query = @"SELECT MaNV, TenNV, ChucVu, NgaySinh, GioiTinh, MatKhau, 
                                LuongMoiGio, MaQuyen, TrangThai 
                                FROM NhanVien 
                                ORDER BY MaNV";

                using (SqlConnection conn = DatabaseConnection.OpenConnection())
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    dtNhanVien = new DataTable();
                    adapter.Fill(dtNhanVien);
                    dgvNhanVien.DataSource = dtNhanVien;

                    if (dgvNhanVien.Columns["NgaySinh"] != null)
                        dgvNhanVien.Columns["NgaySinh"].DefaultCellStyle.Format = "dd/MM/yyyy";

                    if (dgvNhanVien.Columns["LuongMoiGio"] != null)
                    {
                        dgvNhanVien.Columns["LuongMoiGio"].DefaultCellStyle.Format = "N0";
                        dgvNhanVien.Columns["LuongMoiGio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }

                isDataChanged = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnQLNV_Them_Click(object sender, EventArgs e)
        {
            if (!SessionInfo.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền thêm nhân viên!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (frmNhanVienDialog dialog = new frmNhanVienDialog(false))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    DataRow newRow = dtNhanVien.NewRow();
                    newRow["MaNV"] = dialog.MaNV;
                    newRow["TenNV"] = dialog.TenNV;
                    newRow["ChucVu"] = dialog.ChucVu;
                    newRow["NgaySinh"] = dialog.NgaySinh;
                    newRow["GioiTinh"] = dialog.GioiTinh;
                    newRow["MatKhau"] = dialog.MatKhau;
                    newRow["LuongMoiGio"] = dialog.LuongMoiGio;
                    newRow["MaQuyen"] = dialog.MaQuyen;
                    newRow["TrangThai"] = dialog.TrangThai;

                    dtNhanVien.Rows.Add(newRow);
                    isDataChanged = true;

                    MessageBox.Show("Đã thêm nhân viên vào danh sách!\nNhấn 'Lưu' để cập nhật database.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnQLNV_Xoa_Click(object sender, EventArgs e)
        {
            if (!SessionInfo.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền xóa nhân viên!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvNhanVien.SelectedRows.Count > 0)
            {
                string maNV = dgvNhanVien.SelectedRows[0].Cells["MaNV"].Value.ToString();
                string tenNV = dgvNhanVien.SelectedRows[0].Cells["TenNV"].Value.ToString();

                if (MessageBox.Show($"Bạn có chắc muốn xóa:\n{maNV} - {tenNV}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dgvNhanVien.Rows.RemoveAt(dgvNhanVien.SelectedRows[0].Index);
                    isDataChanged = true;
                    MessageBox.Show("Đã xóa! Nhấn 'Lưu' để cập nhật database.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnQLNV_Sua_Click(object sender, EventArgs e)
        {
            if (!SessionInfo.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvNhanVien.SelectedRows.Count > 0)
            {
                DataRow selectedRow = ((DataRowView)dgvNhanVien.SelectedRows[0].DataBoundItem).Row;

                using (frmNhanVienDialog dialog = new frmNhanVienDialog(true, selectedRow))
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        selectedRow["TenNV"] = dialog.TenNV;
                        selectedRow["ChucVu"] = dialog.ChucVu;
                        selectedRow["NgaySinh"] = dialog.NgaySinh;
                        selectedRow["GioiTinh"] = dialog.GioiTinh;
                        selectedRow["MatKhau"] = dialog.MatKhau;
                        selectedRow["LuongMoiGio"] = dialog.LuongMoiGio;
                        selectedRow["MaQuyen"] = dialog.MaQuyen;
                        selectedRow["TrangThai"] = dialog.TrangThai;

                        isDataChanged = true;
                        dgvNhanVien.Refresh();
                        MessageBox.Show("Đã cập nhật! Nhấn 'Lưu' để lưu vào database.",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnQLNV_Save_Click(object sender, EventArgs e)
        {
            if (!isDataChanged)
            {
                MessageBox.Show("Không có thay đổi!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Lưu các thay đổi?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int successCount = 0;
                int errorCount = 0;
                string errorMessages = "";

                try
                {
                    using (SqlConnection conn = DatabaseConnection.OpenConnection())
                    {
                        foreach (DataRow row in dtNhanVien.Rows)
                        {
                            try
                            {
                                if (row.RowState == DataRowState.Added)
                                {
                                    SqlCommand cmd = new SqlCommand("sp_ThemNhanVienMoi", conn);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@MaNV", row["MaNV"]);
                                    cmd.Parameters.AddWithValue("@TenNV", row["TenNV"]);
                                    cmd.Parameters.AddWithValue("@ChucVu", row["ChucVu"]);
                                    cmd.Parameters.AddWithValue("@NgaySinh", row["NgaySinh"]);
                                    cmd.Parameters.AddWithValue("@GioiTinh", row["GioiTinh"]);
                                    cmd.Parameters.AddWithValue("@LuongMoiGio", row["LuongMoiGio"]);
                                    cmd.Parameters.AddWithValue("@MatKhau", row["MatKhau"]);
                                    cmd.Parameters.AddWithValue("@MaQuyen", row["MaQuyen"]);
                                    cmd.Parameters.AddWithValue("@TrangThai", row["TrangThai"]);
                                    cmd.ExecuteNonQuery();

                                    try
                                    {
                                        SqlCommand cmdAccount = new SqlCommand("sp_TaoTaiKhoanNhanVien", conn);
                                        cmdAccount.CommandType = CommandType.StoredProcedure;
                                        cmdAccount.Parameters.AddWithValue("@MaNV", row["MaNV"]);
                                        cmdAccount.ExecuteNonQuery();
                                    }
                                    catch { }

                                    successCount++;
                                }
                                else if (row.RowState == DataRowState.Modified)
                                {
                                    string updateQuery = @"UPDATE NhanVien SET 
                                        TenNV=@TenNV, ChucVu=@ChucVu, NgaySinh=@NgaySinh,
                                        GioiTinh=@GioiTinh, MatKhau=@MatKhau, 
                                        LuongMoiGio=@LuongMoiGio, MaQuyen=@MaQuyen, TrangThai=@TrangThai
                                        WHERE MaNV=@MaNV";

                                    SqlCommand cmd = new SqlCommand(updateQuery, conn);
                                    cmd.Parameters.AddWithValue("@MaNV", row["MaNV"]);
                                    cmd.Parameters.AddWithValue("@TenNV", row["TenNV"]);
                                    cmd.Parameters.AddWithValue("@ChucVu", row["ChucVu"]);
                                    cmd.Parameters.AddWithValue("@NgaySinh", row["NgaySinh"]);
                                    cmd.Parameters.AddWithValue("@GioiTinh", row["GioiTinh"]);
                                    cmd.Parameters.AddWithValue("@MatKhau", row["MatKhau"]);
                                    cmd.Parameters.AddWithValue("@LuongMoiGio", row["LuongMoiGio"]);
                                    cmd.Parameters.AddWithValue("@MaQuyen", row["MaQuyen"]);
                                    cmd.Parameters.AddWithValue("@TrangThai", row["TrangThai"]);
                                    cmd.ExecuteNonQuery();
                                    successCount++;
                                }
                                else if (row.RowState == DataRowState.Deleted)
                                {
                                    string maNV = row["MaNV", DataRowVersion.Original].ToString();
                                    SqlCommand cmd = new SqlCommand("sp_XoaNhanVien", conn);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                                    cmd.ExecuteNonQuery();
                                    successCount++;
                                }
                            }
                            catch (SqlException sqlEx)
                            {
                                errorCount++;
                                string maNV = row.RowState == DataRowState.Deleted
                                    ? row["MaNV", DataRowVersion.Original].ToString()
                                    : row["MaNV"].ToString();
                                errorMessages += $"\n- {maNV}: {sqlEx.Message}";
                            }
                        }
                    }

                    if (errorCount == 0)
                        MessageBox.Show($"Lưu thành công! ({successCount} bản ghi)", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show($"Hoàn tất với lỗi:\n- Thành công: {successCount}\n- Lỗi: {errorCount}{errorMessages}",
                            "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    isDataChanged = false;
                    LoadNhanVienData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnQLNV_Cancel_Click(object sender, EventArgs e)
        {
            if (isDataChanged)
            {
                if (MessageBox.Show("Hủy thay đổi?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    LoadNhanVienData();
            }
        }
        #endregion

        #region Chấm Công
        private void InitializeChamCongTab()
        {
            Panel pnlCCInput = new Panel();
            pnlCCInput.Dock = DockStyle.Top;
            pnlCCInput.Height = 80;
            pnlCCInput.BackColor = Color.WhiteSmoke;

            Label lblNhanVien = new Label();
            lblNhanVien.Text = "Nhân viên:";
            lblNhanVien.Location = new Point(20, 25);
            lblNhanVien.AutoSize = true;

            cboNhanVienCC = new ComboBox();
            cboNhanVienCC.Location = new Point(120, 22);
            cboNhanVienCC.Width = 200;
            cboNhanVienCC.DropDownStyle = ComboBoxStyle.DropDownList;

            Label lblNgay = new Label();
            lblNgay.Text = "Ngày:";
            lblNgay.Location = new Point(350, 25);
            lblNgay.AutoSize = true;

            dtpChamCong = new DateTimePicker();
            dtpChamCong.Location = new Point(410, 22);
            dtpChamCong.Width = 200;
            dtpChamCong.Format = DateTimePickerFormat.Short;

            Label lblLocThang = new Label();
            lblLocThang.Text = "Lọc tháng:";
            lblLocThang.Location = new Point(650, 25);
            lblLocThang.AutoSize = true;

            NumericUpDown nudLocThang = new NumericUpDown();
            nudLocThang.Name = "nudLocThang";
            nudLocThang.Location = new Point(730, 22);
            nudLocThang.Width = 60;
            nudLocThang.Minimum = 1;
            nudLocThang.Maximum = 12;
            nudLocThang.Value = DateTime.Now.Month;

            Label lblLocNam = new Label();
            lblLocNam.Text = "Năm:";
            lblLocNam.Location = new Point(810, 25);
            lblLocNam.AutoSize = true;

            NumericUpDown nudLocNam = new NumericUpDown();
            nudLocNam.Name = "nudLocNam";
            nudLocNam.Location = new Point(860, 22);
            nudLocNam.Width = 80;
            nudLocNam.Minimum = 2020;
            nudLocNam.Maximum = 2100;
            nudLocNam.Value = DateTime.Now.Year;

            Button btnLocChamCong = new Button();
            btnLocChamCong.Text = "Lọc";
            btnLocChamCong.Location = new Point(960, 20);
            btnLocChamCong.Size = new Size(80, 27);
            btnLocChamCong.BackColor = Color.FromArgb(52, 152, 219);
            btnLocChamCong.ForeColor = Color.White;
            btnLocChamCong.FlatStyle = FlatStyle.Flat;
            btnLocChamCong.Click += (s, ev) => {
                LoadChamCongDataFiltered((int)nudLocThang.Value, (int)nudLocNam.Value);
            };

            Button btnHuyLoc = new Button();
            btnHuyLoc.Text = "Hủy lọc";
            btnHuyLoc.Location = new Point(1050, 20);
            btnHuyLoc.Size = new Size(80, 27);
            btnHuyLoc.BackColor = Color.FromArgb(189, 195, 199);
            btnHuyLoc.ForeColor = Color.White;
            btnHuyLoc.FlatStyle = FlatStyle.Flat;
            btnHuyLoc.Click += (s, ev) => LoadChamCongData();

            if (SessionInfo.IsAdmin)
            {
                pnlCCInput.Controls.AddRange(new Control[] {
                    lblNhanVien, cboNhanVienCC, lblNgay, dtpChamCong,
                    lblLocThang, nudLocThang, lblLocNam, nudLocNam,
                    btnLocChamCong, btnHuyLoc
                });
            }
            else
            {
                pnlCCInput.Controls.AddRange(new Control[] {
                    lblNgay, dtpChamCong,
                    lblLocThang, nudLocThang, lblLocNam, nudLocNam,
                    btnLocChamCong, btnHuyLoc
                });
            }

            pnlChamCong = new Panel();
            pnlChamCong.Dock = DockStyle.Bottom;
            pnlChamCong.Height = 60;
            pnlChamCong.BackColor = Color.WhiteSmoke;

            btnCC_Them = CreateButton("Chấm công", 20);
            btnCC_Xoa = CreateButton("Xóa", 140);
            btnCC_BackMenu = CreateButton("← Menu", 620);

            pnlChamCong.Controls.AddRange(new Control[] {
                btnCC_Them, btnCC_Xoa, btnCC_BackMenu
            });

            dgvChamCong = new DataGridView();
            dgvChamCong.Dock = DockStyle.Fill;
            dgvChamCong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChamCong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChamCong.ReadOnly = true;
            dgvChamCong.AllowUserToAddRows = false;

            tabChamCong.Controls.Add(dgvChamCong);
            tabChamCong.Controls.Add(pnlCCInput);
            tabChamCong.Controls.Add(pnlChamCong);

            btnCC_Them.Click += btnCC_Them_Click;
            btnCC_Xoa.Click += btnCC_Xoa_Click;
            btnCC_BackMenu.Click += (s, ev) => tabControl.SelectedTab = tabMenu;
        }

        private void LoadChamCongData()
        {
            try
            {
                string query;

                if (!SessionInfo.IsAdmin)
                {
                    query = @"SELECT NgayDiLam, MaNV, TenNV, ChucVu, SoGioLam
                             FROM vw_ChamCongCaNhan
                             ORDER BY NgayDiLam DESC";
                }
                else
                {
                    query = @"SELECT cc.NgayDiLam, cc.MaNV, nv.TenNV, nv.ChucVu, 8 AS SoGioLam
                             FROM ChamCong cc
                             INNER JOIN NhanVien nv ON cc.MaNV = nv.MaNV
                             ORDER BY cc.NgayDiLam DESC";
                }

                using (SqlConnection conn = DatabaseConnection.OpenConnection())
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    dtChamCong = new DataTable();
                    adapter.Fill(dtChamCong);
                    dgvChamCong.DataSource = dtChamCong;

                    if (dgvChamCong.Columns["NgayDiLam"] != null)
                        dgvChamCong.Columns["NgayDiLam"].DefaultCellStyle.Format = "dd/MM/yyyy";

                    if (dgvChamCong.Columns["SoGioLam"] != null)
                        dgvChamCong.Columns["SoGioLam"].HeaderText = "Giờ Làm (8h/ngày)";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lọc: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCC_Them_Click(object sender, EventArgs e)
        {
            string maNV;

            if (!SessionInfo.IsAdmin)
            {
                maNV = SessionInfo.MaNV;
            }
            else
            {
                if (cboNhanVienCC.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                dynamic selectedItem = cboNhanVienCC.SelectedItem;
                maNV = selectedItem.Value;
            }

            try
            {
                DateTime ngay = dtpChamCong.Value.Date;

                using (SqlConnection conn = DatabaseConnection.OpenConnection())
                {
                    SqlCommand checkCmd = new SqlCommand(
                        "SELECT COUNT(*) FROM ChamCong WHERE MaNV=@MaNV AND NgayDiLam=@Ngay", conn);
                    checkCmd.Parameters.AddWithValue("@MaNV", maNV);
                    checkCmd.Parameters.AddWithValue("@Ngay", ngay);

                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("Đã chấm công ngày này!\n(Một ngày chỉ chấm công 1 lần = 8 giờ)",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO ChamCong (NgayDiLam, MaNV) VALUES (@Ngay, @MaNV)", conn);
                    cmd.Parameters.AddWithValue("@Ngay", ngay);
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("✓ Chấm công thành công!\n(Tính 8 giờ làm việc)", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadChamCongData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCC_Xoa_Click(object sender, EventArgs e)
        {
            if (dgvChamCong.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Xóa bản ghi chấm công này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        string maNV = dgvChamCong.SelectedRows[0].Cells["MaNV"].Value.ToString();
                        DateTime ngay = Convert.ToDateTime(dgvChamCong.SelectedRows[0].Cells["NgayDiLam"].Value);

                        using (SqlConnection conn = DatabaseConnection.OpenConnection())
                        {
                            SqlCommand cmd = new SqlCommand(
                                "DELETE FROM ChamCong WHERE MaNV=@MaNV AND NgayDiLam=@Ngay", conn);
                            cmd.Parameters.AddWithValue("@MaNV", maNV);
                            cmd.Parameters.AddWithValue("@Ngay", ngay);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Xóa thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadChamCongData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        #endregion

        #region Tính Lương
        private void InitializeTinhLuongTab()
        {
            Panel pnlTLInput = new Panel();
            pnlTLInput.Dock = DockStyle.Top;
            pnlTLInput.Height = 120;
            pnlTLInput.BackColor = Color.WhiteSmoke;

            Label lblNV = new Label();
            lblNV.Text = "Nhân viên:";
            lblNV.Location = new Point(20, 15);
            lblNV.AutoSize = true;

            cboNhanVienTL = new ComboBox();
            cboNhanVienTL.Location = new Point(120, 12);
            cboNhanVienTL.Width = 250;
            cboNhanVienTL.DropDownStyle = ComboBoxStyle.DropDownList;

            Label lblThang = new Label();
            lblThang.Text = "Tháng:";
            lblThang.Location = new Point(400, 15);
            lblThang.AutoSize = true;

            nudThang = new NumericUpDown();
            nudThang.Location = new Point(470, 12);
            nudThang.Width = 70;
            nudThang.Minimum = 1;
            nudThang.Maximum = 12;
            nudThang.Value = DateTime.Now.Month;

            Label lblNam = new Label();
            lblNam.Text = "Năm:";
            lblNam.Location = new Point(560, 15);
            lblNam.AutoSize = true;

            nudNam = new NumericUpDown();
            nudNam.Location = new Point(610, 12);
            nudNam.Width = 80;
            nudNam.Minimum = 2020;
            nudNam.Maximum = 2100;
            nudNam.Value = DateTime.Now.Year;

            Label lblThuongPhat = new Label();
            lblThuongPhat.Text = "Thưởng/Phạt:";
            lblThuongPhat.Location = new Point(20, 55);
            lblThuongPhat.AutoSize = true;

            nudThuongPhat = new NumericUpDown();
            nudThuongPhat.Location = new Point(120, 52);
            nudThuongPhat.Width = 150;
            nudThuongPhat.Minimum = -10000000;
            nudThuongPhat.Maximum = 10000000;
            nudThuongPhat.DecimalPlaces = 0;
            nudThuongPhat.ThousandsSeparator = true;

            Label lblNote = new Label();
            lblNote.Text = "* Công thức: Tổng lương = Số ngày làm × 8 giờ × Lương/giờ + Thưởng/Phạt";
            lblNote.Location = new Point(20, 85);
            lblNote.AutoSize = true;
            lblNote.ForeColor = Color.FromArgb(52, 152, 219);
            lblNote.Font = new Font("Segoe UI", 9, FontStyle.Italic);

            pnlTLInput.Controls.AddRange(new Control[] {
                lblNV, cboNhanVienTL, lblThang, nudThang,
                lblNam, nudNam, lblThuongPhat, nudThuongPhat, lblNote
            });

            pnlTinhLuong = new Panel();
            pnlTinhLuong.Dock = DockStyle.Bottom;
            pnlTinhLuong.Height = 60;
            pnlTinhLuong.BackColor = Color.WhiteSmoke;

            btnTL_TinhLuong = CreateButton("Tính lương", 20);
            btnTL_Xoa = CreateButton("Xóa", 140);
            btnTL_BackMenu = CreateButton("← Menu", 620);

            pnlTinhLuong.Controls.AddRange(new Control[] {
                btnTL_TinhLuong, btnTL_Xoa, btnTL_BackMenu
            });

            dgvTinhLuong = new DataGridView();
            dgvTinhLuong.Dock = DockStyle.Fill;
            dgvTinhLuong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTinhLuong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTinhLuong.ReadOnly = true;
            dgvTinhLuong.AllowUserToAddRows = false;

            tabTinhLuong.Controls.Add(dgvTinhLuong);
            tabTinhLuong.Controls.Add(pnlTLInput);
            tabTinhLuong.Controls.Add(pnlTinhLuong);

            btnTL_TinhLuong.Click += btnTL_TinhLuong_Click;
            btnTL_Xoa.Click += btnTL_Xoa_Click;
            btnTL_BackMenu.Click += (s, ev) => tabControl.SelectedTab = tabMenu;
        }

        private void LoadTinhLuongData()
        {
            try
            {
                string query = @"SELECT bl.MaLuong, nv.MaNV, nv.TenNV, nv.ChucVu,
                                bl.TongNgayLamMotThang AS [Số Ngày], 
                                nv.LuongMoiGio AS [Lương/Giờ],
                                bl.ThuongPhat AS [Thưởng/Phạt], 
                                bl.TongLuong AS [Tổng Lương]
                                FROM BangLuong bl
                                INNER JOIN NhanVien nv ON bl.MaLuong = nv.MaLuong
                                ORDER BY bl.MaLuong DESC";

                using (SqlConnection conn = DatabaseConnection.OpenConnection())
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    dtTinhLuong = new DataTable();
                    adapter.Fill(dtTinhLuong);
                    dgvTinhLuong.DataSource = dtTinhLuong;

                    // Format tiền
                    if (dgvTinhLuong.Columns["Lương/Giờ"] != null)
                    {
                        dgvTinhLuong.Columns["Lương/Giờ"].DefaultCellStyle.Format = "N0";
                        dgvTinhLuong.Columns["Lương/Giờ"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                    if (dgvTinhLuong.Columns["Thưởng/Phạt"] != null)
                    {
                        dgvTinhLuong.Columns["Thưởng/Phạt"].DefaultCellStyle.Format = "N0";
                        dgvTinhLuong.Columns["Thưởng/Phạt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                    if (dgvTinhLuong.Columns["Tổng Lương"] != null)
                    {
                        dgvTinhLuong.Columns["Tổng Lương"].DefaultCellStyle.Format = "N0";
                        dgvTinhLuong.Columns["Tổng Lương"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgvTinhLuong.Columns["Tổng Lương"].DefaultCellStyle.BackColor = Color.LightGreen;
                    }

                    // Load combo nhân viên
                    SqlCommand cmd = new SqlCommand("SELECT MaNV, TenNV FROM NhanVien ORDER BY MaNV", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    cboNhanVienTL.Items.Clear();
                    while (reader.Read())
                    {
                        cboNhanVienTL.Items.Add(new
                        {
                            Text = $"{reader["MaNV"]} - {reader["TenNV"]}",
                            Value = reader["MaNV"].ToString()
                        });
                    }
                    cboNhanVienTL.DisplayMember = "Text";
                    cboNhanVienTL.ValueMember = "Value";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTL_TinhLuong_Click(object sender, EventArgs e)
        {
            if (!SessionInfo.IsAdmin)
            {
                MessageBox.Show("Chỉ quản lý mới có quyền tính lương!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboNhanVienTL.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                dynamic selectedItem = cboNhanVienTL.SelectedItem;
                string maNV = selectedItem.Value;
                int thang = (int)nudThang.Value;
                int nam = (int)nudNam.Value;
                decimal thuongPhat = nudThuongPhat.Value;

                using (SqlConnection conn = DatabaseConnection.OpenConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_TinhLuongThang", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);
                    cmd.Parameters.AddWithValue("@ThuongPhat", thuongPhat);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string thongBao = $"✓ TÍNH LƯƠNG THÀNH CÔNG!\n\n" +
                            $"━━━━━━━━━━━━━━━━━━━━━━\n" +
                            $"Nhân viên: {reader["TenNV"]}\n" +
                            $"Tháng: {thang}/{nam}\n" +
                            $"━━━━━━━━━━━━━━━━━━━━━━\n" +
                            $"Số ngày làm: {reader["SoNgayLam"]} ngày\n" +
                            $"Giờ/ngày: {reader["SoGioMotNgay"]} giờ (cố định)\n" +
                            $"Lương/giờ: {Convert.ToDecimal(reader["LuongMoiGio"]):N0} VNĐ\n" +
                            $"Thưởng/Phạt: {Convert.ToDecimal(reader["ThuongPhat"]):N0} VNĐ\n" +
                            $"━━━━━━━━━━━━━━━━━━━━━━\n" +
                            $"TỔNG LƯƠNG: {Convert.ToDecimal(reader["TongLuong"]):N0} VNĐ\n" +
                            $"━━━━━━━━━━━━━━━━━━━━━━";

                        MessageBox.Show(thongBao, "Kết quả tính lương",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                LoadTinhLuongData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTL_Xoa_Click(object sender, EventArgs e)
        {
            if (!SessionInfo.IsAdmin)
            {
                MessageBox.Show("Chỉ quản lý mới có quyền xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvTinhLuong.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Xóa bản ghi lương này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        string maLuong = dgvTinhLuong.SelectedRows[0].Cells["MaLuong"].Value.ToString();

                        using (SqlConnection conn = DatabaseConnection.OpenConnection())
                        {
                            SqlCommand cmdUpdate = new SqlCommand(
                                "UPDATE NhanVien SET MaLuong=NULL WHERE MaLuong=@MaLuong", conn);
                            cmdUpdate.Parameters.AddWithValue("@MaLuong", maLuong);
                            cmdUpdate.ExecuteNonQuery();

                            SqlCommand cmdDelete = new SqlCommand(
                                "DELETE FROM BangLuong WHERE MaLuong=@MaLuong", conn);
                            cmdDelete.Parameters.AddWithValue("@MaLuong", maLuong);
                            cmdDelete.ExecuteNonQuery();
                        }

                        MessageBox.Show("Xóa thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadTinhLuongData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        #endregion

        #region Lịch Làm Việc
        private void InitializeLichLamViecTab()
        {
            pnlLichLV = new Panel();
            pnlLichLV.Dock = DockStyle.Bottom;
            pnlLichLV.Height = 60;
            pnlLichLV.BackColor = Color.WhiteSmoke;

            Button btnXemCaLam = CreateButton("📋 Xem Ca", 20);
            btnXemCaLam.Width = 130;
            btnXemCaLam.Click += btnXemCaLam_Click;

            btnLLV_BackMenu = CreateButton("← Menu", 620);

            pnlLichLV.Controls.AddRange(new Control[] {
                btnXemCaLam, btnLLV_BackMenu
            });

            dgvLichLamViec = new DataGridView();
            dgvLichLamViec.Dock = DockStyle.Fill;
            dgvLichLamViec.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLichLamViec.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLichLamViec.ReadOnly = true;
            dgvLichLamViec.AllowUserToAddRows = false;

            tabLichLamViec.Controls.Add(dgvLichLamViec);
            tabLichLamViec.Controls.Add(pnlLichLV);

            btnLLV_BackMenu.Click += (s, ev) => tabControl.SelectedTab = tabMenu;
        }

        private void LoadLichLamViecData()
        {
            try
            {
                string query = @"SELECT llv.MaCa, 
                                CONVERT(VARCHAR(5), llv.GioBD, 108) AS [Giờ Bắt Đầu],
                                CONVERT(VARCHAR(5), llv.GioKT, 108) AS [Giờ Kết Thúc],
                                COUNT(nv.MaNV) AS [Số Nhân Viên]
                                FROM LichLamViec llv
                                LEFT JOIN NhanVien nv ON llv.MaCa = nv.MaCa
                                GROUP BY llv.MaCa, llv.GioBD, llv.GioKT
                                ORDER BY llv.GioBD";

                using (SqlConnection conn = DatabaseConnection.OpenConnection())
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    dtLichLamViec = new DataTable();
                    adapter.Fill(dtLichLamViec);
                    dgvLichLamViec.DataSource = dtLichLamViec;
                }

                isDataChanged = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXemCaLam_Click(object sender, EventArgs e)
        {
            frmXemCaLam formCaLam = new frmXemCaLam();
            formCaLam.ShowDialog();
        }
        #endregion

        #region Helper Methods
        private Button CreateButton(string text, int x)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Location = new Point(x, 10);
            btn.Size = new Size(100, 40);
            btn.BackColor = Color.FromArgb(52, 152, 219);
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            return btn;
        }
        #endregion
    }

    public static class SessionInfo
    {
        public static string MaNV { get; set; }
        public static string TenNV { get; set; }
        public static string MaQuyen { get; set; }
        public static string TenQuyen { get; set; }
        public static bool IsAdmin { get; set; }

        public static void Clear()
        {
            MaNV = null;
            TenNV = null;
            MaQuyen = null;
            TenQuyen = null;
            IsAdmin = false;
        }
    }
} != null)
                        dgvChamCong.Columns["NgayDiLam"].DefaultCellStyle.Format = "dd/MM/yyyy";

if (dgvChamCong.Columns["SoGioLam"] != null)
    dgvChamCong.Columns["SoGioLam"].HeaderText = "Giờ Làm (8h/ngày)";

if (SessionInfo.IsAdmin)
{
    SqlCommand cmd = new SqlCommand("SELECT MaNV, TenNV FROM NhanVien ORDER BY MaNV", conn);
    SqlDataReader reader = cmd.ExecuteReader();
    cboNhanVienCC.Items.Clear();
    while (reader.Read())
    {
        cboNhanVienCC.Items.Add(new
        {
            Text = $"{reader["MaNV"]} - {reader["TenNV"]}",
            Value = reader["MaNV"].ToString()
        });
    }
    cboNhanVienCC.DisplayMember = "Text";
    cboNhanVienCC.ValueMember = "Value";
}
                }

                isDataChanged = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadChamCongDataFiltered(int thang, int nam)
{
    try
    {
        string query;

        if (!SessionInfo.IsAdmin)
        {
            query = @"SELECT NgayDiLam, MaNV, TenNV, ChucVu, SoGioLam
                             FROM vw_ChamCongCaNhan
                             WHERE MONTH(NgayDiLam)=@Thang AND YEAR(NgayDiLam)=@Nam
                             ORDER BY NgayDiLam DESC";
        }
        else
        {
            query = @"SELECT cc.NgayDiLam, cc.MaNV, nv.TenNV, nv.ChucVu, 8 AS SoGioLam
                             FROM ChamCong cc
                             INNER JOIN NhanVien nv ON cc.MaNV = nv.MaNV
                             WHERE MONTH(cc.NgayDiLam)=@Thang AND YEAR(cc.NgayDiLam)=@Nam
                             ORDER BY cc.NgayDiLam DESC";
        }

        using (SqlConnection conn = DatabaseConnection.OpenConnection())
        {
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            adapter.SelectCommand.Parameters.AddWithValue("@Thang", thang);
            adapter.SelectCommand.Parameters.AddWithValue("@Nam", nam);

            dtChamCong = new DataTable();
            adapter.Fill(dtChamCong);
            dgvChamCong.DataSource = dtChamCong;

            if (dgvChamCong.Columns["NgayDiLam"]