using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class LocalizationCollector
{
    private readonly string filePath;

    public LocalizationCollector()
    {
        filePath = Path.Combine(
            Application.persistentDataPath,
            "translations.csv"
        );

        CreateFileIfNeeded();
    }

    private void CreateFileIfNeeded()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(
                filePath,
                "\"english\",\"polish\"\n",
                new UTF8Encoding(true)
            );
        }
    }

    public void Collect(string englishText)
    {
        if (string.IsNullOrEmpty(englishText))
            return;

        List<string> lines = new List<string>(
            File.ReadAllLines(filePath, Encoding.UTF8)
        );

        foreach (string line in lines)
        {
            if (string.IsNullOrEmpty(line))
                continue;

            if (line == "\"english\",\"polish\"")
                continue;

            string englishPart = GetFirstCsvColumn(line);
            if (englishPart == englishText)
                return;
        }
        string newLine = EscapeCsv(englishText) + ",";
        using (StreamWriter writer = new StreamWriter(
            filePath,
            false,
            new UTF8Encoding(true)))
        {
            foreach (string line in lines)
            {
                writer.WriteLine(line);
            }

            writer.WriteLine(newLine);
        }
    }

    private string EscapeCsv(string text)
    {
        text = text.Replace("\"", "\"\"");
        return "\"" + text + "\"";
    }

    private string GetFirstCsvColumn(string line)
    {
        if (!line.StartsWith("\""))
            return line;

        StringBuilder result = new StringBuilder();

        for (int i = 1; i < line.Length; i++)
        {
            char current = line[i];
            if (current == '"')
            {
                if (i + 1 < line.Length && line[i + 1] == '"')
                {
                    result.Append('"');
                    i++;
                }
                else
                {
                    break;
                }
            }
            else
            {
                result.Append(current);
            }
        }

        return result.ToString();
    }
}
