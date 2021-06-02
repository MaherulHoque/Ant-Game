using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PathCreation;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform pathParent;
    private PathCreator[] paths;

    [SerializeField]
    private GameObject[] antPrefabs;

    [SerializeField]
    private GameObject spiderPrefab;

    [SerializeField]
    private Transform spawnPos;

    [SerializeField]
    private float minSpawnRate = 0.5f;
    [SerializeField]
    private float maxSpawnRate = 5f;
    [SerializeField]
    private float reduceSpawnDelay = 0.001f;

    private Follower newFollower;

    public bool isGameOver;

    private int score;

    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text missedAntText;
    [SerializeField]
    private TextMeshProUGUI highScoreText;
    [SerializeField]
    private TextMeshProUGUI yourScoreText;

    [SerializeField]
    private int maxMissedAnt;
    private int outOfPlayGround;

    private int savedHighScore;

    [SerializeField]
    private GameObject gameOverMenu;

    [SerializeField]
    private GameObject confirmMenu;

    // Start is called before the first frame update
    void Start()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        savedHighScore = PlayerPrefs.GetInt("HighScore");

        Debug.Log("High Score: " + savedHighScore);

        paths = new PathCreator[pathParent.transform.childCount];

        for(int i = 0; i < pathParent.transform.childCount; i++)
        {
            paths[i] = pathParent.transform.GetChild(i).GetComponent<PathCreator>();
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;

        if(outOfPlayGround >= maxMissedAnt)
        {
            // Game Over
            isGameOver = true;
            SaveHighScore(score);
            gameOverMenu.SetActive(true);

            //Sound Manager
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            confirmMenu.SetActive(true);
            Time.timeScale = 0;
        }
        
    }

    IEnumerator SpawnAnt()
    {
        
        int randomPathIndex = Random.Range(0, 3);
        int randomAntSpawn = Random.Range(1, 4);
        int randomAnt = Random.Range(0, antPrefabs.Length);

        for(int i = 0; i < randomAntSpawn; i++)
        {
            GameObject newAnt = Instantiate(antPrefabs[randomAnt], spawnPos.position, Quaternion.identity) as GameObject;
            newAnt.name = "Ant";
            newFollower = newAnt.GetComponent<Follower>();
            newFollower.GetPath(paths[randomPathIndex]);

            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(maxSpawnRate);

        if(maxSpawnRate > minSpawnRate)
            maxSpawnRate -= reduceSpawnDelay;
        

        if (!isGameOver)
            StartCoroutine(SpawnAnt());
        
    }

    IEnumerator SpawnSpider()
    {
        yield return new WaitForSeconds(7f);

        int randomPathIndex = Random.Range(0, 3);
        GameObject newSpider = Instantiate(spiderPrefab, spawnPos.position, Quaternion.identity) as GameObject;
        newSpider.name = "Spider";
        newFollower = newSpider.GetComponent<Follower>();
        newFollower.GetPath(paths[randomPathIndex]);

        if (!isGameOver)
            StartCoroutine(SpawnSpider());
    }

    public void AddScore(int scoreIncrement)
    {
        if (isGameOver) return;

        score += scoreIncrement;
        scoreText.text = "Score: " + score;
    }

    public void CountingOutside(int count)
    {
        if (isGameOver) return;

        outOfPlayGround += count;
        missedAntText.text = "Missed: " + outOfPlayGround + " / " + maxMissedAnt;
    }

    public void SaveHighScore(int yourScore)
    {
        yourScoreText.text = "Your Score: " + yourScore;
        highScoreText.text = "High Score: " + savedHighScore;

        if (savedHighScore < yourScore)
        {
            highScoreText.text = "High Score: " + yourScore;
            PlayerPrefs.SetInt("HighScore", yourScore);
        }
            
    }

    public void PlayGame()
    {
        
        score = 0;
        scoreText.text = "Score: 0";
        outOfPlayGround = 0;
        missedAntText.text = "Missed: " + outOfPlayGround + " / " + maxMissedAnt;
        isGameOver = false;

        // Spawn Ant
        StartCoroutine(SpawnAnt());

        // Spawn Spider
        StartCoroutine(SpawnSpider());
    }

    public void CancelButton()
    {
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
