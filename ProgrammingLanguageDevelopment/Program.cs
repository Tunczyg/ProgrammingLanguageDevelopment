using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguageDevelopment
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataProvider = new DataProvider();
            var staticLanguageFeatures = dataProvider.GetLanguageFeaturesStaticData();
            var parser = new WebParser();
            var sourceCode = parser.GetDataFromStackOverflow();
        }
    }
}
