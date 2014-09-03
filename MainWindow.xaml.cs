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
            Tree = new Node("Root");

            foreach (var file in new DirectoryInfo(Settings.Instance.Directory).GetDirectories())
            {
                Tree.Items.Add(new Node(file.Name));
            }
        }
    }
}
