
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;



public class TestProzLevelGen : MonoBehaviour
{

    [Header("seed")]
    private string seed; 
    private List<string> seedList;
    private string loadSeeds; 
    private System.Random sysRandom;

    [Header("platform generation")]
    private int width;
    private int height;
    private int startPoint;
    private bool danger;
    private bool onlyCheckP;
    private bool falling;
    private bool setJumpPad;
    private bool difficultyHard;
    [SerializeField] Tilemap GroundTilemapProzGen;
    [SerializeField] TileBase tile;
    private int randomGap;
    private bool isGap;
    private List<TestProzLevelGen> platforms;
    private List<TestProzLevelGen> wholeWorld;
    [SerializeField] BoxCollider2D deathZone;
   

    [Header("Moving Platform & JumpPad")]
    [SerializeField] GameObject movingPlatformY;
    [SerializeField] GameObject jumpPad;

    [Header("Player")]
    [SerializeField] Player player;



    [Header("Place Gameobjects")]
    public List <GameObject> objects;
    [SerializeField] GameObject coin;
    [SerializeField]CoinsParent coinsParentScript;
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject checkpoint;
    [SerializeField] GameObject finish;
    [SerializeField] GameObject spikes;




    [Header("Parents")]
    [SerializeField] Transform enemyParent;
    [SerializeField] Transform yMovingPlatformsParent;
    [SerializeField] Transform spikeParent;
    [SerializeField] Transform flagParent;
    [SerializeField] Transform coinParent;
    [SerializeField] Transform fallingParent;
    [SerializeField] Transform JumpPadParent;
 
   
    [Header("DifficultyHard: Material for falling platforms")]
    [SerializeField] GameObject fallingPlatf2Block;
    [SerializeField] GameObject fallingPlatf2BlockNoEnd;
    [SerializeField] GameObject fallingPlatf3Block;
    [SerializeField] GameObject fallingPlatf3BlockNoEnd;
    private int fallingStart;
    private int fallingEnd;
    [SerializeField] Tilemap fallPlatfTileMap;
    [SerializeField] TileBase fallPlatfLeftTile;
    [SerializeField] TileBase fallPlatfCenterTile;
    [SerializeField] TileBase fallPlatfRightTile;



  


    // constructor
    public TestProzLevelGen(int startPoint, int height, int width, bool isGap, bool danger, bool onlyCheckP, bool falling, bool setJumpPad){
        this.width = width;
        this.height = height;
        this.startPoint = startPoint;
        this.isGap = isGap;
        this.danger = danger;
        this.onlyCheckP = onlyCheckP;
        this.falling = falling;
        this.setJumpPad = setJumpPad;

    }



