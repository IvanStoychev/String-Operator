using Backend.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Useful.String.Extensions;

namespace Backend
{
    internal static class StringManipulator
    {
        static Dictionary<string, string> mergeFieldsValues = new Dictionary<string, string>();

        static StringManipulator()
        {
            mergeFieldsValues["{methodName}"] = string.Empty;
            mergeFieldsValues["{formattedParameters}"] = string.Empty;
            mergeFieldsValues["{anchorUrl}"] = string.Empty;
            mergeFieldsValues["{profileUrl}"] = string.Empty;
            mergeFieldsValues["{repoName}"] = string.Empty;
            mergeFieldsValues["{pageName}"] = string.Empty;
            mergeFieldsValues["{anchorLinkParameters}"] = string.Empty;
            mergeFieldsValues["{packageVersion}"] = string.Empty;
            mergeFieldsValues["{parameterType}"] = string.Empty;
            mergeFieldsValues["{parameterName}"] = string.Empty;
        }

        internal static HashSet<string> KeepOnlyMethodSignatures(string text)
        {
            HashSet<string> methodSignaturesSet = new HashSet<string>();

            foreach (string line in text.Split(Environment.NewLine))
                if (line.Contains("public static void"))
                    methodSignaturesSet.Add(line);

            return methodSignaturesSet;
        }

        internal static string BuildGithutWikiFromMethods(HashSet<string> methodSignaturesSet, string packageVersion)
        {
            mergeFieldsValues["{profileUrl}"] = "https://github.com/IvanStoychev";
            mergeFieldsValues["{repoName}"] = "Useful.Sqlite.Extensions";
            mergeFieldsValues["{pageName}"] = "SQLiteDataReaderExtensions";
            mergeFieldsValues["{packageVersion}"] = packageVersion;

            // Matches method accessors, e.g. "public static void".
            Regex regexAccessor = new Regex(@"^(\b\w* ){3}", RegexOptions.Compiled);
            // Matches a "this" parameter.
            Regex regexThisParam = new Regex(@"this \w* \w*", RegexOptions.Compiled);
            // Matches a parameter's type.
            Regex regexParameterType = new Regex(@"^\w* ", RegexOptions.Compiled);
            // Matches a parameter's name.
            Regex regexParameterName = new Regex(@" \w*$", RegexOptions.Compiled);

            StringBuilder methodLinks = new StringBuilder();
            StringBuilder methodDescriptions = new StringBuilder();

            // Collection of all templates that will be used.
            string templateAnchorUrl = File.ReadAllText("Github Wiki anchor url template.txt");
            string templateMethodLink = File.ReadAllText("Github Wiki method link template.txt");
            string templatePlainParameter = File.ReadAllText("Github Wiki plain parameter template.txt");
            string templateFormatedParameter = File.ReadAllText("Github Wiki formatted parameter template.txt");
            string templateMethodDescription = File.ReadAllText("Github Wiki anchor url template.txt");

            HashSet<GithubWikiMethod> githubWikiMethods = new HashSet<GithubWikiMethod>();

            foreach (string methodSig in methodSignaturesSet)
            {
                GithubWikiMethod method = new GithubWikiMethod();

                // Remove method accessors.
                string methodSigTrimmed = regexAccessor.Replace(methodSig, string.Empty);
                string methodName = methodSigTrimmed.Substring("(", false);

                // Get the parameter signature of the method.
                string methodParametersString = methodSigTrimmed.Substring("(", ")", StringInclusionOptions.IncludeNone);
                
                // Remove the "this" parameter.
                methodParametersString = regexThisParam.Replace(methodParametersString, string.Empty);
                string formattedParametersString = regexParameterType.Replace(methodParametersString, "**($1)**");
                formattedParametersString = regexParameterName.Replace(methodParametersString, "_($1)_");

                method.Name = methodName;
                method.FormattedParameters = formattedParametersString;

                //methodLinks.AppendLine
            }

            return null;
        }

        static string ReplaceMergeFields(string template)
        {
            string output = template;

            foreach (var item in mergeFieldsValues)
                output = output.Replace(item.Key, item.Value);

            return output;
        }
    }
}
