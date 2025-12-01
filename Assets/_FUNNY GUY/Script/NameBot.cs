using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameBot : MonoBehaviour
{
    public string[] nameBotArray;
    public TextMeshProUGUI nameBotTxt;
    // Start is called before the first frame update
    void Start()
    {
        int rdNum = Random.Range(100, 999);
        int rd = Random.Range(0, nameBotArray.Length);
        nameBotTxt.text = nameBotArray[rd] + rdNum.ToString();
    }


}
