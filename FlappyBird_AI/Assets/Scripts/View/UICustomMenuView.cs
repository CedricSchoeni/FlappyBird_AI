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
    private GameObject networkListParent;
    [SerializeField]
    private GameObject listElement;
    [SerializeField]
    private GameObject networkListElement;


    public void createNetworkList(List<Entity> list)
    {
        foreach (Weights elem in list)
        {
            GameObject tempObj = Instantiate(this.networkListElement, this.networkListParent.transform);
            tempObj.transform.Find("Name").GetComponent<Text>().text = elem.name;
            string res = (elem.weights.Length > 50) ? elem.weights.Substring(0, 50) : elem.weights;
            tempObj.transform.Find("Params").GetComponent<Text>().text = res;
            tempObj.transform.Find("Edit").GetComponent<Button>().onClick.AddListener(() => MainController._instance.uIHandler.loadEdit(elem));
            tempObj.transform.Find("Select").GetComponent<Button>().onClick.AddListener(() => MainController._instance.setViewData("weights", elem));
        }
    }
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

    public void createNewDifficulty()
    {
        Entity elem = new Difficulties(null, "New Difficulty", 0, 0, 0);
        MainController._instance.uIHandler.loadEdit(elem);
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

