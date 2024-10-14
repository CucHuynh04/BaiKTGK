using DAL.KTGK.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.KTGK
{
    public class SanphamService
    {
        private readonly SanPhamModel _context;

        // Khởi tạo context trong constructor
        public SanphamService()
        {
            _context = new SanPhamModel(); // Khởi tạo context
        }

        // Lấy tất cả dữ liệu từ bảng Sanpham
        public List<Sanpham> GetAll()
        {
            return _context.Sanpham.ToList(); // Lấy tất cả sản phẩm từ DbSet
        }
        public List<LoaiSP> GetAllLoaiSP()
        {
            return _context.LoaiSP.ToList(); // Lấy tất cả loại sản phẩm từ DbSet
        }

        public void Add(Sanpham sanpham)
        {
            _context.Sanpham.Add(sanpham); // Thêm sản phẩm vào DbSet
            _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
        }
        public void Update(Sanpham sanpham)
        {
            var existingSanpham = _context.Sanpham.Find(sanpham.MaSP); // Tìm sản phẩm theo mã
            if (existingSanpham != null)
            {
                // Cập nhật thông tin
                existingSanpham.TenSP = sanpham.TenSP;
                existingSanpham.Ngaynhap = sanpham.Ngaynhap;
                existingSanpham.MaLoai = sanpham.MaLoai;

                _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
            }
        }
        public void Delete(string maSP)
        {
            // Tìm sản phẩm theo mã
            var sanpham = _context.Sanpham.SingleOrDefault(s => s.MaSP == maSP);

            if (sanpham != null)
            {
                _context.Sanpham.Remove(sanpham); // Xóa sản phẩm
                _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
            }
            else
            {
                throw new Exception("Sản phẩm không tồn tại.");
            }
        }
        public List<Sanpham> SearchByName(string name)
{
    return _context.Sanpham
                   .Where(sp => sp.TenSP.Contains(name)) // Tìm sản phẩm có tên chứa chuỗi nhập vào
                   .ToList();
}
        public List<dynamic> GetAllWithLoaiSP()
        {
            var query = from sp in _context.Sanpham
                        join lsp in _context.LoaiSP on sp.MaLoai equals lsp.MaLoai
                        select new
                        {
                            MaSP = sp.MaSP,
                            TenSP = sp.TenSP,
                            Ngaynhap = sp.Ngaynhap,
                            LoaiSP = lsp.TenLoai // Lấy tên loại sản phẩm
                        };

            return query.ToList<dynamic>();
        }


    }
}
