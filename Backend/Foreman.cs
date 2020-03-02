using System;
using System.Collections.Generic;
using System.Text;

namespace Backend
{
    public static class Foreman
    {
        public static void BuildGithutWiki(string text)
        {
            HashSet<string> methodSignaturesSet = StringManipulator.KeepOnlyMethodSignatures(text);
            StringManipulator.BuildGithutWikiFromMethods(methodSignaturesSet, "1.1.0");
        }
    }
}
