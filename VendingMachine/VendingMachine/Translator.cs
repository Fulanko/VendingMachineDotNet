using System;
namespace VendingMachine
{
    public static class Translator
    {
        public static string Language { get; private set; } = "";
        private static Dictionary<string, string> translations = new Dictionary<string, string>();

        public static void SetLanguage(string language)
        {
            string path = Path.Combine(new string[] { Directory.GetCurrentDirectory(), "Translations", $"language{language}.csv" });

            var content = FileReaderCSV.Read(path);
            translations = new Dictionary<string, string>();
            foreach (var row in content)
            {
                if (row.Count() == 2)
                {
                    translations[row[0]] = row[1];
                }
                else
                {
                    throw new Exception($"Invalid file format: {path}");
                }
            }
            Language = language;
        }

        public static string Translate(string message)
        {
            if (translations.ContainsKey(message))
            {
                return translations[message];
            }
            return $"<{message}>";
        }
    }
}