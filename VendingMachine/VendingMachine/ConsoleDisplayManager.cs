using System;
namespace VendingMachine
{ 
	public class ConsoleDisplayManager: IDisplayManager
	{
		public void Print(string message)
        {
			Console.WriteLine(message);
        }
	}
}

