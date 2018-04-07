using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger {

    private static Debugger instance;

    private static string path = Application.dataPath + "/errorlog.txt";

    public static Debugger getInstance()
    {
        if (instance == null)
            createInstance();
        return instance;
    }

    private Debugger()
    {
        // create file if not exists
        if (!System.IO.File.Exists(path))
        {
            System.IO.File.Create(path);
        }
    }

    private static void createInstance()
    {
        instance = new Debugger();
    }

    public void writeToFile(string text)
    {
        using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(path, true))
        {
            file.WriteLine(text);
        }
    }

}
