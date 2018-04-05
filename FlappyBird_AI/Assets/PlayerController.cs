using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    public float flapForce = 20f;
    public bool dead = false;
    public int score = 0;
    private new Rigidbody2D rigidbody2D;

    public DeepNeuralNetwork nn;

    public double pipeCenter;
    public double playerPos;
    public double distanceToPipe;
    public bool aiActive = true;

    // Use this for initialization
    void Start () {
        rigidbody2D = GetComponent<Rigidbody2D>();
        nn = new DeepNeuralNetwork(2, new int[] { 6 }, 2);
    }
	

	void Update () {
        if (dead) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            flap();
        }

        if (aiActive)
        {
            pipeCenter = ColumnSpawner.firstPipe.transform.position.y;
            playerPos = transform.position.y;
            distanceToPipe = (ColumnSpawner.firstPipe.transform.position.x - transform.position.x + 1) / 10;
            double[] output = nn.ComputeOutputs(new double[] { playerPos, pipeCenter, distanceToPipe });
            double maxValue = output.Max();
            int maxIndex = output.ToList().IndexOf(maxValue);
            autoFlap(maxIndex);
        }

        
    }

    void OnCollisionEnter2D()
    {
        dead = true;
        rigidbody2D.velocity = Vector2.zero;
    }

    public void resetGame()
    {
        score = 0;
        dead = false;
        resetPosition();
    }

    private void resetPosition()
    {
        transform.position = new Vector3(-5, 5, 0);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    public void flap()
    {
        if (transform.position.y > GameControl.Instance.maxY) return;
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.velocity = new Vector2(0, flapForce);
    }

    public void autoFlap(int index)
    {
        if (index == 0)
        {
            flap();
        }
    }
}
