using Backend.Github.Objects;
using System;
using System.Collections.Generic;
using System.IO;

namespace Backend
{
    class Program
    {
        static void Main(string[] args)
        {
            string wikiPage = MasterController.GenerateGithubWikiPage(@"J:\Users\Administrator\Source\Repos\Useful.String.Extensions\Useful.String.Extensions\Selector.cs");
            File.WriteAllText(@"D:\Desktop Archive\New Folder (3)\wiki.txt", wikiPage);
        }
    }
}
