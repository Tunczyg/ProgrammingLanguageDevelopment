using System;
using System.Collections.Generic;
using System.IO;

namespace ProgrammingLanguageDevelopment
{ /*
    class Matlab
    {
        //Before launching:
        //From the Project menu, select Add Reference. Select the COM tab in the Add Reference dialog box. 
        //Select the MATLAB application. Refer to your vendor documentation for details.
        MLApp.MLApp Instance;
        
        public Matlab()
        {
            Instance = new MLApp.MLApp();
            // Change to the directory to the project's directory 
            Instance.Execute(Directory.GetCurrentDirectory());
        }

        public object Launch(string funcName, object result, string funcPath = null, params int[] args)
        {
            // Change to the directory where the function is located 
            if(funcPath != null)
                Instance.Execute(funcPath);

            int numOfResults = 1;

            // Call the MATLAB function
            Instance.Feval(funcName, numOfResults, out result, args);

            return result;
        }

    }*/
}
