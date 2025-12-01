using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

[System.Serializable]
public class Character_Select
{
    public string styleSelect;
    public int num_Select;
    public string name;
    public int price;
    public Sprite avatarCharacter;
    public GameObject character_View;
    public bool unlock;
    public bool apply;
    public Sprite iconBuy;
}

[System.Serializable]
public class Map_Select
{
    public string name;
    public int price;
    public GameObject lockBGMap;
}

public class UI_Main : MonoBehaviour
{
    public int gold_main;
    public int metal_main;

    public SoundManager soundManager;

    public GameObject characterPanel;
    public GameObject button;
    public GameObject[] lockBG;
    public GameObject[] select_icon;
    public Image iconBuyGold;
    public Image avatarPlayer;


    public TextMeshProUGUI gold_main_Txt;
    public TextMeshProUGUI metal_main_Txt;

    public Character_Select[] character_Select;
    public Map_Select[] map;

    public TextMeshProUGUI priceTxt;
    public TextMeshProUGUI nameText;

    public Button unLockBtn;
    public Button selectedBtn;
    public Button openShopBtn;
    public Button closeShopBtn;
    public Button playBtn;
    public Button backMap;
    public GameObject map_select;

    public GameObject unlockMapPopup;
    public GameObject failUnlockMapPopup;
    public GameObject successUnlockMapPopup;
    public Button lateUnlockMap;
    public Button unlockMapBtn;
    public Button failOkBtn;
    public Button successOkBtn;

    public Animator animator;

    public int selected;
    public int selected_map;

    public GameObject namePanel;
    public Button nameCloseBtn;
    public Button nameSubmitBtn;
    public TMP_InputField nameInput;
    public Button chargeNameBtn;
    public TextMeshProUGUI playerNameShowMain;
    public TextMeshProUGUI errorInputField;

    public Button pvpBtn;
    public Button createOpenBtn;
    public Button joinOpenBtn;
    public GameObject multiplayPannel;
    public GameObject createPanel;
    public GameObject joinPanel;

    public Button createCloseBtn;
    public Button joinCloseBtn;
    public Button multiplayCloseBtn;

    public Button openStoreBtn;
    public GameObject storePanel;
    public Button closeStroeBtn;

    public TextMeshProUGUI crownValueText;


    void FirstOpenApp()
    {
        PlayerPrefs.SetInt("GoldMain", 0);
        PlayerPrefs.SetInt("MetalMain", 10);
        PlayerPrefs.SetInt("Crown", 0);
        PlayerPrefs.SetString(character_Select[0].name, "Unlock");
        PlayerPrefs.SetString(map[0].name, "Unlock");
        PlayerPrefs.SetString("PlayerName", "PlayerName");
        PlayerPrefs.SetInt("RemoveAds", 0);

    }

    private void Awake()
    {
        priceTxt = GameObject.Find("Canvas/Character/Group_Right/Bottom_Menu/PriceTxt/price").GetComponent<TextMeshProUGUI>();
        nameText = GameObject.Find("Canvas/Character/Group_Left/Text_Name").GetComponent<TextMeshProUGUI>();
        unLockBtn = GameObject.Find("Canvas/Character/Group_Right/Bottom_Menu/Button_Unlock").GetComponent<Button>();
        selectedBtn = GameObject.Find("Canvas/Character/Group_Right/Bottom_Menu/Button_Selected").GetComponent<Button>();
        unLockBtn.onClick.AddListener(() => Unlock_Character());
        selectedBtn.onClick.AddListener(() => Selected_Character());
        openShopBtn.onClick.AddListener(() => Shop());
        closeShopBtn.onClick.AddListener(() => CloseShop());
        playBtn.onClick.AddListener(() => Map_Select());
        backMap.onClick.AddListener(() => Close_Map());
        lateUnlockMap.onClick.AddListener(() => LateUnlock());
        unlockMapBtn.onClick.AddListener(() => UnlockMap());
        failOkBtn.onClick.AddListener(() => FailOk());
        successOkBtn.onClick.AddListener(() => SuccessOk());
        chargeNameBtn.onClick.AddListener(() => ChargeNameShowPanel());
        nameCloseBtn.onClick.AddListener(() => NameCloseBtn());
        nameSubmitBtn.onClick.AddListener(() => NameSubmitBtn());
        pvpBtn.onClick.AddListener(() => multiplayPannel.SetActive(true));
        createOpenBtn.onClick.AddListener(() => createPanel.SetActive(true));
        joinOpenBtn.onClick.AddListener(() => joinPanel.SetActive(true));
        createCloseBtn.onClick.AddListener(() => closeCreateRoomBtn());
        joinCloseBtn.onClick.AddListener(() => closeJoinRoomBtn());
        multiplayCloseBtn.onClick.AddListener(() => multiplayPannel.SetActive(false));
        openStoreBtn.onClick.AddListener(() => OpenStore());
        closeStroeBtn.onClick.AddListener(() => CloseStore());

    }

