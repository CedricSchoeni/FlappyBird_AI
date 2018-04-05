using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UIEditView : ACView, IView
{
    [SerializeField]
    private GameObject difficultyParent;
    [SerializeField]
    private GameObject networkParent;

    public void prepareDifficulties(Difficulties d, bool create = false)
    {
        difficultyParent.SetActive(true);

        difficultyParent.transform.Find("Edit").gameObject.SetActive(!create);
        difficultyParent.transform.Find("Save").gameObject.SetActive(create);

        if (create) return;

        // fill input fields with values from model
        difficultyParent.transform.Find("Name").GetComponentInChildren<Text>().text = d.name;    
        difficultyParent.transform.Find("yMin").GetComponentInChildren<Text>().text = d.yMin.ToString();    
        difficultyParent.transform.Find("yMax").GetComponentInChildren<Text>().text = d.yMax.ToString();    
        difficultyParent.transform.Find("ScoreIncrement").GetComponentInChildren<Text>().text = d.scorePoint.ToString();
        difficultyParent.transform.Find("Edit").GetComponent<Button>().onClick.AddListener(() => updateDifficulties(d));
    }

    public void updateDifficulties(Difficulties d)
    {   
        string name = difficultyParent.transform.Find("Name").GetComponent<InputField>().text;
        string yMin = difficultyParent.transform.Find("yMin").GetComponent<InputField>().text;
        string yMax = difficultyParent.transform.Find("yMax").GetComponent<InputField>().text;
        string scorePoint = difficultyParent.transform.Find("ScoreIncrement").GetComponent<InputField>().text;

        d.name = (name != "") ? name : d.name;
        d.yMin = (yMin.Trim() != "") ? float.Parse(yMin) : d.yMin;
        d.yMax = (yMax.Trim() != "") ? float.Parse(yMax) : d.yMax;
        d.scorePoint = (scorePoint.Trim() != "") ? int.Parse(scorePoint) : d.scorePoint;

        Debug.Log(d.name);
        Debug.Log(d.yMin);
        Debug.Log(d.yMax);
        Debug.Log(d.scorePoint);

        d.update();
    }

    public void prepareNetwork(bool create = false)
    {
        networkParent.SetActive(true);
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
