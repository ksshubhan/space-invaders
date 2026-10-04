using System.Net.Sockets;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public sealed class MazeManager : MonoBehaviour
{
    public static MazeManager Instance { get; private set; }

    // Serialized fields for inspector references
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;
    [SerializeField] private MazeGenerator maze;

    // Prefabs and sizes for maze generation
    [SerializeField] MazeNode nodePrefab;
    [SerializeField] PowerPellet pelletPrefab;
    [SerializeField] Vector2Int mazeSize;

    private MazePlayer player;
    private MazeInvader invader;

    // Game variables
    public int score { get; private set; }
    public int lives { get; private set; }

    public float timeRemaining = 0;
    public bool timeIsRunning = false;

    public Text timeText;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // Find and initialise player and invader
        player = FindObjectOfType<MazePlayer>();
        invader = FindObjectOfType<MazeInvader>();

        // Start a new game 
        NewGame();
    }

    // Resets UI and game state for new game 
    private void NewGame()
    {
        gameOverUI.SetActive(false);
        maze.gameObject.SetActive(true);
        nodePrefab.gameObject.SetActive(true);
        pelletPrefab.gameObject.SetActive(true);
        player.GetComponent<MazePlayer>().enabled = false;
        invader.GetComponent<MazeInvader>().enabled = false;
        StartCoroutine(maze.GenerateMaze(mazeSize));
        player.gameObject.SetActive(true);
        invader.gameObject.SetActive(true);
        timeIsRunning = true;
        SetScore(0);
        SetLives(1);
    }

    // Sets and displays lives
    private void SetLives(int lives)
    {
        this.lives = Mathf.Max(lives, 0);
        livesText.text = this.lives.ToString();
    }

    // Sets and displays score
    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(4, '0');
    }

    // Handles pellet collection functionality
    public void OnPelletCollected(PowerPellet pellet)
    {
        pellet.gameObject.SetActive(false);
        SetScore(score + pellet.score);
        PointsUpdater pointsUpdater = gameObject.AddComponent<PointsUpdater>();
        pointsUpdater.UpdatePoints(pellet.score);

        // Checking if the player has collected all the pellets
        if (score == 9 * 9 * 10)
        {
            GameOver();
        }
    }

    // Handles player death 
    public void OnPlayerKilled(MazePlayer player)
    {
        SetLives(lives - 1);
        player.gameObject.SetActive(false);
        GameOver();
    }

    // Handles invader death functionality
    public void OnInvaderKilled(MazeInvader invader)
    {
        // Handles invader death
        invader.gameObject.SetActive(false);
        maze.InvaderNode = maze.allNodes[Random.Range(0, maze.allNodes.Count)];
        maze.MazeInvader.transform.position = maze.InvaderNode.transform.position;

        SetInvaderPosition();

        maze.MazeInvader.GetComponent<MazeInvader>().lastPathCalculation = 0.0f;

        RespawnInvader(invader);
    }

    // Sets the position of the invader relative to the node
    private void SetInvaderPosition()
    {
        Vector3 ztempI = maze.MazeInvader.transform.position;
        ztempI.z -= 1;
        maze.MazeInvader.transform.position = ztempI;

        Vector3 ytempI = maze.MazeInvader.transform.position;
        ytempI.y -= 1;
        maze.MazeInvader.transform.position = ytempI;
    }

    // Respawns the invader 
    public void RespawnInvader(MazeInvader invader)
    {
        invader.gameObject.SetActive(true);
    }

    private void Update()
    {
        // Checking for game over or completion conditions
        if ((score >= 810 || lives <= 0) && Input.GetKeyDown(KeyCode.Return))
        {
            ScenesManager.Instance.LoadScene(ScenesManager.Scene.GameModeScreen);
        }

        // Updating time display
        if (timeIsRunning)
        {
            if (timeRemaining >= 0)
            {
                timeRemaining += Time.deltaTime;
                DisplayTime(timeRemaining);
            }
        }
    }

    // Handles game over events 
    private void GameOver()
    {
        // Displays game over UI and stops time
        maze.gameObject.SetActive(false);
        nodePrefab.gameObject.SetActive(false);
        pelletPrefab.gameObject.SetActive(false);
        gameOverUI.SetActive(true);
        player.gameObject.SetActive(false);
        invader.gameObject.SetActive(false);
        pelletPrefab.gameObject.SetActive(false);
        timeIsRunning = false;

    }

    // Displays the time passed in minutes and seconds
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
