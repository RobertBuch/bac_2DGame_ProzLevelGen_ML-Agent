using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsParent : MonoBehaviour
{
    public TMP_Text textCoinCounter;
    public static int coins = 0;
   

    void Awake() {
       
    }

    // Start is called before the first frame update
    void Start()
    {
        coins = Checkpoint.savedCoins;
        TextCoinCounter(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void CoinCounter(){
        coins++;
        TextCoinCounter();
    } 

    private void TextCoinCounter(){
          textCoinCounter.text = coins.ToString();
        
    }


}
