using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using QLQB_ChucNang_QLNhanVien_va_LichLamViec.Database;

namespace QLQB_ChucNang_QLNhanVien_va_LichLamViec
{
    public partial class frmNhanVienDialog : Form
    {
        public string MaNV { get; private set; }
        public string TenNV { get; private set; }
        public string ChucVu { get; private set; }
        public DateTime NgaySinh { get; private set; }
        public string GioiTinh { get; private set; }
        public string MatKhau { get; private set; }
        public decimal LuongMoiGio { get; private set; }
        public string MaQuyen { get; private set; }

        private bool isEditMode = false;
        private DataTable dtQuyen;
        private DataTable dtGioiTinh;

        public frmNhanVienDialog(bool isEdit = false, DataRow existingData = null)
        {
            InitializeComponent();
            isEditMode = isEdit;
            this.Text = isEdit ? "Sửa Thông Tin Nhân Viên" : "Thêm Nhân Viên Mới";

            if (isEdit && existingData != null)
            {
                LoadExistingData(existingData);
            }
            else
            {
                GenerateNewMaNV();
            }
        }

        private void frmNhanVienDialog_Load(object sender, EventArgs e)
        {
            try
            {
                LoadGioiTinh();
                LoadQuyen();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateNewMaNV()
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.OpenConnection())
                {
                    string query = "SELECT TOP 1 MaNV FROM NhanVien ORDER BY MaNV DESC";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        string lastMaNV = result.ToString();
                        int number = int.Parse(lastMaNV.Substring(2));
                        txtMaNV.Text = "NV" + (number + 1).ToString("D2");
                    }
                    else
                    {
                        txtMaNV.Text = "NV01";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tạo mã NV: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaNV.Text = "NV01";
            }
        }

        private void LoadGioiTinh()
        {
            try
            {
                // Lấy giới tính từ SQL (DISTINCT từ bảng NhanVien)
                using (SqlConnection conn = DatabaseConnection.OpenConnection())
                {
                    string query = "SELECT DISTINCT GioiTinh FROM NhanVien WHERE GioiTinh IS NOT NULL ORDER BY GioiTinh";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    dtGioiTinh = new DataTable();
                    adapter.Fill(dtGioiTinh);

                    cboGioiTinh.Items.Clear();
                    foreach (DataRow row in dtGioiTinh.Rows)
                    {
                        cboGioiTinh.Items.Add(row["GioiTinh"].ToString().Trim());
                    }

                    // Nếu không có dữ liệu, thêm mặc định
                    if (cboGioiTinh.Items.Count == 0)
                    {
                        cboGioiTinh.Items.Add("Nam");
                        cboGioiTinh.Items.Add("Nữ");
                    }

                    if (cboGioiTinh.Items.Count > 0)
                        cboGioiTinh.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                // Fallback nếu lỗi
                cboGioiTinh.Items.Clear();
                cboGioiTinh.Items.Add("Nam");
                cboGioiTinh.Items.Add("Nữ");
                cboGioiTinh.SelectedIndex = 0;
            }
        }

        private void LoadQuyen()
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.OpenConnection())
                {
                    string query = "SELECT MaQuyen, TenQuyen FROM Quyen ORDER BY MaQuyen";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    dtQuyen = new DataTable();
                    adapter.Fill(dtQuyen);

                    cboMaQuyen.DataSource = dtQuyen;
                    cboMaQuyen.DisplayMember = "TenQuyen";
                    cboMaQuyen.ValueMember = "MaQuyen";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load quyền: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboMaQuyen_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaQuyen.SelectedValue != null)
            {
                string maQuyen = cboMaQuyen.SelectedValue.ToString();
                string chucVuMacDinh = "";

                // Tự động điền chức vụ dựa trên mã quyền
                switch (cboMaQuyen.SelectedValue.ToString())
                {
                    case "Q01":
                        txtChucVu.Text = "Quản lý";
                        break;
                    case "Q02":
                        txtChucVu.Text = "Phục vụ";
                        break;
                    case "Q03":
                        txtChucVu.Text = "Thu ngân";
                        break;
                    case "Q04":
                        txtChucVu.Text = "Bếp";
                        break;
                    case "Q05":
                        txtChucVu.Text = "Bảo vệ";
                        break;
                }

                //txtChucVu.Text = chucVuMacDinh;
            }
            else
            {
                txtChucVu.Text = "Khong co du lieu";
            }
        }

        private void LoadExistingData(DataRow row)
        {
            try
            {
                txtMaNV.Text = row["MaNV"].ToString();
                txtTenNV.Text = row["TenNV"].ToString();
                dtpNgaySinh.Value = Convert.ToDateTime(row["NgaySinh"]);
                txtMatKhau.Text = row["MatKhau"].ToString();

                // Load after form load để đảm bảo các combobox đã được fill
                this.Load += (s, e) =>
                {
                    // Set giới tính
                    string gioiTinh = row["GioiTinh"].ToString().Trim();
                    for (int i = 0; i < cboGioiTinh.Items.Count; i++)
                    {
                        if (cboGioiTinh.Items[i].ToString() == gioiTinh)
                        {
                            cboGioiTinh.SelectedIndex = i;
                            break;
                        }
                    }

                    // Set lương
                    if (row.Table.Columns.Contains("LuongMoiGio"))
                        nudLuongMoiGio.Value = Convert.ToDecimal(row["LuongMoiGio"]);

                    // Set quyền (sẽ tự động set chức vụ qua event)
                    if (row.Table.Columns.Contains("MaQuyen"))
                        cboMaQuyen.SelectedValue = row["MaQuyen"].ToString();

                    // Override chức vụ nếu có
                    if (row.Table.Columns.Contains("ChucVu") && row["ChucVu"] != DBNull.Value)
                        txtChucVu.Text = row["ChucVu"].ToString();
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenNV.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhân viên!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNV.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return false;
            }

            if (nudLuongMoiGio.Value <= 0)
            {
                MessageBox.Show("Vui lòng nhập lương/giờ hợp lệ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudLuongMoiGio.Focus();
                return false;
            }

            if (cboMaQuyen.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn quyền!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboMaQuyen.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtChucVu.Text))
            {
                MessageBox.Show("Chức vụ không được để trống!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            MaNV = txtMaNV.Text.Trim();
            TenNV = txtTenNV.Text.Trim();
            ChucVu = txtChucVu.Text.Trim();
            NgaySinh = dtpNgaySinh.Value;
            GioiTinh = cboGioiTinh.SelectedItem.ToString();
            MatKhau = txtMatKhau.Text.Trim();
            LuongMoiGio = nudLuongMoiGio.Value;
            MaQuyen = cboMaQuyen.SelectedValue.ToString();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}