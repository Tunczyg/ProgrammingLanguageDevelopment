using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ProgrammingLanguageDevelopment
{
    class WebParser
    {
        //assuming Wojtek would make AnnualStatisticData class or of similiar name
        public List<AnnualStatisticData> GetDataFromStackOverflow()
        {
            for(var year = 2015; year <= DateTime.Today.Year; year++)
            {
                var annualStats = GetRawSourceCode("https://insights.stackoverflow.com/survey/"+ year + "/");
                //next step: search for valuable data in raw code and extract them into class
                //might be useful (or not):

                //serialise:
                //string json = JsonConvert.SerializeObject(T);

                //deserialise:
                //JsonConvert.DeserializeObject<T>(json);
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
