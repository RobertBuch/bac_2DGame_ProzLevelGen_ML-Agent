using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIPlay : MonoBehaviour
{



    private string fieldSeed;
    private const int minSeed = 0;
    private const int maxSeed = 999999;
    Regex regex;

    [SerializeField] TMP_InputField seedInputField;


    [SerializeField] bool difficultyHard;
    [SerializeField] Button easy;
    [SerializeField] Button hard;



    // Start is called before the first frame update
    void Start()
    {
        difficultyHard = false;
        easy.interactable = false;
        hard.interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    
    public void OnPlayButtonClicked (){
        Play();
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

    private void EasyHard( bool hardCheck ){

            PlayerPrefs.SetInt("difficulty", difficultyHard ? 1 : 0);
            PlayerPrefs.Save();
        
    }






    private void Play(){
        EasyHard(difficultyHard);
  
        fieldSeed = seedInputField.text;


        if (string.IsNullOrEmpty(fieldSeed) || checkSeedInputField(fieldSeed))
        {
            if (!string.IsNullOrEmpty(fieldSeed)){
                PlayerPrefs.SetString("Seed", fieldSeed);
                PlayerPrefs.Save();
            }else if (string.IsNullOrEmpty(fieldSeed)){
                // sonst lädt er den alten seed
                PlayerPrefs.DeleteKey("Seed");
            }       
            
            
            Checkpoint.lastCheckpoint = Vector2.zero; 
            Checkpoint.savedCoins = 0;

            SceneManager.LoadScene("TestProzGen");
            
            
        }  else {
            seedInputField.text = "";
        }

    }



    private bool checkSeedInputField(string seed){

        regex = new(@"^\d+$");

        if (regex.IsMatch(seed))
        {
            // check numbers
            int seedValue = int.Parse(seed);
            return seedValue >= minSeed && seedValue <= maxSeed;
        }

        return false;



    }




    










}
