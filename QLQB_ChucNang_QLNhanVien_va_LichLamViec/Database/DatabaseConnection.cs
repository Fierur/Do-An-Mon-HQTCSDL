using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLQB_ChucNang_QLNhanVien_va_LichLamViec.Database
{
    public class DatabaseConnection
    {
        private static string serverName = "26.71.28.188";
        private static string databaseName = "QuanLyQuanBar";
        private static string connectionString;
        private static SqlConnection currentConnection = null;

        /// <summary>
        /// Dùng thông tin user + password từ form để đăng nhập
        /// </summary>
        public static bool TestLogin(string username, string password, out string errorMessage, out string tenNV, out string maQuyen)
        {
            errorMessage = "";
            tenNV = "";
            maQuyen = "";

            try
            {
                // Tạo chuỗi kết nối động từ tài khoản người dùng nhập
                string loginConnectionString =
                    $"Data Source={serverName};Initial Catalog={databaseName};User ID={username};Password={password};";

                using (SqlConnection conn = new SqlConnection(loginConnectionString))
                {
                    conn.Open();

                    // Kiểm tra xem user có tồn tại trong bảng NhanVien không
                    string query = "SELECT TenNV, MaQuyen FROM NhanVien WHERE MaNV = @MaNV";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNV", username);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tenNV = reader["TenNV"].ToString();
                                maQuyen = reader["MaQuyen"].ToString();
                            }
                        }
                    }

                    // Lưu chuỗi kết nối hiện tại cho session
                    connectionString = loginConnectionString;
                    return true;
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 18456)
                    errorMessage = "Tên đăng nhập hoặc mật khẩu không đúng!";
                else
                    errorMessage = "Lỗi kết nối: " + ex.Message;

                return false;
            }
            catch (Exception ex)
            {
                errorMessage = "Lỗi: " + ex.Message;
                return false;
            }
        }

        public static SqlConnection GetConnection()
        {
            if (currentConnection == null || currentConnection.State == System.Data.ConnectionState.Closed)
                currentConnection = new SqlConnection(connectionString);
            return currentConnection;
        }

        public static SqlConnection OpenConnection()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }

        public static void CloseConnection()
        {
            if (currentConnection != null && currentConnection.State == System.Data.ConnectionState.Open)
                currentConnection.Close();
        }
    }
}

