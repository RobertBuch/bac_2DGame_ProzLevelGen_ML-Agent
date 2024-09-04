using UnityEngine;


public class EnemyLeRi : MonoBehaviour
{
    private float enemyLiReSpeed;
    private bool startDirection;
    [SerializeField] private Transform groundDetection;

    private float turnDelay = 0.5f;
    private float lastTurn;
    private float RayCheckDistance = 0.3f;
    [SerializeField] private LayerMask Wall;
    private Vector2 RayWallDirection;
   
    RaycastHit2D hit;
   

    // Start is called before the first frame update
    void Start()
    {
        enemyLiReSpeed = 3.5f;
        startDirection = Random.Range(0,2) == 0;
        // direction "-" left, or "+" right
        enemyLiReSpeed = startDirection ? -enemyLiReSpeed : enemyLiReSpeed;


        UpdateDirection();
    
    }

    // Update is called once per frame
    void Update()
    {
    
        transform.Translate(transform.right * enemyLiReSpeed * Time.deltaTime);

    }



    private void FixedUpdate() {
       GroundDetection();
       WallDetection();
    }

    private void GroundDetection(){
   
        hit = Physics2D.Raycast(groundDetection.position, Vector2.down, RayCheckDistance);
        Debug.DrawRay(groundDetection.position, Vector2.down * RayCheckDistance, Color.yellow);

        if (hit.collider == null && Time.time > lastTurn + turnDelay)
        {
            enemyLiReSpeed *= -1; 
            lastTurn = Time.time;
            UpdateDirection(); 
        } 
    }



    private void WallDetection(){
        hit = Physics2D.Raycast(groundDetection.position, RayWallDirection, RayCheckDistance, Wall);
        Debug.DrawRay(groundDetection.position, RayWallDirection * RayCheckDistance, Color.red);
        if (hit.collider != null) 
        {
            enemyLiReSpeed *= -1; 
            UpdateDirection(); 
        } 
    }

    
    private void UpdateDirection(){
         if (enemyLiReSpeed < 0)
        {
            transform.eulerAngles = new Vector2(0, 0);
            RayWallDirection = Vector2.left;

        }
        else
        {
            transform.eulerAngles = new Vector2(0, 180);
            RayWallDirection = Vector2.right;
        }
    }
    
}



