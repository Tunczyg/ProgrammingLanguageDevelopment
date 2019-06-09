using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System;
using System.IO;


namespace ProgrammingLanguageDevelopment
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataProvider = new DataProvider();
            var staticLanguageFeatures = dataProvider.GetLanguageFeaturesDynamicData();
            var stats = dataProvider.GetAnnualStatistics(staticLanguageFeatures);
            var sw = File.CreateText("ostatnia.txt");

            foreach (var bekaxD in stats)

            {
                var json = new JavaScriptSerializer().Serialize(bekaxD);
                sw.WriteLine(json);

            }
        }
    }
}
