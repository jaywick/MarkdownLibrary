using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MarkdownLibrary
{
    public class Node
    {
        public bool IsLeaf { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public Node()
        {
            Items = new ObservableCollection<Node>();
        }

        public Node(string name)
            : this()
        {
            Name = name;
        }

        public ObservableCollection<Node> Items { get; set; }

        public FileSystemInfo FileSystemItem
        {
            get
            {
                var path = System.IO.Path.Combine(Settings.Instance.Directory, Path);

                if (IsLeaf)
                {
                    return new FileInfo(path);
                }
                else
                {
                    return new DirectoryInfo(path);
                }
            }
        }

        public bool IsTopLevel
        {
            get { return Level == 0; }
        }

        public int Level
        {
            get { return Path.Split('\\').Length; }
        }
    }
}
