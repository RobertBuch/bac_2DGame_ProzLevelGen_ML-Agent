using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowMe : MonoBehaviour
{
    public Transform cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cam != null){
            transform.position = new Vector3(cam.position.x, transform.position.y, -10);
        }
      
    }
}
