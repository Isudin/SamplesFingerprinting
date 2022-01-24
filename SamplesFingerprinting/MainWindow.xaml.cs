using Microsoft.WindowsAPICodePack.Dialogs;
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
using System.Windows.Shapes;

namespace SamplesFingerprinting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<SampleModel> _samples = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void PathButton_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();

            if (System.IO.Path.IsPathRooted(PathTextBox.Text))
                dialog.InitialDirectory = "PathTextBox.Text";
            else
                dialog.InitialDirectory = "C:";

            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                PathTextBox.Text = dialog.FileName.ToString();
        }
        private void PathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _samples = new();
            string path = PathTextBox.Text;

            if (!System.IO.Path.IsPathFullyQualified(path) || !System.IO.Path.IsPathRooted(path))
                return;

            DirectoryInfo d = new(path);
            FileInfo[] files = d.GetFiles("*.wav");

            foreach (FileInfo file in files)
            {
                _samples.Add(new SampleModel()
                {
                    Name = file.Name,
                    Path = file.FullName
                });
            }

            SamplesListBox.ItemsSource = _samples;
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            _samples = new();
            string path = System.IO.Path.GetFullPath(PathTextBox.Text);

            if (!System.IO.Path.IsPathRooted(path))
                return;

            DirectoryInfo d = new(path);
            FileInfo[] files = d.GetFiles("*.wav");

            foreach (FileInfo file in files)
            {
                _samples.Add(new SampleModel()
                {
                    Name = file.Name,
                    Path = file.FullName
                });
            }

            SamplesListBox.ItemsSource = _samples;
        }

        private void SampleItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var sample = (SampleModel)SamplesListBox.SelectedItem;
            if (sample != null)
            {
                DataObject data = new(DataFormats.FileDrop, sample.Path);
                DragDrop.DoDragDrop(this.SamplesListBox, data, DragDropEffects.Copy);
            }
        }

        private void SamplesListBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var sample = (SampleModel)SamplesListBox.SelectedItem;
            if (sample != null)
            {
                string[] files = new string[1];
                files[0] = sample.Path;
                DataObject data = new(DataFormats.FileDrop, files);
                DragDrop.DoDragDrop(this.SamplesListBox, data, DragDropEffects.Copy);
            }
        }
    }
}
