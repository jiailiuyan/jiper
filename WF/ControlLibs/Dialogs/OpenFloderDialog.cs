using System.IO;

namespace Ji.WFHelper.ControlLibs.Dialogs
{
    /// <summary>
    /*  opendialogcustom controlex = new opendialogcustom();
            controlex.OpenDialog.InitialDirectory = Path.GetFullPath(Application.ExecutablePath);
            controlex.ShowDialog(this);
            controlex.Dispose();*/

    /// </summary>
    public partial class OpenFloderDialog : OpenFileDialogEx
    {
        public DirectoryInfo Info { get; private set; }

        public OpenFloderDialog(string dir = "")
            : base(dir)
        {
            InitializeComponent();

            this.OpenDialog.Multiselect = false;
            this.OpenDialog.Filter = "folders|*.\n";
        }

        public override void OnFileNameChanged(string fileName)
        {
            SetShowText(fileName);
        }

        public override void OnFolderNameChanged(string folderName)
        {
            SetShowText(folderName);
        }

        public override void SetShowText(string directorypath)
        {
            if (!string.IsNullOrWhiteSpace(directorypath))
            {
                var directoryInfo = new DirectoryInfo(directorypath);
                if (directoryInfo.Exists)
                {
                    base.SetShowText(directorypath);

                    this.textBox1.Text = directoryInfo.Name;
                    Info = directoryInfo;
                    return;
                }
            }

            Info = null;
        }
    }
}