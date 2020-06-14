using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Useful.String.Extensions;
using System.Text.RegularExpressions;

namespace Backend.Github.Objects
{
    internal class GithubWikiMethod
    {
        internal string Name;
        internal string Summary;
        internal string FormattedParameters;
        internal string AnchorUrl;

        internal GithubWikiMethod(string methodSignature, string summary)
        {
            Name = GetMethodName(methodSignature);
            Summary = summary;
            string methodParamters = GetMethodParametersFromSignature(methodSignature);
            string formattedParameters = FormatParameters(methodParamters);
            FormattedParameters = formattedParameters;
            AnchorUrl = $"{Name}-{FormattedParameters.Remove("*", "_").Replace(" ", "-").Replace(",", "")}".ToLower();
        }

        /// <summary>
        /// Retrieves the method name from the given signature.
        /// </summary>
        /// <param name="methodSignature">The signature of the method whose name you want to get.</param>
        /// <returns>A string representation of the method name.</returns>
        static string GetMethodName(string methodSignature)
        {
            Regex regexMethodName = new Regex(@"\w*\(", RegexOptions.Compiled);
            string methodName = regexMethodName.Match(methodSignature).Value.TrimEnd("(");

            return methodName;
        }

        /// <summary>
        /// Retrieves the method's parameters from the given signature.
        /// </summary>
        /// <param name="methodSignature">A signature of a method.</param>
        /// <returns>A string representation of the parameter list.</returns>
        static string GetMethodParametersFromSignature(string methodSignature)
        {
            string methodParameters = methodSignature.Substring("(", ")", StringInclusionOptions.IncludeNone);

            return methodParameters;
        }

        /// <summary>
        /// Formats the given paramters in accordance with the hardcoded
        /// Github wiki standard.
        /// </summary>
        /// <param name="methodParameters"></param>
        /// <returns></returns>
        static string FormatParameters(string methodParameters)
        {
            // Matches a parameter's type.
            Regex regexParameterType = new Regex(@"^\w*", RegexOptions.Compiled);
            // Matches a parameter's name.
            Regex regexParameterName = new Regex(@"\w+$", RegexOptions.Compiled);

            string formattedParametersString = "";
            foreach (string parameter in methodParameters.Split(", ", StringSplitOptions.RemoveEmptyEntries))
            {
                string formattedParameter = regexParameterType.Replace(parameter, "**$0**");
                formattedParameter = regexParameterName.Replace(formattedParameter, "_$0_");

                formattedParametersString += formattedParameter + ", ";
            }
            formattedParametersString = formattedParametersString.TrimEnd(", ");

            return formattedParametersString;
        }
    }
}
