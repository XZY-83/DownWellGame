using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingMgr : MonoBehaviour
{
    #region Singleton
    public static SettingMgr instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    public bool buttonOnStart = false;

    public GameObject OpeningPanel;
    public GameObject setPanel;
    public GameObject setBtn;
    public GameObject exitButton;
    public GameObject ingameSetting;


    [Header("Image")]
    public Sprite muteImg;
    public Sprite originImg;
    public Image bgmImg;
    public Image effImg;

    public bool gPaused;
    int ClickCount = 0;

    public Canvas Setting;

    [HideInInspector]
    public int bgmOff;
    [HideInInspector]
    public int effOff;

    void Start()
    {
        setPanel.SetActive(false);
        if(buttonOnStart) setBtn.SetActive(true);
        //if (SceneManager.GetActiveScene().name == "GameScene" && bgmOff == 0)
        //    SoundManager.instance.PlayBGMSound("Background");  //사운드 시작
    }

    void Update()
    {
        if(!setPanel.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                SettingBtn();
            }
        }

        if (SceneManager.GetActiveScene().name == "StartScene")
        {
            if (OpeningPanel.activeInHierarchy)
                setBtn.GetComponent<Image>().enabled = false;
            else
                setBtn.GetComponent<Image>().enabled = true;
        }
        else
            setBtn.GetComponent<Image>().enabled = true;

        if (Setting.worldCamera == null)
            Setting.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        if (bgmOff == 1)
            bgmImg.sprite = muteImg;
        else
            bgmImg.sprite = originImg;
        
        if (effOff == 1)
            effImg.sprite = muteImg;
        else
            effImg.sprite = originImg;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelBtn();
            ClickCount++;
            if (!IsInvoking("DoubleClick"))
                Invoke("DoubleClick", 1.0f);

        }
        else if (ClickCount == 2)
        {
            CancelInvoke("DoubleClick");
            exitBtn();
        }

    }

    void DoubleClick()
    {
        ClickCount = 0;
    }

    //setting
    public void SettingBtn()
    {
        gPaused = true;
        if ((SceneManager.GetActiveScene().buildIndex == 1) || UIManager.clickPlay)
        {
            exitButton.SetActive(false);
            ingameSetting.SetActive(true);
        }
        else
        {
            exitButton.SetActive(true);
            ingameSetting.SetActive(false);
        }
        setPanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void CancelBtn()
    {
        Time.timeScale = 1;
        gPaused = false;
        setPanel.SetActive(false);
    }
    public void homeBtn()
    {
        Time.timeScale = 1;
        PlayerManager.instance.Init();
        Destroy(this.gameObject);
        SceneManager.LoadScene(0);
    }

/*    private void OnApplicationQuit()
    {
        Application.CancelQuit();
#if !UNITY_EDITOR
        System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
    }*/
    
    public void exitBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void OnApplicationPause(bool pause)
    {
        if (pause) //유저가 홈이나 홀드버튼 눌렸을 때 일시정지
        {
            gPaused = true;
            SettingBtn();
        }
        else
        {
            if (gPaused) //유저가 게임으로 돌아왔을 때
            {
                gPaused = false;
            }
        }
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("Opening");
    }

    //sound
    public void muteSound(string sound)
    {
        switch (sound)
        {
            case "bgm":
                if (bgmOff == 0)
                    bgmOff = 1;
                else
                    bgmOff = 0;
                //SoundManager.instance.SetBgmVolume(bgmOff);
                var bgmvalue = (bgmOff == 1) ? true : false;
                if (Comebiga.SoundManager.instance != null) Comebiga.SoundManager.instance.Mute(Sound.SoundType.BACKGROUND, bgmvalue);

                PlayerPrefs.SetInt("BgmVolume", bgmOff);
                break;
            case "eff":
                if (effOff == 0)
                    effOff = 1;
                else
                    effOff = 0;
                //SoundManager.instance.SetEffVolume(effOff);
                var effvalue = (effOff == 1) ? true : false;
                if (Comebiga.SoundManager.instance != null) Comebiga.SoundManager.instance.Mute(Sound.SoundType.EFFECT, effvalue);

                PlayerPrefs.SetInt("EffVolume", effOff);
                break;
        }
    }
}
