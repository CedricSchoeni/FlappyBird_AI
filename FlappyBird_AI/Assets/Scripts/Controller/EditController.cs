using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditController : ACController, IController {


    private MainController main; // instance of MainController
    public static EditController _instance; // singleton instance of this controller

    private Entity entity;

    private IView UIview; // UIView updates the ui

    // Use this for initialization
    void Start()
    {
        // Singleton - only one instance of this object can exists at a time
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // at this point the class has already been updated with the correct values from the MainController 

        prepareView();

    }

    private void prepareView()
    {
        string type = entity.GetType().ToString();
        bool create = false;
        switch (type)
        {
            case "Difficulties":
                if (((Difficulties)entity).id == null) create = true;
                ((UIEditView)UIview).prepareDifficulties((Difficulties)entity, create);
                break;
            case "Weights":
                if (((Weights)entity).id == null) create = true;
                ((UIEditView)UIview).prepareNetwork((Weights)entity, create);
                break;
            default:
                Debug.LogError("Unknown Entity found: " + type);
                break;
        }
    }

    public override void handleKeyDown(KeyCode key)
    {
        throw new System.NotImplementedException();
    }

    public override void handleLClick()
    {
        throw new System.NotImplementedException();
    }

    public IController updateController(IControllerValue c)
    {
        this.main = ((EditControllerValue)c).main;
        this.UIview = ((EditControllerValue)c).view;
        this.entity = ((EditControllerValue)c).entity;
        return this;
    }


}
