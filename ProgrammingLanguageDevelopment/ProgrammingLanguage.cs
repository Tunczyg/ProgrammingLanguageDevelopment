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
        public List<Paradigm> _Paradigm;  //imperative, structured, object-oriented or multi-paradigm 
        public List<Typing> _Typing;       //typing discipline: static or dynamic
        public List<Level> _Level;        //high-level or low-level
        public List<OperatingSystem> _OperatingSystem; //cross-platform, Linux, Windows, Mac, other

        public enum OperatingSystem
        {
            Other,
            Cross_Platform,
            Linux,
            Unix,
            Windows,
            Mac
        }

        public enum Paradigm
        {
            Other,
            Compiled,
            Distributed,
            Modular,
            Unstructured,
            Action,
            Agent,
            Array,
            Automata,
            Concurrent,
            Declarative,
            Functional,
            Logic_Programming,
            Logic,
            Inductive,
            Constraint,
            Dataflow,
            Reactive,
            Ontology,
            Differentiable,
            Dynamic,
            Concatenative,
            Generic,
            Imperative,
            Procedural,
            Object_Oriented,
            Polymorphic,
            Intentional,
            Literate,
            Metaprogramming,
            Automatic,
            Reflective,
            Macro,
            Template,
            Nondeterministic,
            Parallel,
            Probabilistic,
            Structured,
            Recursive,
            Symbolic,
            Multi_Paradigm,
            Agent_Oriented,
            Array_Oriented,
            Automata_Based,
            Concurrent_Computing,
            Relativistic_Programming,
            Data_Driven,
            Functional_Logic,
            Purely_Functional,
            Abductive_Logic,
            Answer_Set,
            Concurrent_Logic,
            Constraint_Logic,
            Concurrent_Constraint_Logic,
            Flow_Based,
            Scripting,
            Event_Driven,
            Function_Level,
            Point_Free_Style,
            Language_Oriented,
            Domain_Specific,
            Natural_Language_Programming,
            Attribute_Oriented,
            Non_Structured,
            Parallel_Computing,
            Process_Oriented,
            Stack_Based,
            Block_Structured,
            Actor_Based,
            Class_Based,
            Prototype_Based,
            Aspect_Oriented,
            Role_Oriented,
            Subject_Oriented,
            Value_Level,
            Quantum_Programming
        }

        public enum Typing
        {
            Other,
            Weak,
            Dependent_Typed,
            Strong,
            Safe,
            Nominative,
            Polymorphic,
            Manifest,
            Inferred,
            Nominal,
            Structural,
            Duck,
            Dependent,
            Gradual,
            Latent,
            Refinement,
            Substructural,
            Unique,
            Static,
            Dynamic
        }

        public enum Level
        {
            Other,
            Low,
            High
        }

        public ProgrammingLanguage(string name, int year, List<Paradigm> paradigm, List<Typing> typing, List<Level> level, List<OperatingSystem> operating_system = null)
        {
            Name = name;
            Year = year;
            _Paradigm = paradigm;
            _Typing = typing;
            _Level = level;
            _OperatingSystem = operating_system;
        }

        public ProgrammingLanguage(string name, int year, Paradigm paradigm, Typing typing, Level level, OperatingSystem operating_system = OperatingSystem.Other)
        {
            Name = name;
            Year = year;
            _Paradigm = new List<Paradigm>() { paradigm };
            _Typing = new List<Typing>() { typing };
            _Level = new List<Level>() { level };
            _OperatingSystem = new List<OperatingSystem>() { operating_system };
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
