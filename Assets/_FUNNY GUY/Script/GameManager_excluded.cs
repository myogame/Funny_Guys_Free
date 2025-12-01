using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager_excluded: MonoBehaviour
{
    public TextMeshProUGUI qualifiedPoint;
    public Count_Qualified count_Qualified;
    public GameObject lose_Panel;
    public GameObject win_Panel;
    public Animator animatorCanvas;

    public Button loseHomeBtn;
    public Button loseRetryBtn;
    public Button victoryClaimBtn;
    public Button victoryAdsBtn;

    public TextMeshProUGUI goldReward;
    public TextMeshProUGUI metalReward;
    public TextMeshProUGUI victoryRank;

    public GameObject settingPanel;
    public Button settingBtn;
    public Button settingContinueBtn;
    public Button settingRestarBtn;
    public Button settingHomeBtn;
    public TextMeshProUGUI timeCountStart_Txt;

    public int characterCount = 5;
    public bool endGame;



    private void Awake()
    {
        loseHomeBtn.onClick.AddListener(() => LoseHome());
        loseRetryBtn.onClick.AddListener(() => LoseRetry());
        victoryClaimBtn.onClick.AddListener(() => VictoryClaim());
        victoryAdsBtn.onClick.AddListener(() => VictoryAds());
        settingBtn.onClick.AddListener(() => SettingOpen());
        settingContinueBtn.onClick.AddListener(() => SettingContinute());
        settingRestarBtn.onClick.AddListener(() => LoseRetry());
        settingHomeBtn.onClick.AddListener(() => LoseHome());
}

    private void Start()
    {
        lose_Panel.SetActive(false);
        win_Panel.SetActive(false);
        settingPanel.SetActive(false);
      
        StartCoroutine(Countdown());
    }


    // Update is called once per frame
    void Update()
    {
           

        qualifiedPoint.text = count_Qualified.qualified_Value.ToString() + "/" + characterCount.ToString();
        goldReward.text = (100 * (characterCount - count_Qualified.qualified_Value)).ToString();
        victoryRank.text = "#" + count_Qualified.qualified_Value.ToString() + "/" + characterCount.ToString();
        metalReward.text = (characterCount - count_Qualified.qualified_Value).ToString();

        if (count_Qualified.qualified_Value >= characterCount && !count_Qualified.winGame)
            endGame = true;
        if (count_Qualified.winGame)
            endGame = true;

        if (endGame) StartCoroutine(EndGame());




    }

    IEnumerator Countdown()
    {
       
        timeCountStart_Txt.text = "3";
        yield return new WaitForSeconds(1.0f);
        timeCountStart_Txt.text = "2";
        yield return new WaitForSeconds(1.0f);
        timeCountStart_Txt.text = "1";
        yield return new WaitForSeconds(1.0f);
        timeCountStart_Txt.text = "Go!";
        // start the game here
        yield return new WaitForSeconds(1.0f);
        timeCountStart_Txt.gameObject.SetActive(false);
        
        yield return null;
        
    }



    IEnumerator EndGame()
    {
        
        yield return new WaitForSeconds(3);
        if (count_Qualified.qualified_Value >= characterCount && !count_Qualified.winGame)
        {
            lose_Panel.SetActive(true);
            Time.timeScale = 0;

        }
        if (count_Qualified.winGame)
        {

            win_Panel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    void LoseHome()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main_Menu");
        
    }

    void LoseRetry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void VictoryClaim()
    {
        Time.timeScale = 1;
        PlayerPrefs.SetInt("GoldMain", PlayerPrefs.GetInt("GoldMain") + (100 * (characterCount - count_Qualified.qualified_Value)));
        PlayerPrefs.SetInt("MetalMain", PlayerPrefs.GetInt("MetalMain") + (characterCount - count_Qualified.qualified_Value));
        SceneManager.LoadScene("Main_Menu");
    }

    void VictoryAds()
    {
        Time.timeScale = 1;
        PlayerPrefs.SetInt("GoldMain", PlayerPrefs.GetInt("GoldMain") + (200 * (characterCount - count_Qualified.qualified_Value)));
        PlayerPrefs.SetInt("MetalMain", PlayerPrefs.GetInt("MetalMain") + (characterCount - count_Qualified.qualified_Value)*2);
        SceneManager.LoadScene("Main_Menu");
    }

    void SettingOpen()
    {
        settingPanel.SetActive(true);
        Time.timeScale = 0;
    }

    void SettingContinute()
    {
        settingPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
