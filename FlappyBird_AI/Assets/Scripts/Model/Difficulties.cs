using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Difficulties : ACEntity, Entity
{
    public int id;
    public string name;
    public float yMin;
    public float yMax;
    public int scorePoint;

    public Difficulties(int id, string name, float yMin, float yMax, int scorePoint)
    {
        this.id = id;
        this.name = name;
        this.yMin = yMin;
        this.yMax = yMax;
        this.scorePoint = scorePoint;
    }

    public void create()
    {
        throw new System.NotImplementedException();
    }

    public void delete()
    {
        throw new System.NotImplementedException();
    }


    public void update()
    {
        
    }
}
