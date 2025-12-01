using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance;

    void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        if (!RuntimeManager.IsInitialized())
            RuntimeManager.Init();
    }

    private void Start()
    {

        // Initialize the Mobile Ads SDK.
        MobileAds.Initialize((initStatus) =>
        {
            Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
            foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
            {
                string className = keyValuePair.Key;
                AdapterStatus status = keyValuePair.Value;
                switch (status.InitializationState)
                {
                    case AdapterState.NotReady:
                        // The adapter initialization did not complete.
                        MonoBehaviour.print("Adapter: " + className + " not ready.");
                        break;
                    case AdapterState.Ready:
                        // The adapter was successfully initialized.
                        MonoBehaviour.print("Adapter: " + className + " is initialized.");
                        break;
                }
            }
        });

        StartCoroutine(DelayInitialization());
    }


    private IEnumerator DelayInitialization()
    {
        yield return new WaitForSeconds(4);
        Advertising.Initialize();
    }

    







}
