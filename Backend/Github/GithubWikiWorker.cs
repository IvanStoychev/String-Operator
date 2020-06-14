using Backend.Github.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Useful.String.Extensions;

namespace Backend.Github
{
    internal static class GithubWikiWorker
    {
        /// <summary>
        /// Returns a list of method signatures and their summaries from the given source code.
        /// </summary>
        /// <param name="code">Source code from which to extract methods.</param>
        /// <returns>A list of method signatures and their summaries.</returns>
        internal static HashSet<GithubWikiMethod> StripMethodsFromCode(string code)
        {
            code = code.Substring(code.IndexOf("class") + 5);

            // Matches method signature.
            Regex regexMethodSig = new Regex(@"public.*\w* \w*\(.*\)", RegexOptions.Compiled);

            HashSet<GithubWikiMethod> output = new HashSet<GithubWikiMethod>();
            bool read = false;
            StringBuilder methodSummary = new StringBuilder();
            foreach (var line in code.Split(Environment.NewLine))
            {
                if (line.Contains("<summary>")) read = true;

                if (regexMethodSig.IsMatch(line))
                {
                    read = false;
                    string methodSummaryString = methodSummary.ToString();
                    methodSummaryString = methodSummaryString.Substring("<summary>", "</summary>", StringInclusionOptions.IncludeNone);
                    var wikiMethod = new GithubWikiMethod(CleanMethodSignature(line.Trim()), CleanSummary(methodSummaryString));
                    output.Add(wikiMethod);
                    methodSummary = new StringBuilder();
                }

                if (read) methodSummary.AppendLine(line.Trim());
            }

            return output;
        }

        /// <summary>
        /// Builds the Github wiki documentation for the given method.
        /// </summary>
        /// <param name="wikiMethod">Method for which to build documentation.</param>
        /// <param name="packageVersion">The version in which this method was introduced.</param>
        /// <returns>The method's full documentation.</returns>
        internal static string BuildGithubMethodDocumentation(GithubWikiMethod wikiMethod, string packageVersion)
        {
            string template = File.ReadAllText("Github\\Github Wiki method description template.txt");
            template = template.Replace("{methodName}", wikiMethod.Name);
            template = template.Replace("{description}", wikiMethod.Summary);
            template = template.Replace("{packageVersion}", packageVersion);
            template = template.Replace("{formattedParameters}", wikiMethod.FormattedParameters);
            template = template.Replace("{anchorUrl}", wikiMethod.AnchorUrl);

            return template;
        }

        /// <summary>
        /// Builds the Github wiki documentation link for the given method.
        /// </summary>
        /// <param name="wikiMethod">Method for which to build documentation.</param>
        /// <returns>A link to the anchor of the method's full documentation.</returns>
        internal static string BuildGithubMethodDocumentationLink(GithubWikiMethod wikiMethod)
        {
            string template = File.ReadAllText("Github\\Github Wiki method link template.txt");
            template = template.Replace("{methodName}", wikiMethod.Name);
            template = template.Replace("{formattedParameters}", wikiMethod.FormattedParameters);
            template = template.Replace("{anchorUrl}", wikiMethod.AnchorUrl);

            return template;
        }

        internal static void RemoveExistingMethods(string wikiPage, HashSet<GithubWikiMethod> wikiMethods)
        {
            string pageTrimmed = wikiPage.Substring(0, wikiPage.IndexOf("***"));

            foreach (string line in wikiPage.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
            {
                string methodSignature = line.Substring("[", "]", StringInclusionOptions.IncludeNone);
                string methodName = methodSignature.Substring(0, methodSignature.IndexOf(" "));
                string methodFormattedParameters = methodSignature.Substring("(", ")", StringInclusionOptions.IncludeNone);
                if (wikiMethods.Any(m => m.Name == methodName && m.FormattedParameters == methodFormattedParameters))
                    wikiMethods.Remove(wikiMethods.First(m => m.Name == methodName && m.FormattedParameters == methodFormattedParameters));
            }
        }

        /// <summary>
        /// Keeps only the method name and arguments.
        /// </summary>
        /// <param name="method">A method signature, complete with accessors and/or return type.</param>
        /// <returns>A string containing the method name and parameters.</returns>
        static string CleanMethodSignature(string method)
        {
            Regex regexMethodNameAndParameters = new Regex(@"\w*\(.*\)", RegexOptions.Compiled);
            string methodNameAndParameters = regexMethodNameAndParameters.Match(method).Value;
            methodNameAndParameters = RemoveThisParameter(methodNameAndParameters);

            return methodNameAndParameters;
        }

        /// <summary>
        /// Removes "this" parameter from the given method signature.
        /// </summary>
        /// <param name="methodSignature">The method signature to clean.</param>
        /// <returns>A method signature without "this" parameter.</returns>
        static string RemoveThisParameter(string methodSignature)
        {
            // Matches a "this" parameter.
            Regex regexThisParam = new Regex(@"this \w* \w*(, )?", RegexOptions.Compiled);
            string output = regexThisParam.Replace(methodSignature, "");

            return output;
        }

        /// <summary>
        /// Removes triple slashes and newlines from the given summary.
        /// </summary>
        /// <param name="summary">An ordinary, unformatted method summary.</param>
        /// <returns>Just the text of the method summary.</returns>
        static string CleanSummary(string summary)
        {
            string output = summary.Replace("/// ", "").Trim('\r', '\n').Replace(Environment.NewLine, " ");

            return output;
        }
    }
}
