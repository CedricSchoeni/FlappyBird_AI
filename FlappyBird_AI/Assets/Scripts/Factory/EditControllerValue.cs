using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditControllerValue : IControllerValue {

    public MainController main;
    public IView view;
    public Entity entity;

    public EditControllerValue(MainController _main, IView _view, Entity _entity)
    {
        this.main = _main;
        this.view = _view;
        this.entity = _entity;
    }
}
