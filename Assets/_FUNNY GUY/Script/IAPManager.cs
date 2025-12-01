using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour
{
    private string romveAds = "com.test.funnyguys.removeads";
    private string gold1500 = "com.test.funnyguys.gold1500";
    private string gold4000 = "com.test.funnyguys.gold4000";
    private string gold12000 = "com.test.funnyguys.gold12000";
    private string gold25000 = "com.test.funnyguys.gold25000";
    private string gold60000 = "com.test.funnyguys.gold60000";

    public SoundManager soundManager;

    public void OnPurchaseComplete(Product product)
    {
        if (product.definition.id == romveAds)
        {
            soundManager.AudioFX(2);
           PlayerPrefs.SetInt("RemoveAds", 1);
        }
        if (product.definition.id == gold1500)
        {
            soundManager.AudioFX(2);
            PlayerPrefs.SetInt("GoldMain", PlayerPrefs.GetInt("GoldMain")+ 1500);
        }
        if (product.definition.id == gold4000)
        {
            soundManager.AudioFX(2);
            PlayerPrefs.SetInt("GoldMain", PlayerPrefs.GetInt("GoldMain") + 4000);
        }
        if (product.definition.id == gold12000)
        {
            soundManager.AudioFX(2);
            PlayerPrefs.SetInt("GoldMain", PlayerPrefs.GetInt("GoldMain") + 12000);
        }
        if (product.definition.id == gold25000)
        {
            soundManager.AudioFX(2);
            PlayerPrefs.SetInt("GoldMain", PlayerPrefs.GetInt("GoldMain") + 25000);
        }
        if (product.definition.id == gold60000)
        {
            soundManager.AudioFX(2);
            PlayerPrefs.SetInt("GoldMain", PlayerPrefs.GetInt("GoldMain") + 60000);
        }

    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {

        Debug.Log(product.definition.id + "failed because" + failureReason);
    }

}
