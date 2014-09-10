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
using System.Collections.ObjectModel;

namespace MarkdownLibrary
{
    public partial class MainWindow : Window
    {
        public Node Tree { get; set; }

        public MainWindow()
        {
            createTree();
            this.DataContext = this;

            InitializeComponent();
        }

        private void createTree()
        {
            Tree = PopulateTree(new DirectoryInfo(Settings.Instance.Directory));
        }

        public static Node PopulateTree(DirectoryInfo root)
        {
            var stack = new Stack<Tuple<DirectoryInfo, Node>>();
            
            var rootNode = new Node(root.Name);
            stack.Push(Tuple.Create(root, rootNode));

            while (stack.Any())
            {
                var current = stack.Pop();
                var currentDirectory = current.Item1; 
                var currentNode = current.Item2;

                foreach (var child in FileSystemHelper.GetSubdirectories(currentDirectory))
                {
                    var childNode = new Node(child.Name);
                    currentNode.Items.Add(childNode);

                    if (child is DirectoryInfo)
                        stack.Push(Tuple.Create(child, childNode));
                }
                
                foreach (var file in FileSystemHelper.GetFiles(currentDirectory, "*.md"))
                {
                    currentNode.Items.Add(new Node(file.Name));
                }
            }

            return rootNode;
        }
    }
}
