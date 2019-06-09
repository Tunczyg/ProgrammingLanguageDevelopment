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
        public int Quarter { get; }
        public double PopularitySurvey { get; set; }
        public int PullRequestsAmount { get; set; }
        public int PushAmount { get; set; }
        public int StarsAmount { get; set; }
        public int IssuesAmount { get; set; }
        public int PublicationsAmount { get; set; }

        public AnnualStatisticData(string languageName, int year, int quarter)
        {
            LanguageName = languageName;
            Year = year;
            Quarter = quarter;
        }
    }
}