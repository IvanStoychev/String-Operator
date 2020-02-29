using System;
using System.Collections.Generic;
using System.Text;
using Useful.String.Extensions;

namespace Backend
{
    internal static class StringManipulator
    {
        internal static HashSet<string> KeepOnlyMethodSignatures(string text)
        {
            HashSet<string> methodSignaturesSet = new HashSet<string>();

            foreach (string line in text.Split(Environment.NewLine))
                if (line.Contains("public static void"))
                    methodSignaturesSet.Add(line);

            return methodSignaturesSet;
        }

        internal static string BuildGithutWikiFromMethods(HashSet<string> methodSignaturesSet)
        {

        }
    }
}
