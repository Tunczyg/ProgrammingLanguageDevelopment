using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguageDevelopment
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataProvider = new DataProvider();
            for (var year = 2012; year < DateTime.Today.Year; year++)
            {
                Console.WriteLine(@"Proszê czekaæ... Trwa pobieranie danych z roku " + year.ToString() + ".");
                using (var fileStream = new FileStream(year.ToString() + ".txt", FileMode.Append))
                {                    
                    var staticLanguageFeatures = dataProvider.GetLanguageFeaturesDynamicData(year);
                    var stats = dataProvider.GetAnnualStatistics(staticLanguageFeatures, year);
                    using (var streamWriter = new StreamWriter(fileStream))
                    {
                        foreach (var record in stats)

                        {
                            var json = new JavaScriptSerializer().Serialize(record);
                            streamWriter.WriteLine(json);
                        }
                    }
                }
                Console.WriteLine(@"Pobrano dane z roku " + year.ToString() + ".");
            }
            Console.WriteLine(@"Pobieranie danych zakoñczone.");
            Console.WriteLine(@"Dane znajduj¹ siê w œcie¿ce ProgrammingLanguageDevelopment\bin\Debug");
        }
    }
}
