using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMachine;

namespace VendingMachineTests;

[TestClass]
public class ConsoleDisplayManagerTest
{
    ConsoleDisplayManager displayManager = new ConsoleDisplayManager();
    StringWriter stringWriter = new StringWriter();

    [TestMethod]
    [DataRow("Hello World")]
    [DataRow("0.10")]
    public void ShouldPrintToConsole(string message)
    {
        Console.SetOut(stringWriter);
        displayManager.Print(message);
        Assert.AreEqual($"{message}\n", stringWriter.ToString());
    }
}
