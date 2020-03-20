using CSEnergyLib.Utilities;
using System;
using System.Diagnostics;
using System.IO;

namespace CSEnergyLib
{
 
   

    public static class DefaultFilesAndDirectories
    {

        // known library locations
       private static string commonAppdataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
       private static string appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
       public static string defaultLibPath = Path.Combine(commonAppdataPath, @"Solemma\Common\Library");
       public static string userLibPath = Path.Combine(appdataPath, @"Solemma\ClimateStudio\UserLibrary");

        public static string ghWorkflowTemplates = Path.Combine(commonAppdataPath, @"Solemma\Common\GHWorkflows");
 
  
        public static string LibraryDir
        {
            get
            {   
                    return @"C:\ProgramData\Solemma\Common\Library";
            }
        }
         



    }
}

 