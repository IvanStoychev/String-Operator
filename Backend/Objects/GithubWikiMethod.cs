using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Backend.Objects
{
    internal class GithubWikiMethod
    {
        internal string Name;
        internal string Description;
        internal string DocumentationLink;
        internal string FormattedParameters;

        /// <summary>
        /// Extended description of the method, including in which version
        /// it was introduced, its name and parameters, formatted for display.
        /// </summary>
        internal string Documentation { get; private set; }

        bool SetDocumentation(string packageVersion)
        {
            if (!new string[] { Name, Description, FormattedParameters }.Any(x => string.IsNullOrWhiteSpace(x)))
                return false;

            string template = File.ReadAllText("Github Wiki method description template.txt");
            template = template.Replace("{methodName}", Name);
            template = template.Replace("{description}", Description);
            template = template.Replace("{packageVersion}", packageVersion);
            template = template.Replace("{formattedParameters}", FormattedParameters);

            Documentation = template;

            return true;
        }
    }
}
