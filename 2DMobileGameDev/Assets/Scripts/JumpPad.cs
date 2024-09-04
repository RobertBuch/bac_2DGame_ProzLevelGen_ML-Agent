using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpPower = 8f;  
 
  

    // Start is called before the first frame update
    void Start()
    {
       
     
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    

    
    private void OnTriggerStay2D(Collider2D jumppad) {
        // Debug.Log("juuuuuuuuuummmmpp!");
        if ( jumppad.gameObject.CompareTag("Player")){
            
        }
    }


  

}
