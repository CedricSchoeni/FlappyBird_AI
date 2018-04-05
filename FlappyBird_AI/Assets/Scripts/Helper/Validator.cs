using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Validator
{
    
    private static Validator instance;

    public static Validator getInstance()
    {
        if (instance != null)
        {
            return instance;
        }
        else
        {
            createInstance();
            return instance;
        }
    }

    private Validator()
    {

    }

    private static void createInstance()
    {
        instance = new Validator();
    }



    public bool parseFloat(string val)
    {
        float res;
        return float.TryParse(val, out res);
    }

    public bool parseInt(string val)
    {
        int res = 0;
        return int.TryParse(val, out res);
    }

}
