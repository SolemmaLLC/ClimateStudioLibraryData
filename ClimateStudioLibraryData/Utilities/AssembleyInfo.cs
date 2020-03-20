using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace CSEnergyLib.Utilities
{



       public static class ArchsimVersion{
        public  const string ProductVersion = "5.0.0.6";
        public const string Name = "Archsim CS";

        public static string toString() {
            return @"
"+ Name+ " "+ProductVersion ;
        }
    }

    public static class AssembleyInfo
    { 
        static public string AssemblyVersion {
            get {
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                return fvi.FileVersion;
            }
        }

        static public string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }



        public static void createDir(string path)
        {
            try
            {
                // If the directory doesn't exist, create it.
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("createDir: " + ex.Message);
            }
        }



        /// <summary> determines whether a string is formatted as a full file or directory path
        /// </summary>
        /// <param name="test_string">string to test</param>
        /// <returns>true or false</returns>
        public static bool StringIsPath(string test_string)
        {
            try
            {
                if (string.Equals(test_string, Path.GetFullPath(test_string), StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            catch { }

            return false;
        }

        public static bool IsJson(string input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}")
                   || input.StartsWith("[") && input.EndsWith("]");
        }

    }
}
