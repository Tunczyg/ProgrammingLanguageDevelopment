using Newtonsoft.Json;
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
        //assuming Wojtek would make AnnualStatisticData class or of similiar name
        public List<AnnualStatisticData> GetDataFromStackOverflow()
        {
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

                    //following code requires minor changes after creation of AnnualStatisticData
                    var futureAnnualStatisticDataList = languages.Select(item => item.InnerText.Split(null).Where(part => part != "")).ToList();
                    {
                        //another (more convenient) option:
                        //following code won't work until AnnualStatisticData creation and 
                        //addition of [JsonProperty("$property_name")] before its fields
                        //I want to convert: [JsonProperty("$bar-label")] (language name) and [JsonProperty("$lang-language_name")] 

                        //var languagesHtml = languages.Select(language => language.InnerHtml.ToString()).ToList();
                        //foreach (var language in languagesHtml)
                        //JsonConvert.DeserializeObject<AnnualStatisticData>(language);
                    }
                }
            }
            return new List<AnnualStatisticData>();
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
