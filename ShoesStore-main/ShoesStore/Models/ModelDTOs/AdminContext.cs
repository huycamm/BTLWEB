using ShoesStore.Models.ProcedureModels;

namespace ShoesStore.Models.ModelDTOs
{
    public class AdminContext
	{
		public static ProcTienBan30Ngay SoTienBan30NgayGanNhat { get; set; }

        public static ProcTienNhap30Ngay SoTienNhap30NgayGanNhat { get; set; }

        public static ProcTongHDB30Ngay SoHDB30NgayGanNhat { get; set; }

        public static List<ProcTongTienBanHangThang> SoTienBanTrongNam { get; set; }


    }
}
