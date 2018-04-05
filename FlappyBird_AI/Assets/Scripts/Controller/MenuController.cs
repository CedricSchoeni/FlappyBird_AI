using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Data;
using Mono.Data.Sqlite;
using System;

public class MenuController : ACController, IController
{
    private MainController main; // instance of MainController
    public static MenuController _instance; // singleton instance of this controller

    private IView UIview; // UIView updates the ui

    void Start()
    {
        // Singleton - only one instance of this object can exists at a time
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // at this point the class has already been updated with the correct values from the MainController 

        ((UICustomMenuView)UIview).createDifficultiesList(selectAllDifficulties());
        ((UICustomMenuView)UIview).createNetworkList(selectAllNetworks());
    }

    private List<Entity> selectAllDifficulties()
    {
        List<Entity> test = new List<Entity>();
        SqliteConnection con = DBConnection.getDbConnection();
        string stm = "SELECT * FROM Difficulties;";
        using (SqliteCommand cmd = new SqliteCommand(stm, con))
        {
            using (SqliteDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    test.Add(new Difficulties( rdr.GetInt32(0).ToString(), rdr.GetString(1), rdr.GetFloat(2), rdr.GetFloat(3), rdr.GetInt32(4)));
                }
            }
        }
        return test;
    }

    private List<Entity> selectAllNetworks()
    {
        List<Entity> test = new List<Entity>();
        SqliteConnection con = DBConnection.getDbConnection();
        string stm = "SELECT * FROM Weights;";
        using (SqliteCommand cmd = new SqliteCommand(stm, con))
        {
            using (SqliteDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    test.Add(new Weights(rdr.GetInt32(0).ToString(), rdr.GetString(1), rdr.GetString(2)));
                }
            }
        }
        return test;
    }

    public override void handleKeyDown(KeyCode key)
    {
        
    }

    public override void handleLClick()
    {
        
    }

    public IController updateController(IControllerValue c)
    {
        this.main = ((MenuControllerValue)c).main;
        this.UIview = ((MenuControllerValue)c).view;
        return this;
    }
}
