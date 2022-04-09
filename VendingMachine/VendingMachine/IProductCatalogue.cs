using System;
namespace VendingMachine
{
	public interface IProductCatalogue
	{
		public bool Contains(int id);
		public Product GetProduct(int id);
	}
}

