﻿using System.Text;

namespace Aki32_Utilities.Extensions;
public static partial class OwesomeExtensions
{

    /// <summary>
    /// create many template files named by csv list
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputDir">when null, automatically set to {inputFile.DirectoryName}/output_MakeFilesFromCsv</param>
    /// <param name="templateFile"></param>
    /// <returns></returns>
    public static DirectoryInfo MakeFilesFromCsv(this FileInfo inputFile, DirectoryInfo? outputDir, FileInfo templateFile)
    {
        // preprocess
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** MakeFilesFromCsv() Called");
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // to handle Shift-JIS
        if (outputDir is null)
            outputDir = new DirectoryInfo(Path.Combine(inputFile.DirectoryName, UtilConfig.GetNewOutputDirName("MakeFilesFromCsv")));
        if (!outputDir.Exists)
            outputDir.Create();


        // main
        var tempDataEx = Path.GetExtension(templateFile.Name);
        var csv = inputFile.ReadCsv_Rows(ignoreEmptyLine: true);

        foreach (var line in csv)
        {
            var targetName = line[0];

            if (string.IsNullOrEmpty(targetName)) continue;

            try
            {
                var targetPath = Path.Combine(outputDir.FullName, $"{targetName}{tempDataEx}");
                templateFile.CopyTo(targetPath, true);
                Console.WriteLine($"O: {targetPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"X: {ex}");
            }
        }

        // postprocess
        return outputDir;
    }
  
    /// <summary>
    /// create many directories named by csv list
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputDir">when null, automatically set to {inputFile.DirectoryName}/output_MakeFilesFromCsv</param>
    /// <param name="templateDir">when null, will be empty directories</param>
    /// <returns></returns>
    public static DirectoryInfo MakeDirsFromCsv(this FileInfo inputFile, DirectoryInfo? outputDir, DirectoryInfo templateDir = null)
    {
        // preprocess
        if (UtilConfig.ConsoleOutput)
            Console.WriteLine("\r\n** MakeFilesFromCsv() Called");
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // to handle Shift-JIS
        if (outputDir is null)
            outputDir = new DirectoryInfo(Path.Combine(inputFile.DirectoryName, UtilConfig.GetNewOutputDirName("MakeFilesFromCsv")));
        if (!outputDir.Exists)
            outputDir.Create();


        // main
        var csv = inputFile.ReadCsv_Rows(ignoreEmptyLine: true);

        foreach (var line in csv)
        {
            var targetName = line[0];

            if (string.IsNullOrEmpty(targetName)) continue;

            try
            {
                var targetDirPath = Path.Combine(outputDir.FullName, targetName);
                var targetDir = new DirectoryInfo(targetDirPath);
                if (templateDir == null)
                    targetDir.Create();
                else
                    templateDir.CopyTo(targetDir, consoleOutput: false);

                Console.WriteLine($"O: {targetDirPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"X: {ex}");
            }
        }

        // postprocess
        return outputDir;
    }

}
