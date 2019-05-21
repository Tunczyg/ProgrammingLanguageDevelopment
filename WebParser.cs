using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using System.Linq;

namespace ProgrammingLanguageDevelopment
{
    class WebParser
    {
        public List<ProgrammingLanguage> GetDataFromSecondWebsite()
        {

            var html = GetRawSourceCode("https://www.computerscience.org/resources/computer-programming-languages");
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var inputs = from input in htmlDoc.DocumentNode.Descendants("div")
                         where input.Attributes["class"] != null && input.Attributes["class"].Value == "entry-content"
                         select input;

            foreach (var input in inputs)
            {
                Console.WriteLine(input.Attributes["class"].Value);

                var languages = from items in input.ChildNodes.Descendants("section")
                                where items.Attributes["class"] != null
                                && items.Attributes["class"].Value == "sticky-waypoint"
                                select items;

                //help dlaczego language jest puste
                foreach (var language in languages)
                {
                    Console.WriteLine(language.Attributes["class"].Value);
                }
            }

            return new List<ProgrammingLanguage>();
        }

        public List<AnnualStatisticData> GetDataFromStackOverflow(List<ProgrammingLanguage> RequestedLanguages)
        {
            var statData = new List<AnnualStatisticData>();

            for(var year = 2015; year <= DateTime.Today.Year; year++)
            {
                var html = GetRawSourceCode("https://insights.stackoverflow.com/survey/"+ year + "/");
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                var searchingPhrase = "";
                switch(year)
                {
                    case 2015:
                        {
                            searchingPhrase = "techLanguages-" + year;
                            break;
                        }
                    case 2016:
                        {
                            searchingPhrase = "technology-most-popular-technologies-" + year;
                            break;
                        }
                    case 2017:
                        {
                            searchingPhrase = "technology-most-popular-technologies--of-this-category";
                            break;
                        }
                    default:
                        {
                            searchingPhrase = "technology-most-popular-technologies-all-respondents";
                            break;
                        }
                }

                var inputs = from items in htmlDoc.DocumentNode.Descendants("div")
                                         where items.Attributes["id"] != null
                                          && items.Attributes["id"].Value == searchingPhrase
                             select items;
                foreach (var input in inputs)
                {
                    var languages = from items in input.ChildNodes.Descendants("div")
                                    where items.Attributes["class"] != null
                                    && items.Attributes["class"].Value == "bar-row"
                                    select items;
                    //StringSplitOptions.RemoveEmptyEntries
                    var nameAndPopularity = languages
                        .Select(item => item.InnerText
                        .Split(new char[0], StringSplitOptions.RemoveEmptyEntries)
                        .Select(capture => capture.ToString())
                        .ToList())
                        .Select(i => new List<string>() { i[0], i.FirstOrDefault(e => e.Contains("%"))
                        .Replace('.', ',').Replace('%',' ') })
                        .ToList();

                    var ile = languages
                        .Select(item => item.InnerText
                        .Split(new char[0], StringSplitOptions.RemoveEmptyEntries)); 
                    statData.AddRange(from data in nameAndPopularity
                                       where RequestedLanguages.Any(language => 
                                            String.Equals(language.Name,data[0], StringComparison.CurrentCultureIgnoreCase)) 
                                       select new AnnualStatisticData(data[0], year));

                    foreach(var item in statData)
                    {
                        var lanData = nameAndPopularity.FirstOrDefault(data => 
                            String.Equals(item.LanguageName, data[0], StringComparison.CurrentCultureIgnoreCase));
                            Double.TryParse(lanData != null && lanData[1] != null ? lanData[1] : "0", out double result);
                            item.PopularitySurvey = result;
                    }
                }
            }
            return statData;
        }
        
            public List<AnnualStatisticData> GetDataFromBG()
            {
            var data_list = new List<AnnualStatisticData>();


            int amount_of_books;
            string[] tab_of_languages = new string[] {"java","python","c","c%2B%2B","visualBasicNET","c%23","javaScript","php","sql","objectiveC","matlab","assembly","perl","ruby","groovy","swift","go","objectPascal","visualBasic"};

            for(int i=0; i < 19; i++)
            {   
                var name_to_obj = tab_of_languages[i];

            for(var year = 2015; year <= DateTime.Today.Year; year++)
            {
            var html = GetRawSourceCode("https://katalogagh.cyfronet.pl/search/query?match_1=PHRASE&field_1&term_1="+ tab_of_languages[i] +"&facet_date=1.201." + year + "&sort=dateNewest&theme=bgagh");
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var year_to_obj = year;

            var checks = from check in htmlDoc.DocumentNode.Descendants("ul")
                         where check.Attributes["class"] != null && check.Attributes["class"].Value == "page-messages"
                         select check;

            if(!checks.Any())
            {

                 var inputs = from input in htmlDoc.DocumentNode.Descendants("div")
                 where input.Attributes["class"] != null && input.Attributes["class"].Value == "resultCount"
                 select input;
                    if (inputs.Any())
                    {
                     string s1 = (inputs.Last().InnerText);
                     amount_of_books = Int32.Parse(s1.Remove(0,23).Trim().Trim( new Char[] { '.' } ));
                    }
                    else amount_of_books = 0;
            }
            else amount_of_books = 0;

            AnnualStatisticData obj = new AnnualStatisticData(name_to_obj, year_to_obj); 
            data_list.Add(obj);
            obj.PublicationsAmount = amount_of_books;

            /* foreach (AnnualStatisticData obj in data_list)
            {
                 Console.WriteLine("Year: " + obj.Year +
                 "\nLanguage: " + obj.LanguageName +
                 "\nPublicationsAmount: " + obj.PublicationsAmount);
            }*/

            return data_list;
        }

        string GetRawSourceCode(string urlAddress)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                string data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();

                return data;
            }
            return new string("");
        }
    }
}
    
