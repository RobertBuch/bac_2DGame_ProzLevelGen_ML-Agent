using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public GameObject fp;
    private Rigidbody2D rb; 
    private BoxCollider2D bc;
    private float delay;
    private float respawnDelay;
    private Vector2 startPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = fp.GetComponent<Rigidbody2D>();
        bc = fp.GetComponent<BoxCollider2D>();
        delay = 1.2f;
        respawnDelay = 5;
        startPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if( other.gameObject.CompareTag("Player")){
            StartCoroutine(Delay());
        } 
    }
    

    
    private void Respawn(){
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        bc.enabled = true;
        transform.position = startPos;
        // Debug.Log("respawn falling platform");
    }
    

    IEnumerator Delay(){
        yield return new WaitForSeconds(delay);
        rb.isKinematic = false;
        bc.enabled = false;
        yield return new WaitForSeconds(respawnDelay);
        Respawn();
    }





}
