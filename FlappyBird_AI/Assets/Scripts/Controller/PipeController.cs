using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeController : MonoBehaviour,IController {


    public float spawnDelay = 4f;
    public float normalSpawnDelay;
    private float lastSpawnTime = 99999f;

    public int colAmount = 6;

    public float colYMin;
    public float colYMax;
    public bool active = false;

    [SerializeField]
    private GameObject colPrefab;
    [SerializeField]
    private GameObject parentObject;

    private float spawnXPos;

    private GameObject[] colPool;
    private int currentCol = 0;

    [System.NonSerialized]
    public GameObject firstPipe;
    private int firstPipeIndex = 0;

    private Vector2 spawnPos = new Vector2(-25, -25);
    private Vector2 velocity;

    // Use this for initialization
    void Awake()
    {
        colPool = new GameObject[colAmount];
        normalSpawnDelay = spawnDelay;
        createPool();
    }

    void Start()
    {
        spawnXPos = GameController._instance.bgSize.x;
    }

    public void setVelocity(Vector2 v)
    {
        velocity = v;
        for (int i = 0; i < colAmount; i++)
        {
            colPool[i].GetComponent<Rigidbody2D>().velocity = velocity;

        }
    }

    public void createPool()
    {
        for (int i = 0; i < colAmount; i++)
        {
            colPool[i] = (GameObject)Instantiate(colPrefab, spawnPos, Quaternion.identity, parentObject.transform);
        }
        firstPipe = colPool[currentCol];
        firstPipeIndex = 0;
    }

    public void resetPool()
    {
        currentCol = 0;
        spawnDelay = normalSpawnDelay;
        lastSpawnTime = 99999f;
        for (int i = 0; i < colAmount; i++)
        {
            colPool[i].transform.position = spawnPos;
        }
        firstPipe = colPool[currentCol];
        firstPipeIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!active) return;

        lastSpawnTime += Time.deltaTime;

        if (lastSpawnTime >= spawnDelay)
        {
            lastSpawnTime = 0;


            float spawnYPos = Random.Range(colYMin, colYMax);
            //float spawnYPos = 1;

            colPool[currentCol].transform.position = new Vector2(spawnXPos, spawnYPos) + new Vector2(0, 5f);
            currentCol++;
            if (currentCol >= colAmount) currentCol = 0;
        }

        if (firstPipe.transform.position.x <= -6)
        {
            firstPipe = colPool[firstPipeIndex];
            firstPipeIndex++;
            if (firstPipeIndex >= colAmount) firstPipeIndex = 0;
        }
    }

    public IController updateController(IControllerValue c)
    {
        this.colYMin = ((PipeControllerValue)c).colYMin;
        this.colYMax= ((PipeControllerValue)c).colYMax;
        return this;
    }
}
