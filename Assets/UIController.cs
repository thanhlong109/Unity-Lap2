using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    public static UIController Instance;
    public GameObject TryAgainObj;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    public void ShowTryAgain()
    {
       if(TryAgainObj != null && !TryAgainObj.active)
        {
            TryAgainObj.SetActive(true);
        }
    }

    


}
