using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

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

            var infoTables = new List<HtmlNode>();
            foreach (var div in divs)
            {
                var urls = from a in div.Descendants("a")
                           where a.Attributes["href"] != null
                           select a.Attributes["href"].Value;
                foreach (var url in urls)
                {
                    var html2 = GetRawSourceCode("https://en.wikipedia.org" + url);
                    //skip empty entries
                    if (String.IsNullOrEmpty(html2))
                        continue;

                    var htmlDoc2 = new HtmlDocument();
                    htmlDoc2.LoadHtml(html2);

                    //extracts the Wikipedia table from the righ column
                    infoTables.AddRange(from table in htmlDoc2.DocumentNode.Descendants("table")
                                        where table.Attributes["class"] != null
                                        && table.Attributes["class"].Value.Contains("infobox")
                                        select table);
                }
            }

            foreach (var table in infoTables)
            {
                var name = "";

                var names = from caption in table.Descendants("caption")
                            where caption.Attributes["class"] != null && caption.Attributes["class"].Value == "summary"
                            select caption;

                if (names.Last().InnerText != null) name = names.Last().InnerText;

                var rgx = new Regex(@"(?<=First&#160;appeared</th><td>)(.*?)(?=<)");
                var year = rgx.Matches(table.InnerHtml).FirstOrDefault();
                int int_year = 0;
                if (year != null) Int32.TryParse(year.ToString(), out int_year);


                 rgx = new Regex(@"(?<=Paradigm</a></th><td><a)(.*?)(?=</td>)");
                var paradigm = rgx.Matches(table.InnerHtml).FirstOrDefault();              
                rgx = new Regex(@"(?<=>)(.*?)(?=</a>)");
                if(paradigm!=null) paradigm=rgx.Matches(paradigm.Value).FirstOrDefault();


                rgx = new Regex(@"(?<=Typing discipline</a></th><td>)(.*?)(?=</td>)");
                var typing = rgx.Matches(table.InnerHtml).FirstOrDefault(); 
                rgx = new Regex(@"(?<=>)(.*?)(?=</a>)");
                if(typing!=null) typing=rgx.Matches(typing.Value).FirstOrDefault();
                

                rgx = new Regex(@"(?<=OS</a></th><td>)(.*?)(?=</td>)");
                var oper_sys = rgx.Matches(table.InnerHtml).FirstOrDefault();
                rgx = new Regex(@"(?<=>)(.*?)(?=</a>)");
                if(oper_sys!=null) oper_sys=rgx.Matches(oper_sys.Value).FirstOrDefault();

                string level="";
        
                Enum.TryParse(paradigm.Value, out ProgrammingLanguage.Paradigm _Paradigm);
                Enum.TryParse(typing.Value, out ProgrammingLanguage.Typing _Typing);
                Enum.TryParse(level, out ProgrammingLanguage.Level _Level);
                Enum.TryParse(oper_sys.Value, out ProgrammingLanguage.OperatingSystem _Operating_system);

                //usually no info about level - leave blank or find an indirect way of extraction

                Console.WriteLine("\t Name: " + name +
                "\t Int_Year: " + int_year +
                "\t Typing: " + typing +
                "\t Paradigm: " + paradigm +
                "\t Oper_sys: " + oper_sys);
                list.Add(new ProgrammingLanguage(name, int_year, _Paradigm, _Typing, _Level, _Operating_system));
            }
             
            return list;
        }

        public List<AnnualStatisticData> GetDataFromStackOverflowWeb(List<ProgrammingLanguage> RequestedLanguages)
        {
            var statData = new List<AnnualStatisticData>();

            for (var year = 2015; year <= DateTime.Today.Year; year++)
            {
                var html = GetRawSourceCode("https://insights.stackoverflow.com/survey/" + year + "/");
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                var searchingPhrase = "";
                switch (year)
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
                                           String.Equals(language.Name, data[0], StringComparison.CurrentCultureIgnoreCase))
                                      select new AnnualStatisticData(data[0], year));

                    foreach (var item in statData)
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

        public List<AnnualStatisticData> GetDataFromGitHubWeb(List<ProgrammingLanguage> RequestedLanguages, List<AnnualStatisticData> ExistingStats)
        {
            var statData = new List<AnnualStatisticData>();

            // Create a request for the URL.   
            WebRequest request = WebRequest.Create(
              "https://madnight.github.io/githut/gh-pull-request_56d080.json");
            // If required by the server, set the credentials.  
            request.Credentials = CredentialCache.DefaultCredentials;

            // Get the response.  
            WebResponse response = request.GetResponse();
            // Display the status.  
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server. 
            // The using block ensures the stream is automatically closed. 
            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                
                var responseFromServer = reader.ReadToEnd().Replace("}\n{", "},\n{").Replace("}\r\n{", "},\n{");
                var jsonResponseFormat = "{\"items\": [\n" + responseFromServer + "\n]}";
                var jsons = JObject.Parse(jsonResponseFormat).SelectToken("items");

                var statsGH = new List<AnnualStatisticData>();
                foreach (var outerLoopItem in jsons)
                {
                    var name = outerLoopItem.SelectToken("name").ToString();
                    var year = outerLoopItem.SelectToken("year").ToString();

                    var allQuaters = jsons.Where(innerLoopItem =>
                    string.Equals(innerLoopItem.SelectToken("name").ToString(), name, StringComparison.CurrentCultureIgnoreCase) &&
                    string.Equals(innerLoopItem.SelectToken("year").ToString(), year, StringComparison.CurrentCultureIgnoreCase));
                    var annualSum = allQuaters.Sum(item => int.Parse(item.SelectToken("count").ToString()));
                    var newRecord = new AnnualStatisticData(name, int.Parse(year));
                    newRecord.PullRequestsAmount = annualSum;
                    if (statsGH.Any(rec => rec.LanguageName == newRecord.LanguageName && rec.Year == newRecord.Year))
                        continue;
                    statsGH.Add(newRecord);
                }

                //selecting only requested languages
                var requestedStats = statsGH.Where(record => 
                    RequestedLanguages.Any(language => 
                    string.Equals(language.Name, record.LanguageName, StringComparison.CurrentCultureIgnoreCase)))
                    .ToList();
                //extract stats consistent with existing stats
                var existingStatsDuplicates = requestedStats.Where(record => ExistingStats.Any(item =>
                    string.Equals(item.LanguageName, record.LanguageName, StringComparison.CurrentCultureIgnoreCase) &&
                    item.Year == record.Year))
                    .ToList();
                //and those non consistent
                var existingStatsNonDuplicates = ExistingStats.Where(record =>
                    existingStatsDuplicates.Any(item =>
                    item == record)).ToList();

                //complete duplicated stats
                foreach (var stat in existingStatsDuplicates)
                {
                    var missingData = ExistingStats.FirstOrDefault(record => 
                    string.Equals(stat.LanguageName, record.LanguageName, StringComparison.CurrentCultureIgnoreCase) &&
                    stat.Year == record.Year);
                    stat.PopularitySurvey = missingData.PopularitySurvey;
                }
                //records from GH only:
                var ghOnly = requestedStats.Where(stat1 => !existingStatsDuplicates.Any(stat2 => stat1 == stat2));

                //add to the returned result
                statData.AddRange(existingStatsDuplicates);
                statData.AddRange(existingStatsNonDuplicates);
                statData.AddRange(ghOnly);
            }
            // Close the response.  
            response.Close();            
            return statData;
        }

        public List<AnnualStatisticData> GetDataFromBG(List<ProgrammingLanguage> RequestedLanguages)
        {
            var data_list = new List<AnnualStatisticData>();
            int amount_of_books;
            
            for (int i = 0; i < RequestedLanguages.Count; i++)
            {
                var name_of_language = RequestedLanguages[i].Name;

                if(name_of_language == "c++")
                    name_of_language = "c%2B%2B";

                if(name_of_language == "c#")
                    name_of_language = "c%23";
          

                for (var year = 2015; year <= DateTime.Today.Year; year++)
                {
                    var html = GetRawSourceCode("https://katalogagh.cyfronet.pl/search/query?match_1=PHRASE&field_1&term_1=" + name_of_language + "&facet_date=1.201." + year + "&sort=dateNewest&theme=bgagh");
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);

                    var year_to_obj = year;

                    var checks = from check in htmlDoc.DocumentNode.Descendants("ul")
                                 where check.Attributes["class"] != null && check.Attributes["class"].Value == "page-messages"
                                 select check;

                    if (!checks.Any())
                    {

                        var inputs = from input in htmlDoc.DocumentNode.Descendants("div")
                                     where input.Attributes["class"] != null && input.Attributes["class"].Value == "resultCount"
                                     select input;
                        if (inputs.Any())
                        {
                            string s1 = (inputs.Last().InnerText);
                            amount_of_books = Int32.Parse(s1.Remove(0, 23).Trim().Trim(new Char[] { '.' }));
                        }
                        else amount_of_books = 0;
                    }
                    else amount_of_books = 0;

                    AnnualStatisticData obj = new AnnualStatisticData(name_of_language, year_to_obj);
                    data_list.Add(obj);
                    obj.PublicationsAmount = amount_of_books;

                    /* foreach (AnnualStatisticData obj in data_list)
                    {
                         Console.WriteLine("Year: " + obj.Year +
                         "\nLanguage: " + obj.LanguageName +
                         "\nPublicationsAmount: " + obj.PublicationsAmount);
                    }*/
                }                
            }
            return data_list;
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
