using Backend.Github;
using Backend.Github.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Backend
{
    public static class MasterController
    {
        public static string GenerateGithubWikiPage(string filePath)
        {
            string code = FileOperator.ReadFileText(filePath);
            string projPath = FileOperator.GetProjPath(filePath);
            string projText = FileOperator.ReadFileText(projPath);
            string packageVersion = FileOperator.GetPackageVersion(projText);
            string projWikiUrl = FileOperator.GetProjectWikiUrl(projText);
            HashSet<GithubWikiMethod> methods = GithubWikiWorker.StripMethodsFromCode(code);
            StringBuilder fullDescriptions = new StringBuilder();
            StringBuilder links = new StringBuilder();

            GithubWikiWorker.RemoveExistingMethods("", methods);

            foreach (var method in methods)
            {
                string methodFullDescription = GithubWikiWorker.BuildGithubMethodDocumentation(method, packageVersion);
                string methodLink = GithubWikiWorker.BuildGithubMethodDocumentationLink(method);

                fullDescriptions.AppendLine(methodFullDescription);
                links.AppendLine(methodLink);
            }

            string separator = File.ReadAllText("Github\\Github Wiki separator.txt");

            return links.ToString() + separator + fullDescriptions.ToString().TrimEnd(Environment.NewLine.ToCharArray());
        }
    }
}
