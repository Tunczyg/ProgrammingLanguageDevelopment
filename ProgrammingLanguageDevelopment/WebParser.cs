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
        public List<ProgrammingLanguage> GetDataFromComputerScienceWeb(List<ProgrammingLanguage> ExistingLanguages)
        {

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

                    foreach (var exist in ExistingLanguages)
                    {
                        if (exist.Name.ToLower() == name.ToLower())
                        {
                            int year = 0;
                            string paradigm = "";
                            var typing = ""; var level = "";

                            //year
                            if (exist.Year == 0)
                            {
                                Regex rgx = new Regex(@"\d{4}");
                                MatchCollection matches = rgx.Matches(language.InnerText);
                                if (matches.Count > 0) year = int.Parse(matches.Last().Value);
                                exist.Year = year;
                            }

                            //paradigm
                            Regex rgx2 = new Regex(@"object-oriented|imperative|structure|multi-paradigm");
                            MatchCollection matches2 = rgx2.Matches(language.InnerText);
                            if (matches2.Count > 0)
                            {
                                ProgrammingLanguage.Paradigm para_enum;
                                paradigm = matches2.Last().Value.ToLower().Replace("-", "_");
                                paradigm = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(paradigm);

                                Enum.TryParse(paradigm, out para_enum);
                                if (!(exist._Paradigm.Any(item => item == para_enum)))
                                    exist._Paradigm.Add(para_enum);
                            }
                            
                            //typing
                            Regex rgx3 = new Regex(@"static|dynamic");
                            MatchCollection matches3 = rgx3.Matches(language.InnerText);
                            if (matches3.Count > 0)
                            { 
                                typing = matches3.Last().Value.ToLower();
                                typing = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(typing);
                                ProgrammingLanguage.Typing type_enum;

                                Enum.TryParse(typing, out type_enum);
                                if (!(exist._Typing.Any(item => item == type_enum)))
                                    exist._Typing.Add(type_enum);
                            }

                            //level
                            Regex rgx4 = new Regex(@"low|high");
                            MatchCollection matches4 = rgx4.Matches(language.InnerText);
                            if (matches4.Count > 0)
                            {
                                ProgrammingLanguage.Level lvl_enum;
                                level = matches4.Last().Value.ToLower();
                                level = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(level);

                                Enum.TryParse(level, out lvl_enum);
                                if (!(exist._Level.Any(item => item == lvl_enum)))
                                    exist._Level.Add(lvl_enum);
                            }

                        }
                    }
                }        

            }
            return ExistingLanguages;
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
                var names = from caption in table.Descendants("caption")
                            where caption.Attributes["class"] != null && caption.Attributes["class"].Value == "summary"
                            select caption;

                if (names.Count() == 0 || names.Last() == null)  continue;                
                var name = names.Last().InnerText;

                var rgx = new Regex(@"(?<=First&#160;appeared</th><td>)(.*?)(?=<)");
                var year = rgx.Matches(table.InnerHtml).FirstOrDefault();
                int int_year = 0;
                if (year != null) Int32.TryParse(year.ToString(), out int_year);


                rgx = new Regex(@"(?<=Paradigm</a></th><td><a)(.*?)(?=</td>)");
                var maches_paradigm = rgx.Matches(table.InnerHtml);
                var paradigms = new List<ProgrammingLanguage.Paradigm>();
                if (maches_paradigm.Count > 0)
                {
                    rgx = new Regex("(?<=\">)(.*?)(?=</a>)");
                    var p = rgx.Matches(maches_paradigm.First().Value);
                    foreach (Match match in p)
                    {
                        var capture = match.Value.ToLower().Replace("-", "_");
                        capture = capture.Replace(" ", "_");
                        capture = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(capture);

                        Enum.TryParse(capture, out ProgrammingLanguage.Paradigm result);
                        if (result == ProgrammingLanguage.Paradigm.Other &&
                            paradigms.Any(item => item == ProgrammingLanguage.Paradigm.Other))
                            continue;
                        paradigms.Add(result);
                    }
                }

                rgx = new Regex(@"(?<=Typing discipline</a></th><td>)(.*?)(?=</td>)");
                var maches_typing = rgx.Matches(table.InnerHtml);
                var typings = new List<ProgrammingLanguage.Typing>();
                if (maches_typing.Count > 0)
                {
                    rgx = new Regex("(?<=\">)(.*?)(?=</a>)");
                    var t = rgx.Matches(maches_typing.First().Value);
                    foreach (Match match in t)
                    {
                        var capture = match.Value.ToLower().Replace("-", "_");
                        capture = capture.Replace(" ", "_");
                        capture = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(capture);

                        Enum.TryParse(capture, out ProgrammingLanguage.Typing result);
                        if (result == ProgrammingLanguage.Typing.Other &&
                            typings.Any(item => item == ProgrammingLanguage.Typing.Other))
                            continue;
                        typings.Add(result);
                    }
                }

                rgx = new Regex(@"(?<=OS</a></th><td>)(.*?)(?=</td>)");
                var maches_os = rgx.Matches(table.InnerHtml);
                var oss = new List<ProgrammingLanguage.OperatingSystem>();
                if (maches_os.Count > 0)
                {
                    rgx = new Regex("(?<=\">)(.*?)(?=</a>)");
                    var o = rgx.Matches(maches_os.First().Value);
                    foreach (Match match in o)
                    {
                        var capture = match.Value.ToLower().Replace("-", "_");
                        capture = capture.Replace(" ", "_");
                        capture = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(capture);

                        Enum.TryParse(capture, out ProgrammingLanguage.OperatingSystem result);
                        if (result == ProgrammingLanguage.OperatingSystem.Other &&
                            oss.Any(item => item == ProgrammingLanguage.OperatingSystem.Other))
                            continue;
                        oss.Add(result);
                    }
                }

                //usually no info about level - leave blank or find an indirect way of extraction
                var lvls = new List<ProgrammingLanguage.Level>();
                Enum.TryParse("", out ProgrammingLanguage.Level lvl);
                lvls.Add(lvl);

                //if we dont have sufficient information we skip that record
                if ((paradigms.Any() ? 1 : 0) + (typings.Any() ? 1 : 0) + (oss.Any() ? 1 : 0) < 2)
                    continue;
                list.Add(new ProgrammingLanguage(name, int_year, paradigms, typings, lvls, oss));
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

            //Request URL must be changed based on current json link - if throws 404 then update the URL
            WebRequest request = WebRequest.Create(
              "https://madnight.github.io/githut/gh-pull-request_ed268f.json");
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

        public List<AnnualStatisticData> GetDataFromGitHubWebLink1(List<ProgrammingLanguage> RequestedLanguages, List<AnnualStatisticData> ExistingStats)
        {
            var statData = new List<AnnualStatisticData>();

            //Request URL must be changed based on current json link - if throws 404 then update the URL
            WebRequest request = WebRequest.Create(
              "https://madnight.github.io/githut/gh-push-event_772e60.json");
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

        public List<AnnualStatisticData> GetDataFromGitHubWebLink2(List<ProgrammingLanguage> RequestedLanguages, List<AnnualStatisticData> ExistingStats)
        {
            var statData = new List<AnnualStatisticData>();

            //Request URL must be changed based on current json link - if throws 404 then update the URL
            WebRequest request = WebRequest.Create(
              "https://madnight.github.io/githut/gh-star-event_06869c.json");
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

        public List<AnnualStatisticData> GetDataFromGitHubWebLink3(List<ProgrammingLanguage> RequestedLanguages, List<AnnualStatisticData> ExistingStats)
        {
            var statData = new List<AnnualStatisticData>();

            //Request URL must be changed based on current json link - if throws 404 then update the URL
            WebRequest request = WebRequest.Create(
              "https://madnight.github.io/githut/gh-issue-event_374d40.json");
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
        public List<AnnualStatisticData> GetDataFromBG(List<AnnualStatisticData> ExistingStats)
        {
            var names = ExistingStats.Select(item => item.LanguageName);
            //!! includes edition of object passed to function !!
            foreach (var language in names)
            {
                var name_of_language = language;
                //getting rid of special signs, parenthesis and slashes before searching
                name_of_language = name_of_language.Replace("++", "%2B%2B");
                name_of_language = name_of_language.Replace("#", "%23");
                var rgx = new Regex(@".*?(?=\()");
                var matches = rgx.Matches(name_of_language);
                if (matches.Count > 0) name_of_language = matches.First().Value;
                else
                {
                    rgx = new Regex(@".*?(?=\/)");
                    matches = rgx.Matches(name_of_language);
                    if (matches.Count > 0) name_of_language = matches.First().Value;
                }

                for (var year = 2015; year <= DateTime.Today.Year; year++)
                {
                    int amount_of_books = 0;
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
                            amount_of_books = int.Parse(s1.Remove(0, 23).Trim().Trim(new Char[] { '.' }));
                        }
                    }
                    var existingRecord = ExistingStats.FirstOrDefault(item =>
                        item.LanguageName.Equals(name_of_language, StringComparison.CurrentCultureIgnoreCase) &&
                        item.Year == year_to_obj);
                    if (existingRecord != null)
                        existingRecord.PublicationsAmount = amount_of_books;
                }                
            }
            return ExistingStats;
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
