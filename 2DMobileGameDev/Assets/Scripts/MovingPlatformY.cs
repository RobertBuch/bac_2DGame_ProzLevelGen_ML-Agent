using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformY : MonoBehaviour
{

    [Header("test")]
    public Vector2[] waypoints;
    public float speed;
    private int point;
    private float tolerance = 0.1f;

    private Transform originalParent;  



    // Start is called before the first frame update
    void Start()
    {
        speed = 3;
        point = 0;
        waypoints = new Vector2[2];
        SetWaypoints();

    }

    // Update is called once per frame
    void Update()
    {
        

    }


    private void FixedUpdate() {
        MovePoint();
    }

    private void SetWaypoints(){
        waypoints[0] = new Vector2(transform.localPosition.x, transform.localPosition.y - 2.7f);
        waypoints[1] = new Vector2(transform.localPosition.x, transform.localPosition.y + 2.7f);

    }



    private void MovePoint(){
        transform.localPosition = (Vector3)Vector2.MoveTowards((Vector2)transform.localPosition, waypoints[point], speed * Time.deltaTime); 
        
        if ( Vector2.Distance(new Vector2(transform.localPosition.x, transform.localPosition.y), waypoints[point]) < tolerance) {
            if( point == 0 ){
                point += 1;

            } else {
                point = 0;
            }
        } 
    }




    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            originalParent = other.transform.parent;
            other.transform.parent = this.transform;
        }
    }


    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = originalParent; 
        }
    }



}
