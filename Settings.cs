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
        private static Settings _instance;
        public static Settings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Settings.Load();
                }
                return _instance;
            }
            private set { _instance = value; }
        }

        private static Settings Load()
        {
            var appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var settingsFile = FileSystemHelper.BuildPath(appdataPath, "Jay Wick Labs", "MarkdownLibrary", "settings.xml");
            
            var instance = XmlRealizer.Realize<Settings>(settingsFile);
            instance.loadDefaultsIfRequired();

            return instance;
        }

        private void loadDefaultsIfRequired()
        {
            if (string.IsNullOrEmpty(Directory)) 
                Directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        #endregion

        [XmlElement("directory")]
        public string Directory { get; set; }
    }
}
