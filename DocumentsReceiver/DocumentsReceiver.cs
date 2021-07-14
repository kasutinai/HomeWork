using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;

namespace DocReceiver
{
    public class DocumentsReceiver
    {
        public event Action DocumentsReady;
        public event Action TimedOut;

        private FileSystemWatcher _watcher;
        private Timer _timer = new Timer();

        private string _targetDirectory;
        private List<string> _fileList;

        public void Start(string targetDirectory, int waitingInterval, List<string> fileList)
        {
            var dir = new DirectoryInfo(targetDirectory);
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Directory {targetDirectory} not found");

            _targetDirectory = targetDirectory;
            _fileList = fileList;
            _watcher = new FileSystemWatcher(targetDirectory);

            _fileList.ForEach(file => _watcher.Filters.Add(file));

            _watcher.Created += CheckFiles;
            _watcher.Deleted += CheckFiles;
            _watcher.Renamed += CheckFiles;

            _watcher.EnableRaisingEvents = true;

            _timer.Interval = waitingInterval;
            _timer.Elapsed += IntervalElapsed;
            _timer.Start();
        }

        private void IntervalElapsed(object sender, ElapsedEventArgs e)
        {
            Unsubscribe();
            TimedOut?.Invoke();
        }

        private void CheckFiles(object sender, FileSystemEventArgs e)
        {
            int i = 0;
            var dir = new DirectoryInfo(_targetDirectory);
            foreach (FileInfo file in dir.GetFiles())
            {
                if (_fileList.Contains(file.Name))
                    i++;
            }

            if (i == _fileList.Count)
            {
                Unsubscribe();
                DocumentsReady?.Invoke();
            }
        }

        private void Unsubscribe()
        {
            _watcher.Created -= CheckFiles;
            _watcher.Deleted -= CheckFiles;
            _watcher.Renamed -= CheckFiles;

            _timer.Elapsed -= IntervalElapsed;
        }
    }
}
