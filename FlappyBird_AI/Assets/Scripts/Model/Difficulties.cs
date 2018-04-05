using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Difficulties : ACEntity
{
    public string name;
    public float yMin;
    public float yMax;
    public int scorePoint;

    public Difficulties(string id, string name, float yMin, float yMax, int scorePoint)
    {
        this.addProperty("name", this.name);
        this.addProperty("yMin", this.yMin);
        this.addProperty("yMax", this.yMax);
        this.addProperty("scorePoint", this.scorePoint);

        this.setProperty("id", id);
        this.setProperty("name", name);
        this.setProperty("yMin", yMin);
        this.setProperty("yMax", yMax);
        this.setProperty("scorePoint", scorePoint);
    }

    public void update()
    {
        this.setProperty("id", id);
        this.setProperty("name", name);
        this.setProperty("yMin", yMin);
        this.setProperty("yMax", yMax);
        this.setProperty("scorePoint", scorePoint);
        base.update();
    }
}
