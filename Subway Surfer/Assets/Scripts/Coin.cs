using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
   [SerializeField] AudioSource coinEat;

    
    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
        UIAndScoreManager.score++;
        coinEat.Play();
       
    }

    
}
