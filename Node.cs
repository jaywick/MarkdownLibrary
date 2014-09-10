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
        public bool IsLeaf
        {
            get { return FileSystemItem is FileInfo; }
        }

        public string Name
        {
            get { return FileSystemItem.Name; }
        }

        public ObservableCollection<Node> Items
        {
            get; set;
        }

        public FileSystemInfo FileSystemItem
        {
            get; private set;
        }

        public Node()
        {
            Items = new ObservableCollection<Node>();
        }

        public Node(FileSystemInfo fileSystemItem)
            : this()
        {
            FileSystemItem = fileSystemItem;
        }
    }
}
