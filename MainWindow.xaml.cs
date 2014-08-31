using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace MarkdownLibrary
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            populateTree();
        }

        private void populateTree()
        {
            TreeMap = new Dictionary<string, Node>();
            foreach (var file in FileSystemHelper.FindFiles(Settings.Instance.Directory, "*.md"))
	        {
                addToTree(file);
	        }
        }

        private void addToTree(FileInfo file)
        {
            var baseParts = Settings.Instance.Directory.Split('\\');
            var currentPaths = file.FullName.Split('\\');

            var lineage = currentPaths.Skip(baseParts.Length);

            int i = 0;
            dynamic next = treeFolders;
            foreach (var part in lineage)
            {
                var currentLineage = lineage.Take(i + 1).ToArray();
                var path = string.Join("\\", currentLineage);
                var isLeaf = part == lineage.Last();

                next = add(part, next, path, isLeaf);
                ++i;
            }
        }

        private TreeViewItem add(string item, dynamic parent, string path, bool isLeaf)
        {
            if (TreeMap.ContainsKey(path))
                return TreeMap[path].Item;

            var treeItem = new TreeViewItem
            {
                Header = item,
            };
            
            parent.Items.Add(treeItem);

            var node = new Node
            {
                IsLeaf = isLeaf,
                Path = path,
                Item = treeItem,
            };

            TreeMap.Add(path, node);
            
            return treeItem;
        }

        private Dictionary<string, Node> TreeMap;

        private class Node
        {
            public bool IsLeaf { get; set; }
            
            public string Path { get; set; }

            public TreeViewItem Item { get; set; }

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
}
