using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnSpawner : MonoBehaviour {

    public float spawnDelay = 4f;
    private float lastSpawnTime = 99999f;


    public int colAmount = 5;
    public float colYMin = -3f;
    public float colYMax = 3f;

    public GameObject colPrefab;
    public float spawnXPos = 20.48f;

    private GameObject[] colPool;
    private int currentCol = 0;

    public static GameObject firstPipe;
    private int firstPipeIndex = 0;

    private Vector2 spawnPos = new Vector2(-25,-25);

	// Use this for initialization
	void Start () {
        colPool = new GameObject[colAmount];
        createPool();
    }

    public void createPool()
    {
        for (int i = 0; i < colAmount; i++)
        {
            colPool[i] = (GameObject)Instantiate(colPrefab, spawnPos, Quaternion.identity);
        }
        firstPipe = colPool[currentCol];
        firstPipeIndex = 0;
    }

    public void resetPool()
    {
        currentCol = 0;
        lastSpawnTime = 99999f;
        for (int i = 0; i < colAmount; i++)
        {
            colPool[i].transform.position = spawnPos;
        }
        firstPipe = colPool[currentCol];
        firstPipeIndex = 0;
    }
	// Update is called once per frame
	void Update () {
        lastSpawnTime += Time.deltaTime;

        if (!GameControl.Instance.gameOver && lastSpawnTime >= spawnDelay)
        {
            lastSpawnTime = 0;
             

            float spawnYPos = Random.Range(colYMin, colYMax);
            //float spawnYPos = 1;

            colPool[currentCol].transform.position = new Vector2(spawnXPos, spawnYPos);
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
}
