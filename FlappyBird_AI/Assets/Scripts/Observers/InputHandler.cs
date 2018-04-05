using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour {


    public MainController main;

    private KeyCode[] customKeys = new KeyCode[] {
        KeyCode.Escape
    };

    void Start()
    {

    }
	void Update () {
        // goes through the predefined array of keys that could be pressed
        foreach (KeyCode key in customKeys)
            if (Input.GetKeyDown(key))
                main.handleKeyDown(key);
        if (!EventSystem.current.IsPointerOverGameObject())
            if (Input.GetMouseButtonDown(0))
                main.handleLClick();


    }
}
