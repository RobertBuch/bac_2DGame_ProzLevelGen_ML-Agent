using System.Collections.Generic;
using UnityEngine;

public class ParaBack : MonoBehaviour
{
    [Header("para")]
    private float paraSpeed;
    [SerializeField] private GameObject cameraObject;

    private List<Material> images = new List<Material>();
    private List<GameObject> canvasPlanes = new List<GameObject>();
    private List<float> imageAcc = new List<float>(); 
    private float cameraPosX; 
    private float cameraDistance; 

    public GameObject bgPlane;
 
    private float speed;

    private float zPositionSpacing;

    private float lastPlane;  
    private float zSpace;

    private float testspeed;




    void Start()
    {
        zSpace = 10f;
        paraSpeed = 0.1f;
        cameraPosX = cameraObject.transform.position.x; 

        Collector();
   
        SetZPositions();

    }

    void Update(){

        Move();
        Offsets();
    }


    private void Collector(){
        int childs = transform.childCount;
        for (int i = 0; i < childs; i++)
        {
            bgPlane = transform.GetChild(i).gameObject;
            canvasPlanes.Add(bgPlane);
            images.Add(bgPlane.GetComponent<Renderer>().material);
        }
    }



    void SetZPositions()
    {
        lastPlane = canvasPlanes.Count * zSpace;
        imageAcc.Clear(); 

        int i = 0; 

        foreach (var bg in canvasPlanes)
        {

            zPositionSpacing = (i + 1) * zSpace;
            bg.transform.position = new Vector3(bg.transform.position.x, bg.transform.position.y, zPositionSpacing);

            testspeed = 1 - (bg.transform.position.z - cameraObject.transform.position.z) / lastPlane;
            speed = testspeed * testspeed;
            imageAcc.Add(speed);

            i++; 
        }

    }
    
    void Move()
    {
        cameraDistance = cameraObject.transform.position.x - cameraPosX;
        transform.position = new Vector2(cameraObject.transform.position.x, transform.position.y);
    }

    void Offsets()
    {
        int index = 0;
        foreach (var image in images)
        {
            float textureSpeed = imageAcc[index] * paraSpeed;
            image.SetTextureOffset("_MainTex", new Vector2(cameraDistance * textureSpeed, 0));
            index++;
        }

    }


}

