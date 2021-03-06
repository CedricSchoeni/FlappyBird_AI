﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour {


    public static MainController _instance;

    private IDictionary<string, System.Object> viewData = new Dictionary<string, System.Object>();

    private ACController viewController;

    public ControllerValueFactory factory;
    private int loadIndex;

    public GameObject scripts;
    public UIHandler uIHandler;



    // Singleton - only one instance of this object can exists at a time
    void Awake() {
		
        if (_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
	}

    void Start()
    {
        factory = new ControllerValueFactory();
    }

    void Update()
    {

    }

    public void setViewData(string key, System.Object data)
    {
        if (!viewData.ContainsKey(key))
            viewData.Add(key, data);
        else
            viewData[key] = data;
    }

    public void sceneLoaded()
    {
        IView view;
        InputHandler inputHandler = scripts.GetComponent<InputHandler>();
        uIHandler = scripts.GetComponent<UIHandler>(); 
        inputHandler.main = this;
        switch (loadIndex)
        {
            case 0: // MainMenu loaded
                viewController = null;
                break;
            case 1: // MainMenu_Custom loaded
                view = scripts.GetComponent<UICustomMenuView>(); // get view from _SCRIPTS_ GameObject
                viewController = (MenuController)scripts.GetComponent<MenuController>().updateController(
                        factory.getMenuValues(this, view)
                        );
                break;
            case 2: // GameScene Normalplay loaded
                view = scripts.GetComponent<UIGameView>(); // get view from _SCRIPTS_ GameObject
                IControllerValue pVal = new PipeControllerValue(-2f, 2f); // FACTORY
                viewController = (GameController)scripts.GetComponent<GameController>().updateController(
                        factory.getGameValues(this, view, (PipeControllerValue)pVal, false, 1)
                        );
                break;
            case 3: // GameScene AIplay loaded
                IControllerValue pVal2 = null;
                Weights w1 = null;
                int scorePoint = 0;
                if (viewData.ContainsKey("difficulty"))
                {
                    Difficulties temp = (Difficulties)viewData["difficulty"];
                    pVal2 = new PipeControllerValue(temp.yMin, temp.yMax);
                    scorePoint = temp.scorePoint;
                }
                else
                {
                    Debug.LogError("Difficulties not set.");
                }

                if (viewData.ContainsKey("weights"))
                    w1 = (Weights)viewData["weights"];

                view = scripts.GetComponent<UIGameView>(); // get view from _SCRIPTS_ GameObject
                
                viewController = (GameController)scripts.GetComponent<GameController>().updateController(
                        factory.getGameValues(this, view, (PipeControllerValue)pVal2, true, scorePoint, w1)
                        );
                break;
            case 4: // Edit Model loaded
                Entity e = null;

                if (viewData.ContainsKey("entity"))
                    e = (Entity)viewData["entity"];
                else
                {
                    Debug.LogError("Difficulties not set.");
                }

                view = scripts.GetComponent<UIEditView>(); // get view from _SCRIPTS_ GameObject

                viewController = (EditController)scripts.GetComponent<EditController>().updateController(
                        factory.getEditValues(this, view, e)
                        );
                break;
            default:

                break;
        }
        viewData.Clear();
    }

    private void loadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void loadScene(int number)
    {
        switch (number)
        {
            case 0:
                loadScene("MainMenu");
                loadIndex = number;
                break;
            case 1:
                loadScene("MainMenu_Custom");
                loadIndex = number;
                break;
            case 2:
                loadScene("GameScene");
                loadIndex = number;
                break;
            case 3:
                loadScene("GameScene");
                loadIndex = number;
                break;
            case 4:
                loadScene("EditScene");
                loadIndex = number;
                break;
            default:

                break;
        }

    }

    public void handleKeyDown(KeyCode key)
    {
        if (viewController == null)
        {
            // MainController will handle the event
        } else
        {
            viewController.handleKeyDown(key);
        }
    }

    public void handleLClick()
    {
        if (viewController == null)
        {
            // MainController will handle the event
        }
        else
        {
            viewController.handleLClick();
        }
    }

}
