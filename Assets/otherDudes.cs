using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class otherDudes : MonoBehaviour
{
	public GameObject enemies;
    Vector3[] newEnemPos;
    public int[] scores;
    public int[] gauges;
    public int max;
    public Tilemap flowerRef;
    flowerSet fscript;
	int children = 15;
	int total;
    bool waiting = false;
    int movespeed = 15;
    int smallP = 5;
    int bigP = 10;
    scoreboard scoreB;
    float startPosX;
    float startPosY;
    GameObject player;
    playerControls pInfo;
    
    // Start is called before the first frame update
    void Start()
    {
    	GameObject ground = GameObject.Find("ground");
        tileSet groundtiles = ground.GetComponent<tileSet>();
        total = groundtiles.total;

        GameObject foliage = GameObject.Find("foliage");
        fscript = foliage.GetComponent<flowerSet>();
        flowerRef = foliage.GetComponent<Tilemap>();

        GameObject canvas = GameObject.Find("Canvas");
        scoreB = canvas.GetComponent<scoreboard>();

        player = GameObject.Find("player");
        pInfo = player.GetComponent<playerControls>();

        gauges = new int[children];
        scores = new int[children];
        

        for(int i = 0; i < children; i++){
        	Instantiate(enemies, transform);
            gauges[i] = 0;
            scores[i] = 0;
            
        }
        startPosX = fscript.startX + 2.5f;
        startPosY = fscript.startY + 2.5f;
        for(int i = 0; i < transform.childCount; i++){
        	Transform child = transform.GetChild(i);
        	child.transform.position = new Vector3(startPosX, startPosY, -1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(fscript.flowerNumbers <= 0 && pInfo.gauge <= 0){
            Time.timeScale = 0;
        }
        if(!waiting){
            StartCoroutine(wait());
        }else{
            moveEnem();
        }
    }

    void moveEnem(){
        for(int i = 0; i < transform.childCount; i++){
            Transform child = transform.GetChild(i);
            Vector3 target = newEnemPos[i];
            if(target.x - child.transform.position.x == 1){
                SpriteRenderer rend = child.GetComponent<SpriteRenderer>();
                rend.flipX = false;
            }else if(target.x - child.transform.position.x == -1){
                SpriteRenderer rend = child.GetComponent<SpriteRenderer>();
                rend.flipX = true;
            }

            child.transform.position = Vector3.Lerp(child.transform.position, target, Time.deltaTime*movespeed);

            //check if overlapping flower
            if(gauges[i] <= 100-smallP && (int)(child.transform.position.y-.5f) >=0 && (int)(child.transform.position.x-.5f)>=0 && (int)(child.transform.position.y) < total && (int)(child.transform.position.x) < total && 
                (fscript.flowerTiles[(int)(child.transform.position.y-.5f), (int)(child.transform.position.x-.5f)] == 1 || fscript.flowerTiles[(int)(child.transform.position.y), (int)(child.transform.position.x-.5f)] == 1 || 
                                fscript.flowerTiles[(int)(child.transform.position.y-.5f), (int)(child.transform.position.x)] == 1)){

                Vector3Int pos;
                if(fscript.flowerTiles[(int)(child.transform.position.y-.5f), (int)(child.transform.position.x)] == 1){
                    pos = new Vector3Int((int)(child.transform.position.x),(int)(child.transform.position.y-.5f),0);
                }else if(fscript.flowerTiles[(int)(child.transform.position.y), (int)(child.transform.position.x-.5f)] == 1){
                    pos = new Vector3Int((int)(child.transform.position.x-.5f),(int)(child.transform.position.y),0);
                }else{
                    pos = new Vector3Int((int)(child.transform.position.x-.5f),(int)(child.transform.position.y-.5f),0);
                }

                if(flowerRef.GetSprite(pos) == fscript.small[0].sprite || 
                    flowerRef.GetSprite(pos) == fscript.small[1].sprite || 
                    flowerRef.GetSprite(pos) == fscript.small[2].sprite){

                    flowerRef.SetTile(pos,fscript.unuseable[0]);
                    fscript.flowerNumbers -= 1;
                    fscript.flowerTiles[pos.y,pos.x] = 0;
                    gauges[i] += smallP;
                }

                if(gauges[i] <= 100-bigP){
                    if(flowerRef.GetSprite(pos) == fscript.big[0].sprite){
                        changebigFlowers(1,pos,i);
                    }else if(flowerRef.GetSprite(pos) == fscript.big[1].sprite){
                        changebigFlowers(2,pos,i);
                    }else if(flowerRef.GetSprite(pos) == fscript.big[2].sprite){
                        changebigFlowers(3,pos,i);
                    }
                }
            }else if(gauges[i] <= 100-bigP && (int)(child.transform.position.y-.5f)-1 >=2 && (int)(child.transform.position.x-.5f)-1>=2 && (int)(child.transform.position.y-.5f) < total-2 && (int)(child.transform.position.x-.5f) < total-2 && 
                    (fscript.flowerTiles[(int)(child.transform.position.y-.5f)-1, (int)(child.transform.position.x-.5f)-1] == 1 || fscript.flowerTiles[(int)(child.transform.position.y)-1, (int)(child.transform.position.x-.5f)-1] == 1 ||
                        fscript.flowerTiles[(int)(child.transform.position.y-.5f)-1, (int)(child.transform.position.x)-1] == 1)){

                Vector3Int pos;
                if(fscript.flowerTiles[(int)(child.transform.position.y-.5f)-1, (int)(child.transform.position.x)-1] == 1){
                    pos = new Vector3Int((int)(child.transform.position.x)-1,(int)(child.transform.position.y-.5f)-1,0);
                }else if(fscript.flowerTiles[(int)(child.transform.position.y)-1, (int)(child.transform.position.x-.5f)-1] == 1){
                    pos = new Vector3Int((int)(child.transform.position.x-.5f)-1,(int)(child.transform.position.y)-1,0);
                }else{
                    pos = new Vector3Int((int)(child.transform.position.x-.5f)-1,(int)(child.transform.position.y-.5f)-1,0);
                }
                
                if(flowerRef.GetSprite(pos) == fscript.big[0].sprite){
                    changebigFlowers(1,pos, i);
                }else if(flowerRef.GetSprite(pos) == fscript.big[1].sprite){
                    changebigFlowers(2,pos, i);
                }else if(flowerRef.GetSprite(pos) == fscript.big[2].sprite){
                    changebigFlowers(3,pos, i);
                }

            }

            //check overlap hive
            if(gauges[i] > 0 && child.transform.position.y >= fscript.startY && child.transform.position.y < fscript.startY + 5 && 
                child.transform.position.x >= fscript.startX && child.transform.position.x < fscript.startX + 5){
                scores[i] += gauges[i];
                gauges[i] = 0;
                scoreB.changePoints(scores[i]);
            }

            
        }
    }

    void changebigFlowers(int index, Vector3Int pos, int child){
        flowerRef.SetTile(pos, fscript.unuseable[index]);
        fscript.flowerNumbers -= 4;
        for(int y = 0; y < 2; y++){
            for(int x = 0; x < 2; x++){
                fscript.flowerTiles[pos.y + y, pos.x + x] = 0;
            }
        }
        gauges[child] += bigP;
    }

    Vector3[] getNewPositions(){
        Vector3[] newPos = new Vector3[children];

        Vector3[] directions = new Vector3[8]{Vector3.left, Vector3.right, Vector3.down, Vector3.up,
                                            new Vector3(1,1,0),new Vector3(1,-1,0),new Vector3(-1,1,0),new Vector3(-1,-1,0)};
        for(int i = 0; i < transform.childCount; i++){
            Transform child = transform.GetChild(i);
            int childX = (int)child.localPosition.x;
            int childY = (int)child.localPosition.y;
            List<Vector3> filt = new List<Vector3>();
            List<Vector3> Flowerfilt = new List<Vector3>();
            
            if(gauges[i] >= 100){
                int x;
                int y;
                if(child.transform.position.x > startPosX){
                    x = -1;
                }else if(child.transform.position.x < startPosX){
                    x = 1;
                }else{
                    x = 0;
                }
                if(child.transform.position.y > startPosY){
                    y = -1;
                }else if(child.transform.position.y < startPosY){
                    y = 1;
                }else{
                    y = 0;
                }
                newPos[i] = new Vector3(child.transform.position.x + x,child.transform.position.y + y,-1);
                continue;
            }

            //check if too close to player
            if(pInfo.repel && child.transform.position.x >= player.transform.position.x - 3 && child.transform.position.x < player.transform.position.x + 3 &&
                 child.transform.position.y >= player.transform.position.y - 3 && child.transform.position.y < player.transform.position.y + 3){

                int x;
                int y;

                if(child.transform.position.x < player.transform.position.x && child.transform.position.x - 2f >= 0){
                    x = -1;
                }else if(child.transform.position.x >= player.transform.position.x && child.transform.position.x + 2f < total){
                    x = 1;
                }else{
                    x = 0;
                }
                if(child.transform.position.y < player.transform.position.x && child.transform.position.y - 2f >= 0){
                    y = -1;
                }else if(child.transform.position.y >= player.transform.position.x && child.transform.position.y + 2f < total){
                    y = 1;
                }else{
                    y = 0;
                }

                newPos[i] = new Vector3(child.transform.position.x + x,child.transform.position.y + y,-1);
                continue;
            }
            
            if(child.transform.position.y <= 2 || child.transform.position.x <= 2 || 
                child.transform.position.y > total - 2 || child.transform.position.x > total-2){
                int y = 1;
                int x = 1;
                if(child.transform.position.y > total - 2){
                    y = -1;
                }
                if(child.transform.position.x > total - 2){
                    x = -1;
                }
                newPos[i] = new Vector3(child.transform.position.x + x,child.transform.position.y + y,-1);
                continue;
            }
            

            foreach(Vector3 d in directions){
                int moveX = (int)d.x;
                int moveY = (int)d.y;
                if(/*childY - 2f >= 0 && childY + 1.25f < total && childX - 2f >= 0 && childX + 1.25f < total &&*/
                    childY + moveY >= 2 && childY + moveY < total-2f && 
                    childX + moveX >= 2 && childX + moveX < total-2f){
                    filt.Add(d);
                    if(fscript.flowerTiles[childY + moveY,childX + moveX] == 1){
                        Flowerfilt.Add(d);
                    }
                }
            }
            Vector3[] filtArray = filt.ToArray();
            Vector3[] flowerArray = Flowerfilt.ToArray();
            int len = filtArray.Length;
            int flowerlen = flowerArray.Length;
            if(flowerlen > 0){
                Vector3 chosen = flowerArray[Random.Range(0,flowerlen)];;
                Vector3 target = new Vector3(child.transform.position.x + chosen.x, child.transform.position.y + chosen.y, -1);
                newPos[i] = target;
                continue;
            }
            if(len > 0){
                Vector3 chosen = filtArray[Random.Range(0,len)];;
                Vector3 target = new Vector3(child.transform.position.x + chosen.x, child.transform.position.y + chosen.y, -1);
                newPos[i] = target;
            }else{
                newPos[i] = child.transform.position;
            }
            
        }
        return newPos;
    }

    IEnumerator wait(){
        newEnemPos = getNewPositions();
        waiting = true;
        yield return new WaitForSeconds(.25f);
        waiting = false;
    }
}
