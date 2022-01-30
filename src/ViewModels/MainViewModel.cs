using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using file_drop_sample.Models;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;
using Syroot.Windows.IO;

namespace file_drop_sample.ViewModels
{
    public interface IDragDrop : IDragSource, IDropTarget
    {
    }

    public class MainViewModel : ObservableObject, IDragDrop
    {
        public MainViewModel()
        {
            this.LoadFilesCommand = new RelayCommand(LoadFiles);
        }

        public ICommand LoadFilesCommand { get; }

        public ObservableCollection<FileModel> Files { get; set; } = new();

        private void LoadFiles()
        {
            var dlg = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "All files (*.*)|*.*",
                InitialDirectory = KnownFolders.Downloads.Path
            };

            if (dlg.ShowDialog(Application.Current.MainWindow) == true)
            {
                var files = dlg.FileNames;
                foreach (var file in files)
                {
                    this.Files.Add(new FileModel(file));
                }
            }
        }

        public void StartDrag(IDragInfo dragInfo)
        {
            // drag&drop inside the ListBox with control key
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                GongSolutions.Wpf.DragDrop.DragDrop.DefaultDragHandler.StartDrag(dragInfo);
            }
            else
            {
                var files = dragInfo.SourceItems.OfType<FileModel>().Select(f => f.File).ToArray();

                var dataObject = new DataObject();
                var sc = new System.Collections.Specialized.StringCollection();
                sc.AddRange(files!);
                dataObject.SetFileDropList(sc);

                dragInfo.DataObject = dataObject;
                dragInfo.Effects = DragDropEffects.Copy;
            }
        }

        public bool CanStartDrag(IDragInfo dragInfo)
        {
            return true;
        }

        public void Dropped(IDropInfo dropInfo)
        {
        }

        public void DragDropOperationFinished(DragDropEffects operationResult, IDragInfo dragInfo)
        {
        }

        public void DragCancelled()
        {
        }

        public bool TryCatchOccurredException(Exception exception)
        {
            return GongSolutions.Wpf.DragDrop.DragDrop.DefaultDragHandler.TryCatchOccurredException(exception);
        }

        public void DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.DragInfo?.VisualSource is null
                && dropInfo.Data is DataObject dataObject
                && dataObject.GetDataPresent(DataFormats.FileDrop)
                && dataObject.ContainsFileDropList())
            {
                // Note that you can have more than one file.
                dropInfo.Effects = dataObject.GetData(DataFormats.FileDrop) is string[] files
                                   && files.Any(File.Exists)
                    ? DragDropEffects.Copy
                    : DragDropEffects.None;
            }
            else
            {
                GongSolutions.Wpf.DragDrop.DragDrop.DefaultDropHandler.DragOver(dropInfo);
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.DragInfo?.VisualSource is null
                && dropInfo.Data is DataObject dataObject
                && dataObject.GetDataPresent(DataFormats.FileDrop)
                && dataObject.ContainsFileDropList())
            {
                // Note that you can have more than one file.
                var files = dataObject.GetFileDropList();
                foreach (var file in files)
                {
                    this.Files.Add(new FileModel(file));
                }
            }
            else
            {
                GongSolutions.Wpf.DragDrop.DragDrop.DefaultDropHandler.Drop(dropInfo);
            }
        }
    }
}