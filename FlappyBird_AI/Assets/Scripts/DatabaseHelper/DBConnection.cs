using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Data;
using Mono.Data.Sqlite;
using System;

public class DBConnection
{
    
    private static SqliteConnection dbConnection;
    private static string connectionString = "URI=file:" + Application.dataPath + "mainDb.sqlite";

    public static SqliteConnection getDbConnection()
    {
        if (DBConnection.dbConnection != null){
            return DBConnection.dbConnection;
        }else{
            DBConnection.createConnection();
            return DBConnection.dbConnection;
        }
    }

    private static void createConnection()
    {
        try
        {
            if (!System.IO.File.Exists(Application.dataPath + "mainDb.sqlite"))
            {
                // db seed will create a new database with tables
                DBSeed seed = new DBSeed(connectionString);
                seed.resetDatabase();
                
                Debug.LogWarning("Database not found. Creating a new one. This leads to a complete loss of data.");
            }
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}
