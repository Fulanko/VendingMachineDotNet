using System;
namespace VendingMachine
{
	public class ProductCatalogue: IProductCatalogue
	{
		private Dictionary<int, Product> products = new Dictionary<int, Product>();

		public ProductCatalogue(string currency)
		{
			// init catalogue based on currency
			switch (currency)
            {
				case "EUR":
				case "USD":
					products[1] = new Product("COLA", 1);
					products[2] = new Product("CHIPS", 0.5m);
					products[3] = new Product("CANDY", 0.65m);
					break;
				case "JPY":
					products[1] = new Product("COLA", 100);
					products[2] = new Product("CHIPS", 50);
					products[3] = new Product("CANDY", 65);
					break;
			}
		}

		public bool Contains(int id)
        {
			return products.ContainsKey(id);
        }

		public Product GetProduct(int id)
        {
			if (products.ContainsKey(id))
            {
				return products[id];
            } else
            {
				throw new Exception($"Product with id {id} does not exist in ProductCatalogue.");
            }
        }
	}
}

