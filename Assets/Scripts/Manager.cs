using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum gameStatus { 
    next, play, gameover, win 
};

public class Manager : Loader<Manager>
{
    [SerializeField]
    int totalWave;
    [SerializeField]
    Text totalMoneyLabel;
    [SerializeField]
    Text currentWave;
    [SerializeField]
    Text playBtnLabel;
    [SerializeField]
    Text totalEscapedLabel;
    [SerializeField]
    Button playBtn;
    [SerializeField]//задавать параметр в юнити
    GameObject spawnPoint;//сюда поместим эл-т spawn
    [SerializeField]
    Enemy[] enemies;//сюда поместим всех противников
    [SerializeField]
    int totalEnemies = 3;//сколько всего должно появится врагов
    [SerializeField]
    int enemiesPerSpawn;//кол-во появляемых противников

    int waveNumber = 0;
    int totalMoney = 10;
    int totalEscaped = 0;
    int roundEscaped = 0;
    int totalKilled = 0;
    int whichEnemiesToSpawn = 0;
    int enemiesToSpawn = 0;
    gameStatus currentState = gameStatus.play;
    AudioSource audioSource;

    public List<Enemy> EnemyList = new List<Enemy>();

    const float spawnDelay = 1.5f;

    public int TotalMoney
    {
        get
        {
            return totalMoney;
        }
        set
        {
            totalMoney = value;
            totalMoneyLabel.text = totalMoney.ToString();
        }
    }

    public AudioSource AudioSource
    {
        get
        {
            return audioSource;
        }
    }
    public int TotalEscaped
    {
        get
        {
            return totalEscaped;
        }
        set
        {
            totalEscaped = value;
        }
    }
    public int RoundEscaped
    {
        get
        {
            return roundEscaped;
        }
        set
        {
            roundEscaped = value;
        }
    }
    public int TotalKilled
    {
        get
        {
            return totalKilled;
        }
        set
        {
            totalKilled = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playBtn.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        ShowMenu();
    }

    private void Update()
    {
        HandleEscape();
    }

    IEnumerator Spawn()
    {
        if(enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies){
            for(int i = 0; i < enemiesPerSpawn; ++i)
            {
                if (EnemyList.Count < totalEnemies){
                    Enemy newEnemy = Instantiate(enemies[Random.Range(0,enemiesToSpawn)] as Enemy);
                    newEnemy.transform.position = spawnPoint.transform.position;
                }
            }
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }
    public void UnregisterEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }
    public void DestroyEnemies()
    {
        foreach(Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }
        EnemyList.Clear();
    }

    public void AddMoney(int amount)
    {
        TotalMoney += amount;
    }

    public void SubtractMoney( int amount)
    {
        TotalMoney -= amount;
    }

    public void IsWaveOver()
    {
        totalEscapedLabel.text = "Прошло: " + totalEscaped + "/10";
        if (roundEscaped+totalKilled == totalEnemies)
        {
            if (waveNumber<=enemies.Length)
            {
                enemiesToSpawn = waveNumber;
            }
            SetCurrentGameState();
            ShowMenu();
        }
    }

    public void SetCurrentGameState()
    {
        if (totalEscaped >= 10)
        {
            currentState = gameStatus.gameover;
        }
        else if (waveNumber ==0 && (roundEscaped+totalKilled) == 0) {
            currentState = gameStatus.play;
        }
        else if (waveNumber>=totalWave)
        {
            currentState = gameStatus.win;
        }
        else
        {
            currentState = gameStatus.next;
        }
    }

    public void PlayButtonPressed()
    {
        switch (currentState)
        {
            case gameStatus.next:
                waveNumber += 1;
                totalEnemies += waveNumber;
                break;
            default:
                totalEnemies = 3;
                TotalEscaped = 0;
                TotalMoney = 10;

                TowerManager.Instance.DestroyAllTowers();
                TowerManager.Instance.RenameTagBuildSite();
                totalMoneyLabel.text = TotalMoney.ToString();
                totalEscapedLabel.text = "Прошло: " + totalEscaped + "/10";
                audioSource.PlayOneShot(SoundManager.Instance.Newgame);
                break;
        }
        DestroyEnemies();
        totalKilled = 0;
        RoundEscaped = 0;
        currentWave.text = "Волна: " + (waveNumber + 1);
        StartCoroutine(Spawn());
        playBtn.gameObject.SetActive(false);
    }

    public void ShowMenu()
    {
        switch (currentState)
        {
            case gameStatus.gameover:
                playBtnLabel.text = "Сыграть ещё раз!?";
                AudioSource.PlayOneShot(SoundManager.Instance.Gameover);
                break;
            case gameStatus.next:
                playBtnLabel.text = "Следующая волна";break;
            case gameStatus.play:
                playBtnLabel.text = "Начать игру";break;
            case gameStatus.win:
                playBtnLabel.text = "Новая игра";break;
        }
        playBtn.gameObject.SetActive(true);
    }

    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TowerManager.Instance.DisableDrag();
            TowerManager.Instance.TowerBtnPressed = null;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void AboutScene()
    {
        SceneManager.LoadScene(1);
    }
    public void MainScene()
    {
        SceneManager.LoadScene(0);
    }

}
