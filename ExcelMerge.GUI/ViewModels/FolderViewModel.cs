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
using System.Security.Cryptography;

namespace ExcelMerge.GUI.ViewModels
{
    public class FolderViewModel : BindableBase
    {
        private ObservableCollection<string> srcFiles;
        public ObservableCollection<string> SrcFiles
        {
            get => srcFiles;
            set => SetProperty(ref srcFiles, value);
        }

        private ObservableCollection<string> dstFiles;
        public ObservableCollection<string> DstFiles
        {
            get => dstFiles;
            set => SetProperty(ref dstFiles, value);
        }
        public class AlignedFile
        {
            public string SrcFile { get; set; }
            public string DstFile { get; set; }
            public bool IsMatched { get; set; }
            public bool IsSame { get; set; }
            public bool SrcIsFolder { get; set; }
            public bool DstIsFolder { get; set; }
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
                srcFolderPath = value;
                // 加载源文件夹中的 Excel 文件
                LoadSrcFiles(srcFolderPath);
                AlignFiles();
                OnPropertyChanged(nameof(SrcFolderPath));
            }
        }

        private string dstFolderPath;
        public string DstFolderPath
        {
            get => dstFolderPath;
            set
            {
                dstFolderPath = value;
                // 加载目标文件夹中的 Excel 文件
                LoadDstFiles(dstFolderPath);
                AlignFiles();
                OnPropertyChanged(nameof(DstFolderPath));
            }
        }

        public void LoadSrcFiles(string folderPath)
        {
            SrcFiles.Clear();
            var files = Directory.GetFiles(folderPath, "*.xls")
                .Concat(Directory.GetFiles(folderPath, "*.xlsx"))
                .Concat(Directory.GetFiles(folderPath, "*.xlsm"))
                .Concat(Directory.GetFiles(folderPath, "*.csv"))
                .Concat(Directory.GetFiles(folderPath, "*.tsv"));
            var directories = Directory.GetDirectories(folderPath);

            if (!files.Any() && !directories.Any())
            {
                MessageBox.Show("源文件夹中没有找到相关文件或子文件夹。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            foreach (var file in files)
            {
                SrcFiles.Add(file);
            }

            foreach (var directory in directories)
            {
                SrcFiles.Add(directory);
            }
        }

        public void LoadDstFiles(string folderPath)
        {
            DstFiles.Clear();
            var files = Directory.GetFiles(folderPath, "*.xls")
                .Concat(Directory.GetFiles(folderPath, "*.xlsx"))
                .Concat(Directory.GetFiles(folderPath, "*.xlsm"))
                .Concat(Directory.GetFiles(folderPath, "*.csv"))
                .Concat(Directory.GetFiles(folderPath, "*.tsv"));
            var directories = Directory.GetDirectories(folderPath);

            if (!files.Any() && !directories.Any())
            {
                MessageBox.Show("目标文件夹中没有找到相关文件或子文件夹。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            foreach (var file in files)
            {
                DstFiles.Add(file);
            }

            foreach (var directory in directories)
            {
                DstFiles.Add(directory);
            }
        }

        public string ComputeHash(string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            {
                using (SHA256 hash = SHA256.Create())
                {
                    byte[] bytes = hash.ComputeHash(stream);
                    return BitConverter.ToString(bytes).Replace("-", "").ToLower();
                }
            }

        }
        public void AlignFiles()
        {
            var srcFileNames = SrcFiles.Select(f => System.IO.Path.GetFileName(f)).ToList();
            var dstFileNames = DstFiles.Select(f => System.IO.Path.GetFileName(f)).ToList();

            var allFiles = srcFileNames.Union(dstFileNames).OrderBy(f => f).ToList();

            AlignedFiles.Clear();
            foreach (var file in allFiles)
            {
                var srcFile = srcFileNames.Contains(file) ? file : null;
                var dstFile = dstFileNames.Contains(file) ? file : null;
                var srcPath = srcFile != null ? System.IO.Path.Combine(SrcFolderPath, srcFile) : null;
                var dstPath = dstFile != null ? System.IO.Path.Combine(DstFolderPath, dstFile) : null;

                var srcIsFolder = srcPath != null && Directory.Exists(srcPath);
                var dstIsFolder = dstPath != null && Directory.Exists(dstPath);

                if (srcFile != dstFile)
                    AlignedFiles.Add(new AlignedFile { SrcFile = srcFile, DstFile = dstFile, IsMatched = false, IsSame = false, SrcIsFolder = srcIsFolder, DstIsFolder = dstIsFolder });
                // 相同文件名
                else
                {
                    // 都是文件夹
                    if (srcIsFolder && dstIsFolder)
                    {
                        AlignedFiles.Add(new AlignedFile { SrcFile = file, DstFile = file, IsMatched = true, IsSame = true, SrcIsFolder = true, DstIsFolder = true });
                    }
                    // 一个是文件夹，一个是文件
                    else if (srcIsFolder || dstIsFolder)
                    {
                        AlignedFiles.Add(new AlignedFile { SrcFile = file, DstFile = file, IsMatched = true, IsSame = false, SrcIsFolder = srcIsFolder, DstIsFolder = dstIsFolder });
                    }
                    // 都是文件
                    else
                    {
                        var srcHash = ComputeHash(srcPath);
                        var dstHash = ComputeHash(dstPath);
                        AlignedFiles.Add(new AlignedFile { SrcFile = file, DstFile = file, IsMatched = true, IsSame = srcHash == dstHash, SrcIsFolder = false, DstIsFolder = false });
                    }
                }
            }
        }

        public FolderViewModel()
        {
            SrcFiles = new ObservableCollection<string>();
            DstFiles = new ObservableCollection<string>();
            AlignedFiles = new ObservableCollection<AlignedFile>();
        }

        public FolderViewModel(string srcFolderPath, string dstFolderPath) : this()
        {
            SrcFolderPath = srcFolderPath;
            DstFolderPath = dstFolderPath;
        }
    }
}
