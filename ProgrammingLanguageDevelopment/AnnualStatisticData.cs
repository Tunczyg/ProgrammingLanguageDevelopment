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
    class AnnualStatisticData
    {
        public string LanguageName { get; }
        public int Year { get; }
        public double TiobeIndex { get; set; }
        public double PopularitySurvey { get; set; }
        public int PullRequestsAmount { get; set; }
        public int PublicationsAmount { get; set; }

        public AnnualStatisticData(string languageName, int year)
        {
            LanguageName = languageName;
            Year = year;
        }
    }
}