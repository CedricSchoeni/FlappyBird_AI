using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : ACController, IController {

    private MainController main; // instance of MainController
    public static GameController _instance; // singleton instance of this controller

    public bool runAi; // this boolean determines wheter the ai plays or not
    public bool active = false; // the game starts as soon as this value is set to true
    private int generation = 0;

    private IView UIview; // UIView updates the ui
    private PipeController pManager; // places the pipes
    private BackgroundController bgManager; // manages the scrolling background

    private Entity weights;

    // SerializeField allows these private fields to be shown in the editor
    // this way they can be assigned there

    [SerializeField]
    private GameObject redBirdPrefab;
    [SerializeField]
    private GameObject greenBirdPrefab;
    [SerializeField]
    private GameObject blueBirdPrefab;
    [SerializeField]
    private GameObject orangeBirdPrefab;
    [SerializeField]
    private GameObject yellowBirdPrefab;

    // amount of birds of that color
    private int redBirdCount = 5;
    private int greenBirdCount = 5;
    private int blueBirdCount = 5;
    private int orangeBirdCount = 5;

    // list containing all birds in the game
    private List<BirdController> birds;
    private List<BirdController> redBirds;
    private List<BirdController> greenBirds;
    private List<BirdController> blueBirds;
    private List<BirdController> orangeBirds;
    private BirdController yellowBird;


    private Vector2 bgVelocity; // scroll speed
    private int speedPointer = 0;
    private float[] speedIncrements = new float[] { -1.5f, -1.75f, -2f, -2.25f, -2.5f, -2.75f, -3f}; // whenever the score reaches a need scoreincrement the background will move faster
    private int scoreIncrement;
    private int currentIncrement = 0;
    public Vector2 bgSize = new Vector2(19.2f, 10f);
    public Vector2 spawnPos = new Vector2(-5, 5);

    void Start()
    {
        // Singleton - only one instance of this object can exists at a time
        if (_instance == null)
        {
            _instance = this;
        } else
        {
            Destroy(gameObject);
            return;
        }

        // at this point the class has already been updated with the correct values from the MainController 
        // this means we can initialize the game
        currentIncrement = scoreIncrement;
        bgVelocity = new Vector2(speedIncrements[speedPointer++], 0);
        setVelocity(bgVelocity);
        if (!runAi) UIview.activate("generation", false);
        createPlayers();
        startGame();
    }


    private void startGame()
    {
        pManager.active = true;
        active = true;
    }

    private void setVelocity(Vector2 v)
    {
        pManager.setVelocity(v);
        bgManager.setVelocity(v);
    }

    private void createPlayers()
    {
        Debug.Log(weights);
        birds = new List<BirdController>();
        if (runAi)
        {
            redBirds = initBirds(redBirdPrefab, redBirdCount);
            greenBirds = initBirds(greenBirdPrefab, greenBirdCount);
            blueBirds = initBirds(blueBirdPrefab, blueBirdCount);
            orangeBirds = initBirds(orangeBirdPrefab, orangeBirdCount);
            if (weights != null)
            {
                yellowBird = initBirds(yellowBirdPrefab, 1, true, true)[0];
            } else
                yellowBird = initBirds(yellowBirdPrefab, 1)[0];

        }
        else
        {
            yellowBird = initBirds(yellowBirdPrefab, 1, false)[0];
        }
    }
    
    private double[] setWeights()
    {
        string[] arr = ((Weights)weights).weights.Split('|');
        double[] res = new double[arr.Length];
        for (int i = 0; i < res.Length; i++)
        {
            res[0] = double.Parse(arr[0]);
        }
        return res;
    }

    private List<BirdController> initBirds(GameObject prefab, int count, bool runAi = true, bool presetWeights = false)
    {
        List<BirdController> list = new List<BirdController>();
        for (int i = 0; i < count; i++)
        {
            BirdController bird;
            if (!presetWeights)
                bird = (BirdController)Instantiate(prefab, spawnPos, Quaternion.identity).GetComponent<BirdController>().updateController(main.factory.getBirdValues(runAi));
            else
            {
                double[] weights = setWeights();
                bird = (BirdController)Instantiate(prefab, spawnPos, Quaternion.identity).GetComponent<BirdController>().updateController(main.factory.getBirdValues(runAi, weights));
            }
                
            list.Add(bird);
            birds.Add(bird); 
        }
        return list;
    }




    void Update()
    {
        if (!active) return;

        // since the BirdController and PipeController have no access to either the other or parentController 
        // we set the values from the parent controller 
        foreach (BirdController bird in birds)
        {
            bird.firstPipePos = pManager.firstPipe.transform.position;
        }
        // check if birds are still alive
        bool alive = false;
        for (int i = 0; i < birds.Count; i++)
        {
            if (!birds[i].dead)
            {
                alive = true;
                break;
            }
        }



        // get highest score
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
        // update score text
        UIview.setText("score", "Score: " + highestScore);

        // update speed if a new scoreincrement is reached
        if (highestScore >= currentIncrement && speedPointer != speedIncrements.Length)
        {
            currentIncrement += scoreIncrement;
            bgVelocity = new Vector2(speedIncrements[speedPointer++], 0);
            setVelocity(bgVelocity);
            // update spawndelay from pManager 
            pManager.spawnDelay -= 0.3333333333f;
        }

        if (alive) return;        
        UIview.setText("last", "Last: " + highestScore);

        if (!runAi)
        {
            resetGame();
            return;
        }

        // can be skipped for singleplayer
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

        UIview.setText("generation", "Generation: " + ++generation);
        resetGame();
        
        
    }

    private void resetGame()
    {
        speedPointer = 0;
        bgVelocity = new Vector2(speedIncrements[speedPointer++], 0);
        setVelocity(bgVelocity);
        currentIncrement = scoreIncrement;
        UIview.setText("score", "Score: " + 0);
        //scrollSpeed = -1.5f;
        pManager.resetPool();
        bgManager.reset();
        for (int i = 0; i < birds.Count; i++)
        {
            birds[i].resetGame();
        }
    }

    public void saveBestNetwork()
    {
        // get highest score
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
        double[] bestWeights = birds[highestIndex].nn.GetWeights();
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        foreach (double value in bestWeights)
        {
            builder.Append(value);
            builder.Append('|');
        }

        string res = builder.ToString();
        Entity weights = new Weights(null, "autogen_" + (int)System.DateTime.Now.Ticks, res);
        weights.update();
        main.loadScene(0);
    }




    public IController updateController(IControllerValue c)
    {
        pManager = (PipeController)gameObject.GetComponent<PipeController>().updateController(((GameControllerValue)c).pControllerValues);
        bgManager = (BackgroundController)gameObject.GetComponent<BackgroundController>();

        this.main = ((GameControllerValue)c).main;
        this.UIview = ((GameControllerValue)c).view;
        this.weights = ((GameControllerValue)c).weights;

        this.runAi = ((GameControllerValue)c).aiActive;
        this.scoreIncrement = ((GameControllerValue)c).scoreIncrement;
        return this;
    }

    public override void handleKeyDown(KeyCode key)
    {
        ((UIGameView)UIview).switchMenu();
    }

    public override void handleLClick()
    {
        if (!runAi)
            yellowBird.flap();
    }
}
