using System;
namespace VendingMachine
{
	public class Product
	{
		private string _name;
		public string name { get { return Translator.Translate(_name); } private set { _name = value; } }
		public decimal price { get; private set; }

		public Product(string name, decimal price)
		{
			this._name = name;
			this.price = price;
		}
	}
}

