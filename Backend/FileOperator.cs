using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Useful.String.Extensions;

namespace Backend
{
    internal class FileOperator
    {
        internal static string ReadFileText(string filePath)
        {
            string fileText = File.ReadAllText(filePath);
            return fileText;
        }

        internal static string GetPackageVersion(string projText)
        {
            string projVersion = projText.Substring("<Version>", "</Version>", StringInclusionOptions.IncludeNone);
            return projVersion;
        }

        internal static string GetProjectWikiUrl(string projText)
        {
            string wikiUrl = projText.Substring("<PackageProjectUrl>", "</PackageProjectUrl>", StringInclusionOptions.IncludeNone);
            return wikiUrl;
        }

        internal static string GetProjPath(string filePath)
        {
            string fileDirectory = Directory.GetParent(filePath).FullName;
            string projPath = Directory.EnumerateFiles(fileDirectory, "*.csproj").First();
            return projPath;
        }
    }
}
