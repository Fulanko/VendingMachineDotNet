using System;
namespace VendingMachine
{
	public interface ITellerMachine
	{
        public decimal customerBalance { get; }
        public char CurrencySymbol { get; }
		public void AddCoin(decimal coin);
		public decimal ReturnChange();
		public void DeductFromBalance(decimal amount);
		public bool HasExactChange(decimal price);
	}
}