    private void Start()
    {
        storePanel.SetActive(false);
        multiplayPannel.SetActive(false);
        createPanel.SetActive(false);
        joinPanel.SetActive(false);
        unLockBtn.gameObject.SetActive(true);
        selectedBtn.gameObject.SetActive(false);
        unlockMapPopup.SetActive(false);
        failUnlockMapPopup.SetActive(false);
        successUnlockMapPopup.SetActive(false);
        errorInputField.text = "Please enter a name to change.";
        namePanel.SetActive(false);

        if (PlayerPrefs.GetInt("FirstOpen") == 0)
        {
            FirstOpenApp();
            PlayerPrefs.SetInt("FirstOpen", 1);
        }


        addCharacter();

        lockBG = new GameObject[character_Select.Length];
        select_icon = new GameObject[character_Select.Length];
        for (int i = 0; i < character_Select.Length; i++)
        {
            if (PlayerPrefs.GetString(character_Select[i].name) == "Unlock")
            {
                priceTxt.text = "0";
                character_Select[i].unlock = true;
            }
                

            lockBG[i] = GameObject.Find("Canvas/Character/Group_Right/ScrollRect/Content/" + i.ToString() + "/Lock");
            select_icon[i] = GameObject.Find("Canvas/Character/Group_Right/ScrollRect/Content/" + i.ToString() + "/SelectIcon");
            select_icon[i].SetActive(false);

            if (character_Select[i].unlock)
            {
                lockBG[i].SetActive(false);
            }
            
        }

        //Set select Character
        Select_Character(PlayerPrefs.GetInt("Select"));
        select_icon[PlayerPrefs.GetInt("Select")].SetActive(true);
        map_select.SetActive(false);

        for(int i = 0; i < map.Length; i++)
            if (PlayerPrefs.GetString(map[i].name) == "Unlock")
                map[i].lockBGMap.SetActive(false);
            

    }

    void Update()
    {
        gold_main = PlayerPrefs.GetInt("GoldMain");
        metal_main = PlayerPrefs.GetInt("MetalMain");

        gold_main_Txt.text = gold_main.ToString();
        metal_main_Txt.text = metal_main.ToString();

        avatarPlayer.sprite = character_Select[selected].avatarCharacter;

        playerNameShowMain.text = PlayerPrefs.GetString("PlayerName");
        crownValueText.text = PlayerPrefs.GetInt("Crown").ToString();

        if (PlayerPrefs.GetString(character_Select[selected].name) == "Unlock")
        {
            priceTxt.text = "0";
            character_Select[selected].unlock = true;
            unLockBtn.gameObject.SetActive(false);
            selectedBtn.gameObject.SetActive(true);
            lockBG[selected].SetActive(false);
        }
        else
        {
            character_Select[selected].unlock = false;
            unLockBtn.gameObject.SetActive(true);
            selectedBtn.gameObject.SetActive(false);
            lockBG[selected].SetActive(true);
        }


        if (character_Select[selected].styleSelect == "GOLD")
        {
            if (character_Select[selected].price > gold_main)
                unLockBtn.interactable = false;
            else unLockBtn.interactable = true;
        }

        if (character_Select[selected].styleSelect == "METAL")
        {
            if (character_Select[selected].price > metal_main)
                unLockBtn.interactable = false;
            else unLockBtn.interactable = true;
        }

      

        character_Select[selected].character_View.SetActive(true);

        if (PlayerPrefs.GetString(map[selected_map].name) == "Unlock")
            map[selected_map].lockBGMap.SetActive(false);
        else
            map[selected_map].lockBGMap.SetActive(true);
        


    }

    void addCharacter()
    {
        for (int i = 0; i < character_Select.Length; i++)
        {
            GameObject newButton = Instantiate(button);
            RectTransform rectTransform = newButton.GetComponent<RectTransform>();
            rectTransform.SetParent(characterPanel.transform, false);

            newButton.gameObject.GetComponent<Image>().sprite = character_Select[i].avatarCharacter;
            newButton.transform.name = "" + i;
            character_Select[i].character_View.SetActive(false);
         

        }
    }

    public void Select_Character(int index)
    {
        soundManager.AudioFX(0);

        selected = index;
        iconBuyGold.sprite = character_Select[selected].iconBuy; 
        nameText.text = character_Select[selected].name;
        priceTxt.text = character_Select[selected].price.ToString();
        for (int i = 0; i < character_Select.Length; i++)
        {
            character_Select[i].character_View.SetActive(false);
        }
    }
    
