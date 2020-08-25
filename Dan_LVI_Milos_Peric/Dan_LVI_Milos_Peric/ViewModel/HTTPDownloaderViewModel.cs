using Dan_LVI_Milos_Peric.Command;
using Dan_LVI_Milos_Peric.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Dan_LVI_Milos_Peric.ViewModel
{
    class HTTPDownloaderViewModel : ViewModelBase
    {
        private HTTPDownloaderView hTTPDownloaderView;

        #region Constructor

        public HTTPDownloaderViewModel(HTTPDownloaderView hTTPDownloaderView)
        {
            this.hTTPDownloaderView = hTTPDownloaderView;
        }
        #endregion

        #region Properties

        private string htmlLink;
        public string HtmlLink
        {
            get { return htmlLink; }
            set
            {
                htmlLink = value;
                OnPropertyChanged("HtmlLink");
            }
        }

        private string folderName;
        public string FolderName
        {
            get { return folderName; }
            set
            {
                folderName = value;
                OnPropertyChanged("FolderName");
            }
        }

        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        private string htmlFilePath;
        public string HtmlFilePath
        {
            get { return htmlFilePath; }
            set
            {
                htmlFilePath = value;
                OnPropertyChanged("HtmlFilePath");
            }
        }

        private string archiveFileName;
        public string ArchiveFileName
        {
            get { return archiveFileName; }
            set
            {
                archiveFileName = value;
                OnPropertyChanged("ArchiveFileName");
            }
        }

        private string htmlDownload;
        public string HtmlDownload
        {
            get { return htmlDownload; }
            set
            {
                htmlDownload = value;
                OnPropertyChanged("HtmlDownload");
            }
        }
        #endregion

        #region Commands

        private ICommand downloadCommand;
        public ICommand DownloadCommand
        {
            get
            {
                if (downloadCommand == null)
                {
                    downloadCommand = new RelayCommand(param => DownloadCommandExecute(), param => CanDownloadCommandExecute());
                }
                return downloadCommand;
            }
        }

        private void DownloadCommandExecute()
        {
            StringBuilder file = new StringBuilder();
            file.Append(@"..\..\.\");
            file.Append(FolderName);
            file.Append("\\");
            file.Append(FileName);
            file.Append(".html");
            HtmlFilePath = file.ToString();
            try
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        if (!Directory.Exists(@"..\..\.\" + FolderName))
                        {
                            Directory.CreateDirectory(@"..\..\.\" + FolderName);
                        }
                        client.DownloadFile(HtmlLink, HtmlFilePath);
                        MessageBox.Show("File downloaded successfully to Project root directory.", "Success!");
                    }
                    catch (WebException e)
                    {
                        MessageBox.Show("Html link is not valid!", "Warning!");
                        return;
                        throw e;                       
                    }
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanDownloadCommandExecute()
        {
            if (string.IsNullOrEmpty(HtmlLink) || string.IsNullOrEmpty(FolderName) || string.IsNullOrEmpty(FileName))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private ICommand compressCommand;
        public ICommand CompressCommand
        {
            get
            {
                if (compressCommand == null)
                {
                    compressCommand = new RelayCommand(param => CompressCommandExecute(), param => CanCompressCommandExecute());
                }
                return compressCommand;
            }
        }

        private void CompressCommandExecute()
        {
            try
            {
                if (string.IsNullOrEmpty(ArchiveFileName))
                {
                    MessageBox.Show("Please enter zip archive file name.", "Warning!");
                    return;
                }
                string dirname = @"..\..\.\" + FolderName;
                string zippath = @"..\..\.\" + ArchiveFileName + ".zip";
                if (File.Exists(zippath))
                {
                    MessageBox.Show("Archive with same name already exists please choose another name.", "Warning!");
                    return;
                }
                if (!File.Exists(HtmlFilePath))
                {
                    MessageBox.Show("Noting to compress, please download the web page first.", "Warning!");
                    return;
                }
                ZipFile.CreateFromDirectory(dirname, zippath);
                MessageBox.Show("File archived successfully in Project root directory.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanCompressCommandExecute()
        {
            if (!File.Exists(HtmlFilePath))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private ICommand quitCommand;
        public ICommand QuitCommand
        {
            get
            {
                if (quitCommand == null)
                {
                    quitCommand = new RelayCommand(param => QuitCommandExecute());
                }
                return quitCommand;
            }
        }

        private void QuitCommandExecute()
        {
            try
            {
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
    }
}
