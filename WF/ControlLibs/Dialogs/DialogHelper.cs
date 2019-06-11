using System.Collections.Generic;
using System.Windows.Forms;

namespace Ji.WFHelper.ControlLibs.Dialogs
{
    public static class DialogHelper
    {
        public static List<string> SearchFiles(string filter = "所有文件(*.*)|*.*", bool multiselect = true)
        {
            List<string> filPath = new List<string>();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = multiselect;
            dialog.DereferenceLinks = true;

            dialog.Filter = filter;
            dialog.FilterIndex = 0;
            dialog.RestoreDirectory = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                filPath.AddRange(dialog.FileNames);
            }
            return filPath;
        }

        public static string SearchFolder(string dir = "")
        {
            var d = new OpenFloderDialog(dir);
            d.ShowDialog();
            return d.Info != null ? d.Info.FullName : string.Empty;
        }

        public static string SaveFile(string defaultExt = "", string filter = "所有文件(*.*)|*.*")
        {
            string filPath = string.Empty;
            SaveFileDialog dialog = new SaveFileDialog();
            if (!string.IsNullOrWhiteSpace(defaultExt))
            {
                dialog.DefaultExt = defaultExt;
                dialog.AddExtension = true;
            }
            dialog.Filter = filter;
            dialog.FilterIndex = 0;
            dialog.DereferenceLinks = true;
            dialog.RestoreDirectory = false;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                filPath = dialog.FileName;
            }
            return filPath;
        }

        public static string SaveFloder(string tittle = "")
        {
            string filPath = string.Empty;
            var dialog = new FolderBrowserDialog();
            //dialog.RootFolder = System.Environment.SpecialFolder.MyDocuments;
            dialog.Description = tittle;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                filPath = dialog.SelectedPath;
            }
            return filPath;
        }
    }
}