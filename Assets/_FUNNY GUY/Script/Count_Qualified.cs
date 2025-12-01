using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Count_Qualified : MonoBehaviour
{
    public int qualified_Value;
    public bool winGame;
    public bool loseGame;
    public Animator animatorCanvas;
    public GameManager gameManager;

    public SoundManager soundManager;

    PhotonView photonView;
    int enemyNum;

    private void Awake()
    {
        
        soundManager = GameObject.Find("AudioFX").GetComponent<SoundManager>();
        photonView = GetComponent<PhotonView>();
    }


    private void Start()
    {
        qualified_Value = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Bot")
        {
            
            Destroy(other.gameObject);
            if(!winGame && !loseGame)
            qualified_Value++;
            soundManager.AudioFX(2);

            if (SceneManager.GetActiveScene().name == "GamePlay_1" || SceneManager.GetActiveScene().name == "GamePlay_3" || SceneManager.GetActiveScene().name == "GamePlay_4" || SceneManager.GetActiveScene().name == "GamePlay_5")
            {
                if (qualified_Value == gameManager.characterCount && !winGame) //Race
                {
                    animatorCanvas.SetTrigger("Qualified");
                    loseGame = true;
                    winGame = false;

                }
            }
               
            if (SceneManager.GetActiveScene().name == "GamePlay_2" || SceneManager.GetActiveScene().name == "GamePlay_6" || SceneManager.GetActiveScene().name == "GamePlay_7")
            {
                if (qualified_Value == gameManager.characterCount - 1) //Execed Player
                {
                    animatorCanvas.SetTrigger("Qualified");
                    winGame = true;
                    loseGame = false;

                }
            }


        }
        
            if (other.gameObject.tag == "Player" && !loseGame && !winGame)
            {
                
                animatorCanvas.SetTrigger("Qualified");
                if (SceneManager.GetActiveScene().name == "GamePlay_1" || SceneManager.GetActiveScene().name == "GamePlay_3" || SceneManager.GetActiveScene().name == "GamePlay_4" || SceneManager.GetActiveScene().name == "GamePlay_5")
                    {
                        winGame = true;
                        loseGame = false;

                    }
                    
                if (SceneManager.GetActiveScene().name == "GamePlay_2" || SceneManager.GetActiveScene().name == "GamePlay_6" || SceneManager.GetActiveScene().name == "GamePlay_7") 
                    {
                        loseGame = true;
                        winGame = false;
                    }

                soundManager.AudioFX(2);
                qualified_Value++;
            }


        if (SceneManager.GetActiveScene().name == "GamePlay_1_Multiplay" || SceneManager.GetActiveScene().name == "GamePlay_3_Multiplay" || SceneManager.GetActiveScene().name == "GamePlay_4_Multiplay" || SceneManager.GetActiveScene().name == "GamePlay_5_Multiplay")
        {
            if (other.gameObject.tag == "EnemyPlayer" && !winGame)
            {
                animatorCanvas.SetTrigger("Qualified");
                //photonView.RPC("LoseGame", RpcTarget.All);
                loseGame = true;
            }

            if (other.gameObject.tag == "MultiPlayer" && !loseGame)
            {
                animatorCanvas.SetTrigger("Qualified");
                winGame = true;

            }
        }

        if (SceneManager.GetActiveScene().name == "GamePlay_2_Multiplay" || SceneManager.GetActiveScene().name == "GamePlay_6_Multiplay" || SceneManager.GetActiveScene().name == "GamePlay_7_Multiplay")
        {
           
            if (other.gameObject.tag == "EnemyPlayer")
            {
                Destroy(other.gameObject);
                if (enemyNum == 1)
                {
                    animatorCanvas.SetTrigger("Qualified");
                    winGame = true;
                }
            }

            if (other.gameObject.tag == "MultiPlayer" && !winGame)
            {
                animatorCanvas.SetTrigger("Qualified");
                loseGame = true;

            }
        }



        if (other.gameObject.tag == "DINO" && !loseGame && !winGame)
            {
                    animatorCanvas.SetTrigger("Qualified");
                    Destroy(other.gameObject);
                    winGame = true;
                    loseGame = false;
            }
        
    }


    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "GamePlay_1_Multiplay" || SceneManager.GetActiveScene().name == "GamePlay_2_Multiplay" || SceneManager.GetActiveScene().name == "GamePlay_3_Multiplay" || SceneManager.GetActiveScene().name == "GamePlay_4_Multiplay" || SceneManager.GetActiveScene().name == "GamePlay_5_Multiplay" || SceneManager.GetActiveScene().name == "GamePlay_6_Multiplay" || SceneManager.GetActiveScene().name == "GamePlay_7_Multiplay")
        {
            enemyNum = GameObject.FindGameObjectsWithTag("EnemyPlayer").Length;
        }
    }


}
