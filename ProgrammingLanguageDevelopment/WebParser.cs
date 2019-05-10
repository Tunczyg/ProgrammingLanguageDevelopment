using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProgrammingLanguageDevelopment
{
    class WebParser
    {
        public List<ProgrammingLanguage> GetDataFromComputerScienceWeb()
        {
            List<ProgrammingLanguage> list = new List<ProgrammingLanguage>();
            var html = GetRawSourceCode("https://www.computerscience.org/resources/computer-programming-languages");
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var inputs = from input in htmlDoc.DocumentNode.Descendants("div")
                         where input.Attributes["class"] != null && input.Attributes["class"].Value == "entry-content"
                         select input;

            foreach (var input in inputs)
            {
                var languages = from items in input.Descendants("section")
                                where items.Attributes["class"] != null && items.Attributes["class"].Value == "page-section sticky-waypoint"
                                && items.Attributes["id"] != null && items.Attributes["id"].Value != "what-are-computer-programming-languages"
                                select items;

                foreach (var language in languages)
                {
                    var names = from lang in language.Descendants("h2")
                                where lang.Attributes["class"] != null && lang.Attributes["class"].Value == "section-title"
                                select lang;
                    
                    var name = names.Last().InnerText;

                    int year = 0; var paradigm = "";
                    var typing = ""; var level = "";

                    Regex rgx = new Regex(@"\d{4}");
                    MatchCollection matches = rgx.Matches(language.InnerText);
                    if (matches.Count > 0) year = int.Parse(matches.Last().Value);

                    Regex rgx2 = new Regex(@"object-oriented|imperative|structure|multi-paradigm");
                    MatchCollection matches2 = rgx2.Matches(language.InnerText);
                    if (matches2.Count > 0) paradigm = matches2.Last().Value.ToLower();

                    Regex rgx3 = new Regex(@"static|dynamic");
                    MatchCollection matches3 = rgx3.Matches(language.InnerText);
                    if (matches3.Count > 0) typing = matches3.Last().Value.ToLower();

                    Regex rgx4 = new Regex(@"low|high");
                    MatchCollection matches4 = rgx4.Matches(language.InnerText);
                    if (matches4.Count > 0) level = matches4.Last().Value.ToLower();

                    list.Add(new ProgrammingLanguage(name, year, paradigm, typing, level));
                }
            }
            return list;
        }

        public List<ProgrammingLanguage> GetDataFromAdminsChoice()
        {
            List<ProgrammingLanguage> list = new List<ProgrammingLanguage>();
            var html = GetRawSourceCode("https://www.adminschoice.com/top-10-most-popular-programming-languages");
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var inputs = from input in htmlDoc.DocumentNode.Descendants("div")
                         where input.Attributes["class"] != null && input.Attributes["class"].Value == "entry clearfix"
                         select input;

            foreach (var input in inputs)
            {
                var languages = from items in input.Descendants("h2")
                                select items;

                foreach (var language in languages)
                {
                    var names = from lang in input.Descendants("h2")
                                select lang;
                    
                    var name = names.Last().InnerText;

                    int year = 0; var paradigm = "";
                    var typing = ""; var level = "";

                    Regex rgx = new Regex(@"\d{4}");
                    MatchCollection matches = rgx.Matches(language.InnerText);
                    if (matches.Count > 0) year = int.Parse(matches.Last().Value);

                    Regex rgx2 = new Regex(@"object-oriented|imperative|structure|multi-paradigm");
                    MatchCollection matches2 = rgx2.Matches(language.InnerText);
                    if (matches2.Count > 0) paradigm = matches2.Last().Value.ToLower();

                    Regex rgx3 = new Regex(@"static|dynamic");
                    MatchCollection matches3 = rgx3.Matches(language.InnerText);
                    if (matches3.Count > 0) typing = matches3.Last().Value.ToLower();

                    Regex rgx4 = new Regex(@"low|high");
                    MatchCollection matches4 = rgx4.Matches(language.InnerText);
                    if (matches4.Count > 0) level = matches4.Last().Value.ToLower();

                    list.Add(new ProgrammingLanguage(name, year, paradigm, typing, level));
                }
            }
            return list;
        }

        public List<ProgrammingLanguage> GetDataFromWikipedia()
        {
            var list = new List<ProgrammingLanguage>();
            var html = GetRawSourceCode("https://en.wikipedia.org/wiki/List_of_programming_languages?fbclid=IwAR07gQRfjtQ9f-Qe6_n_aOn4N7cJq1ds_UbfMMKZJhTZBeDMUwSoD1tdlzA");
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var divs = from items in htmlDoc.DocumentNode.Descendants("div")
                          where items.Attributes["class"] != null
                          && items.Attributes["class"]
                          .Value == "div-col columns column-width"
                          select items;
            foreach(var div in divs)
            {
                var uls = from ul in div.Descendants("ul")
                          select ul;
                
                foreach(var ul in uls)
                {
                    var lis = from li in ul.Descendants("li")
                                 select li;

                    foreach (var li in lis)
                    {

                        var urls = from a in li.Descendants("a")
                                        where a.Attributes["href"] != null
                                        select a.Attributes["href"].Value;

                        foreach(var url in urls)
                        {
                            var html2 = GetRawSourceCode("https://en.wikipedia.org" + url);
                            //skip empty entries
                            if (String.IsNullOrEmpty(html2))
                                continue;

                            var htmlDoc2 = new HtmlDocument();
                            htmlDoc2.LoadHtml(html2);

                            //extracts the Wikipedia table from the righ column
                            var tables = from table in htmlDoc2.DocumentNode.Descendants("table")
                                         where table.Attributes["class"] != null
                                         && table.Attributes["class"].Value == "infobox vet"
                                         select table;

                            //now: ensure that the table contains at least two interesting values (e.g. year, paradigm)+
                            //otherwise - skip it and continue+
                            //if it has sufficient data: create new instance of ProgrammingLanguage+
                            //fill that instance with data+
                            //add this instance to list+
                            var names = from name in table.Descendants("caption")
                                        where name.Attributes["class"] != null
                                        && name.Attributes["class"].Value == "summary"
                                        select name;

                            var name = name.Last().InnerText;

                            int year = 0; var paradigm = "";
                            var typing = ""; var level = "";
                            var dataCounter=0;

                            Regex rgx = new Regex(@"\d{4}");
                            MatchCollection matches = rgx.Matches(table.InnerText);
                            if (matches.Count > 0) {
                                year = int.Parse(matches.FirstOrDefault());
                                dataCounter++;
                            }
                           
                            Regex rgx2 = new Regex(@"object-oriented|imperative|structure|structured|multi-paradigm|procedural|functional|Imperative");
                            MatchCollection matches2 = rgx2.Matches(table.InnerText);
                            if (matches2.Count > 0) {
                                paradigm = matches2.FirstOrDefault().Value.ToLower();
                                dataCounter++;
                            }

                            Regex rgx3 = new Regex(@"static|dynamic|Static|Dynamic");
                            MatchCollection matches3 = rgx3.Matches(table.InnerText);
                            if (matches3.Count > 0) {
                                typing = matches3.Last().Value.ToLower();
                                dataCounter++;
                            }

                            Regex rgx4 = new Regex(@"low|high|high-level|low-level");
                            MatchCollection matches4 = rgx4.Matches(table.InnerText);
                            if (matches4.Count > 0) {
                                level = matches4.Last().Value.ToLower();
                                dataCounter++;
                            }

                            if(dataCounter<2)
                                continue;


                            list.Add(new ProgrammingLanguage(name, year, paradigm, typing, level));
                        }                        
                    }
                }                
            }
            return list;
        }


        public List<AnnualStatisticData> GetDataFromStackOverflowWeb(List<ProgrammingLanguage> RequestedLanguages)
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

        string GetRawSourceCode(string urlAddress)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            try
            {
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
            }
            catch (Exception ex)
            {
            }

            return new string("");
        }
    }
}
    
