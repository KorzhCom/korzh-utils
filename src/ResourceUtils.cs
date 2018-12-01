using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;



namespace Korzh.Utils
{
    public static class ResourceUtils
    {
        public static Stream GetResourceStream(this Type type, string resourceFolder, string resourceFileName) {
            var assembly = typeof(ResourceUtils).GetTypeInfo().Assembly;
            return assembly.GetResourceStream(resourceFolder, resourceFileName);
        }

        public static Stream GetResourceStream(this Assembly assembly, string resourceFolder, string resourceFileName) {
             
            string[] nameParts = assembly.FullName.Split(',');
                
            string resourceName = nameParts[0] + "." +  resourceFolder + "." + resourceFileName;

            var resources = new List<string>(assembly.GetManifestResourceNames());
            if (resources.Contains(resourceName))
                return assembly.GetManifestResourceStream(resourceName);
            else
                return null;
        }
        public static void CreateFileByResource(this Type type, string folder, string fileName) {
            var assembly = typeof(ResourceUtils).GetTypeInfo().Assembly;
            assembly.CreateFileByResource(folder, fileName);
        }

        public static void CreateFileByResource(this Assembly assembly, string folder, string fileName) {
            var resStream = GetResourceStream(assembly, folder, fileName);
            if (resStream == null) {
                throw new ResourceUtilsException($"Can't load resource {folder}/{fileName}");
            }

            resStream.Position = 0;
            using (StreamReader sr = new StreamReader(resStream))
            using (StreamWriter sw = File.CreateText(fileName)) {
                sw.Write(sr.ReadToEnd());
                sw.Flush();
            }
        }

        public static string GetResourceAsString(this Type type, string folder, string fileName) {
            var assembly = typeof(ResourceUtils).GetTypeInfo().Assembly;
            return assembly.GetResourceAsString(folder, fileName);
        }

        public static string GetResourceAsString(this Assembly assembly, string folder, string fileName) {
            string fileContent;
            using (StreamReader sr = new StreamReader(GetResourceStream(assembly, folder, fileName))) {
                fileContent = sr.ReadToEnd();
            }
            return fileContent;
        }
    }

    public class ResourceUtilsException : Exception {
        public ResourceUtilsException(string message) : base(message) { }
    }
}
