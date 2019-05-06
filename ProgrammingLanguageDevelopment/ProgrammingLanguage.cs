using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLanguageDevelopment
{
    class ProgrammingLanguage
    {
        public string Name;
        public int Year;            //first appeared
        public string _Paradigm;  //imperative, structured, object-oriented or multi-paradigm 
        public string _Typing;       //typing discipline: static or dynamic
        public string _Level;        //high-level or low-level

       /* public enum Paradigm
        {
            Imperative,
            Structured,
            Object_oriented,
            Multi_paradigm
        }

        public enum Typing
        {
            Static,
            Dynamic
        }

        public enum Level
        {
            Low,
            High
        }
        */

        public ProgrammingLanguage(string name, int year, string paradigm, string typing, string level)
        {
            Name = name;
            Year = year;
            _Paradigm = paradigm;
            _Typing = typing;
            _Level = level;
        }
    }
}
