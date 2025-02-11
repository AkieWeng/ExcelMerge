using ExcelMerge.GUI.ViewModels;
using System;
using System.Collections.Generic;
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
using static ExcelMerge.GUI.ViewModels.FolderViewModel;
using System.IO;

namespace ExcelMerge.GUI.Views
{
    /// <summary>
    /// FolderView.xaml 的交互逻辑
    /// </summary>
    public partial class FolderView : UserControl
    {
        public FolderView()
        {
            InitializeComponent();
        }

        private FolderViewModel GetViewModel()
        {
            return DataContext as FolderViewModel;
        }

        private void ListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox != null && listBox.SelectedItem != null)
            {
                var selectedItem = listBox.SelectedItem as FolderViewModel.AlignedFile;
                if (selectedItem != null && selectedItem.IsMatched)
                {
                    string srcFilePath = System.IO.Path.Combine(GetViewModel().SrcFolderPath, selectedItem.SrcFile);
                    string dstFilePath = System.IO.Path.Combine(GetViewModel().DstFolderPath, selectedItem.DstFile);
                    var parentWindow = Window.GetWindow(this) as MainWindow;
                    if (parentWindow != null)
                    {
                        var mainWindowViewModel = parentWindow.DataContext as MainWindowViewModel;
                        if (mainWindowViewModel != null)
                        {
                            mainWindowViewModel.SrcPath = GetViewModel().SrcFolderPath;
                            mainWindowViewModel.DstPath = GetViewModel().DstFolderPath;
                            var diffView = new DiffView();
                            var diffViewModel = new DiffViewModel(srcFilePath, dstFilePath);
                            diffView.DataContext = diffViewModel;
                            mainWindowViewModel.Content = diffView;
                        }
                    }
                }
            }
        }

        private void SrcFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string folderPath = dialog.SelectedPath;
                // 更新源文件夹路径
                GetViewModel().SrcFolderPath = folderPath;
            }
        }

        private void DstFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string folderPath = dialog.SelectedPath;
                // 更新目标文件夹路径
                GetViewModel().DstFolderPath = folderPath;
            }
        }

        private void SrcScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalChange != 0)
            {
                DstScrollViewer.ScrollToVerticalOffset(e.VerticalOffset);
            }
        }

        private void DstScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalChange != 0)
            {
                SrcScrollViewer.ScrollToVerticalOffset(e.VerticalOffset);
            }
        }


    }
}
