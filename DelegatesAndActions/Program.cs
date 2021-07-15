using System;
using System.Collections.Generic;
using System.IO;
using DocReceiver;

namespace DelegatesAndActions
{    
    class Program
    {
        private static bool _close = false;
        static void Main(string[] args)
        {
            try
            {
                string targetDirectory = "C:\\Passport";
                List<string> fileList = new() { "Паспорт.jpg", "Заявление.txt", "Фото.jpg" };
                int waitingInterval = 10000;
                DocumentsReceiver doc = new(targetDirectory, waitingInterval, fileList);

                doc.DocumentsReady += Ready;
                doc.TimedOut += TimedOut;

                doc.Start();

                while (!_close)
                {
                }

                System.Threading.Thread.Sleep(1000);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void TimedOut()
        {
            Console.WriteLine("Time is out.");
            _close = true;
        }

        private static void Ready()
        {
            Console.WriteLine("Documents are ready.");
            _close = true;
        }
    }
}
