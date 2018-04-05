using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour,IController
{
    [SerializeField]
    private GameObject backgroundParent;
    [SerializeField]
    private GameObject firstBg;
    [SerializeField]
    private GameObject secondBg;

    private Vector2 velocity;

    private Vector2 repeatV2;

    public void setVelocity(Vector2 v)
    {
        velocity = v;
        backgroundParent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        backgroundParent.GetComponent<Rigidbody2D>().velocity = velocity;
    }

    void Start()
    {
        repeatV2 = new Vector2(GameController._instance.bgSize.x, 5);
    }
    void Update()
    {
        if (firstBg.transform.position.x <= -repeatV2.x)
        {

            firstBg.transform.position = repeatV2;
            secondBg.transform.position = new Vector2(0, repeatV2.y);
        }
        if (secondBg.transform.position.x <= -repeatV2.x)
        {
            firstBg.transform.position = new Vector2(0, repeatV2.y);
            secondBg.transform.position = repeatV2;
        }

    }


    public void reset()
    {
        backgroundParent.transform.position = new Vector2(0, 0);
        firstBg.transform.position = new Vector2(0, repeatV2.y);
        secondBg.transform.position = repeatV2;
    }

    public IController updateController(IControllerValue c)
    {
        throw new System.NotImplementedException();
    }
}
