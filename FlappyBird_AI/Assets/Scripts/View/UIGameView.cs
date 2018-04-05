using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameView : ACView, IView
{

    [SerializeField]
    private Text score;
    [SerializeField]
    private Text generation;
    [SerializeField]
    private Text last;


    public void activate(string name, bool active)
    {
        switch (name)
        {
            case "score":
                score.gameObject.SetActive(active);
                break;
            case "generation":
                generation.gameObject.SetActive(active);
                break;
            case "last":
                last.gameObject.SetActive(active);
                break;
            default:
                break;
        }
    }

    public void setText(string name, string text, bool concat = false)
    {
        string s;
        switch (name)
        {
            case "score":
                s = (concat) ? score.text + text : text;
                score.text = s;
                break;
            case "generation":
                s = (concat) ? generation.text + text : text;
                generation.text = s;
                break;
            case "last":
                s = (concat) ? last.text + text : text;
                last.text = s;
                break;
            default:
                break;
        }
    }

    void Start()
    {

    }
}
