using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLQB_ChucNang_QLNhanVien_va_LichLamViec.Model
{
    public class NhanVien
    {
        public string MaNV { get; set; }
        public string TenNV { get; set; }
        public string ChucVu { get; set; }
        public string MaQuyen { get; set; }
        public string TenQuyen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public decimal LuongMoiGio { get; set; }
    }

    // Class lưu thông tin session đăng nhập
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
}
