using Npgsql;
using System;

namespace DB
{    
    public class DBConnector:IDisposable
    {
        const string connectionString = "Host=localhost;Username=otus;Password=otus;Database=otus";
        const int min = 5, max = 40;
        private NpgsqlConnection _connection;
        private NpgsqlDataReader _reader;
        private string _tableName;
        private void connectDB()
        {
            _connection = new NpgsqlConnection(connectionString);
            _connection.Open();
        }

        public void ShowAllTables()
        {
            _tableName = "student";
            ShowTable();
            _tableName = "course";
            ShowTable();
            _tableName = "studentscourses";
            ShowTable();
        }
        private void GetReader()
        {
            connectDB();

            var sql = $"SELECT * FROM {_tableName}";
            using var cmd = new NpgsqlCommand(sql, _connection);
            _reader = cmd.ExecuteReader();
        }
        public void ShowTable()
        {
            GetReader();

            Console.WriteLine(_tableName);
            ShowLine(true);

            while (_reader.Read())
            {
                ShowLine();
            }

            _connection.Dispose();
        }

        private void ShowLine(bool header = false)
        {
            var minStr = min.ToString();
            var maxStr = max.ToString();

            for (int i = 0; i < _reader.FieldCount; i++)
            {
                var str = header ? _reader.GetName(i).Trim() : _reader[i].ToString().Trim();

                if (max < str.Length)
                    str = str.Substring(0, max);

                var columnSize = _reader.GetColumnSchema()[i].ColumnSize;
                string columnSizeStr;

                if (columnSize < min || columnSize.Equals(null))
                    columnSizeStr = min > _reader.GetName(i).Trim().Length ? minStr : _reader.GetName(i).Trim().Length.ToString();
                else if (columnSize > max)
                    columnSizeStr = maxStr;
                else columnSizeStr = columnSize.ToString();

                var format = string.Format("{{0,{0}}} ", columnSizeStr);

                Console.Write(format, str);
            }
            Console.WriteLine();
        }

        public string Add()
        {
            Console.WriteLine("Input table to add a new record (student, course, studentscourses)");
            _tableName = Console.ReadLine().ToLower().Trim();

            try
            {
                GetReader();

                var insertStr = "";
                var parametersStr = "";

                for (int i = 1; i < _reader.FieldCount; i++)
                {
                    insertStr += _reader.GetName(i).Trim() + ",";
                    parametersStr += $":{_reader.GetName(i).Trim()},";
                }

                Console.WriteLine($"Input record values '{insertStr.Replace(",", ";")}'");
                insertStr = $"INSERT INTO {_tableName} ({insertStr.Substring(0, insertStr.Length-1)}) VALUES ({parametersStr.Substring(0,parametersStr.Length-1)})";                

                string inputStr = Console.ReadLine().Trim();                
                string[] inputPart = inputStr.Split(";");
                
                using var connectionAdd = new NpgsqlConnection(connectionString);
                connectionAdd.Open();

                using var cmd = new NpgsqlCommand(insertStr, connectionAdd);                

                for (int i = 1; i < _reader.FieldCount; i++)
                {
                    var curInputPart = inputPart[i - 1];
                    var curFieldType = _reader.GetFieldType(i);

                    if (curInputPart.Equals(null))
                        continue;

                    object curObject = curInputPart;

                    if (curFieldType == Type.GetType("System.DateTime"))
                        curObject = DateTime.Parse(curInputPart);
                    else if (curFieldType == Type.GetType("System.Int32"))
                        curObject = Int32.Parse(curInputPart);
                    else if (curFieldType == Type.GetType("System.Double"))
                        curObject = Double.Parse(curInputPart);

                    cmd.Parameters.AddWithValue($":{_reader.GetName(i).Trim()}", curObject);
                }

                return $"Insert into {_tableName} table. Affected rows count: {cmd.ExecuteNonQuery().ToString()}";
            }
            catch (Exception error)
            {
                return error.Message;
            }

        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
