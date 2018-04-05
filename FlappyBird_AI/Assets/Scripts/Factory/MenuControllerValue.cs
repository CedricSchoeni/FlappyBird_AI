using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControllerValue : IControllerValue
{

    public MainController main;
    public IView view;

    public MenuControllerValue(MainController _main, IView _view)
    {
        this.main = _main;
        this.view = _view;
    }
}
