using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{

    [SerializeField] GameObject MainMenuPanel;
    [SerializeField] GameObject InputSeedPanel;
    [SerializeField] GameObject SelectLevelPanel;
    [SerializeField] GameObject OptionsPanel;


    // Start is called before the first frame update
    void Start()
    {
        Checkpoint.lastCheckpoint = Vector2.zero; 
        Checkpoint.savedCoins = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnPlayButtonClicked (){
        ShowInputSeedPanel();
    }

    public void OnSelectLevelButtonClicked(){
        ShowSelectLevelPanel();
    }

    public void OnOptionButtonClicked(){
        ShowOptionsPanel();
    }

    public void OnQuitButtonClicked(){
        Quit();
    }


    private void ShowInputSeedPanel(){
        MainMenuPanel.SetActive(false);
        InputSeedPanel.SetActive(true);

    }

    private void ShowSelectLevelPanel(){

        MainMenuPanel.SetActive(false);
        SelectLevelPanel.SetActive(true);
        Checkpoint.lastCheckpoint = Vector2.zero; 
        Checkpoint.savedCoins = 0; 
    }

    private void ShowOptionsPanel(){
        MainMenuPanel.SetActive(false);
        OptionsPanel.SetActive(true);
    }


    private void Quit() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_ANDROID
        AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
            .GetStatic<AndroidJavaObject>("currentActivity");
        activity.Call("finishAndRemoveTask");
        #else
        Application.Quit();
        #endif
    }


}
