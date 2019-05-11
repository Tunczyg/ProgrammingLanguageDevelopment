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
        public Paradigm _Paradigm;  //imperative, structured, object-oriented or multi-paradigm 
        public Typing _Typing;       //typing discipline: static or dynamic
        public Level _Level;        //high-level or low-level
        public OperatingSystem _OperatingSystem; //cross-platform, Linux, Windows, Mac, other

        public enum OperatingSystem
        {
            Cross_platform,
            Linux,
            Windows,
            Mac,
            Other
        }

        public enum Paradigm
        {
            Imperative,
            Structured,
            Object_oriented,
            Multi_paradigm,
            Other
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

        public ProgrammingLanguage(string name, int year, Paradigm paradigm, Typing typing, Level level, OperatingSystem operating_system = new OperatingSystem())
        {
            Name = name;
            Year = year;
            _Paradigm = paradigm;
            _Typing = typing;
            _Level = level;
            _OperatingSystem = operating_system;
        }

        public ProgrammingLanguage(string name, int year, string paradigm, string typing, string level, string operating_system = "")
        {
            Name = name;
            Year = year;
            Enum.TryParse(paradigm, out Paradigm _Paradigm);
            Enum.TryParse(typing, out Typing _Typing);
            Enum.TryParse(level, out Level _Level);
            Enum.TryParse(operating_system, out OperatingSystem _Operating_system);
        }
    }
}
