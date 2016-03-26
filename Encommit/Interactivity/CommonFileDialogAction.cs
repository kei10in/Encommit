using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace Encommit.Interactivity
{
    public class FileDialogAction : TriggerAction<DependencyObject>
    {
        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register("FilePath", typeof(string), typeof(FileDialogAction), new PropertyMetadata(""));


        public bool IsFolderPicker
        {
            get { return (bool)GetValue(IsFolderPickerProperty); }
            set { SetValue(IsFolderPickerProperty, value); }
        }

        public static readonly DependencyProperty IsFolderPickerProperty =
            DependencyProperty.Register("IsFolderPicker", typeof(bool), typeof(FileDialogAction), new PropertyMetadata(false));

        protected override void Invoke(object parameter)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = IsFolderPicker;

            var result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                FilePath = dialog.FileName;
            }
        }
    }
}