    void Unlock_Character()
    {
        if (character_Select[selected].styleSelect == "GOLD" && character_Select[selected].price <= gold_main)
        {
                soundManager.AudioFX(2);
                PlayerPrefs.SetInt("GoldMain", PlayerPrefs.GetInt("GoldMain") - character_Select[selected].price);
                unLockBtn.gameObject.SetActive(false);
                selectedBtn.gameObject.SetActive(true);
                character_Select[selected].unlock = true;
                PlayerPrefs.SetString(character_Select[selected].name, "Unlock");
          
        }

        if (character_Select[selected].styleSelect == "METAL" && character_Select[selected].price <= metal_main)
        {
            
                soundManager.AudioFX(2);
                PlayerPrefs.SetInt("MetalMain", PlayerPrefs.GetInt("MetalMain") - character_Select[selected].price);
                unLockBtn.gameObject.SetActive(false);
                selectedBtn.gameObject.SetActive(true);
                character_Select[selected].unlock = true;
                PlayerPrefs.SetString(character_Select[selected].name, "Unlock");
           
          
        }




    }

    void Selected_Character()
    {
        character_Select[selected].apply = true;
        for (int i = 0; i < character_Select.Length; i++)
        {
            select_icon[i].SetActive(false);
        }

        select_icon[selected].SetActive(true);

        PlayerPrefs.SetInt("Select", selected);
        soundManager.AudioFX(0);
    }

    void Shop()
    {
        animator.SetBool("OpenShop", true);
        soundManager.AudioFX(0);
    }
    void CloseShop()
    {
       
        animator.SetBool("OpenShop", false);
        for (int i = 0; i < character_Select.Length; i++)
        {
            character_Select[i].character_View.SetActive(false);
        }

        Select_Character(PlayerPrefs.GetInt("Select"));
        soundManager.AudioFX(0);
    }
    
    void Map_Select()
    {
        map_select.SetActive(true);
        soundManager.AudioFX(0);
    }
    
    void Close_Map()
    {
        map_select.SetActive(false);
        soundManager.AudioFX(0);
    }

  

    public void LoadMap(int index)
    {
        selected_map = index;
        if (PlayerPrefs.GetString(map[selected_map].name) == "Unlock")
        {
            soundManager.AudioFX(0);
            SceneManager.LoadScene(map[selected_map].name);
        }
        else {
            soundManager.AudioFX(1);
            unlockMapPopup.SetActive(true);

        }


    }

    void UnlockMap()
    {
        if(metal_main >= map[selected_map].price)
        {
            PlayerPrefs.SetInt("MetalMain", PlayerPrefs.GetInt("MetalMain") - map[selected_map].price);
            successUnlockMapPopup.SetActive(true);
            PlayerPrefs.SetString(map[selected_map].name, "Unlock");
            soundManager.AudioFX(2);
        }
        else
        {
            failUnlockMapPopup.SetActive(true);
            soundManager.AudioFX(0);
        }
    }

    void LateUnlock() 
    {
        unlockMapPopup.SetActive(false);
        soundManager.AudioFX(0);
    }
    void FailOk()
    {
        failUnlockMapPopup.SetActive(false);
        unlockMapPopup.SetActive(false);
        soundManager.AudioFX(0);
    }
    void SuccessOk()
    {
        successUnlockMapPopup.SetActive(false);
        unlockMapPopup.SetActive(false);
        soundManager.AudioFX(0);
    }

    void NameSubmitBtn()
    {
        
        string PlayerNickName = nameInput.text;
        if (PlayerNickName.Length < 13)
        {
            errorInputField.text = "Please enter a name to change.";
            PlayerPrefs.SetString("PlayerName", PlayerNickName);
            PhotonNetwork.NickName = PlayerNickName;
            namePanel.SetActive(false);
            soundManager.AudioFX(0);
        }
        else 
        {
            errorInputField.gameObject.SetActive(true);
            errorInputField.text = "Name is too long";
            soundManager.AudioFX(1);
        }
            
    }

    void ChargeNameShowPanel()
    {
        namePanel.SetActive(true);
        soundManager.AudioFX(0);
    }
    void NameCloseBtn()
    {
        namePanel.SetActive(false);
        soundManager.AudioFX(0);
    }

    void OpenStore()
    {
        storePanel.SetActive(true);
        soundManager.AudioFX(0);
    }

    void CloseStore()
    {
        storePanel.SetActive(false);
        soundManager.AudioFX(0);
    }

    void closeCreateRoomBtn()
    {
        createPanel.SetActive(false);
        PhotonNetwork.LeaveRoom();

    }
    void closeJoinRoomBtn()
    {
        joinPanel.SetActive(false);
        PhotonNetwork.LeaveRoom();

    }
}
