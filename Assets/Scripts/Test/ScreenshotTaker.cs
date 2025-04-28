using UnityEngine;
using System.IO;

public class ScreenshotTaker : MonoBehaviour
{
    public string folderName = "Screenshots";
    public string baseFileName = "screenshot";
    public KeyCode screenshotKey = KeyCode.F5;

    private int nextScreenshotNumber = 1;
    private string folderPath;

    private void Start()
    {
        // Set and create folder if needed
        folderPath = Path.Combine(Application.dataPath, folderName);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log($"Created folder: {folderPath}");
        }

        // Initialize the next screenshot number by scanning existing files
        InitializeNextScreenshotNumber();
    }

    private void Update()
    {
        if (Input.GetKeyDown(screenshotKey))
        {
            TakeScreenshot();
        }
    }

    private void InitializeNextScreenshotNumber()
    {
        // Get all files matching the pattern
        string[] files = Directory.GetFiles(folderPath, $"{baseFileName}_*.png");

        int highestNumber = 0;
        foreach (var file in files)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            string numberPart = fileName.Replace(baseFileName + "_", "");

            if (int.TryParse(numberPart, out int number))
            {
                if (number > highestNumber)
                {
                    highestNumber = number;
                }
            }
        }

        nextScreenshotNumber = highestNumber + 1;
        Debug.Log($"Next screenshot will be numbered: {nextScreenshotNumber}");
    }

    private void TakeScreenshot()
    {
        string fileName = $"{baseFileName}_{nextScreenshotNumber}.png";
        string filePath = Path.Combine(folderPath, fileName);

        ScreenCapture.CaptureScreenshot(filePath);
        Debug.Log($"Screenshot saved to: {filePath}");

        nextScreenshotNumber++;
    }
}

