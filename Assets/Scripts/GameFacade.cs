using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public sealed class GameFacade : MonoBehaviour
{
    private Player player;
    private Invaders invaders;
    private MysteryShip mysteryShip;
    private Bunker[] bunkers;
    public Text scoreText;
    public Text livesText;
    public GameObject gameOverUI;
    public int score { get; private set; }
    public int lives { get; private set; }
    public void AwakeGame()
    {
        this.player = FindObjectOfType<Player>();
        this.invaders = FindObjectOfType<Invaders>();
        this.mysteryShip = FindObjectOfType<MysteryShip>();
        this.bunkers = FindObjectsOfType<Bunker>();
    }

    public void StartGame()
    {
        this.player.killed += OnPlayerKilled;
        this.mysteryShip.killed += OnMysteryShipKilled;
        this.invaders.killed += OnInvaderKilled;
        NewGame();
    }

    public void UpdateGame()
    {
        if (this.lives <= 0 && Input.GetKeyDown(KeyCode.Return))
            NewGame();
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);
    }
 



    private void NewGame()
    {
        if (LevelController.TextLevel == "Hard")
        {
            this.mysteryShip.SetWeapon(AssetDatabase.LoadAssetAtPath<CircleGun>($"Assets/Prefabs/CircleGun.prefab"));
        }
        else
        {
            this.mysteryShip.SetWeapon(AssetDatabase.LoadAssetAtPath<RocketGun>($"Assets/Prefabs/RocketGun.prefab"));
        }
        this.gameOverUI.SetActive(false);
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        invaders.Detach(this.player);
        invaders.Detach(this.mysteryShip);
        this.invaders.ResetAll();
        this.invaders.gameObject.SetActive(true);
        this.mysteryShip.ResetAll();
        this.mysteryShip.gameObject.SetActive(true);
        for (int i = 0; i < this.bunkers.Length; i++)
            this.bunkers[i].ResetBunker();
        Respawn();
        invaders.Attach(this.player);
        invaders.Attach(this.mysteryShip);
    }

    private void Respawn()
    {
        Vector3 position = this.player.transform.position;
        position.x = 0.0f;
        this.player.transform.position = position;
        this.player.gameObject.SetActive(true);
    }

    private void GameOver()
    {
        this.gameOverUI.SetActive(true);
        this.invaders.gameObject.SetActive(false);
        for (int i = 0; i < this.bunkers.Length; i++)
            this.bunkers[i].gameObject.SetActive(false);
        this.mysteryShip.StopAttacking();
        this.mysteryShip.gameObject.SetActive(false);
    }

    private void SetScore(int score)
    {
        this.score = score;
        this.scoreText.text = this.score.ToString().PadLeft(4, '0');
    }

    private void SetLives(int lives)
    {
        this.lives = Mathf.Max(lives, 0);
        this.livesText.text = this.lives.ToString();
    }

    private void OnPlayerKilled()
    {
        SetLives(this.lives - 1);
        this.player.gameObject.SetActive(false);
        if (this.lives > 0) Respawn();
        else GameOver();
    }

    private void OnInvaderKilled(Invader invader)
    {
        SetScore(this.score + invader.score);
        if (this.invaders.AmountKilled == this.invaders.TotalAmount)
            NewRound();
    }

    private void OnMysteryShipKilled(MysteryShip mysteryShip)
    {
        SetScore(this.score + mysteryShip.score);
    }
}
