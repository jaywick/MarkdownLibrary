using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MarkdownLibrary
{
    [XmlType("settings")]
    public class Settings
    {
        #region Singleton Access
        private static readonly Lazy<Settings> instance = new Lazy<Settings>(() => Load());
        public static Settings Instance { get { return instance.Value; } }

        private static Settings Load()
        {
            var appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var settingsFile = FileSystemHelper.BuildPath(appdataPath, "Jay Wick Labs", "MarkdownLibrary");
            return XmlRealizer.Realize<Settings>(settingsFile);
        }
        #endregion

        public Settings()
        {
            Directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        [XmlAttribute("directory")]
        public string Directory { get; set; }
    }
}
