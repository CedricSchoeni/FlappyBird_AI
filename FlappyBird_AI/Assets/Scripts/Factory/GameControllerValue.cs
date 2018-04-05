using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerValue : IControllerValue
{

    public MainController main;
    public IControllerValue pControllerValues;
    public bool aiActive;
    public IView view;
    public int scoreIncrement;

    public GameControllerValue(MainController _main, IView _view, PipeControllerValue _pControllerValue, bool _aiActive, int _scoreIncrement)
    {
        this.main = _main;
        this.view = _view;
        this.pControllerValues = _pControllerValue;
        this.aiActive = _aiActive;
        this.scoreIncrement = _scoreIncrement;
    }
}
