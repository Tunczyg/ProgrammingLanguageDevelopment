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
            var year = 2019;
            var dataProvider = new DataProvider();
            var staticLanguageFeatures = dataProvider.GetLanguageFeaturesDynamicData(year);
            var stats = dataProvider.GetAnnualStatistics(staticLanguageFeatures, year);
            var count = stats.Count;

            var count2013 = stats.Where(e => e.Year == 2013).ToList(); //91
            var count2013_1 = stats.Where(e => e.Year == 2013 && e.Quarter == 1).ToList(); //39
            var count2013_2 = stats.Where(e => e.Year == 2013 && e.Quarter == 2).ToList();//16
            var count2013_3 = stats.Where(e => e.Year == 2013 && e.Quarter == 3).ToList(); //16
            var count2013_4 = stats.Where(e => e.Year == 2013 && e.Quarter == 4).ToList(); //20
            var count2013_0 = stats.Where(e => e.Year == 2013 && e.Quarter != 1 && e.Quarter != 2 && e.Quarter != 3 && e.Quarter != 4).ToList();


            var count2014 = stats.Where(e => e.Year == 2014).ToList();
            var count2014_1 = stats.Where(e => e.Year == 2014 && e.Quarter == 1).ToList();//54
            var count2014_2 = stats.Where(e => e.Year == 2014 && e.Quarter == 2).ToList();//18
            var count2014_3 = stats.Where(e => e.Year == 2014 && e.Quarter == 3).ToList();//17
            var count201_4 = stats.Where(e => e.Year == 2014 && e.Quarter == 4).ToList();//16
            var count2014_0 = stats.Where(e => e.Year == 2014 && e.Quarter != 1 && e.Quarter != 2 && e.Quarter != 3 && e.Quarter != 4).ToList();


            var count2015 = stats.Where(e => e.Year == 2015).ToList(); //51
            var count2015_1 = stats.Where(e => e.Year == 2015 && e.Quarter == 1).ToList(); //68
            var count2015_2 = stats.Where(e => e.Year == 2015 && e.Quarter == 2).ToList(); //68
            var count2015_3 = stats.Where(e => e.Year == 2015 && e.Quarter == 3).ToList(); //70
            var count2015_4 = stats.Where(e => e.Year == 2015 && e.Quarter == 4).ToList(); //68
            var count2015_0 = stats.Where(e => e.Year == 2015 && e.Quarter != 1 && e.Quarter != 2 && e.Quarter != 3 && e.Quarter != 4).ToList();


            var count2016 = stats.Where(e => e.Year == 2016).ToList(); //51
            var count2016_1 = stats.Where(e => e.Year == 2016 && e.Quarter == 1).ToList(); //50
            var count2016_2 = stats.Where(e => e.Year == 2016 && e.Quarter == 2).ToList();
            var count2016_3 = stats.Where(e => e.Year == 2016 && e.Quarter == 3).ToList(); //1
            var count2016_4 = stats.Where(e => e.Year == 2016 && e.Quarter == 4).ToList();
            var count2016_0 = stats.Where(e => e.Year == 2016 && e.Quarter != 1 && e.Quarter != 2 && e.Quarter != 3 && e.Quarter != 4).ToList();


            var count2017 = stats.Where(e => e.Year == 2017).ToList(); //51
            var count2017_1 = stats.Where(e => e.Year == 2017 && e.Quarter == 1).ToList(); //46
            var count2017_2 = stats.Where(e => e.Year == 2017 && e.Quarter == 2).ToList(); //2
            var count2017_3 = stats.Where(e => e.Year == 2017 && e.Quarter == 3).ToList(); //2
            var count2017_4 = stats.Where(e => e.Year == 2017 && e.Quarter == 4).ToList(); //1
            var count2017_0 = stats.Where(e => e.Year == 2017 && e.Quarter != 1 && e.Quarter != 2 && e.Quarter != 3 && e.Quarter != 4).ToList();


            var count2018 = stats.Where(e => e.Year == 2018).ToList(); //49
            var count2018_1 = stats.Where(e => e.Year == 2018 && e.Quarter == 1).ToList(); //45
            var count2018_2 = stats.Where(e => e.Year == 2018 && e.Quarter == 2).ToList(); //2
            var count2018_3 = stats.Where(e => e.Year == 2018 && e.Quarter == 3).ToList(); //1
            var count2018_4 = stats.Where(e => e.Year == 2018 && e.Quarter == 4).ToList(); //1
            var count2018_0 = stats.Where(e => e.Year == 2018 && e.Quarter != 1 && e.Quarter != 2 && e.Quarter != 3 && e.Quarter != 4).ToList();


            var count2019 = stats.Where(e => e.Year == 2019).ToList();
            var count2019_1 = stats.Where(e => e.Year == 2019 && e.Quarter == 1).ToList(); //41
            var count2019_2 = stats.Where(e => e.Year == 2019 && e.Quarter == 2).ToList(); //1
            var count2019_3 = stats.Where(e => e.Year == 2019 && e.Quarter == 3).ToList();
            var count2019_4 = stats.Where(e => e.Year == 2019 && e.Quarter == 4).ToList();
            var count2019_0 = stats.Where(e => e.Year == 2019 && e.Quarter != 1 && e.Quarter != 2 && e.Quarter != 3 && e.Quarter != 4).ToList();

            using (var fileStream = new FileStream(year.ToString() + ".txt", FileMode.Append)) 
            using (var streamWriter = new StreamWriter(fileStream))
            {
                foreach (var bekaxD in stats)

                {
                    var json = new JavaScriptSerializer().Serialize(bekaxD);
                    streamWriter.WriteLine(json);
                }
            }
            /*
            var sw1 = File.CreateText(year + "_1.txt");
            foreach (var bekaxD in count2013_1)

            {
                var json = new JavaScriptSerializer().Serialize(bekaxD);
                sw1.WriteLine(json);
            }

            var sw2 = File.CreateText(year + "_2.txt");
            foreach (var bekaxD in count2013_2)

            {
                var json = new JavaScriptSerializer().Serialize(bekaxD);
                sw2.WriteLine(json);
            }
            var sw3 = File.CreateText(year + "_3.txt");
            foreach (var bekaxD in count2013_3)

            {
                var json = new JavaScriptSerializer().Serialize(bekaxD);
                sw3.WriteLine(json);
            }

            var sw4 = File.CreateText(year + "_4.txt");
            foreach(var bekaxD in count2013_4)
            {
                var json = new JavaScriptSerializer().Serialize(bekaxD);
                sw4.WriteLine(json);
            }
            */

        }
    }
}
