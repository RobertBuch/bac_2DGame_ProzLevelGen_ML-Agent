using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UISelectLevel : MonoBehaviour
{
    private List<string> seedList;
    [SerializeField] private List<Button> scrollViewSeedButtons;
    private string loadSeeds; 
    private bool difficultyHard;
    private TextMeshProUGUI seed;
    private string startSeed;

    [SerializeField] Button easy;
    [SerializeField] Button hard;

    
    // Start is called before the first frame update
    void Start()
    {
        seedList = new List<string>();

        LoadseedList();
        difficultyHard = false;
        easy.interactable = false;
        hard.interactable = true;
      

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnEasyButtonClicked(){
        difficultyHard = false;
        easy.interactable = false;
        hard.interactable = true;
    }

    public void OnHardButtonClicked(){
        difficultyHard = true;
        easy.interactable = true;
        hard.interactable = false;
    }

  


    private void EasyHard( bool difficultyHard ){

            PlayerPrefs.SetInt("difficulty", difficultyHard ? 1 : 0);
            PlayerPrefs.Save();
        
    }






    public void OnSeedButtonClicked(Button button)
    {
        seed = button.GetComponentInChildren<TextMeshProUGUI>();
        if (seed != null && seed.text != "Empty")
        {
            startSeed = seed.text;
            Play(startSeed);
        }
        
    }


    private void Play(string startSeed){
        EasyHard(difficultyHard);

        PlayerPrefs.SetString("Seed", startSeed);
        PlayerPrefs.Save();

        Checkpoint.lastCheckpoint = Vector2.zero; 
        Checkpoint.savedCoins = 0; 
        
        SceneManager.LoadScene("TestProzGen");
            
    }



    private void LoadseedList(){
        
        if (PlayerPrefs.HasKey("seedList")){
            loadSeeds = PlayerPrefs.GetString("seedList", "");


            if(!string.IsNullOrEmpty(loadSeeds)){
                seedList = loadSeeds.Split(",").ToList();

                SetButtons();
            }
        }
    }


    private void SetButtons(){
        int seedIndex = seedList.Count - 1;
        for (int i = 0; i < scrollViewSeedButtons.Count; i++){
            if (seedIndex >= 0) {

                scrollViewSeedButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = seedList[seedIndex];
                seedIndex--;
            }
        }
    }



}
