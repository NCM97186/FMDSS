using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FMDSS.Globals
{
    public class SaveDataToTxtFile
    {
        private static string fileType;
        private static string fileName;
        private static string filePath;
        private static string InputString;
        public SaveDataToTxtFile(string fileTypes, string fileNames, string filePaths, string InputStrings)
        {
            fileType = fileTypes;
            fileName = fileNames;
            filePath = filePaths;
            InputString = InputStrings;
        }
        public bool WritetxtFile()
        {
            bool output = false;
            FileInfo fi = new FileInfo(filePath);
            try
            {
                // Check if file already exists. If yes, delete it.     
                if (fi.Exists)
                {
                    fi.Delete();
                }
                if (fileType == "txt")
                {
                    using (StreamWriter sw = fi.CreateText())
                    {
                        sw.WriteLine("New file created: {0}", DateTime.Now.ToString());
                        sw.WriteLine(InputString);
                        output = true;

                    }
                    // Write file contents on console.     
                    using (StreamReader sr = File.OpenText(filePath))
                    {
                        string s = "";
                        while ((s = sr.ReadLine()) != null)
                        {
                            //Console.WriteLine(s);
                        }
                    }
                }

            }
            catch
            {
                output = false;
            }
            return output;
        }


    }
}

