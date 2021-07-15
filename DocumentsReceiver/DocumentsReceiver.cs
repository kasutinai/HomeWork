using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;

namespace DocReceiver
{
    public class DocumentsReceiver:IDisposable
    {
        public event Action DocumentsReady;
        public event Action TimedOut;

        private FileSystemWatcher _watcher;
        private Timer _timer = new Timer();

        private string _targetDirectory;
        private int _waitingInterval;
        private List<string> _fileList;

        public DocumentsReceiver(string targetDirectory, int waitingInterval, List<string> fileList)
        {
            _targetDirectory = targetDirectory;
            _fileList = fileList;
            _waitingInterval = waitingInterval;
        }

        public void Start()
        {
            var dir = new DirectoryInfo(_targetDirectory);
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Directory {_targetDirectory} not found");

            _watcher = new FileSystemWatcher(_targetDirectory);
            //Filters is supported since .NET 5.0 and .NET Core	3.0 only
            //It's not so important here to use the list of watchers instead
            //_fileList.ForEach(file => _watcher.Filters.Add(file));

            _watcher.Created += CheckFiles;
            _watcher.Deleted += CheckFiles;
            _watcher.Renamed += CheckFiles;

            _watcher.EnableRaisingEvents = true;

            _timer.Interval = _waitingInterval;
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

        public void Dispose()
        {
            _watcher.Dispose();
            _timer.Dispose();
        }
    }
}
