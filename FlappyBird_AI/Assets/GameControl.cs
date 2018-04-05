using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {


    public static GameControl Instance;

    public float scrollSpeed = -1.5f;
    public bool gameOver = false;
    public float maxY = 4.5f;

    public bool run = true;

    public Text scoreText;
    public Text genText;
    public GameObject gameOverText;

    private int generation = 0;

    public GameObject redBirdPrefab;
    public GameObject greenBirdPrefab;
    public GameObject blueBirdPrefab;
    public GameObject orangeBirdPrefab;
    public GameObject yellowBirdPrefab;

    public int redBirdCount;
    public int greenBirdCount;
    public int blueBirdCount;
    public int orangeBirdCount;

    List<PlayerController> birds;
    List<PlayerController> redBirds;
    List<PlayerController> greenBirds;
    List<PlayerController> blueBirds;
    List<PlayerController> orangeBirds;
    PlayerController yellowBird;



    // Use this for initialization
    void Awake () {
        // Singleton Architecture only allows one Instance of this object to exists.
		if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(this);
        }
	}
	
    void Start()
    {

        if (run)
        {
            birds = new List<PlayerController>();
            redBirds = initBirds(redBirdPrefab, redBirdCount);
            greenBirds = initBirds(greenBirdPrefab, greenBirdCount);
            blueBirds = initBirds(blueBirdPrefab, blueBirdCount);
            orangeBirds = initBirds(orangeBirdPrefab, orangeBirdCount);
            yellowBird = initBirds(yellowBirdPrefab, 1)[0];
        }

    }


    private List<PlayerController> initBirds(GameObject prefab, int count)
    {
        List<PlayerController> list = new List<PlayerController>();
        for (int i = 0; i < count; i++)
        {
            PlayerController Bird = Instantiate(prefab).GetComponent<PlayerController>();
            list.Add(Bird);
            birds.Add(Bird);
        }
        return list;
    }


    // Update is called once per frame
    void Update () {
		if (gameOver && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (run)
        {

            bool active = false;
            for (int i = 0; i < birds.Count; i++)
            {
                if (!birds[i].dead)
                {
                    active = true;
                    break;
                }
            }

            int highestIndex = -1;
            int highestScore = 0;
            for (int i = 0; i < birds.Count; i++)
            {
                if (birds[i].score >= highestScore)
                {
                    highestIndex = i;
                    highestScore = birds[i].score;
                }
            }

            // speed updates
            if (highestScore == 1 && scrollSpeed == -1.5f) scrollSpeed -= .25f;
            if (highestScore == 2 && scrollSpeed == -1.75f) scrollSpeed -= .25f;
            if (highestScore == 3 && scrollSpeed == -2f) scrollSpeed -= .25f;
            if (highestScore == 4 && scrollSpeed == -2.25f) scrollSpeed -= .25f;
            if (highestScore == 5 && scrollSpeed == -2.5f) scrollSpeed -= .25f;
            if (highestScore == 6 && scrollSpeed == -2.75f) scrollSpeed -= .25f;

            scoreText.text = "Score: " + highestScore;

            if (!active)
            { // end of this generation
                double[] bestWeights = birds[highestIndex].nn.GetWeights();


                for (int i = 0; i < redBirds.Count; i++)
                {
                    // generates a completely random set of weights
                    redBirds[i].nn.InitializeWeights();
                }

                for (int i = 0; i < greenBirds.Count; i++)
                {
                    // update weights towards best weights scaled by a factor
                    greenBirds[i].nn.updateWeights(bestWeights, 80, 50);
                }

                for (int i = 0; i < blueBirds.Count; i++)
                {
                    // update weights towards best weights scaled by a factor
                    blueBirds[i].nn.updateWeights(bestWeights, 90, 20);
                }

                for (int i = 0; i < orangeBirds.Count; i++)
                {
                    // copy of yellow Bird but with small mutation chance
                    orangeBirds[i].nn.updateWeights(bestWeights, 100, 10);
                }

                // Yellow Bird will always have the best weights
                yellowBird.nn.SetWeights(bestWeights);

                //Debug.Log("Highest Score: " + highestScore + " Scored by " + Birds[highestIndex].gameObject.name);

                scoreText.text = "Score: " + highestScore;
                genText.text = "Generation: " + ++generation;
                resetGame();
            }
        }

    }

    private void resetGame()
    {
        scrollSpeed = -1.5f;
        this.gameObject.GetComponent<ColumnSpawner>().resetPool();
        for (int i = 0; i < birds.Count; i++)
        {
            birds[i].resetGame();
        }
        
    }


    public void Die()
    {
        gameOver = true;
        gameOverText.SetActive(true);
    }
}
