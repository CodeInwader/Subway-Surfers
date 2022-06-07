using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public List<GameObject> Coins = new List<GameObject>();

   public void ReCycleCoins()
   {
        foreach (GameObject element in Coins)
        {
            if(element.activeInHierarchy != true)
            {
                element.SetActive(true);
            
            }
        }
   }
}
