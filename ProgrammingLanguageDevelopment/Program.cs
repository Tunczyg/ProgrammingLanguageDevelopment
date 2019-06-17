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
            Console.WriteLine(@"Witamy w programie pobieraj�cym dane do symulacji rozwoju j�zyk�w programowania.");
            Console.WriteLine(@"Czas pobierania danych zale�y bezpo�rednio od szybko�ci Twojego ��cza internetowego");
            Console.WriteLine(@"dlatego przed kontynuacj� prosimy o upewnienie si� czy posiadasz wydajne po��czenie.");
            Console.WriteLine(@"Aby kontynuowa� naci�nij dowolny klawisz.");
            Console.ReadKey();
            for (var year = 2012; year < DateTime.Today.Year; year++)
            {
                Console.WriteLine(@"Prosz� czeka�... Trwa pobieranie danych z roku " + year.ToString() + ".");
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
            Console.WriteLine(@"Pobieranie danych zako�czone.");
            Console.WriteLine(@"Dane znajduj� si� w �cie�ce ProgrammingLanguageDevelopment\bin\Debug");
        }
    }
}
