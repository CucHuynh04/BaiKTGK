using BUS.KTGK;
using DAL.KTGK.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace De02
{
    public partial class Form1 : Form
    {
        private SanphamService sanphamService;
        public Form1()
        {
            InitializeComponent();
            sanphamService = new SanphamService(); // Khởi tạo dịch vụ
            LoadData();
            dataGridView1.CellClick += dataGridView1_CellClick;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SanphamService sanphamService = new SanphamService();
            var loaiSPList = sanphamService.GetAllLoaiSP(); // Lấy danh sách loại sản phẩm

            cmbLoaiSP.DataSource = loaiSPList; // Gán danh sách cho ComboBox
            cmbLoaiSP.DisplayMember = "TenLoai"; // Hiển thị tên loại sản phẩm
            cmbLoaiSP.ValueMember = "MaLoai"; // Giá trị của loại sản phẩm
        }
        private void LoadData()
        {

            var sanphamList = sanphamService.GetAll();
            dataGridView1.DataSource = sanphamService.GetAll(); // Gán DataSource cho DataGridView
            var productList = sanphamService.GetAllWithLoaiSP(); // Gọi phương thức lấy sản phẩm với loại sản phẩm
            dataGridView1.DataSource = productList;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                // Lấy mã sản phẩm từ dòng được chọn
                var maSP = dataGridView1.CurrentRow.Cells["MaSP"].Value.ToString();

                // Tạo một đối tượng Sanpham mới
                var sanpham = new Sanpham
                {
                    MaSP = maSP,
                    TenSP = txtTENSP.Text,
                    Ngaynhap = dtpNgayNhap.Value, // Lấy giá trị ngày từ DateTimePicker
                    MaLoai = cmbLoaiSP.SelectedValue.ToString()
                };

                // Sửa sản phẩm thông qua dịch vụ
                sanphamService.Update(sanpham);

                // Tải lại dữ liệu để hiển thị thông tin mới
                LoadData(); // Giả sử bạn đã định nghĩa phương thức LoadData()
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                var maSP = dataGridView1.CurrentRow.Cells["MaSP"].Value.ToString(); // Lấy mã sản phẩm

                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        // Gọi phương thức xóa từ SanphamService
                        sanphamService.Delete(maSP);
                        LoadData(); // Tải lại dữ liệu
                        MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string maSP = txtMASP.Text;
            string tenSP = txtTENSP.Text;

            // Lấy giá trị loại sản phẩm từ ComboBox
            string maLoaiSP = cmbLoaiSP.SelectedValue.ToString(); // Lấy mã loại sản phẩm

            // Gọi phương thức thêm sản phẩm
            SanphamService sanphamService = new SanphamService();
            var sanpham = new Sanpham
            {
                MaSP = maSP,
                TenSP = tenSP,
                Ngaynhap = dtpNgayNhap.Value, // Lấy ngày nhập từ DateTimePicker
                MaLoai = maLoaiSP // Gán mã loại sản phẩm từ ComboBox
            };

            sanphamService.Add(sanpham); // Giả sử bạn đã có phương thức Add trong SanphamService
            LoadData(); // 
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận thoát", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Application.Exit(); // Thoát ứng dụng
            }
        }

        private void btnSeach_Click(object sender, EventArgs e)
        {
            string searchName = textBox1.Text.Trim(); // Lấy tên sản phẩm từ TextBox
            var result = sanphamService.SearchByName(searchName); // Gọi phương thức tìm kiếm

            dataGridView1.DataSource = result; // Hiển thị kết quả vào DataGridView
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem có phải là hàng dữ liệu không
            if (e.RowIndex >= 0)
            {
                // Lấy dữ liệu từ hàng được nhấn
                var row = dataGridView1.Rows[e.RowIndex];
                txtMASP.Text = row.Cells["MaSP"].Value.ToString();
                txtTENSP.Text = row.Cells["TenSP"].Value.ToString();
                dtpNgayNhap.Value = DateTime.Parse(row.Cells["Ngaynhap"].Value.ToString());
                cmbLoaiSP.SelectedValue = row.Cells["LoaiSP"].Value.ToString(); // Chọn loại sản phẩm
            }
        }


        private void dtpNgayNhap_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
