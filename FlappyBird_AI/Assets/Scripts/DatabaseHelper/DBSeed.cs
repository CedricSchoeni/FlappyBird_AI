using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Data;
using Mono.Data.Sqlite;
using System;

public class DBSeed {

    private string conString;
    public DBSeed(string conString)
    {
        this.conString = conString;
    }

    public void resetDatabase()
    {
        // create db file
        SqliteConnection.CreateFile("mainDb.sqlite");

        // connect to db
        SqliteConnection dbConnection = new SqliteConnection(conString);
        dbConnection.Open();

        // create table
        string sql = "CREATE TABLE Difficulties (id INTEGER PRIMARY KEY AUTOINCREMENT, name VARCHAR(50), ymin FLOAT, ymax FLOAT, scorePoint INTEGER);";
        SqliteCommand command = new SqliteCommand(sql, dbConnection);
        command.ExecuteNonQuery();

        sql = "CREATE TABLE Weights (id INTEGER PRIMARY KEY AUTOINCREMENT, name VARCHAR(50), weights TEXT);";
        command = new SqliteCommand(sql, dbConnection);
        command.ExecuteNonQuery();

        // insert base data
        sql = "INSERT INTO Difficulties (name, ymin, ymax, scorePoint) VALUES('Easy', -0, 0, 10);";
        command = new SqliteCommand(sql, dbConnection);
        command.ExecuteNonQuery();
        sql = "INSERT INTO Difficulties (name, ymin, ymax, scorePoint) VALUES('Normal', -1, 1, 5);";
        command = new SqliteCommand(sql, dbConnection);
        command.ExecuteNonQuery();
        sql = "INSERT INTO Difficulties (name, ymin, ymax, scorePoint) VALUES('Hard', -2, 2, 2);";
        command = new SqliteCommand(sql, dbConnection);
        command.ExecuteNonQuery();

        dbConnection.Close();
    }

}
