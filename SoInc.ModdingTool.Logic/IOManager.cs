﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SoInc.ModdingTool.Logic
{
    public class IOManager
    {
        /// <summary>
        /// checks the Path and the directory if its exists and create it if its not
        /// </summary>
        /// <param name="path"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public bool CheckPath(string path, bool create = true)
        {
            if (Directory.Exists(Path.GetDirectoryName(path)))
            {
                return true;
            }
            else if (create)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                return true;
            }

            if (File.Exists(path))
            {
                return true;
            }
            else if (create)
            {
                File.Create(path);
                return true;
            }
            return false;
        }

        /// <summary>
        /// returns true if the Path exists
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool CheckDirectoryOrFile(string path)
        {
            if (Regex.IsMatch(path, @"\.(\w|\d)+$"))
            {
                return File.Exists(path);
            }
            else
            {
                return Directory.Exists(path);
            }
        }
        
        /// <summary>
        /// replaces all Windows-Explorer forbidden characters
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CreatePathFriendlyName(string input)
        {
            var regex = new Regex("[~#%&*{}/\\<>:;?+|\"]");
            return regex.Replace(input, "-");
        }

        /// <summary>
        /// Saves the File to THe Path Asyc
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static void WriteFileAsync<T>(string path, T data) where T:class, new()
        {
             Task.Run(() => WriteFile<T>(path, data));
        }

        /// <summary>
        /// Saves the File to the Folder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="data"></param>
        protected static void WriteFile<T>(string path, T data) where T:class, new()
        {
            try
            {
                var xmlManager = new XmlManager<T>();
                xmlManager.WriteFile(path, data);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteError(ex);
            }
        }
    }
}
