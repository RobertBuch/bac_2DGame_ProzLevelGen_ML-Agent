using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public static Vector2 lastCheckpoint;
    public static int savedCoins;


    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("checkpoint = " + savedCoins);

    }

    // Update is called once per frame
    void Update()
    {
       
    }


   
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            lastCheckpoint = gameObject.transform.position;
            GetComponent<SpriteRenderer>().color = Color.white;
            // Debug.Log("Checkpoint = " + lastCheckpoint);
            savedCoins = CoinsParent.coins;
            // Debug.Log("checkpoint savedcoins = " + savedCoins);
            
            
        }
    }
}





