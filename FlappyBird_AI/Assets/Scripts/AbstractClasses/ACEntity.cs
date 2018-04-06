using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Data;
using Mono.Data.Sqlite;
using System;
using System.Reflection;

public abstract class ACEntity : Entity
{
    private FieldInfo[] fields;
    public string id;
    protected string tableName;
    protected SqliteConnection conn;
    protected IDictionary<String, System.Object> managedProperties = new Dictionary<String, System.Object>();



    public ACEntity()
    {
        this.tableName = GetType().Name.ToLower();
        fields = GetType().GetFields();
        this.conn = DBConnection.getDbConnection();
        this.addProperty("id", this.id);
    }

    protected void addProperty(string name, System.Object property)
    {
        this.managedProperties.Add(name, property);
    }

    protected void setProperty(string name, System.Object property)
    {
        if (this.managedProperties.ContainsKey(name))
        {
            this.managedProperties[name] = property;
            Type myType = this.GetType();
            FieldInfo myFieldInfo = myType.GetField(name);
            myFieldInfo.SetValue(this, property);
        }
    }

    protected string getColumnValues(bool ignoreId)
    {
        string columns = "";
        foreach (KeyValuePair<string, System.Object> entry in managedProperties)
        {
            if (ignoreId && entry.Key.Equals("id"))
            {
                continue;
            }

            columns += "`" + entry.Key + "`,";
        }
        return columns.Substring(0, columns.Length - 1);
    }

    protected String getFieldValues(bool ignoreId)
    {
        string values = "";
        foreach (KeyValuePair<string, System.Object> entry in managedProperties)
        {

            if (entry.Key.Equals("id") && ignoreId)
                continue;
            string val = entry.Value.ToString();
            if (entry.Key.Equals("id") && !ignoreId)
                values += (",");

            else if (val == null)
                values += ("null,");
            else
                values += ("'" + val + "',");
        }        
        return values.Substring(0, values.Length - 1);
    }

    public void save()
    {   
        string query = "INSERT INTO `" + this.tableName + "`(" + this.getColumnValues(true) + ") VALUES ( " + this.getFieldValues(true) + " );";
        SqliteCommand stmt = new SqliteCommand(query, conn);
        stmt.ExecuteNonQuery();
    }

    public void create()
    { 
    }

    public void update()
    {
        if (id == null)
        {
            save();
            return;
        }
        string query = "UPDATE`" + this.tableName + "` SET ";
        bool ignoreId = true;
        foreach (KeyValuePair<string, System.Object> entry in managedProperties)
        {
            string key = entry.Key;
            string val = entry.Value.ToString();
            if (entry.Key.Equals("id") && ignoreId)
                continue;
            query += "`" + key + "`='" + val + "',";
        }
        query = query.Substring(0, query.Length - 1) + "WHERE id = " + this.id + ";";

        SqliteCommand stmt = new SqliteCommand(query, conn);
        stmt.ExecuteNonQuery();
    }

    public void delete()
    {
        string query = "DELETE FROM `" + this.tableName + "` WHERE id = " + this.id + ";";
        SqliteCommand stmt = new SqliteCommand(query, conn);
        stmt.ExecuteNonQuery();
    }
}
