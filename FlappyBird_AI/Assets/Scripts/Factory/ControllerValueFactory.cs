using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerValueFactory
{


    public IControllerValue getGameValues(MainController main, IView view, PipeControllerValue pipeControllerValue, bool isActive, int scoreIncrement)
    {
        return new GameControllerValue(main, view, pipeControllerValue, isActive, scoreIncrement);
    }

    public IControllerValue getBirdValues(bool isActive)
    {
        return new BirdControllerValue(isActive);
    }

    public IControllerValue getPipeValues(float colYMin, float colYMax)
    {
        return new PipeControllerValue(colYMin, colYMax);
    }

    public IControllerValue getMenuValues(MainController main, IView view)
    {
        return new MenuControllerValue(main, view);
    }

    public IControllerValue getEditValues(MainController main, IView view, Entity entity)
    {
        return new EditControllerValue(main, view, entity);
    }


}
