using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

public class GameManager : MonoBehaviour
{
    public float TurnDelay = .1f;
    public float LevelStartDelay = 2f;
    public static GameManager Instance = null;
    public Boardmanager BoardScript;
    public int PlayerFoodPoints = 100;
    [HideInInspector] public bool PlayersTurn = true;

    private Text levelText;
    private GameObject levelImage;
    private int Level = 1;
    private IList<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        BoardScript = GetComponent<Boardmanager>();
        InitGame();
    }
    
    void OnLevelWasLoaded(int index)
    {
        Level++;
        InitGame();
    }

    void InitGame()
    {
        doingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + Level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", LevelStartDelay);

        enemies.Clear();
        BoardScript.SetupScene(Level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        levelText.text = "After " + Level + " days, you starved.";
        levelImage.SetActive(true);
        enabled = false;
    }
    void Update()
    {
        if (PlayersTurn || enemiesMoving || doingSetup) return;

        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(TurnDelay);
        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(TurnDelay);
        }
        foreach (var enemy in enemies)
        {
            enemy.MoveEnemy();
            yield return new WaitForSeconds(enemy.MoveTime);
        }
        PlayersTurn = true;
        enemiesMoving = false;
    }
}
