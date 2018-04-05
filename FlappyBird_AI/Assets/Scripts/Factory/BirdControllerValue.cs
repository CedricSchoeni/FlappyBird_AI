using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdControllerValue : IControllerValue{

    public bool aiActive;

    public BirdControllerValue(bool _aiActive)
    {
        this.aiActive = _aiActive;
    }

}
