using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using DirectoryChecker;

namespace DirectoryChecker
{
    public partial class Form1 : Form
    {
        private List<FolderMonitor> _monitors;

        public Form1()
        {
            InitializeComponent();
            _monitors = new List<FolderMonitor>();
        }

        private void btnAddFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string path = dialog.SelectedPath;
                    if (!lstFolders.Items.Contains(path))
                    {
                        var monitor = new FolderMonitor(path);
                        monitor.OnFileSystemEvent += Monitor_OnFileSystemEvent;
                        _monitors.Add(monitor);
                        lstFolders.Items.Add(path);
                        AddLog($"Додано моніторинг папки: {path}");
                    }
                }
            }
        }

        private void btnRemoveFolder_Click(object sender, EventArgs e)
        {
            if (lstFolders.SelectedIndex != -1)
            {
                string path = lstFolders.SelectedItem.ToString();
                var monitor = _monitors.FirstOrDefault(m => m.Path == path);
                if (monitor != null)
                {
                    monitor.Dispose();
                    _monitors.Remove(monitor);
                }
                lstFolders.Items.Remove(path);
                AddLog($"Видалено моніторинг папки: {path}");
            }
        }

        private void Monitor_OnFileSystemEvent(object sender, FileSystemEventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                AddLog($"{DateTime.Now:HH:mm:ss} - {e.ChangeType}: {e.FullPath}");
            }));
        }

        private void AddLog(string message)
        {
            if (lstLogs.Items.Count > 1000)
            {
                lstLogs.Items.RemoveAt(0);
            }
            lstLogs.Items.Add(message);
            lstLogs.TopIndex = lstLogs.Items.Count - 1;
        }
    }
}