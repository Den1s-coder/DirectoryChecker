using System;
using System.IO;
using System.Linq;

namespace DirectoryChecker
{
    public class FolderMonitor : IDisposable
    {
        private readonly FileSystemWatcher _watcher;
        private DateTime _lastEventTime = DateTime.MinValue;
        private string _lastEventPath = string.Empty;
        private WatcherChangeTypes _lastEventType;
        private readonly string[] _fileFilters = new[] { "*.*" }; // Можна змінити на потрібні розширення

        public string Path { get; }
        public event FileSystemEventHandler OnFileSystemEvent;

        public FolderMonitor(string path)
        {
            Path = path;
            _watcher = new FileSystemWatcher(path)
            {
                NotifyFilter = NotifyFilters.CreationTime
                            | NotifyFilters.DirectoryName
                            | NotifyFilters.FileName
                            | NotifyFilters.LastWrite
                            | NotifyFilters.Size,
                EnableRaisingEvents = true,
                IncludeSubdirectories = true,
                Filter = "*.*"
            };

            _watcher.Created += OnCreated;
            _watcher.Deleted += OnDeleted;
            _watcher.Changed += OnChanged;
            _watcher.Renamed += OnRenamed;
        }

        private bool IsDuplicateEvent(FileSystemEventArgs e)
        {
            var now = DateTime.Now;
            var isDuplicate = (now - _lastEventTime).TotalMilliseconds < 100
                            && _lastEventPath == e.FullPath
                            && _lastEventType == e.ChangeType;

            _lastEventTime = now;
            _lastEventPath = e.FullPath;
            _lastEventType = e.ChangeType;

            return isDuplicate;
        }

        private bool IsFileTypeAllowed(string filePath)
        {
            return _fileFilters.Any(filter =>
                filter.EndsWith(".*") ||
                filePath.EndsWith(filter.TrimStart('*'), StringComparison.OrdinalIgnoreCase));
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (!IsDuplicateEvent(e) && IsFileTypeAllowed(e.FullPath))
            {
                OnFileSystemEvent?.Invoke(this, e);
            }
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            if (!IsDuplicateEvent(e) && IsFileTypeAllowed(e.FullPath))
            {
                OnFileSystemEvent?.Invoke(this, e);
            }
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (!IsDuplicateEvent(e) && IsFileTypeAllowed(e.FullPath))
            {
                try
                {
                    if (File.Exists(e.FullPath))
                    {
                        var attr = File.GetAttributes(e.FullPath);
                        if ((attr & FileAttributes.Directory) != FileAttributes.Directory)
                        {
                            OnFileSystemEvent?.Invoke(this, e);
                        }
                    }
                }
                catch (IOException)
                {
                    // Ігноруємо помилки доступу до файлу
                }
                catch (UnauthorizedAccessException)
                {
                    // Ігноруємо помилки доступу
                }
            }
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            if (!IsDuplicateEvent(e) && IsFileTypeAllowed(e.FullPath))
            {
                OnFileSystemEvent?.Invoke(this, e);
            }
        }

        public void Dispose()
        {
            _watcher?.Dispose();
        }
    }
}