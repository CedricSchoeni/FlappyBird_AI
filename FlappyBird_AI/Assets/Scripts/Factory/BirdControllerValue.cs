using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdControllerValue : IControllerValue{

    public bool aiActive;
    public double[] weights;

    public BirdControllerValue(bool _aiActive, double[] _weights)
    {
        this.aiActive = _aiActive;
        this.weights = _weights;
    }

}
