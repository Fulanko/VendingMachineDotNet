using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMachine;

namespace VendingMachineTests;

[TestClass]
public class TranslatorTest
{
    public TranslatorTest()
    {
    }

    [TestMethod]
    public void ShouldInitializeTranslationLanguage()
    {
        // valid translation
        Translator.SetLanguage("EN");
    }

    [TestMethod]
    [ExpectedException(typeof(FileNotFoundException))]
    public void ShouldThrowExceptionOnInvalidLanguage()
    {
        Translator.SetLanguage("LU");
    }

    [TestMethod]
    public void ShouldTranslateMessage()
    {
        Translator.SetLanguage("DE");

        Assert.AreEqual(Translator.Translate("SOLD_OUT"), "Ausverkauft");
        Assert.AreEqual(Translator.Translate("THANK_YOU"), "Vielen Dank");
        Assert.AreEqual(Translator.Translate("PRICE"), "Preis");

        Assert.AreEqual(Translator.Translate("MISSING_TRANSLATION"), "<MISSING_TRANSLATION>");
    }
}