    // Start is called before the first frame update
    void Start()
    {   
        

        difficultyHard = PlayerPrefs.GetInt("difficulty", 0) == 1;
        
        //startPoint = 0;
        randomGap = 0;
        fallingStart = 0;
        fallingEnd = 0;
        isGap = false;
        danger = false;
        onlyCheckP = false;
        falling = false;
        setJumpPad = false;
        platforms = new List<TestProzLevelGen>();
        wholeWorld = new List<TestProzLevelGen>();
        objects = new List<GameObject>();
        sysRandom = new System.Random();
        seedList = new List<string>();
        // Debug.Log(platforms);
        GeneratePlatforms();
        platforms.Clear();
        
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    
    private void GeneratePlatforms(){


        GenerateSeed();
     
        int counter = 0; 

        while( counter < 25){  
            counter++;
            RandomHeight();
            RandomWidth();

            BuildPlatforms();
            
        }
        WholeWorld();
        PlaceWorld();
        
    }


    private void GenerateSeed(){

        if (PlayerPrefs.HasKey("Seed"))
        {
            seed = PlayerPrefs.GetString("Seed", "");
        }



        if (string.IsNullOrEmpty(seed)){
            seed = sysRandom.Next(0, 999999).ToString();
            PlayerPrefs.SetString("Seed", seed);
            PlayerPrefs.Save();
        }

        

        sysRandom = new System.Random(seed.GetHashCode()); //1
        Debug.Log("prozlevelgen seed = " + seed);
        Debug.Log("difficulty = " + difficultyHard);
        LoadSeedList();
        SaveSeedinList(seed);
    }


    private void LoadSeedList(){
        if (PlayerPrefs.HasKey("seedList")){
            loadSeeds = PlayerPrefs.GetString("seedList", "");

            if(!string.IsNullOrEmpty(loadSeeds)){
                seedList = loadSeeds.Split(",").ToList();
            }
        }
    }

    private void SaveSeedinList(string seed){
        if( seedList.Contains(seed)){
            seedList.Remove(seed);
        }
        seedList.Add(seed);

        if (seedList.Count > 5){
            seedList.RemoveAt(0);
        }

        PlayerPrefs.SetString("seedList", string.Join(",", seedList));
        PlayerPrefs.Save();
    }



    private void RandomHeight(){
        // width random between -1 and 6
        // if more than y0 auf y2, then set a movingplatform vertical
        
        int minHeight = 0;
        int maxHeight = 0;

        if (sysRandom.NextDouble() < 0.25){
            height = sysRandom.Next(minHeight, maxHeight);
        } else{
            // max 4
            height = sysRandom.Next(-1, 7);
        }
    }



    private void RandomWidth(){
        width = sysRandom.Next(2, 13);

    }



    private void BuildPlatforms(){
            TestProzLevelGen platform = new TestProzLevelGen(startPoint, height, width, false, false, false, false, false);
            startPoint += width;
            platforms.Add(platform);               
    }



    private void WholeWorld(){
        int startPointx = 0;
        int previousHeight = 0;
        int previousPlatfEnd = 0;
        int prevI = 0;
        int counter = 0;
        int lastPlatformIndex = -1; 
        bool firstPlatform = true;
        bool skip = false;
        

        for (int i = 0; i < platforms.Count; i++){

            if( firstPlatform == false && platforms[i].height - previousHeight > 3 ){     

                // player jump: 1.5 to 4.5
                if(sysRandom.NextDouble() < 0.5){ 
                    SetMovingPlatform(startPointx+2.35f); 
                    skip = true;
                    startPointx+=3;
                } else {
                    previousPlatfEnd += platforms[prevI].width;
                    SetJumpPad(previousPlatfEnd, previousHeight);
                    if (lastPlatformIndex != -1) {
                        wholeWorld[lastPlatformIndex].setJumpPad = true; 
                    }
                }
                
   

            } else{
                firstPlatform = false;
            }
            
            previousHeight = platforms[i].height;
            previousPlatfEnd = startPointx;
            prevI = i;
  
            
            // Place platforms
            if ( skip == false ){
                
                wholeWorld.Add(new TestProzLevelGen(startPointx, platforms[i].height, platforms[i].width, false, false, false, false, false));
                startPointx += platforms[i].width;
                counter += 1;
                // prev platf, no gaps
                lastPlatformIndex = counter - 1; 

            } else {
                skip = false;
                i--; 
                
            }
           
            // Place gaps
            // player can jump 7(-1) blocks // max 3
            if ( difficultyHard == true ){
                randomGap = sysRandom.Next(0, 4); 
                if ( !(randomGap == 0)){
                    wholeWorld.Add(new TestProzLevelGen(startPointx, 0, randomGap, true, false, false, false, false)); 
                    startPointx += randomGap; 
                    counter += 1; 
                } 
            }
             
        }
        PlaceFinishFlag();
        PlaceObjects(startPointx);
    }










    private void PlaceFinishFlag(){
        for (int i = wholeWorld.Count - 1; i >= 0; i--) {
            if (!wholeWorld[i].isGap) {
                PlaceFinish(wholeWorld[i]);
                wholeWorld[i].onlyCheckP = true;
                break;
            }
        }
    }




     private void PlaceFinish(TestProzLevelGen platf){
        // always at the end of the level
        finish = Instantiate(finish, new Vector2(platf.startPoint + platf.width-1, (float)(platf.height + 1.5)), Quaternion.identity);
        finish.transform.SetParent(flagParent.transform, true);  
    }






    private void SetJumpPad( int previousPlatformEnd, int previousHeight){
        jumpPad = Instantiate(jumpPad, new Vector2(previousPlatformEnd -1 , previousHeight + 1.25f), Quaternion.identity);
        jumpPad.transform.SetParent(JumpPadParent.transform, true);  
    }
 
    






    private void SetMovingPlatform(float previousPlatformEnd){
        //movingPlatformY = Instantiate(movingPlatformY, new Vector2(previousPlatformEnd + 0.15f, 3.5f), Quaternion.identity);
        movingPlatformY = Instantiate(movingPlatformY, new Vector2(previousPlatformEnd, 3.5f), Quaternion.identity); 
        movingPlatformY.transform.SetParent(yMovingPlatformsParent.transform, true);  
    }



   private void PlaceWorld(){
        int index = 0;
   
        foreach(TestProzLevelGen groundTile in wholeWorld ){
            if (groundTile.isGap == false ){
                for (int i = groundTile.startPoint; i < groundTile.startPoint + groundTile.width; i++){
                    
                    if ( groundTile.falling == false ){

                        Vector3Int pos = new Vector3Int(i, groundTile.height, 0);
                        GroundTilemapProzGen.SetTile(pos, tile);

                        // place stable ground
                       if( groundTile.width > 3 ){
                            StableGround(groundTile, index, i);    
                       }
                                      
                    } else { 
                        Vector3Int fallPos = new Vector3Int(i, groundTile.height, 0);
                        if ( !(i >= groundTile.fallingStart && i <= groundTile.fallingEnd) ){

                            if( i == groundTile.startPoint){
                                fallPlatfTileMap.SetTile(fallPos, fallPlatfLeftTile);
                            } else if(i == groundTile.startPoint + groundTile.width-1){ 
                                fallPlatfTileMap.SetTile(fallPos, fallPlatfRightTile);
                            } else{
                                fallPlatfTileMap.SetTile(fallPos, fallPlatfCenterTile);
                            }
                        } else {
                            continue;
                        }
                    }
                } 
            } index++;
        }
    }

    
    // i = xblocks, index = platformindex; stableGround = creates ground under the platform
    private void StableGround(TestProzLevelGen groundTile, int index, int i){
        Vector3Int pos = new();

    
            if ( index < wholeWorld.Count  ) {

                for( int j = groundTile.height; j > -6; j--){       
                    pos = new Vector3Int(i, j, 0);
                    GroundTilemapProzGen.SetTile(pos, tile);
                }
            }
    }
    


    private void PlaceObjects(int worldLength){

        int quarterWorldLength = worldLength / 3;
        int xlength = 0;
        bool firstPlatform = true;
        int checkpointCounter = 0;
        bool noMoreCheckpoints = false;
        

        for (int i = 0; i < wholeWorld.Count; i++){
   
            xlength += wholeWorld[i].width;

            if ( noMoreCheckpoints == false && quarterWorldLength <= xlength && wholeWorld[i].isGap == false && wholeWorld[i].setJumpPad == false ){

                // 3 flags
                if( checkpointCounter < 2 ){
                    PlaceCheckPoint(wholeWorld[i]);
                    quarterWorldLength += worldLength / 3;
                    wholeWorld[i].onlyCheckP = true;
                    checkpointCounter++;
                    
                } else {
                    noMoreCheckpoints = true;
                }

            }



            if(firstPlatform == false){
                // set coins and enemys
                if ( wholeWorld[i].onlyCheckP == false && wholeWorld[i].isGap == false ){

                    PlaceCoins(wholeWorld[i]);

                    if (wholeWorld[i].setJumpPad == false){

                        // set spikes and enemys
                        if ( wholeWorld[i].width > 2 && sysRandom.NextDouble() < 0.7  ){
                            PlaceSpikes(wholeWorld[i]);
                            wholeWorld[i].danger = true;
                        }

                        // set enemys on emptys platforms
                        if ( wholeWorld[i].danger == false && sysRandom.NextDouble() < 0.5 && wholeWorld[i].width > 3){
                            PlaceEnemey(wholeWorld[i]);
                            wholeWorld[i].danger = true;
                        }

                        if( wholeWorld[i].onlyCheckP == false && wholeWorld[i].danger == false && sysRandom.NextDouble() < 0.5 ){
                            PlaceFallingPlatf(wholeWorld[i]);
                        }
                    }
                }
            } else{
                firstPlatform = false;
                SetPlayer(wholeWorld[i]);
            }
        } 
        DeathZone(worldLength);
    }

    

    
    private void DeathZone(int worldLength){
        int littleMoreSpace = 50;
        int deathZoneLength = worldLength + littleMoreSpace + 20;
        // gameobject position x = -20, y= -5
        deathZone.size = new Vector2(deathZoneLength, 0.5f);
        deathZone.offset = new Vector2(deathZoneLength / 2, 0);

    }


  

    private void PlaceCheckPoint(TestProzLevelGen platf){

        int block = RandomNumber(platf.startPoint+1, platf.startPoint + platf.width-1);
        checkpoint = Instantiate(checkpoint, new Vector2(block, (float)(platf.height + 1.5)), Quaternion.identity);
        checkpoint.transform.SetParent(flagParent.transform, true);  
    }



   



    private void PlaceCoins(TestProzLevelGen platf){

        int block = RandomNumber(platf.startPoint+1, platf.startPoint + platf.width-1);
        coin = Instantiate(coin, new Vector2(block, (float)(platf.height + 3.15)), Quaternion.identity);
        coin.transform.SetParent(coinParent.transform, true);  
        coin.GetComponent<Coins>().coinsParent = coinsParentScript;
    }



    private void PlaceSpikes(TestProzLevelGen platf){
        List <float> positions = new();
        List <float> freePos = new();

        float xPosition=0;
        int spikesCounter = 0;
        int freePosCounter = 0;
            // - spikes // never at the beginning and end of a platform, and not forever in a row, and never on a platform with 2blocks

        int max = platf.width /2;
        int quantity = RandomNumber(1 , max); 

        // delete position = first block and last block

        for (float i = 0; i < platf.width; i++){    
            positions.Add(platf.startPoint + i);
            freePos.Add(platf.startPoint + i);
        }

        if (platf.width == 3){
           // always in the center of the 3 block platform
           spikes = Instantiate(spikes, new Vector2((float)(platf.startPoint+1.5), (float)(platf.height + 1.5f)), Quaternion.identity);
           spikes.transform.SetParent(spikeParent.transform, true); 
           platf.danger = true; 
        }else {

            for (int i = 0; i < quantity; i++){
                // 4 spikes in a row possible
                if ( positions.Count > 0 && spikesCounter < 3){
                    int j = 0;
                    do{
                        // +2 blocks, -2 blocks
                        j = RandomNumber(2, positions.Count-1);
                    }while (positions[j] == -1);

                    xPosition = positions[j];
                    spikes = Instantiate(spikes, new Vector2(xPosition, (float)(platf.height + 1.5f)), Quaternion.identity);
                    spikes.transform.SetParent(spikeParent.transform, true);   
                    positions[j] = -1;
                    platf.danger = true;
                    spikesCounter++;
                }
            }
        }


        // check free space min. 4 block
        // set enemys
        for(int i = 0; i < positions.Count; i++){               
                if (positions[i] != -1)
                {
                    freePosCounter++;

                    if(freePosCounter == 4){
                        enemy = Instantiate(enemy, new Vector2(freePos[i-1], platf.height + 2), Quaternion.identity);
                        enemy.transform.SetParent(enemyParent.transform, true);  
                        freePosCounter = 0;
                    }
                }else {
                    freePosCounter = 0;
                }
        }
    }



   private void PlaceEnemey(TestProzLevelGen platf){
        // enemys always start in the middle of a platform, and never on a platform with 2blocks or 3blocks
        int place = platf.startPoint + platf.width/2;
        enemy = Instantiate(enemy, new Vector2(place, platf.height + 2), Quaternion.identity); 
        enemy.transform.SetParent(enemyParent.transform, true); 
        platf.danger = true;  
    } 


    private void PlaceFallingPlatf(TestProzLevelGen platf){

        if(platf.width == 2){
            fallingPlatf2Block = Instantiate(fallingPlatf2Block, new Vector2(platf.startPoint+ 0.5f, platf.height+ 0.5f), Quaternion.identity); 
            fallingPlatf2Block.transform.SetParent(fallingParent.transform, true); 
            platf.isGap = true;

        } else if(platf.width == 3){
            fallingPlatf3Block = Instantiate(fallingPlatf3Block, new Vector2(platf.startPoint + 0.5f, platf.height + 0.5f), Quaternion.identity); 
            fallingPlatf3Block.transform.SetParent(fallingParent.transform, true); 
            platf.isGap = true;
          
        } else if(platf.width == 4){
            platf.fallingStart = platf.startPoint + 1;
            platf.fallingEnd = platf.startPoint + platf.width - 2;
            fallingPlatf2BlockNoEnd = Instantiate(fallingPlatf2BlockNoEnd, new Vector2(platf.fallingStart + 0.5f, platf.height + 0.5f), Quaternion.identity); 
            fallingPlatf2BlockNoEnd.transform.SetParent(fallingParent.transform, true); 
            platf.falling = true; 

        } else {
            // dont touch first and last block, 2. fallingplatform length = 3block, random max is exclusive
            platf.fallingStart = RandomNumber(platf.startPoint + 1, platf.startPoint + platf.width - 3);
            platf.fallingEnd = platf.fallingStart + 2;
  
            fallingPlatf3BlockNoEnd = Instantiate(fallingPlatf3BlockNoEnd, new Vector2(platf.fallingStart + 0.5f, platf.height + 0.5f), Quaternion.identity); 
            fallingPlatf3BlockNoEnd.transform.SetParent(fallingParent.transform, true);
            platf.falling = true; 
 

        }
    }





    private void SetPlayer(TestProzLevelGen platf){
        // on the first platform
        player.SetStartPos(new Vector2(platf.startPoint + 1.5f, platf.height + 2));
    }



    private int RandomNumber(int min, int max){
        return sysRandom.Next(min, max);
    }
}










