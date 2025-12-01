using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class SellectCharacter : MonoBehaviour
{
    public UI_Main uI_Main;
   
   
    private void Awake()
    {
        uI_Main = GameObject.Find("Canvas").GetComponent<UI_Main>();
     
       
    }

    public void SelectBtn()
    {
        if (uI_Main != null)
            uI_Main.Select_Character(int.Parse(transform.name));
    }

    public void Select_Map()
    {
        if (uI_Main != null)
            uI_Main.LoadMap(int.Parse(transform.name));
    }
}
