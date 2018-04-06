using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ACView : MonoBehaviour{
    [SerializeField]
    protected GameObject optionMenu;
    
    public void switchOptionMenu()
    {
        bool active = optionMenu.activeSelf;
        optionMenu.SetActive(!active);
        Time.timeScale = (!active) ? 0f : 1f;
    }
}
