using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSEnergyLib.LibraryObjects
{
    // keeps track of document grid objects
    public sealed class CSLibraryInMemory
    {
        private static CSLibraryInMemory instance;
        public static CSLibraryInMemory Instance
        {
            get
            {
                if (instance == null) instance = new CSLibraryInMemory();
                return instance;
            }
        }

        public CSLibrary Library;
        private List<CSLibrary> DefaultLibraries;
        private List<CSLibrary> UserLibraries;


        private CSLibraryInMemory()
        {
            InitializeOrClear();
            LoadLibrariesFromDirectories();


        }


        public void Save()
        {


            ////------
            //// DEFAULT LOCATION
            ////------
            //if (!Directory.Exists(DefaultFilesAndDirectories.defaultLibPath))
            //{
            //    Directory.CreateDirectory(DefaultFilesAndDirectories.defaultLibPath);
            //}

            //var defaultLib = this.Library.getDefaultObjects();
            //HashSet<string> libNames = new HashSet<string>();
            //foreach (var s in defaultLib.getAllObjects().Select(x => x.LibraryName))
            //{
            //    libNames.Add(s);
            //}

            //foreach (var name in libNames.ToList())
            //{
            //    string path = Path.Combine(DefaultFilesAndDirectories.defaultLibPath, name + ".csl");
            //    var oneLibToSerialize = this.Library.getFromSpecificLibrary(name);
            //    using (FileStream fileStream = File.Create(path))
            //    {
            //        Serializer.Serialize(fileStream, oneLibToSerialize);
            //    }
            //}


            //------
            // USER LOCATION
            //------
            //if (!Directory.Exists(DefaultFilesAndDirectories.userLibPath))
            //{
            //    Directory.CreateDirectory(DefaultFilesAndDirectories.userLibPath);
            //}
            //try
            //{
            //    var userLib = UserLibraries[0]; //this.Library.getUserObjects();
            //    var filePath = Path.Combine(DefaultFilesAndDirectories.userLibPath, "userLib.csl");

            //    using (FileStream fileStream = File.Create(filePath))
            //    {
            //        Serializer.Serialize(fileStream, userLib);
            //    }
            //}
            //catch { }

            //HashSet<string> userLibNames = new HashSet<string>();
            //foreach (var s in userLib.getAllObjects().Select(x => x.LibraryName))
            //{
            //    if (!string.IsNullOrWhiteSpace(s))
            //    {
            //        userLibNames.Add(s);
            //    }
            //}

            //foreach (var name in userLibNames.ToList())
            //{
            //    string path = Path.Combine(DefaultFilesAndDirectories.defaultLibPath, name);
            //    var oneLibToSerialize = this.Library.getFromSpecificLibrary(name);
            //    CSV.CSVImportExport.ExportLibrary(oneLibToSerialize, path);
            //}
        }


        public void InitializeOrClear()
        {
            var culture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US"); // use US culture
            var fullDefault = LibraryDefaults.GetDefaultLibrary();
            DefaultLibraries = new List<CSLibrary>();
            UserLibraries = new List<CSLibrary>();
            DefaultLibraries.Add(fullDefault); //add hardcoded minimum default lib
            UserLibraries.Add(new CSLibrary()); //add blank user library
            Library = new CSLibrary();
            Library.Append(fullDefault);
            Thread.CurrentThread.CurrentCulture = culture; // set it back
        }
        public void LoadLibrariesFromDirectories() {

            Debug.WriteLine("LoadLibrariesFromDirectories");
            //------
            // DEFAULT LOCATION
            //------
            if (Directory.Exists(DefaultFilesAndDirectories.defaultLibPath))
            {


                //Binary Library Files
                var libFiles = Directory.GetFiles(DefaultFilesAndDirectories.defaultLibPath, "*.csl");
                foreach (var p in libFiles)
                {
                    Stopwatch sp = new Stopwatch();
                    sp.Start();
                    string name = Path.GetFileName(p).Replace(".csl", "");

                    CSLibrary importedLib = null;
                    using (FileStream fileStream = File.OpenRead(p))
                    {
                        importedLib = Serializer.Deserialize<CSLibrary>(fileStream);
                    }

                    importedLib.Name = name;
                    importedLib.IsLocked = true;
                    importedLib.IsDefault = true;
                    importedLib.applyLibName(name);
                    importedLib.applyLock(true);
                    importedLib.applyIsDefault(true);
                    this.DefaultLibraries.Add(importedLib);
                    this.Library.Append(importedLib);
                    sp.Stop();
                    Debug.WriteLine("Loading binary " + p + " - " + sp.ElapsedMilliseconds + "ms");
                }
            }

            //------
            // USER LOCATION
            //------
            if (Directory.Exists(DefaultFilesAndDirectories.userLibPath))
            {
                //Binary Library Files
                var libFiles = Directory.GetFiles(DefaultFilesAndDirectories.userLibPath, "*.csl");
                foreach (var p in libFiles)
                {
                    Stopwatch sp = new Stopwatch();
                    sp.Start();
                    string name = Path.GetFileName(p).Replace(".csl", "");

                    CSLibrary importedLib = null;
                    using (FileStream fileStream = File.OpenRead(p))
                    {
                        importedLib = Serializer.Deserialize<CSLibrary>(fileStream);
                    }

                    importedLib.Name = name;
                    importedLib.IsLocked = true;
                    importedLib.IsDefault = false;
                    importedLib.applyLibName(name);
                    importedLib.applyLock(true);
                    importedLib.applyIsDefault(false);
                    this.UserLibraries.Add(importedLib);
                    this.Library.Append(importedLib);
                    sp.Stop();
                    Debug.WriteLine("Loading binary " + p + " - " + sp.ElapsedMilliseconds + "ms");
                }
            }

        }

        //public void Update()
        //{
        //    Stopwatch sp = new Stopwatch();
        //    sp.Start();
        //    Library.Clear();
        //    //foreach (var l in this.DefaultLibraries)
        //    //{
        //    //    this.Library.Append(l);          
        //    //}
        //    //foreach (var l in this.UserLibraries)
        //    //{               
        //    //    this.Library.Append(l);
        //    //}
        //    sp.Stop();
        //    Debug.WriteLine("Update library: " + sp.ElapsedMilliseconds + "ms");
        //}

        public string ImportUserLibraryCSV(string Directory)
        {
            string name = Path.GetFileName(Directory);
            CSLibrary importedLib = null;
            try
            {
                importedLib = CSV.CSVImportExport.ImportLibrary(Directory);
            }
            catch {
            }
            if (importedLib != null)
            {
                try
                {
                    importedLib.Name = name;
                    importedLib.IsLocked = true;
                    importedLib.IsDefault = true;
                    importedLib.applyLibName(name);
                    importedLib.applyLock(false);
                    importedLib.applyIsDefault(false);
                    this.Library.Merge(importedLib);
                    this.UserLibraries.Add(importedLib);

                    return "Library merged from " + Directory;
                }
                catch { return "Library could not be imported"; }
            }
            else { return "Library could not be imported"; }
        }

        public string ExportLibraryCSL(string[] libnames, string directory, string name ) {

            try
            {
                var oneLibToSerialize = new CSLibrary();
            foreach (var n in libnames){ 
                oneLibToSerialize.Merge(Library.getFromSpecificLibrary(n));
            }

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
             var filePath = Path.Combine(directory, name + ".csl");

            using (FileStream fileStream = File.Create(filePath))
            {
                Serializer.Serialize(fileStream, oneLibToSerialize);
            }

            return "Library written to " + filePath;
        }
            catch(Exception e) {

                return "Export failed "  + e.Message;
            }
}


        public string ExportLibraryCSV(string[] libnames, string directory, string name)
        {

            try
            {
                var oneLibToSerialize = new CSLibrary();
                foreach (var n in libnames)
                {
                    oneLibToSerialize.Merge(Library.getFromSpecificLibrary(n));
                }

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                var filePath = Path.Combine(directory, name);

                CSV.CSVImportExport.ExportLibrary(oneLibToSerialize, filePath);

                return "Library written to " + filePath;
            }
            catch(Exception e) {

                return "Export failed "  + e.Message;
            }
        }

    }
}
