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
using MarkdownSharp;

namespace MarkdownLibrary
{
    public partial class MainWindow : Window
    {
        public Node Tree { get; set; }
        private Markdown renderer;

        public MainWindow()
        {
            createTree();
            this.DataContext = this;

            renderer = new MarkdownSharp.Markdown(new MarkdownOptions
            {
                AutoHyperlink = true,
                AutoNewLines = true,
            });
            
            InitializeComponent();
        }

        private void createTree()
        {
            Tree = PopulateTree(new DirectoryInfo(Settings.Instance.Directory));
        }

        public static Node PopulateTree(DirectoryInfo root)
        {
            var stack = new Stack<Tuple<DirectoryInfo, Node>>();
            
            var rootNode = new Node(root);
            stack.Push(Tuple.Create(root, rootNode));

            while (stack.Any())
            {
                var current = stack.Pop();
                var currentDirectory = current.Item1; 
                var currentNode = current.Item2;

                foreach (var child in FileSystemHelper.GetSubdirectories(currentDirectory))
                {
                    var childNode = new Node(child);
                    currentNode.Items.Add(childNode);

                    if (child is DirectoryInfo)
                        stack.Push(Tuple.Create(child, childNode));
                }
                
                foreach (var file in FileSystemHelper.GetFiles(currentDirectory, "*.md"))
                {
                    currentNode.Items.Add(new Node(file));
                }
            }

            return rootNode;
        }

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var node = e.NewValue as Node;
            
            loadNote(node.IsLeaf ? node.FileSystemItem.FullName : null);
        }

        private void loadNote(string file)
        {
            if (file == null)
            {
                textSource.Text = "";
                return;
            }

            textSource.Text = File.ReadAllText(file);
        }

        private void TextSource_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            webBrowser.NavigateToString(render(e.Source.ToString()));
        }

        private string render(string source)
        {
            var body = renderer.Transform(textSource.Text);
            var style = Properties.Resources.ResourceManager.GetObject("css_style");
            return string.Format("<html><head><style>{0}</style></head><body>{1}</body></html>", style, body);
        }
    }
}
