using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMachine;

namespace VendingMachineTests;

[TestClass]
public class ProductTest
{
    Product product = new Product("CANDY", 0.65m);

    [TestMethod]
    public void ShouldGetTranslatedName()
    {
        Translator.SetLanguage("EN");
        Assert.AreEqual(product.name, "Candy");
        Translator.SetLanguage("DE");
        Assert.AreEqual(product.name, "Süßigkeit");
        Translator.SetLanguage("FR");
        Assert.AreEqual(product.name, "Bonbons");
    }
}
