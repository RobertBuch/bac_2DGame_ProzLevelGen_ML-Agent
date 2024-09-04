using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Coins : MonoBehaviour
{

    public CoinsParent coinsParent;

    
    void Start()
    {


    }

    
    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            coinsParent.CoinCounter();
            Destroy(gameObject);

        }
    }

}

