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

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (e.AddedItems.Count > 0)
            //{
            //    var selectedItem = e.AddedItems[0] as AlignedFile; 
            //    if (selectedItem != null)
            //    {
            //        var diffView = new DiffView();
            //        var diffViewModel = diffView.DataContext as DiffViewModel; 

            //        if (diffViewModel != null)
            //        {
            //            diffViewModel.SrcPath = selectedItem.SrcFile;
            //            diffViewModel.DstPath = selectedItem.DstFile;
            //        }

            //        // 导航到 DiffView 页面
            //        var parentWindow = Window.GetWindow(this) as MainWindow; // 假设 MainWindow 是你的主窗口
            //        if (parentWindow != null)
            //        {
            //            parentWindow.MainFrame.Navigate(diffView); // 假设 MainFrame 是主窗口中的 Frame 控件
            //        }
            //    }
            //}
        }

        //private void ExcelFileListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        //{
        //    if (ExcelFileListBox.SelectedItem != null)
        //    {
        //        string selectedFile = ExcelFileListBox.SelectedItem.ToString();
        //        // 按照原来的逻辑进行表格的差异比较
        //        CompareExcelFiles(selectedFile);
        //    }
        //}

        //private void CompareExcelFiles(string filePath)
        //{
        //    // 实现表格差异比较的逻辑
        //}

        private void SrcFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string folderPath = dialog.SelectedPath;
                // 更新源文件夹路径
                GetViewModel().SrcFolderPath = folderPath;
                // 加载源文件夹中的 Excel 文件
                GetViewModel().LoadSrcExcelFiles(folderPath);
                //var viewSource = (CollectionViewSource)FindResource("DstFilesViewSource");
                //viewSource.View.Refresh();
                GetViewModel().AlignFiles();
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
                // 加载目标文件夹中的 Excel 文件
                GetViewModel().LoadDstExcelFiles(folderPath);
                //var viewSource = (CollectionViewSource)FindResource("SrcFilesViewSource");
                //viewSource.View.Refresh();
                GetViewModel().AlignFiles();
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
