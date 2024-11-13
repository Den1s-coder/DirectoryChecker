using System;
using System.Windows.Forms;

public partial class MainForm : Form
{
    private List<FolderMonitor> _monitors;

    public MainForm()
    {
        InitializeComponent();
        _monitors = new List<FolderMonitor>();
    }

    private void InitializeComponent()
    {
        this.btnAddFolder = new Button();
        this.lstLogs = new ListBox();
        this.lstFolders = new ListBox();
        this.btnRemoveFolder = new Button();

        // Form
        this.Text = "Моніторинг папок";
        this.Size = new System.Drawing.Size(800, 600);

        // Add Folder Button
        this.btnAddFolder.Text = "Додати папку";
        this.btnAddFolder.Location = new System.Drawing.Point(10, 10);
        this.btnAddFolder.Click += BtnAddFolder_Click;

        // Remove Folder Button
        this.btnRemoveFolder.Text = "Видалити папку";
        this.btnRemoveFolder.Location = new System.Drawing.Point(120, 10);
        this.btnRemoveFolder.Click += BtnRemoveFolder_Click;

        // Folders ListBox
        this.lstFolders.Location = new System.Drawing.Point(10, 40);
        this.lstFolders.Size = new System.Drawing.Size(300, 200);

        // Logs ListBox
        this.lstLogs.Location = new System.Drawing.Point(10, 250);
        this.lstLogs.Size = new System.Drawing.Size(770, 300);

        // Add controls
        this.Controls.AddRange(new Control[] {
            this.btnAddFolder,
            this.btnRemoveFolder,
            this.lstFolders,
            this.lstLogs
        });
    }

    private void BtnAddFolder_Click(object sender, EventArgs e)
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

    private void BtnRemoveFolder_Click(object sender, EventArgs e)
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
        // Викликаємо через Invoke, оскільки подія приходить з іншого потоку
        this.BeginInvoke(new Action(() =>
        {
            AddLog($"{DateTime.Now:HH:mm:ss} - {e.ChangeType}: {e.FullPath}");
        }));
    }

    private void AddLog(string message)
    {
        if (lstLogs.Items.Count > 1000) // Обмежуємо кількість повідомлень
        {
            lstLogs.Items.RemoveAt(0);
        }
        lstLogs.Items.Add(message);
        lstLogs.TopIndex = lstLogs.Items.Count - 1; // Прокручуємо до останнього повідомлення
    }
}