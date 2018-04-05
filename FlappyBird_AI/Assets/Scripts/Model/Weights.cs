using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weights : ACEntity {
    public string name;
    public string weights;

    public Weights(string id, string name, string weights)
    {
        this.addProperty("name", this.name);
        this.addProperty("weights", this.weights);

        this.setProperty("id", id);
        this.setProperty("name", name);
        this.setProperty("weights", weights);
    }

    public void update()
    {
        this.setProperty("id", id);
        this.setProperty("name", name);
        this.setProperty("weights", weights);
        base.update();
    }
}
