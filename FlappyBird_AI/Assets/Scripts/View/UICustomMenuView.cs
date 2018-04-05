using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UICustomMenuView : ACView, IView
{
    [SerializeField]
    private GameObject listParent;
    [SerializeField]
    private GameObject listElement;

    public void createDifficultiesList(List<Entity> list)
    {
        foreach (Difficulties elem in list)
        {
            GameObject tempObj = Instantiate(this.listElement, this.listParent.transform);
            tempObj.transform.Find("Name").GetComponent<Text>().text = elem.name;
            tempObj.transform.Find("Params").GetComponent<Text>().text = "yMin: " + elem.yMin + "yMax: " + elem.yMax;
            tempObj.transform.Find("Edit").GetComponent<Button>().onClick.AddListener(() => MainController._instance.uIHandler.loadEdit(elem));
            tempObj.transform.Find("Play").GetComponent<Button>().onClick.AddListener(() => MainController._instance.uIHandler.loadAI(elem));
        }
    }

    public void activate(string name, bool active)
    {
        throw new System.NotImplementedException();
    }

    public void setText(string name, string text, bool concat = false)
    {
        throw new System.NotImplementedException();
    }
}

