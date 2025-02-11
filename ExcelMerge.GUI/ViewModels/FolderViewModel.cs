﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Windows;
using Prism.Mvvm;
using FastWpfGrid;
using ExcelMerge.GUI.Settings;
using ExcelMerge.GUI.Behaviors;
using System.ComponentModel;

namespace ExcelMerge.GUI.ViewModels
{
    public class FolderViewModel : BindableBase
    {
        private ObservableCollection<string> srcExcelFiles;
        public ObservableCollection<string> SrcExcelFiles
        {
            get => srcExcelFiles;
            set => SetProperty(ref srcExcelFiles, value);
        }

        private ObservableCollection<string> dstExcelFiles;
        public ObservableCollection<string> DstExcelFiles
        {
            get => dstExcelFiles;
            set => SetProperty(ref dstExcelFiles, value);
        }
        public class AlignedFile
        {
            public string SrcFile { get; set; }
            public string DstFile { get; set; }
            public bool IsMatched { get; set; }
        }

        private ObservableCollection<AlignedFile> alignedFiles;
        public ObservableCollection<AlignedFile> AlignedFiles
        {
            get => alignedFiles;
            set => SetProperty(ref alignedFiles, value);
        }

        private string srcFolderPath;
        public string SrcFolderPath
        {
            get => srcFolderPath;
            set
            {
                if (srcFolderPath != value)
                {
                    srcFolderPath = value;
                    // 加载源文件夹中的 Excel 文件
                    LoadSrcExcelFiles(srcFolderPath);
                    AlignFiles();
                }
            }
        }

        private string dstFolderPath;
        public string DstFolderPath
        {
            get => dstFolderPath; set
            {
                if (dstFolderPath != value)
                {
                    dstFolderPath = value;
                    // 加载目标文件夹中的 Excel 文件
                    LoadDstExcelFiles(dstFolderPath);
                    AlignFiles();
                }
            }
        }

        public void LoadSrcExcelFiles(string folderPath)
        {
            SrcExcelFiles.Clear();
            var files = Directory.GetFiles(folderPath, "*.xls")
                .Concat(Directory.GetFiles(folderPath, "*.xlsx"))
                .Concat(Directory.GetFiles(folderPath, "*.csv"))
                .Concat(Directory.GetFiles(folderPath, "*.tsv"));
            foreach (var file in files)
            {
                SrcExcelFiles.Add(file);
            }
        }

        public void LoadDstExcelFiles(string folderPath)
        {
            DstExcelFiles.Clear();
            var files = Directory.GetFiles(folderPath, "*.xls")
                .Concat(Directory.GetFiles(folderPath, "*.xlsx"))
                .Concat(Directory.GetFiles(folderPath, "*.csv"))
                .Concat(Directory.GetFiles(folderPath, "*.tsv"));
            foreach (var file in files)
            {
                DstExcelFiles.Add(file);
            }
        }
        public void AlignFiles()
        {
            var srcFiles = SrcExcelFiles.Select(f => System.IO.Path.GetFileName(f)).ToList();
            var dstFiles = DstExcelFiles.Select(f => System.IO.Path.GetFileName(f)).ToList();

            var allFiles = srcFiles.Union(dstFiles).OrderBy(f => f).ToList();

            AlignedFiles.Clear();
            foreach (var file in allFiles)
            {
                var srcFile = srcFiles.Contains(file) ? file : null;
                var dstFile = dstFiles.Contains(file) ? file : null;
                AlignedFiles.Add(new AlignedFile { SrcFile = srcFile, DstFile = dstFile, IsMatched = srcFile == dstFile });
            }
        }

        public FolderViewModel()
        {
            SrcExcelFiles = new ObservableCollection<string>();
            DstExcelFiles = new ObservableCollection<string>();
            AlignedFiles = new ObservableCollection<AlignedFile>();
        }

        public FolderViewModel(string srcFolderPath, string dstFolderPath) : this()
        {
            SrcFolderPath = srcFolderPath;
            DstFolderPath = dstFolderPath;
        }
    }
}
