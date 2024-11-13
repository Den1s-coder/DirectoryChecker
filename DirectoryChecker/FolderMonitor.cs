using System;
using System.IO;

public class FolderMonitor : IDisposable
{
    private readonly FileSystemWatcher _watcher;
    public string Path { get; }
    public event FileSystemEventHandler OnFileSystemEvent;

    public FolderMonitor(string path)
    {
        Path = path;
        _watcher = new FileSystemWatcher(path)
        {
            NotifyFilter = NotifyFilters.Attributes
                        | NotifyFilters.CreationTime
                        | NotifyFilters.DirectoryName
                        | NotifyFilters.FileName
                        | NotifyFilters.LastAccess
                        | NotifyFilters.LastWrite
                        | NotifyFilters.Security
                        | NotifyFilters.Size,
            EnableRaisingEvents = true,
            IncludeSubdirectories = true
        };

        _watcher.Created += OnCreated;
        _watcher.Deleted += OnDeleted;
        _watcher.Changed += OnChanged;
        _watcher.Renamed += OnRenamed;
    }

    private void OnCreated(object sender, FileSystemEventArgs e)
    {
        OnFileSystemEvent?.Invoke(this, e);
    }

    private void OnDeleted(object sender, FileSystemEventArgs e)
    {
        OnFileSystemEvent?.Invoke(this, e);
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        OnFileSystemEvent?.Invoke(this, e);
    }

    private void OnRenamed(object sender, RenamedEventArgs e)
    {
        OnFileSystemEvent?.Invoke(this, e);
    }

    public void Dispose()
    {
        _watcher?.Dispose();
    }
}