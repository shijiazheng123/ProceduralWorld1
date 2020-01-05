using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class flowerSet : MonoBehaviour
{
	public int[,] availablegrid;
	public int[,] flowerTiles;
	public Tile sFlower1;
	public Tile sFlower2;
	public Tile sFlower3;
	public Tile mushroom;
	public Tile sRock;
	public Tile bFlower1;
	public Tile bFlower2;
	public Tile bFlower3;
	public Tile sBush;
	public Tile bRock;
	public Tile unuseS;
	public Tile unusebigR;
	public Tile unusebigL;
	public Tile unusebigP;
	public Tile[] small;
	public Tile[] big;
	public Tile[] unuseable;
	public Tile bBush;
	public Tile wide;
	public Tile tall;
	public Tile hive;
	public int range;
	public int startX;
	public int startY;
	public int flowerNumbers = 0;
	public GameObject worm;
	int children = 5;
	int movespeed = 5;
	bool waiting;
	int total;
	Vector3[] newWormPos;
	GameObject player;
	playerControls pInfo;
	
    // Start is called before the first frame update
    void Start()
    {

    	Tilemap map = GetComponent<Tilemap>();
    	range = 100;

        GameObject ground = GameObject.Find("ground");
        tileSet groundtiles = ground.GetComponent<tileSet>();
        total = groundtiles.total;

        player = GameObject.Find("player");
        pInfo = player.GetComponent<playerControls>();


        small = new Tile[5] {sFlower1, sFlower2, sFlower3, mushroom, sRock};
        big = new Tile[5] {bFlower1, bFlower2, bFlower3, sBush, bRock};
        unuseable = new Tile[4] {unuseS,unusebigR, unusebigP, unusebigL};
        availablegrid = new int[groundtiles.total,groundtiles.total];
        flowerTiles = new int[groundtiles.total,groundtiles.total];
        for(int y = 0; y < groundtiles.total; y++){
        	for(int x = 0; x < groundtiles.total; x++){
        		availablegrid[y,x] = 1;
        		flowerTiles[y,x] = 0;
        	}
        }

        startX = Random.Range((groundtiles.total/4),(groundtiles.total/4)*3);
        startY = Random.Range((groundtiles.total/4),(groundtiles.total/2));
        bool hivePlaced = false;
        while(!hivePlaced){
        	if(groundtiles.grid[startY,startX] == 1 && groundtiles.grid[startY,startX+1] == 1 && groundtiles.grid[startY,startX+2] == 1 && groundtiles.grid[startY,startX+3] == 1 && groundtiles.grid[startY,startX+4] == 1 &&
        		groundtiles.grid[startY+1,startX] == 1 && groundtiles.grid[startY+1,startX+1] == 1 && groundtiles.grid[startY+1,startX+2] == 1 && groundtiles.grid[startY+1,startX+3] == 1 && groundtiles.grid[startY+1,startX+4] == 1 &&
        		groundtiles.grid[startY+2,startX] == 1 && groundtiles.grid[startY+2,startX+1] == 1 && groundtiles.grid[startY+2,startX+2] == 1 && groundtiles.grid[startY+2,startX+3] == 1 && groundtiles.grid[startY+2,startX+4] == 1 &&
        		groundtiles.grid[startY+3,startX] == 1 && groundtiles.grid[startY+3,startX+1] == 1 && groundtiles.grid[startY+3,startX+2] == 1 && groundtiles.grid[startY+3,startX+3] == 1 && groundtiles.grid[startY+3,startX+4] == 1 &&
        		groundtiles.grid[startY+4,startX] == 1 && groundtiles.grid[startY+4,startX+1] == 1 && groundtiles.grid[startY+4,startX+2] == 1 && groundtiles.grid[startY+4,startX+3] == 1 && groundtiles.grid[startY+4,startX+4] == 1){

        		for(int y = 0; y < 5; y++){
        			for(int x = 0; x < 5; x++){
        				availablegrid[startY+y,startX+x] = 0;
        			}
        		}

        		map.SetTile(new Vector3Int(startX,startY,0),hive);
        		hivePlaced = true;
        	}else if(startX < groundtiles.total-3){
        		startX +=1;
        	}else{
        		startX = 0;
        		startY +=1;
        	}
        }

        for(int i = 0; i < children; i++){
        	Instantiate(worm, transform);
        }

        for(int i = 0; i < transform.childCount; i++){
        	Transform child = transform.GetChild(i);
        	int x = Random.Range(0, (groundtiles.total/4)*3);
        	int y = Random.Range(0, (groundtiles.total/4)*3);
        	bool childPlaced = false;
        	while(!childPlaced){
        		if(y - 1 >= 0 && x - 1 >= 0 && y + 1 < groundtiles.total && x + 1 < groundtiles.total &&
        			groundtiles.grid[y,x] == 1 && availablegrid[y,x] == 1){
        			child.localPosition = new Vector3Int(x,y,0);
        			availablegrid[y,x] = 0;
        			availablegrid[y-1,x+1] = 0;
        			availablegrid[y+1,x-1] = 0;
        			availablegrid[y-1,x-1] = 0;
        			availablegrid[y+1,x+1] = 0;
        			childPlaced = true;
        		}else if(x < groundtiles.total){
        			x += 1;
        		}else{
        			x = 0;
        			y +=1;
        		}
        	}
        }

        Debug.Log(startX);
        Debug.Log(startY);

        for(int y = 0; y < groundtiles.total; y++){
        	for(int x = 0; x < groundtiles.total; x++){
        		if(availablegrid[y,x] == 1){
        			int choice = Random.Range(0,range);
        			if((y+1 < groundtiles.total && availablegrid[y+1,x] == 1 && 
        			    x+1 < groundtiles.total && availablegrid[y,x+1] == 1 && availablegrid[y+1,x+1] == 1) && 
        				((choice < 30 && groundtiles.grid[y,x] == 0 && groundtiles.grid[y+1,x] == 0 && groundtiles.grid[y,x+1] == 0 && groundtiles.grid[y+1,x+1] == 0) || 
        				(choice < 10 && groundtiles.grid[y,x] == 1 && groundtiles.grid[y+1,x] == 1 && groundtiles.grid[y,x+1] == 1 && groundtiles.grid[y+1,x+1] == 1))){
        				
        				int which;
        				if(choice >= 1 && groundtiles.grid[y,x] == 1 && groundtiles.grid[y+1,x] == 1 && groundtiles.grid[y,x+1] == 1 && groundtiles.grid[y+1,x+1] == 1){
        					which = Random.Range(3,5);
        					map.SetTile(new Vector3Int(x,y,0), big[which]);
        				}else{
        					which = Random.Range(0,3);
        					map.SetTile(new Vector3Int(x,y,0), big[which]);
        				}
        				for(int plusy = 0; plusy < 2; plusy++){
        					for(int plusx = 0; plusx < 2; plusx++){
        						availablegrid[y + plusy,x + plusx] = 0;
        						if(which >= 0 && which < 3){
        							flowerTiles[y,x] = 1;
        							flowerNumbers += 1;
        						}
        					}
        				}

        			}else if((groundtiles.grid[y,x] == 0 && choice < 80) || (groundtiles.grid[y,x] == 1 && choice < 15)){

        				
        				if(choice >=4 && groundtiles.grid[y,x] == 1){
        					map.SetTile(new Vector3Int(x,y,0), small[Random.Range(3,5)]);
        				}else{
        					map.SetTile(new Vector3Int(x,y,0), small[Random.Range(0,3)]);
        					flowerTiles[y,x] = 1;
        					flowerNumbers += 1;
        				}
        				availablegrid[y,x] = 0;

        			}else if((groundtiles.grid[y,x] == 1 && y+1 < groundtiles.total && groundtiles.grid[y+1,x] == 1 && availablegrid[y+1,x] == 1 && y+2 < groundtiles.total && groundtiles.grid[y+2,x] == 1 && availablegrid[y+2,x] == 1 &&
        			    	x+1 < groundtiles.total && groundtiles.grid[y,x+1] == 1 && availablegrid[y,x+1] == 1 && groundtiles.grid[y+1,x+1] == 1 && availablegrid[y+1,x+1] == 1 && groundtiles.grid[y+2,x+1] == 1 && availablegrid[y+2,x+1] == 1 &&
        			    	x+2 < groundtiles.total && groundtiles.grid[y,x+2] == 1 && availablegrid[y,x+2] == 1 && groundtiles.grid[y+1,x+2] == 1 && availablegrid[y+1,x+2] == 1 && groundtiles.grid[y+2,x+2] == 1 && availablegrid[y+2,x+2] == 1) &&
        					choice >=50 && choice < 55){

        				int whichBig = Random.Range(0,3);
        				if(whichBig == 0){
        					map.SetTile(new Vector3Int(x,y,0), bBush);
        					for(int plusy = 0; plusy < 3; plusy++){
        						for(int plusx = 0; plusx < 3; plusx++){
        							availablegrid[y + plusy,x + plusx] = 0;
        						}
        					}
        				}else if(whichBig == 1){
        					map.SetTile(new Vector3Int(x,y,0), wide);
        					for(int plusy = 0; plusy < 2; plusy++){
        						for(int plusx = 0; plusx < 3; plusx++){
        							availablegrid[y + plusy,x + plusx] = 0;
        						}
        					}
        				}else{
        					map.SetTile(new Vector3Int(x,y,0), tall);
        					for(int plusy = 0; plusy < 3; plusy++){
        						for(int plusx = 0; plusx < 2; plusx++){
        							availablegrid[y + plusy,x + plusx] = 0;
        						}
        					}
        				}
        			}
        		}
        	}
        } 
        for(int i = 0; i < transform.childCount; i++) {
        	Transform child = transform.GetChild(i);
        	int x = (int)child.transform.position.x;
        	int y = (int)child.transform.position.y;
        	availablegrid[y-1,x+1] = 1;
        	availablegrid[y+1,x-1] = 1;
        	availablegrid[y-1,x-1] = 1;
        	availablegrid[y+1,x+1] = 1;
        }      

    }

    // Update is called once per frame
    void Update()
    {
    	if(flowerNumbers <= 0 && pInfo.gauge <= 0){
    		Time.timeScale = 0;
    	}
    	
    	if(!waiting){
    		StartCoroutine(wait());
    	}else{
    		moveWorms();
    	}


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
        	foreach(Vector3 d in directions){
        		int moveX = (int)d.x;
        		int moveY = (int)d.y;

        		if(childY + moveY >= 2 && childY + moveY < total-2 && 
        			childX + moveX >= 2 && childX + moveX < total-2 && 
        			availablegrid[childY + moveY, childX + moveX] == 1 &&
        			childY + moveY - 1 >= 0 && childX + moveX - 1 >= 0 &&
        			availablegrid[childY + moveY - 1, childX + moveX - 1] == 1){

        			filt.Add(d);

        		}
        	}
        	Vector3[] filtArray = filt.ToArray();
        	int len = filtArray.Length;
        	if(len > 0){
        		Vector3 chosen = filtArray[Random.Range(0,len)];
        		
        		Vector3 target = new Vector3(child.transform.position.x + chosen.x, child.transform.position.y + chosen.y, 0);
        		newPos[i] = target;
        	}else{
        		newPos[i] = child.localPosition;
        	}

        	if(child.transform.position.x >= player.transform.position.x - 1 && child.transform.position.x < player.transform.position.x + 1 &&
        		child.transform.position.y >= player.transform.position.y - 1 && child.transform.position.y < player.transform.position.y + 1){

        		pInfo.repel = true;

        	}
        }
        return newPos;
    }

    void moveWorms(){
    	
    	for(int i = 0; i < transform.childCount; i++){
        	Transform child = transform.GetChild(i);
        	Vector3 target = newWormPos[i];
        	if(target.x - child.transform.position.x == 1){
        		SpriteRenderer rend = child.GetComponent<SpriteRenderer>();
        		rend.flipX = true;
        	}else if(target.x - child.transform.position.x == -1){
        		SpriteRenderer rend = child.GetComponent<SpriteRenderer>();
        		rend.flipX = false;
        	}
        	child.transform.position = Vector3.Lerp(child.transform.position, target, Time.deltaTime*movespeed);
        	
        }
    }

    IEnumerator wait(){
    	newWormPos = getNewPositions();
    	waiting = true;
    	yield return new WaitForSeconds(.5f);
    	waiting = false;
    }

}
