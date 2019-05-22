using System.Collections.Generic;

namespace ProgrammingLanguageDevelopment
{
    class DataProvider
    {
        public List<ProgrammingLanguage> GetLanguageFeaturesStaticData()
        {
            //from most to least popular according to TIOBE (23rd march 2019)
            return new List<ProgrammingLanguage>() {
                new ProgrammingLanguage("java", 1995, ProgrammingLanguage.Paradigm.Multi_paradigm, ProgrammingLanguage.Typing.Static, ProgrammingLanguage.Level.High),
                new ProgrammingLanguage("c", 1972, ProgrammingLanguage.Paradigm.Structured, ProgrammingLanguage.Typing.Static, ProgrammingLanguage.Level.High),
                new ProgrammingLanguage("python", 1990, ProgrammingLanguage.Paradigm.Multi_paradigm, ProgrammingLanguage.Typing.Dynamic, ProgrammingLanguage.Level.High),
                new ProgrammingLanguage("c++", 1983, ProgrammingLanguage.Paradigm.Multi_paradigm, ProgrammingLanguage.Typing.Static, ProgrammingLanguage.Level.High),
                new ProgrammingLanguage("visualBasicNET", 2001, ProgrammingLanguage.Paradigm.Multi_paradigm, ProgrammingLanguage.Typing.Static, ProgrammingLanguage.Level.High),
                new ProgrammingLanguage("c#", 2000, ProgrammingLanguage.Paradigm.Multi_paradigm, ProgrammingLanguage.Typing.Dynamic, ProgrammingLanguage.Level.High),
                new ProgrammingLanguage("javaScript", 1995, ProgrammingLanguage.Paradigm.Multi_paradigm, ProgrammingLanguage.Typing.Dynamic, ProgrammingLanguage.Level.High),
                new ProgrammingLanguage("php", 1995, ProgrammingLanguage.Paradigm.Multi_paradigm, ProgrammingLanguage.Typing.Dynamic, ProgrammingLanguage.Level.High),
                new ProgrammingLanguage("sql", 1995, ProgrammingLanguage.Paradigm.Multi_paradigm, ProgrammingLanguage.Typing.Static, ProgrammingLanguage.Level.High),
                new ProgrammingLanguage("objectiveC", 1983, ProgrammingLanguage.Paradigm.Multi_paradigm, ProgrammingLanguage.Typing.Dynamic, ProgrammingLanguage.Level.High),
                new ProgrammingLanguage("matlab", 1980, ProgrammingLanguage.Paradigm.Multi_paradigm, ProgrammingLanguage.Typing.Dynamic, ProgrammingLanguage.Level.High),
                new ProgrammingLanguage("assembly", 1949, ProgrammingLanguage.Paradigm.Imperative, ProgrammingLanguage.Typing.Static, ProgrammingLanguage.Level.Low),
                new ProgrammingLanguage("perl", 1987, ProgrammingLanguage.Paradigm.Multi_paradigm, ProgrammingLanguage.Typing.Dynamic, ProgrammingLanguage.Level.High),
                new ProgrammingLanguage("r", 1993, ProgrammingLanguage.Paradigm.Multi_paradigm, ProgrammingLanguage.Typing.Dynamic, ProgrammingLanguage.Level.High),
                new ProgrammingLanguage("ruby", 1995, ProgrammingLanguage.Paradigm.Multi_paradigm, ProgrammingLanguage.Typing.Dynamic, ProgrammingLanguage.Level.High),
                new ProgrammingLanguage("groovy", 2003, ProgrammingLanguage.Paradigm.Object_oriented, ProgrammingLanguage.Typing.Dynamic, ProgrammingLanguage.Level.High),
                new ProgrammingLanguage("swift", 2014, ProgrammingLanguage.Paradigm.Multi_paradigm, ProgrammingLanguage.Typing.Static, ProgrammingLanguage.Level.High),
                new ProgrammingLanguage("go", 2009, ProgrammingLanguage.Paradigm.Multi_paradigm, ProgrammingLanguage.Typing.Static, ProgrammingLanguage.Level.High),
                new ProgrammingLanguage("objectPascal", 1986, ProgrammingLanguage.Paradigm.Multi_paradigm, ProgrammingLanguage.Typing.Dynamic, ProgrammingLanguage.Level.Low),
                new ProgrammingLanguage("visualBasic", 1991, ProgrammingLanguage.Paradigm.Object_oriented, ProgrammingLanguage.Typing.Static, ProgrammingLanguage.Level.High),
            };
        }

        public List<ProgrammingLanguage> GetLanguageFeaturesDynamicData()
        {
            var parser = new WebParser();
            var wikiLang = parser.GetDataFromWikipedia();
            var langData = parser.GetDataFromComputerScienceWeb(wikiLang);
            //Patrycja TODO: check if wikiLang has already all the data extracted by GetDataFromComputerScienceWeb.
            //if so, return unchangd set, if not - add information extracted by GetDataFromComputerScienceWeb and return it

            return langData;
        }

        public List<AnnualStatisticData> GetAnnualStatistics(List<ProgrammingLanguage> requestedLanguages)
        {
            var parser = new WebParser();
            var annualStatsSO = parser.GetDataFromStackOverflowWeb(requestedLanguages);
            var annualStatsSOGH = parser.GetDataFromGitHubWeb(requestedLanguages, annualStatsSO); 
            var annualStatsSOGHBG = parser.GetDataFromBG(requestedLanguages, annualStatsSOGH);
            //basing on GetDataFromGitHubWeb: 
            //mandatory:
            //add information about language ONLY if alread exists in annualStatsSOGH
            //optional:
            //otherwise - if annualStatsSOGH does not contain it but the language is in requestedLanguages
            //add a new instance of AnnualStatisticData to the returned set with information from BG

            return annualStatsSOGHBG;
        }
        
    }
}
