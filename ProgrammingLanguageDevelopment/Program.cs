using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguageDevelopment
{
    class Program
    {
        static void Main(string[] args)
        {
            //from most to least popular according to TIOBE (23rd march 2019)
            ProgrammingLanguage java = new ProgrammingLanguage
            {
                Year = 1995,
                Paradigm = "multiparadigm",
                Typing = "static",
                Level = "high"
            };

            ProgrammingLanguage c = new ProgrammingLanguage
            {
                Year = 1972,
                Paradigm = "structured",
                Typing = "static",
                Level = "high"
            };

            ProgrammingLanguage python = new ProgrammingLanguage
            {
                Year = 1990,
                Paradigm = "multiparadigm",
                Typing = "dynamic",
                Level = "high"
            };

            ProgrammingLanguage cpp = new ProgrammingLanguage
            {
                Year = 1983,
                Paradigm = "multiparadigm",
                Typing = "static",
                Level = "high"
            };

            ProgrammingLanguage visualBasicNET = new ProgrammingLanguage
            {
                Year = 2001,
                Paradigm = "multiparadigm",
                Typing = "static",
                Level = "high"
            };

            ProgrammingLanguage cSharp = new ProgrammingLanguage
            {
                Year = 2000,
                Paradigm = "multiparadigm",
                Typing = "dynamic",
                Level = "high"
            };

            ProgrammingLanguage javaScript = new ProgrammingLanguage
            {
                Year = 1995,
                Paradigm = "multiparadigm",
                Typing = "dynamic",
                Level = "high"
            };

            ProgrammingLanguage php = new ProgrammingLanguage
            {
                Year = 1995,
                Paradigm = "multiparadigm",
                Typing = "dynamic",
                Level = "high"
            };

            ProgrammingLanguage sql = new ProgrammingLanguage
            {
                Year = 1974,
                Paradigm = "multiparadigm",
                Typing = "static",
                Level = "high"
            };

            ProgrammingLanguage objectiveC = new ProgrammingLanguage
            {
                Year = 1983,
                Paradigm = "multiparadigm",
                Typing = "dynamic",
                Level = "high"
            };

            ProgrammingLanguage matlab = new ProgrammingLanguage
            {
                Year = 1980,
                Paradigm = "multiparadigm",
                Typing = "dynamic",
                Level = "high"
            };


            ProgrammingLanguage assembly = new ProgrammingLanguage
            {
                Year = 1949,
                Paradigm = "imperative",
                Typing = "static",
                Level = "low"
            };


            ProgrammingLanguage perl = new ProgrammingLanguage
            {
                Year = 1987,
                Paradigm = "multiparadigm",
                Typing = "dynamic",
                Level = "high"
            };

            ProgrammingLanguage r = new ProgrammingLanguage
            {
                Year = 1993,
                Paradigm = "multiparadigm",
                Typing = "dynamic",
                Level = "high"
            };

            ProgrammingLanguage ruby = new ProgrammingLanguage
            {
                Year = 1995,
                Paradigm = "multiparadigm",
                Typing = "dynamic",
                Level = "high"
            };

            ProgrammingLanguage groovy = new ProgrammingLanguage
            {
                Year = 2003,
                Paradigm = "object",
                Typing = "dynamic",
                Level = "high"
            };

            ProgrammingLanguage swift = new ProgrammingLanguage
            {
                Year = 2014,
                Paradigm = "multiparadigm",
                Typing = "static",
                Level = "high"
            };

            ProgrammingLanguage go = new ProgrammingLanguage
            {
                Year = 2009,
                Paradigm = "multiparadigm",
                Typing = "static",
                Level = "high"
            };

            ProgrammingLanguage objectPascal = new ProgrammingLanguage
            {
                Year = 1986,
                Paradigm = "multiparadigm",
                Typing = "dynamic",
                Level = "low"
            };

            ProgrammingLanguage visualBasic = new ProgrammingLanguage
            {
                Year = 1991,
                Paradigm = "object",
                Typing = "static",
                Level = "high"
            };
        }
    }
}
