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
            Application.streamingAssetsPath,
            "translations.csv"
        );
        Debug.Log(filePath);
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

        List<string> lines = ReadCsvRecords();

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
        Debug.Log("Added: " + englishText);
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

    public string LoadTranslate(string englishText)
    {
        int language = PlayerPrefs.GetInt("Language", 0);

        if (language == 0)
            return englishText;
        if (string.IsNullOrEmpty(englishText))
            return englishText;

        List<string> records = ReadCsvRecords();

        foreach (string record in records)
        {
            if (string.IsNullOrEmpty(record))
                continue;

            if (record == "\"english\",\"polish\"")
                continue;

            string englishPart = GetFirstCsvColumn(record);

            if (englishPart == englishText)
            {
                string polishPart = GetSecondCsvColumn(record);

                if (!string.IsNullOrEmpty(polishPart))
                    return polishPart;

                return englishText;
            }
        }

        return englishText;
    }

    private string GetSecondCsvColumn(string line)
    {
        bool insideQuotes = false;
        bool firstColumnFinished = false;

        StringBuilder result = new StringBuilder();

        for (int i = 0; i < line.Length; i++)
        {
            char current = line[i];

            if (current == '"')
            {
                if (insideQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    if (firstColumnFinished)
                        result.Append('"');

                    i++;
                }
                else
                {
                    insideQuotes = !insideQuotes;

                    if (!insideQuotes && !firstColumnFinished)
                    {
                        firstColumnFinished = true;
                    }
                }

                continue;
            }

            if (!firstColumnFinished)
            {
                continue;
            }

            if (current == ',' && !insideQuotes)
            {
                continue;
            }

            result.Append(current);
        }

        return result.ToString();
    }

    private List<string> ReadCsvRecords()
    {
        string content = File.ReadAllText(filePath, Encoding.UTF8);

        List<string> records = new List<string>();
        StringBuilder currentRecord = new StringBuilder();

        bool insideQuotes = false;

        for (int i = 0; i < content.Length; i++)
        {
            char c = content[i];

            if (c == '"')
            {
                currentRecord.Append(c);

                if (insideQuotes && i + 1 < content.Length && content[i + 1] == '"')
                {
                    currentRecord.Append(content[i + 1]);
                    i++;
                }
                else
                {
                    insideQuotes = !insideQuotes;
                }
            }
            else if ((c == '\n' || c == '\r') && !insideQuotes)
            {
                if (currentRecord.Length > 0)
                {
                    records.Add(currentRecord.ToString());
                    currentRecord.Clear();
                }

                if (c == '\r' && i + 1 < content.Length && content[i + 1] == '\n')
                    i++;
            }
            else
            {
                currentRecord.Append(c);
            }
        }

        if (currentRecord.Length > 0)
            records.Add(currentRecord.ToString());

        return records;
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
