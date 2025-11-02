using QLQB_ChucNang_QLNhanVien_va_LichLamViec.Database;
using QLQB_ChucNang_QLNhanVien_va_LichLamViec.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
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

        public frmMain()
        {
            InitializeComponent();
            InitializeTabControls();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Hiển thị thông tin user
            lblWelcome.Text = $"Chào mừng, {SessionInfo.TenNV} ({SessionInfo.TenQuyen})";

            // Ẩn các tab không phải menu ban đầu
            tabControl.SelectedTab = tabMenu;

            // Kiểm tra quyền truy cập
            CheckPermissions();
        }

        private void CheckPermissions()
        {
            // Nếu không phải admin (Q01), ẩn một số chức năng
            if (!SessionInfo.IsAdmin)
            {
                btnMenuQLNV.Enabled = false;
                btnMenuTinhLuong.Enabled = false;
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
                DatabaseConnection.CloseConnection();
                this.Close();
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Load dữ liệu khi chuyển tab
            if (tabControl.SelectedTab == tabQuanLyNV)
            {
                LoadNhanVienData();
            }
            else if (tabControl.SelectedTab == tabChamCong)
            {
                LoadChamCongData();
            }
            else if (tabControl.SelectedTab == tabTinhLuong)
            {
                LoadTinhLuongData();
            }
            else if (tabControl.SelectedTab == tabLichLamViec)
            {
                LoadLichLamViecData();
            }
        }
        #endregion

        #region Quản Lý Nhân Viên
        private void InitializeQLNVTab()
        {
            // Panel buttons
            pnlQLNV = new Panel();
            pnlQLNV.Dock = DockStyle.Bottom;
            pnlQLNV.Height = 60;
            pnlQLNV.BackColor = Color.WhiteSmoke;

            btnQLNV_Them = CreateButton("Thêm", 20);
            btnQLNV_Xoa = CreateButton("Xóa", 140);
            btnQLNV_Sua = CreateButton("Sửa", 260);
            btnQLNV_Save = CreateButton("Lưu", 380);
            btnQLNV_Cancel = CreateButton("Hủy", 500);
            btnQLNV_BackMenu = CreateButton("← Menu", 1050);

            pnlQLNV.Controls.AddRange(new Control[] {
                btnQLNV_Them, btnQLNV_Xoa, btnQLNV_Sua,
                btnQLNV_Save, btnQLNV_Cancel, btnQLNV_BackMenu
            });

            // DataGridView
            dgvNhanVien = new DataGridView();
            dgvNhanVien.Dock = DockStyle.Fill;
            dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNhanVien.MultiSelect = false;
            dgvNhanVien.ReadOnly = true;
            dgvNhanVien.AllowUserToAddRows = false;

            tabQuanLyNV.Controls.Add(dgvNhanVien);
            tabQuanLyNV.Controls.Add(pnlQLNV);

            // Events
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
                                LuongMoiGio, MaQuyen FROM NhanVien ORDER BY MaNV";

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

                    dtNhanVien.Rows.Add(newRow);
                    isDataChanged = true;

                    MessageBox.Show("Đã thêm nhân viên vào danh sách!\nNhấn nút 'Lưu' để lưu vào database.",
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

                if (MessageBox.Show($"Bạn có chắc muốn xóa nhân viên:\n{maNV} - {tenNV}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dgvNhanVien.Rows.RemoveAt(dgvNhanVien.SelectedRows[0].Index);
                    isDataChanged = true;

                    MessageBox.Show("Đã xóa nhân viên khỏi danh sách!\nNhấn nút 'Lưu' để cập nhật database.",
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
                MessageBox.Show("Bạn không có quyền sửa thông tin nhân viên!", "Thông báo",
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

                        isDataChanged = true;
                        dgvNhanVien.Refresh();

                        MessageBox.Show("Đã cập nhật thông tin nhân viên!\nNhấn nút 'Lưu' để lưu vào database.",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnQLNV_Save_Click(object sender, EventArgs e)
        {
            if (!isDataChanged)
            {
                MessageBox.Show("Không có thay đổi để lưu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn lưu các thay đổi vào database?", "Xác nhận",
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

                                    // MaCa = NULL (không truyền hoặc truyền NULL)
                                    SqlParameter paramMaCa = new SqlParameter("@MaCa", SqlDbType.Char, 10);
                                    paramMaCa.Value = DBNull.Value;
                                    cmd.Parameters.Add(paramMaCa);

                                    cmd.Parameters.AddWithValue("@MaQuyen", row["MaQuyen"]);
                                    cmd.ExecuteNonQuery();
                                    successCount++;
                                }
                                else if (row.RowState == DataRowState.Modified)
                                {
                                    string updateQuery = @"UPDATE NhanVien SET 
                                        TenNV = @TenNV, ChucVu = @ChucVu, NgaySinh = @NgaySinh,
                                        GioiTinh = @GioiTinh, MatKhau = @MatKhau, 
                                        LuongMoiGio = @LuongMoiGio, MaQuyen = @MaQuyen
                                        WHERE MaNV = @MaNV";

                                    SqlCommand cmd = new SqlCommand(updateQuery, conn);
                                    cmd.Parameters.AddWithValue("@MaNV", row["MaNV"]);
                                    cmd.Parameters.AddWithValue("@TenNV", row["TenNV"]);
                                    cmd.Parameters.AddWithValue("@ChucVu", row["ChucVu"]);
                                    cmd.Parameters.AddWithValue("@NgaySinh", row["NgaySinh"]);
                                    cmd.Parameters.AddWithValue("@GioiTinh", row["GioiTinh"]);
                                    cmd.Parameters.AddWithValue("@MatKhau", row["MatKhau"]);
                                    cmd.Parameters.AddWithValue("@LuongMoiGio", row["LuongMoiGio"]);
                                    cmd.Parameters.AddWithValue("@MaQuyen", row["MaQuyen"]);
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
                    {
                        MessageBox.Show($"Lưu dữ liệu thành công!\nĐã xử lý: {successCount} bản ghi",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Hoàn tất với một số lỗi:\n" +
                            $"- Thành công: {successCount}\n" +
                            $"- Lỗi: {errorCount}\n\n" +
                            $"Chi tiết lỗi:{errorMessages}",
                            "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    isDataChanged = false;
                    LoadNhanVienData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi lưu dữ liệu: " + ex.Message + "\n\n" + ex.StackTrace,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnQLNV_Cancel_Click(object sender, EventArgs e)
        {
            if (isDataChanged)
            {
                if (MessageBox.Show("Bạn có muốn hủy các thay đổi?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    LoadNhanVienData();
                }
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

            pnlCCInput.Controls.AddRange(new Control[] {
                lblNhanVien, cboNhanVienCC, lblNgay, dtpChamCong
            });

            pnlChamCong = new Panel();
            pnlChamCong.Dock = DockStyle.Bottom;
            pnlChamCong.Height = 60;
            pnlChamCong.BackColor = Color.WhiteSmoke;

            btnCC_Them = CreateButton("Chấm công", 20);
            btnCC_Xoa = CreateButton("Xóa", 140);
            btnCC_Sua = CreateButton("Sửa", 260);
            btnCC_Save = CreateButton("Lưu", 380);
            btnCC_Cancel = CreateButton("Hủy", 500);
            btnCC_BackMenu = CreateButton("← Menu", 1050);

            pnlChamCong.Controls.AddRange(new Control[] {
                btnCC_Them, btnCC_Xoa, btnCC_Sua,
                btnCC_Save, btnCC_Cancel, btnCC_BackMenu
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
            btnCC_Sua.Click += btnCC_Sua_Click;
            btnCC_Save.Click += btnCC_Save_Click;
            btnCC_Cancel.Click += btnCC_Cancel_Click;
            btnCC_BackMenu.Click += (s, ev) => tabControl.SelectedTab = tabMenu;
        }

        private void LoadChamCongData()
        {
            try
            {
                string query = @"SELECT cc.NgayDiLam, cc.MaNV, nv.TenNV, nv.ChucVu
                                FROM ChamCong cc
                                INNER JOIN NhanVien nv ON cc.MaNV = nv.MaNV
                                ORDER BY cc.NgayDiLam DESC";

                using (SqlConnection conn = DatabaseConnection.OpenConnection())
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    dtChamCong = new DataTable();
                    adapter.Fill(dtChamCong);
                    dgvChamCong.DataSource = dtChamCong;

                    SqlCommand cmd = new SqlCommand("SELECT MaNV, TenNV FROM NhanVien", conn);
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

                isDataChanged = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCC_Them_Click(object sender, EventArgs e)
        {
            if (cboNhanVienCC.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                dynamic selectedItem = cboNhanVienCC.SelectedItem;
                string maNV = selectedItem.Value;
                DateTime ngay = dtpChamCong.Value.Date;

                using (SqlConnection conn = DatabaseConnection.OpenConnection())
                {
                    SqlCommand checkCmd = new SqlCommand(
                        "SELECT COUNT(*) FROM ChamCong WHERE MaNV = @MaNV AND NgayDiLam = @Ngay", conn);
                    checkCmd.Parameters.AddWithValue("@MaNV", maNV);
                    checkCmd.Parameters.AddWithValue("@Ngay", ngay);

                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("Nhân viên đã chấm công ngày này!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO ChamCong (NgayDiLam, MaNV) VALUES (@Ngay, @MaNV)", conn);
                    cmd.Parameters.AddWithValue("@Ngay", ngay);
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Chấm công thành công!", "Thông báo",
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
                if (MessageBox.Show("Bạn có chắc muốn xóa bản ghi này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        string maNV = dgvChamCong.SelectedRows[0].Cells["MaNV"].Value.ToString();
                        DateTime ngay = Convert.ToDateTime(dgvChamCong.SelectedRows[0].Cells["NgayDiLam"].Value);

                        using (SqlConnection conn = DatabaseConnection.OpenConnection())
                        {
                            SqlCommand cmd = new SqlCommand(
                                "DELETE FROM ChamCong WHERE MaNV = @MaNV AND NgayDiLam = @Ngay", conn);
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

        private void btnCC_Sua_Click(object sender, EventArgs e)
        {
            dgvChamCong.ReadOnly = false;
            MessageBox.Show("Bạn có thể chỉnh sửa dữ liệu. Nhấn Lưu để cập nhật.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCC_Save_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn lưu các thay đổi?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = DatabaseConnection.OpenConnection())
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(
                            "SELECT * FROM ChamCong", conn);
                        SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                        adapter.Update(dtChamCong);
                    }

                    MessageBox.Show("Lưu thành công!", "Thông báo",
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

        private void btnCC_Cancel_Click(object sender, EventArgs e)
        {
            LoadChamCongData();
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

            pnlTLInput.Controls.AddRange(new Control[] {
                lblNV, cboNhanVienTL, lblThang, nudThang,
                lblNam, nudNam, lblThuongPhat, nudThuongPhat
            });

            pnlTinhLuong = new Panel();
            pnlTinhLuong.Dock = DockStyle.Bottom;
            pnlTinhLuong.Height = 60;
            pnlTinhLuong.BackColor = Color.WhiteSmoke;

            btnTL_TinhLuong = CreateButton("Tính lương", 20);
            btnTL_Xoa = CreateButton("Xóa", 140);
            btnTL_Save = CreateButton("Lưu", 260);
            btnTL_Cancel = CreateButton("Hủy", 380);
            btnTL_BackMenu = CreateButton("← Menu", 1050);

            pnlTinhLuong.Controls.AddRange(new Control[] {
                btnTL_TinhLuong, btnTL_Xoa, btnTL_Save,
                btnTL_Cancel, btnTL_BackMenu
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
            btnTL_Save.Click += btnTL_Save_Click;
            btnTL_Cancel.Click += btnTL_Cancel_Click;
            btnTL_BackMenu.Click += (s, ev) => tabControl.SelectedTab = tabMenu;
        }

        private void LoadTinhLuongData()
        {
            try
            {
                string query = @"SELECT bl.MaLuong, nv.MaNV, nv.TenNV, nv.ChucVu,
                                bl.TongNgayLamMotThang, bl.ThuongPhat, bl.TongLuong
                                FROM BangLuong bl
                                INNER JOIN NhanVien nv ON bl.MaLuong = nv.MaLuong";

                using (SqlConnection conn = DatabaseConnection.OpenConnection())
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    dtTinhLuong = new DataTable();
                    adapter.Fill(dtTinhLuong);
                    dgvTinhLuong.DataSource = dtTinhLuong;

                    SqlCommand cmd = new SqlCommand("SELECT MaNV, TenNV FROM NhanVien", conn);
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
                        string thongBao = $"Tính lương thành công!\n\n" +
                            $"Nhân viên: {reader["TenNV"]}\n" +
                            $"Tháng: {thang}/{nam}\n" +
                            $"Số ngày làm: {reader["SoNgayLam"]}\n" +
                            $"Số giờ/ca: {reader["SoGioMotCa"]}\n" +
                            $"Lương/giờ: {Convert.ToDecimal(reader["LuongMoiGio"]):N0} VNĐ\n" +
                            $"Thưởng/Phạt: {Convert.ToDecimal(reader["ThuongPhat"]):N0} VNĐ\n" +
                            $"Tổng lương: {Convert.ToDecimal(reader["TongLuong"]):N0} VNĐ";

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
                if (MessageBox.Show("Bạn có chắc muốn xóa bản ghi lương này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        string maLuong = dgvTinhLuong.SelectedRows[0].Cells["MaLuong"].Value.ToString();

                        using (SqlConnection conn = DatabaseConnection.OpenConnection())
                        {
                            SqlCommand cmdUpdate = new SqlCommand(
                                "UPDATE NhanVien SET MaLuong = NULL WHERE MaLuong = @MaLuong", conn);
                            cmdUpdate.Parameters.AddWithValue("@MaLuong", maLuong);
                            cmdUpdate.ExecuteNonQuery();

                            SqlCommand cmdDelete = new SqlCommand(
                                "DELETE FROM BangLuong WHERE MaLuong = @MaLuong", conn);
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

        private void btnTL_Save_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn lưu các thay đổi?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = DatabaseConnection.OpenConnection())
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(
                            "SELECT * FROM BangLuong", conn);
                        SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                        adapter.Update(dtTinhLuong);
                    }

                    MessageBox.Show("Lưu thành công!", "Thông báo",
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

        private void btnTL_Cancel_Click(object sender, EventArgs e)
        {
            LoadTinhLuongData();
        }
        #endregion

        #region Lịch Làm Việc
        private void InitializeLichLamViecTab()
        {
            pnlLichLV = new Panel();
            pnlLichLV.Dock = DockStyle.Bottom;
            pnlLichLV.Height = 60;
            pnlLichLV.BackColor = Color.WhiteSmoke;

            btnLLV_Them = CreateButton("Thêm", 20);
            btnLLV_Xoa = CreateButton("Xóa", 140);
            btnLLV_Sua = CreateButton("Sửa", 260);
            btnLLV_Save = CreateButton("Lưu", 380);
            btnLLV_Cancel = CreateButton("Hủy", 500);
            btnLLV_BackMenu = CreateButton("← Menu", 1050);

            pnlLichLV.Controls.AddRange(new Control[] {
                btnLLV_Them, btnLLV_Xoa, btnLLV_Sua,
                btnLLV_Save, btnLLV_Cancel, btnLLV_BackMenu
            });

            dgvLichLamViec = new DataGridView();
            dgvLichLamViec.Dock = DockStyle.Fill;
            dgvLichLamViec.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLichLamViec.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLichLamViec.ReadOnly = false;
            dgvLichLamViec.AllowUserToAddRows = false;

            tabLichLamViec.Controls.Add(dgvLichLamViec);
            tabLichLamViec.Controls.Add(pnlLichLV);

            btnLLV_Them.Click += btnLLV_Them_Click;
            btnLLV_Xoa.Click += btnLLV_Xoa_Click;
            btnLLV_Sua.Click += btnLLV_Sua_Click;
            btnLLV_Save.Click += btnLLV_Save_Click;
            btnLLV_Cancel.Click += btnLLV_Cancel_Click;
            btnLLV_BackMenu.Click += (s, ev) => tabControl.SelectedTab = tabMenu;
        }

        private void LoadLichLamViecData()
        {
            try
            {
                string query = @"SELECT llv.MaCa, llv.GioBD, llv.GioKT,
                                COUNT(nv.MaNV) AS SoNhanVien
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

        private void btnLLV_Them_Click(object sender, EventArgs e)
        {
            if (!SessionInfo.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền thêm ca làm việc!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRow newRow = dtLichLamViec.NewRow();
            newRow["MaCa"] = "";
            newRow["GioBD"] = TimeSpan.FromHours(8);
            newRow["GioKT"] = TimeSpan.FromHours(16);
            newRow["SoNhanVien"] = 0;

            dtLichLamViec.Rows.Add(newRow);
            dgvLichLamViec.CurrentCell = dgvLichLamViec.Rows[dgvLichLamViec.Rows.Count - 1].Cells[0];
            isDataChanged = true;
        }

        private void btnLLV_Xoa_Click(object sender, EventArgs e)
        {
            if (!SessionInfo.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền xóa ca làm việc!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvLichLamViec.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa ca làm việc này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dgvLichLamViec.Rows.RemoveAt(dgvLichLamViec.SelectedRows[0].Index);
                    isDataChanged = true;
                }
            }
        }

        private void btnLLV_Sua_Click(object sender, EventArgs e)
        {
            if (!SessionInfo.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền sửa lịch làm việc!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dgvLichLamViec.ReadOnly = false;
            MessageBox.Show("Bạn có thể chỉnh sửa dữ liệu. Nhấn Lưu để cập nhật.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLLV_Save_Click(object sender, EventArgs e)
        {
            if (!isDataChanged)
            {
                MessageBox.Show("Không có thay đổi để lưu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn lưu các thay đổi vào database?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = DatabaseConnection.OpenConnection())
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(
                            "SELECT MaCa, GioBD, GioKT FROM LichLamViec", conn);
                        SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                        adapter.Update(dtLichLamViec);
                    }

                    MessageBox.Show("Lưu dữ liệu thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isDataChanged = false;
                    LoadLichLamViecData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi lưu dữ liệu: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLLV_Cancel_Click(object sender, EventArgs e)
        {
            if (isDataChanged)
            {
                if (MessageBox.Show("Bạn có muốn hủy các thay đổi?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    LoadLichLamViecData();
                }
            }
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
}