using UnityEngine.Events;
using UnityEngine;

public class Player : MonoBehaviour
{   

    [Header("jumping")]
    private Rigidbody2D rb;
    public float jumpPower; 
	private bool ground; 
	private bool jumpPad;  
	const float groundCircle = 0.2f; 
	private float jumpPadBoost;
	private float jumpBoost;

	[SerializeField] private Transform checkGround;		
	[SerializeField] private LayerMask groundLayer;	


    [Header("animator")]

    private Animator animator;
    
    [Header("respawn")]

    public GameObject ParticleDeath;
    public GameObject restartMenu;
    public GameObject shadowPanel;
    private Vector2 startPlayer; 

    [Header("mobile")]
    public Joystick joy;
    private float mobileInputPlayerX;
	public float speed = 25f;
	bool jump = false;

	[Header("UnityEvents")]
	private Vector3 vc; 

	//private AudioSource audioSource;

	[Header("UnityGroundEvent")]

	public UnityEvent Ground;

	[System.Serializable]
	public class MoveEvent : UnityEvent<bool> { }


    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
		vc = Vector3.zero; 
		jumpBoost = 0f;
		jumpPower = 14f;
		jumpPadBoost = 10f;	
		jumpPad = false;	
		
		if (Ground == null){
			Ground = new UnityEvent();
		}
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        // spawn Position
        RespawnPlayer();

        
        // audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
    }


    private void FixedUpdate() {

        Movement(mobileInputPlayerX * Time.fixedDeltaTime, jump);
		jump = false;

        CheckGround();

  		if (ground && jumpPad) {
        	Jumping();
    	}

    }

	private void CheckGround(){
		bool grounded = ground;
		ground = false;

		Collider2D[] coll = Physics2D.OverlapCircleAll(checkGround.position, groundCircle, groundLayer);
		foreach (Collider2D collider in coll)
		{
			if (collider.gameObject != gameObject)
			{	
				ground = true;
				if (!grounded)
				{
					Ground.Invoke();
				}
			}
		}
	}
    private void PlayerInput(){  

        if (joy.Horizontal >= 0.2f)
		{        
            mobileInputPlayerX = speed;
            transform.localScale = new Vector3(1, 1, 1);
			animator.SetBool("runPlayer", true);
        } else if (joy.Horizontal <= -0.2f)
        {

            mobileInputPlayerX = -speed;
            transform.localScale = new Vector3(-1, 1, 1);
			animator.SetBool("runPlayer", true);
        } else{
            mobileInputPlayerX = 0f;
			animator.SetBool("runPlayer", false);
        }

		if ( joy.Vertical >= 0.5f )
		{
			jump = true;
			animator.SetBool("jumpPlayer", true);
			
		} else {
			animator.SetBool("jumpPlayer", false);
		}
    }


	private void OnTriggerStay2D(Collider2D check) {
		if ( check.gameObject.CompareTag("agentJumpPad")){
			jumpPad = true;
		}
	}
	private void OnTriggerExit2D(Collider2D check) {
		if (check.gameObject.CompareTag("agentJumpPad")){
        	jumpPad = false;
    	}
	}

   private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Trap"))
        {

            DeadPlayer();  
        }
    }


    private void DeadPlayer(){ 
        // AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
        //audioSource.PlayOneShot(audioSource.clip);
        gameObject.SetActive(false);
        restartMenu.SetActive(true);
        shadowPanel.SetActive(true);

    

    }

    private void RespawnPlayer(){
        restartMenu.SetActive(false);
        shadowPanel.SetActive(false);
        gameObject.SetActive(true);

        Debug.Log("respawn");
        if (Checkpoint.lastCheckpoint != Vector2.zero){
            transform.position = Checkpoint.lastCheckpoint;

        } else {

            transform.position = startPlayer;
            
        }

    }

    public void SetStartPos(Vector2 newPosition){
        startPlayer = newPosition;
    }


    public void Die(){
        DeadPlayer();
    }
  

	private void Jumping(){
		jumpBoost = 0;
		if (jumpPad)
		{
				jumpBoost = jumpPadBoost; 
		}
		rb.velocity = new Vector2(rb.velocity.x, 0); 
		rb.AddForce(new Vector2(0f, jumpPower  + jumpBoost),ForceMode2D.Impulse);
		// audioSource.Play();
		ground = false;
	}



	public void Movement(float mobileInputPlayerX, bool jump)
	{
			Vector2 targetVelocity = new Vector2(mobileInputPlayerX * 10f, rb.velocity.y); 
			//  Smoothing to avoid gaining lateral momentum at platform edges.
			rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref vc, 0.07f); 
			
		if (ground && jump)
		{
			Jumping();
		}
	}
}









