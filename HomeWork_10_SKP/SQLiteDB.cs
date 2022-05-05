using System;
using System.Data.SQLite;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_10_SKP
{
    internal class SQLiteDB : IRepository
    {
        public SQLiteDB()
        {
            _pathToDB = "sqllite.db";
        }

        string _pathToDB;

        /// <summary>
        /// Проверка существования базы данных
        /// </summary>
        public void CheckDBExist()
        {
            if(!File.Exists(_pathToDB)) CreateDB();            
        }

        /// <summary>
        /// Создание базы данных
        /// </summary>
        public void CreateDB()
        {
            //SQLiteConnection.CreateFile(_pathToDB);
            using(SQLiteConnection connection = new SQLiteConnection("Data Source="+ _pathToDB+ ";Mode=ReadWriteCreate;"))
            {
                connection.Open(); //даже если БД не существует то создаться автоматически
                SQLiteCommand command = connection.CreateCommand();
                command.Connection = connection;
                command.CommandText = "CREATE TABLE Clients(_id INTEGER NOT NULL PRIMARY KEY UNIQUE, Name TEXT NOT NULL)"; 
                command.ExecuteNonQuery();

            }
        }
        
        public void Load()
        {
            
        }

        public void Save()
        {
            
        }
    }
}
