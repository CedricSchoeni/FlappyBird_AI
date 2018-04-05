using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeControllerValue : IControllerValue {

    public float colYMin;
    public float colYMax;

    public PipeControllerValue(float _colYMin, float _colYMax)
    {
        this.colYMin = _colYMin;
        this.colYMax = _colYMax;
    }
}
