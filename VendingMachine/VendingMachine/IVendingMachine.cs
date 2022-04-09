using System;
namespace VendingMachine
{
	public interface IVendingMachine
	{
		public void InsertCoin(decimal coin);
		public void SelectProduct(int id);
		public void DisplayInventory();
		public void ReturnCoins();
		public void SelectLanguage(string language);
	}
}

