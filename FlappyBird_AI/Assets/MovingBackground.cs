using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackground : MonoBehaviour {


    private new Rigidbody2D rigidbody2D;

	// Use this for initialization
	void Start () {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = new Vector2(GameControl.Instance.scrollSpeed, 0);
	}
	
	// Update is called once per frame
	void Update () {
        if (rigidbody2D.velocity.x != GameControl.Instance.scrollSpeed)
        {
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.velocity = new Vector2(GameControl.Instance.scrollSpeed, 0);
        }
		if (GameControl.Instance.gameOver)
        {
            rigidbody2D.velocity = Vector2.zero;
        }
	}
}
