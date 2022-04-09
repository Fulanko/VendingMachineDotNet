using System;
namespace VendingMachine
{
	public static class FileReaderCSV
	{
        public static List<List<string>> Read(string filepath)
        {
            var output = new List<List<string>>();
            if (File.Exists(filepath))
            {
                string[] lines = System.IO.File.ReadAllLines(filepath);
                foreach (string line in lines)
                {
                    var columns = line.Split(',').ToList();
                    output.Add(columns);
                }
                return output;
            }
            else
            {
                throw new FileNotFoundException($"LanguageFile not found: {filepath}");
            }
        }
    }
}

