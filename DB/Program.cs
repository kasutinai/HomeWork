using System;

namespace DB
{
    class Program
    {        
        static void Main(string[] args)
        {
            var db = new DBConnector();

            db.ShowAllTables();

            Console.WriteLine(db.Add());            

            db.ShowAllTables();

            Console.ReadLine();
        }        
    }
}
