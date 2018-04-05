using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour {


    private MainController main;

	void Start () {
        main = MainController._instance;
        // give _SCRIPTS_ gameObject to the MainController and let him know the scene is loaded
        main.scripts = this.gameObject;
        main.sceneLoaded();
	}

    public void loadMainMenu()
    {
        main.loadScene(0);
    }

    public void loadCustomization()
    {
        main.loadScene(1);
    }

    public void loadGame()
    {
        main.loadScene(2);
    }

    public void loadAI(Difficulties d)
    {
        //Debug.Log(d.name);
        main.setViewData("difficulty", d);
        main.loadScene(3);
    }

    public void loadEdit(Entity e)
    {
        main.setViewData("entity", e);
        main.loadScene(4);
    }
}
