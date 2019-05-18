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
    
