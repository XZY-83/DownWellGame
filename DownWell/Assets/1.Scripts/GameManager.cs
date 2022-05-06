using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    private StageManager stageManager;
    private MapManager mapManager;
    private PlayerManager playerManager;

    public GameObject playerPrefab;
    private GameObject playerCharacter;
    public Transform startPos;

    [Header("UI")]
    public Score score;
    public CoinCount coin;

    [HideInInspector] public float enemyActiveRangeOffset = 0;

    private GameObject gameoverPanel;

    //public GameObject boss;

    private void Start()
    {
        stageManager = StageManager.instance;
        mapManager = MapManager.instance;
        playerManager = PlayerManager.instance;

        // Player Init
        PlayerManager.instance.selectedCharacter.InitPlayerValues(playerPrefab);

        //playerPrefab = Instantiate(playerPrefab, startPos.position, Quaternion.identity);
        //playerPrefab.SetActive(false);

        //Camera.main.GetComponent<SmoothFollow>().InitFollowCamera(playerPrefab.transform);

        //PlayerManager.instance.player = playerPrefab;

        // Timer
        //GetComponent<Timer>().StartTimer();

        // Play BGM
        //SoundManager.instance.PlayBGMSound("Background");
        if (Comebiga.SoundManager.instance != null) Comebiga.SoundManager.instance.Play("Background");

        // Gameover Panel Init
        gameoverPanel = GameObject.Find("GameOver");
        gameoverPanel.GetComponent<Image>().enabled = true;
        gameoverPanel.SetActive(false);

        StartStage();
    }

    public void StartStage()
    {
        // Level Generation
        MapManager.instance.GenerateBeforeUpdate();

        // Player Initialization
        PlayerManager.instance.InstantiateAndInit(startPos.position);
        //playerPrefab = Instantiate(playerPrefab, startPos.position, Quaternion.identity);

        //Camera.main.GetComponent<SmoothFollow>().InitFollowCamera(playerPrefab.transform);

        //PlayerManager.instance.player = playerPrefab;

        // Timer
        GetComponent<Timer>().StartTimer();
    }

    public void ClearStage()
    {
        stageManager.NextStage();

        Camera.main.GetComponent<SmoothFollow>().StartStage();

        mapManager.Clear();

        mapManager.GenerateBeforeUpdate();

        playerManager.playerObject.transform.position = startPos.position;
    }

    public void GameOver()
    {
        // Timer
        GetComponent<Timer>().EndTimer();

        // Gameover UI Panel
        Invoke("GameOverPanel", 3f);
    }

    void GameOverPanel()
    {
        gameoverPanel.SetActive(true);
    }

    #region Deprecated
    public bool CheckTargetRange(Transform enemy)
    {
        float height = Camera.main.orthographicSize * 2;
        float width = height * (9 / 16);

        float h_tarTothis = Mathf.Abs(playerPrefab.transform.position.y - enemy.position.y);

        if (height / 2 + enemyActiveRangeOffset < enemy.position.y - playerPrefab.transform.position.y)
            Destroy(enemy.gameObject);

        if (h_tarTothis < height / 2 + enemyActiveRangeOffset)
            return true;

        return false;
    }
    #endregion


    //public void StageEnd()
    //{
    //    //SceneManager.LoadScene(0);
    //    boss.SetActive(true);
    //    Camera.main.GetComponent<SmoothFollow>().StartBossCamera();
    //    MapManager.instance.GenerateInfinity(PlayerManager.instance.player.transform, 10);
    //}

    public void homeBtn()
    {
        SceneManager.LoadScene(0);
    }
}
