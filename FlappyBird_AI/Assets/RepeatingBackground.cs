using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBackground : MonoBehaviour {


    private BoxCollider2D groundCollider;
    private float width;

	// Use this for initialization
	void Start () {
        groundCollider = GetComponentInChildren<BoxCollider2D>();
        width = groundCollider.size.x * transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {

        if (transform.position.x < -width)
        {
            Vector2 offset = new Vector2(width * 2,0);
            transform.position = (Vector2)transform.position + offset;
        }
		
	}
}
