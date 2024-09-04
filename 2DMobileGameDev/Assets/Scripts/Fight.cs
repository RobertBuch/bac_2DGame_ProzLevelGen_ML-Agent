using UnityEngine;

public class Fight : MonoBehaviour
{

    public float fightRange = 1.2f;
    private Animator animator;
    public Transform fightPosition;
    public LayerMask Enemy; 
    public SpriteRenderer sword;
    public GameObject ParticleEnemyDeath;


    private bool mobileButtonFight = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sword.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (mobileButtonFight == true){
            animator.SetTrigger("fightPlayer");
            Hit();
            mobileButtonFight = false;
        }
    }


    public void MobileFight(){
        mobileButtonFight = true;
    }

    private void Hit(){
        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(fightPosition.position, fightRange, Enemy);
        foreach (Collider2D hit in hitEnemy){
            Instantiate(ParticleEnemyDeath, hit.transform.position, Quaternion.identity);
            Destroy(hit.gameObject);
        }
    }


    public void ShowSword(){
        sword.enabled = true;
    }

    public void HideSword(){
        sword.enabled = false;
    }

 
}
