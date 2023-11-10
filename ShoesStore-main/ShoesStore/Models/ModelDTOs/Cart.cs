using ShoesStore.ViewModels;
using System.Security.AccessControl;

namespace ShoesStore.Models.ModelDTOs
{
	public class CartItem
	{
		public GiayDTO Shoes { get; set; }

		public int Quantity { get; set; }
    }

	public class Cart
	{
		List<CartItem> Items = new List<CartItem>();

		public IEnumerable<CartItem> GetItems
		{
			get { return Items; }
		}

		public void Add(GiayDTO piece, int quantity = 1)
		{
			var item = Items.FirstOrDefault(x => x.Shoes.MaGiay == piece.MaGiay);
			if(item == null)
			{
				Items.Add(new CartItem
                {
					Shoes = piece,
					Quantity = quantity
				});
			}
			else
			{
				item.Quantity += quantity;
            }
		}

	}
}
