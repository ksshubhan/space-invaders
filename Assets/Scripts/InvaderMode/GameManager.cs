using UnityEngine;
using UnityEngine.UI;

// Responsible for game mechanics, text and display
public sealed class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;

    private Player player;
    private Invaders invaders;
    private MysteryShip mysteryShip;
    private Bunker[] bunkers;

    // Properties for score and lives
    public int score { get; private set; }
    public int lives { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            // Destroys duplicate instance if it already exists
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        invaders = FindObjectOfType<Invaders>();
        mysteryShip = FindObjectOfType<MysteryShip>();
        bunkers = FindObjectsOfType<Bunker>();

        NewGame();
    }

    private void Update()
    {
        // Check for game over condition and restart the game if 'Enter' is pressed
        if (lives <= 0 && Input.GetKeyDown(KeyCode.Return))
        {
            ScenesManager.Instance.LoadScene(ScenesManager.Scene.GameModeScreen);
            NewGame();
        }
    }

    // Setting up a new game
    private void NewGame()
    {
        gameOverUI = FindObjectOfType<GameOver>().gameObject;
        scoreText = FindObjectOfType<Score>().GetComponent<Text>();
        livesText = FindObjectOfType<Lives>().GetComponent<Text>();

        gameOverUI.SetActive(false);

        SetScore(0);
        SetLives(3);
        NewRound();
    }

    // Setting up a new round
    private void NewRound()
    {
        invaders.ResetInvaders();
        invaders.gameObject.SetActive(true);

        // Reset all bunkers
        for (int i = 0; i < bunkers.Length; i++)
        {
            bunkers[i].ResetBunker();
        }

        Respawn();
    }

    // Once a round is over, the player will return to their initial starting position
    private void Respawn()
    {
        Vector3 position = player.transform.position;
        position.x = 0f;
        player.transform.position = position;
        player.gameObject.SetActive(true);
    }

    // 'Game over' screen
    private void GameOver()
    {
        gameOverUI.SetActive(true);
        invaders.gameObject.SetActive(false);
    }

    // 'Score' text 
    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(4, '0');
    }

    // 'Lives' Text
    private void SetLives(int lives)
    {
        this.lives = Mathf.Max(lives, 0);
        livesText.text = this.lives.ToString();
    }

    // Handling player death
    public void OnPlayerKilled(Player player)
    {
        // When the player is killed, they lose a life
        SetLives(lives - 1);

        player.gameObject.SetActive(false);

        // Depending on the number of lives left, a new round is started or the game is over
        if (lives > 0)
        {
            Invoke(nameof(NewRound), 1f);
        }
        else
        {
            GameOver();
        }
    }

    // Handling invader death
    public void OnInvaderKilled(Invader invader)
    {
        invader.gameObject.SetActive(false);

        // Update score and invoke PointsUpdater to update points on the server
        SetScore(score + invader.score);
        PointsUpdater pointsUpdater = gameObject.AddComponent<PointsUpdater>();
        pointsUpdater.UpdatePoints(invader.score);

        // If no more invaders are alive, start a new round
        if (invaders.GetAliveCount() == 0)
        {
            NewRound();
        }
    }

    // Handling MysteryShip death
    public void OnMysteryShipKilled(MysteryShip mysteryShip)
    {
        // Gain extra points when the mystery ship is killed
        SetScore(score + mysteryShip.score);
    }

    // Setting player boundaries
    public void OnBoundaryReached()
    {
        if (invaders.gameObject.activeSelf)
        {
            invaders.gameObject.SetActive(false);

            OnPlayerKilled(player);
        }
    }
}
