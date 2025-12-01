using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using EasyMobile;

public class GameManager : MonoBehaviour
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

    public AudioSource main_Sound;
    public SoundManager soundManager;

   

    int stop;

    public GameObject dropdownShadow;

    string checkLoseRetry;
   

    

    private void Awake()
    {
       
        loseHomeBtn.onClick.AddListener(() => LoseHome());
        loseRetryBtn.onClick.AddListener(() => LoseRetry());
        victoryClaimBtn.onClick.AddListener(() => VictoryClaim());
        victoryAdsBtn.onClick.AddListener(() => ShowAds_Reward());
        settingBtn.onClick.AddListener(() => SettingOpen());
        settingContinueBtn.onClick.AddListener(() => SettingContinute());
        settingRestarBtn.onClick.AddListener(() => LoseRetry());
        settingHomeBtn.onClick.AddListener(() => LoseHome());
        main_Sound = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        soundManager = GameObject.Find("AudioFX").GetComponent<SoundManager>();
       
    }

    private void Start()
    {
        lose_Panel.SetActive(false);
        win_Panel.SetActive(false);
        settingPanel.SetActive(false);
        victoryAdsBtn.interactable = false;
        StartCoroutine(Countdown());
    }


    // Update is called once per frame
    void Update()
    {

        if(SceneManager.GetActiveScene().name == "GamePlay_1" || SceneManager.GetActiveScene().name == "GamePlay_3" || SceneManager.GetActiveScene().name == "GamePlay_4" || SceneManager.GetActiveScene().name == "GamePlay_5")
        {
            qualifiedPoint.text = count_Qualified.qualified_Value.ToString() + "/" + characterCount.ToString();
            goldReward.text = (100 * (characterCount - count_Qualified.qualified_Value + 1)).ToString();
            victoryRank.text = "#" + count_Qualified.qualified_Value.ToString() + "/" + characterCount.ToString();
            metalReward.text = (characterCount - count_Qualified.qualified_Value + 1).ToString();
        }
        if (SceneManager.GetActiveScene().name == "GamePlay_2" || SceneManager.GetActiveScene().name == "GamePlay_6" || SceneManager.GetActiveScene().name == "GamePlay_7")
        {
            qualifiedPoint.text = (characterCount - count_Qualified.qualified_Value).ToString();
            goldReward.text = "500";
            victoryRank.text = "#1";
            metalReward.text = "5";
        }

        if (SceneManager.GetActiveScene().name == "GamePlay_1_Multiplay" || SceneManager.GetActiveScene().name == "GamePlay_3_Multiplay" || SceneManager.GetActiveScene().name == "GamePlay_4_Multiplay" || SceneManager.GetActiveScene().name == "GamePlay_5_Multiplay")
        {
            qualifiedPoint.text = count_Qualified.qualified_Value.ToString() + "/1";
            goldReward.text = "100";
            victoryRank.text = "#1";
            metalReward.text = "1";
            victoryAdsBtn.interactable = false;
            settingRestarBtn.interactable = false;
            loseRetryBtn.interactable = false;
        }
        if (SceneManager.GetActiveScene().name == "GamePlay_2_Multiplay" || SceneManager.GetActiveScene().name == "GamePlay_6_Multiplay" || SceneManager.GetActiveScene().name == "GamePlay_7_Multiplay")
        {
           
            int playerNum = GameObject.FindGameObjectsWithTag("MultiPlayer").Length;
            int EnemyNum = GameObject.FindGameObjectsWithTag("EnemyPlayer").Length;
            qualifiedPoint.text = (playerNum+EnemyNum).ToString();
            goldReward.text = "100";
            victoryRank.text = "#1";
            metalReward.text = "1";
            victoryAdsBtn.interactable = false;
            settingRestarBtn.interactable = false;
            loseRetryBtn.interactable = false;
        }




        if (count_Qualified.winGame || count_Qualified.loseGame) StartCoroutine(EndGame());

       
        if (main_Sound.volume < 0.5f)
            main_Sound.volume += 0.001f;

        if (PlayerPrefs.GetInt("Shadow") == 1)
        {
            QualitySettings.shadows = ShadowQuality.Disable;
            dropdownShadow.GetComponent<TMP_Dropdown>().value = 1;
        }
            

        if (PlayerPrefs.GetInt("Shadow") == 0) {

            QualitySettings.shadows = ShadowQuality.All;
            dropdownShadow.GetComponent<TMP_Dropdown>().value = 0;

        }

        if (Advertising.IsRewardedAdReady())
            victoryAdsBtn.interactable = true;


    }

    IEnumerator Countdown()
    {
        main_Sound.Play();
        timeCountStart_Txt.text = "3";
        yield return new WaitForSeconds(1.0f);

       
        timeCountStart_Txt.text = "2";
        yield return new WaitForSeconds(1.0f);

       
        timeCountStart_Txt.text = "1";
        yield return new WaitForSeconds(1.0f);
        timeCountStart_Txt.text = "Go!";
        soundManager.AudioFX(3);
        // start the game here
        yield return new WaitForSeconds(1.0f);
        timeCountStart_Txt.gameObject.SetActive(false);


        yield return null;
        
    }



    IEnumerator EndGame()
    {

        yield return new WaitForSeconds(2);
        if (count_Qualified.loseGame )
            lose_Panel.SetActive(true);  

        if (count_Qualified.winGame)
            win_Panel.SetActive(true);

        Time.timeScale = 0;
    }

    void LoseHome()
    {
       
        if (!Advertising.IsInterstitialAdReady())
        {
            soundManager.AudioFX(0);
            Time.timeScale = 1;
            SceneManager.LoadScene("Main_Menu");
        }

        if (PlayerPrefs.GetInt("RemoveAds") == 0)
            ShowAds_Inter();

        checkLoseRetry = "LoseHome";

    }

    void LoseRetry()
    {
       
        if (!Advertising.IsInterstitialAdReady())
        {
            soundManager.AudioFX(0);
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (PlayerPrefs.GetInt("RemoveAds") == 0)
            ShowAds_Inter();

        checkLoseRetry = "LoseRetry";

        

    }

    void VictoryClaim()
    {
        soundManager.AudioFX(0);
        //GamePlay_1 RACE
        //GamePlay_2 Sarvival
        Time.timeScale = 1;
        if (SceneManager.GetActiveScene().name == "GamePlay_1" || SceneManager.GetActiveScene().name == "GamePlay_3" || SceneManager.GetActiveScene().name == "GamePlay_4" || SceneManager.GetActiveScene().name == "GamePlay_5")
        {
            PlayerPrefs.SetInt("GoldMain", PlayerPrefs.GetInt("GoldMain") + (100 * (characterCount - count_Qualified.qualified_Value + 1)));
            PlayerPrefs.SetInt("MetalMain", PlayerPrefs.GetInt("MetalMain") + (characterCount - count_Qualified.qualified_Value + 1));
        }
        if (SceneManager.GetActiveScene().name == "GamePlay_2" || SceneManager.GetActiveScene().name == "GamePlay_6" || SceneManager.GetActiveScene().name == "GamePlay_7")
        {
            PlayerPrefs.SetInt("GoldMain", PlayerPrefs.GetInt("GoldMain") + 500);
            PlayerPrefs.SetInt("MetalMain", PlayerPrefs.GetInt("MetalMain") + 5);
        }

        if (SceneManager.GetActiveScene().name == "GamePlay_1_Multiplay" || SceneManager.GetActiveScene().name == "GamePlay_2_Multiplay" || SceneManager.GetActiveScene().name == "GamePlay_3_Multiplay" || SceneManager.GetActiveScene().name == "GamePlay_4_Multiplay" || SceneManager.GetActiveScene().name == "GamePlay_5_Multiplay" || SceneManager.GetActiveScene().name == "GamePlay_6_Multiplay" || SceneManager.GetActiveScene().name == "GamePlay_7_Multiplay")
        {
           
            PlayerPrefs.SetInt("Crown", PlayerPrefs.GetInt("Crown") + 1);
 
        }
       
        SceneManager.LoadScene("Main_Menu");
    }

    void VictoryAds()
    {
        soundManager.AudioFX(0);
        Time.timeScale = 1;
        if (SceneManager.GetActiveScene().name == "GamePlay_1" || SceneManager.GetActiveScene().name == "GamePlay_3" || SceneManager.GetActiveScene().name == "GamePlay_4" || SceneManager.GetActiveScene().name == "GamePlay_5")
        {
            PlayerPrefs.SetInt("GoldMain", PlayerPrefs.GetInt("GoldMain") + (200 * (characterCount - count_Qualified.qualified_Value + 1)));
            PlayerPrefs.SetInt("MetalMain", PlayerPrefs.GetInt("MetalMain") + (characterCount - count_Qualified.qualified_Value + 1) * 2);
        }
        if (SceneManager.GetActiveScene().name == "GamePlay_2" || SceneManager.GetActiveScene().name == "GamePlay_6" || SceneManager.GetActiveScene().name == "GamePlay_7")
        {
            PlayerPrefs.SetInt("GoldMain", PlayerPrefs.GetInt("GoldMain") + 1000);
            PlayerPrefs.SetInt("MetalMain", PlayerPrefs.GetInt("MetalMain") + 10);
        }
        SceneManager.LoadScene("Main_Menu");
    }

    void SettingOpen()
    {
        soundManager.AudioFX(0);
        settingPanel.SetActive(true);
        Time.timeScale = 0;
    }

    void SettingContinute()
    {
        soundManager.AudioFX(0);
        settingPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void ShadowHadle(int val)
    {
        PlayerPrefs.SetInt("Shadow", val);

    }

    #region Ads
    void OnEnable()
    {
        Advertising.InterstitialAdCompleted += InterstitialAdCompletedHandler;
         Advertising.RewardedAdCompleted += RewardedAdCompletedHandler;
    }

    

    // Unsubscribe
    void OnDisable()
    {
        Advertising.RewardedAdCompleted -= RewardedAdCompletedHandler;
        Advertising.InterstitialAdCompleted -= InterstitialAdCompletedHandler;
    }

    void ShowAds_Inter()
    {
        // Check if interstitial ad is ready
        bool isReady = Advertising.IsInterstitialAdReady();

        // Show it if it's ready
        if (isReady)
        {
            Advertising.ShowInterstitialAd();
        }
    }

    void ShowAds_Reward()
    {
       
        // Check if rewarded ad is ready
        bool isReady = Advertising.IsRewardedAdReady();
        // Show it if it's ready
        if (isReady)
        {
            Advertising.ShowRewardedAd();
        }
    }


    // The event handler
    void InterstitialAdCompletedHandler(InterstitialAdNetwork network, AdPlacement placement)
    {
        
        switch (checkLoseRetry)
        {
            case "LoseHome":
                SceneManager.LoadScene("Main_Menu");
                break;
            case "LoseRetry":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
        }

        soundManager.AudioFX(0);
        Time.timeScale = 1;
        Debug.Log("Interstitial ad has been closed.");
    }

    void RewardedAdCompletedHandler(RewardedAdNetwork network, AdPlacement placement)
    {
        
       
        VictoryAds();
        Debug.Log("Rewarded ad has completed. The user should be rewarded now.");
    }
    #endregion Ads


}
