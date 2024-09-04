using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            ResetCheckpoint();
            ResetPlayerPrefsSeed();
            SceneManager.LoadScene("TestProzGen");
        }
    }

    private void ResetCheckpoint() {
        Checkpoint.lastCheckpoint = Vector2.zero;
        Checkpoint.savedCoins = 0;
    }

    private void ResetPlayerPrefsSeed(){
        PlayerPrefs.DeleteKey("Seed");
        PlayerPrefs.Save();
    }
}


