using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BirdController : MonoBehaviour,IController {

    public int score = 0;

    private float flapForce = 4f;
    public bool dead = false;
    private new Rigidbody2D rigidbody2D;
    private Animator anim;
    private float maxY;

    public DeepNeuralNetwork nn;

    private double pipeCenter;
    private double playerPos;
    //private double distanceToPipe;
    private bool aiActive = false;

    public Vector2 firstPipePos = new Vector2(0,0);
    private Vector2 spawnPos;




    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        nn = new DeepNeuralNetwork(2, new int[] { 6 }, 2);
        maxY = GameController._instance.bgSize.y - 2f;
        spawnPos = GameController._instance.spawnPos;
    }

    void Update () {
        if (!aiActive || dead) return;

        pipeCenter = firstPipePos.y;
        playerPos = transform.position.y;
        //distanceToPipe = (firstPipePos.x - transform.position.x - spawnPos.x) / 10;
        double[] output = nn.ComputeOutputs(new double[] { playerPos, pipeCenter });
        double maxValue = output.Max();
        int maxIndex = output.ToList().IndexOf(maxValue);
        autoFlap(maxIndex);
    }

    public IController updateController(IControllerValue c)
    {
        this.aiActive = ((BirdControllerValue)c).aiActive;
        return this;
    }

    void OnCollisionEnter2D()
    {
        die();
    }

    private void die()
    {
        dead = true;
        rigidbody2D.velocity = Vector2.zero;
        transform.rotation = new Quaternion(180,0,0, 0);
        anim.SetTrigger("Die");
    }
    public void resetGame()
    {
        score = 0;
        dead = false;
        anim.Rebind();
        resetPosition();
    }



    public void flap()
    {
        if (transform.position.y > maxY) return;
        if (transform.position.y <= 0) // This should never happen 
        {
            die(); 
            return;
        }
        anim.SetTrigger("Flap");
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.velocity = new Vector2(0, flapForce);
    }

    private void autoFlap(int index)
    {
        if (index == 0)
        {
            flap();
        }
    }

    private void resetPosition()
    {
        transform.position = spawnPos;
        rigidbody2D.velocity = Vector2.zero;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
}
