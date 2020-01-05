using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class playerControls : MonoBehaviour
{
	int movespeed = 10;
	public Tilemap flowerRef;
	public int points = 0;
	public int gauge = 0;
	flowerSet fscript;
	tileSet groundtiles;
	SpriteRenderer rend;
	scoreboard scoreB;
	Camera mainCamera;
    int smallP = 5;
    int bigP = 10;
    public bool repel = false;
    bool prevRepstatus = false;
    public int repMax = 20;
    public float reptimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        GameObject foliage = GameObject.Find("foliage");
        fscript = foliage.GetComponent<flowerSet>();
        flowerRef = foliage.GetComponent<Tilemap>();
        transform.position = new Vector3(fscript.startX + 2.5f, fscript.startY + 2.5f, -2);
        rend = GetComponent<SpriteRenderer>();

        GameObject ground = GameObject.Find("ground");
        groundtiles = ground.GetComponent<tileSet>();

        GameObject canvas = GameObject.Find("Canvas");
        scoreB = canvas.GetComponent<scoreboard>();

        mainCamera = Camera.main;
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    // Update is called once per frame
    void Update()
    {
        if(fscript.flowerNumbers <= 0 && gauge <= 0){
            Time.timeScale = 0;
            scoreB.end();
        }
        if (Input.GetKey (KeyCode.A) && transform.position.x - 1f >= 0) {
         	transform.Translate (Vector3.left * movespeed * Time.deltaTime);
         	rend.flipX = true;
     	}else if(Input.GetKey (KeyCode.D) && transform.position.x + .25f < groundtiles.total) {
         	transform.Translate (Vector3.right * movespeed * Time.deltaTime);
         	rend.flipX = false;
     	}
     	if (Input.GetKey (KeyCode.W) && transform.position.y + .25f < groundtiles.total) {
         	transform.Translate (Vector3.up * movespeed * Time.deltaTime); 
     	}else if(Input.GetKey (KeyCode.S) && transform.position.y - 1f >= 0) {
         	transform.Translate (Vector3.down * movespeed * Time.deltaTime);
     	}

     	var cameraHalfWidth = mainCamera.orthographicSize * ((float)Screen.width / Screen.height);

     	mainCamera.transform.localPosition = new Vector3(
        Mathf.Clamp(transform.localPosition.x, 0 + cameraHalfWidth +.5f, groundtiles.total - cameraHalfWidth+.5f),
        Mathf.Clamp(transform.localPosition.y, 0 + mainCamera.orthographicSize, groundtiles.total - mainCamera.orthographicSize+.5f),
        -10
    	);

    	if(gauge > 0 && transform.position.y >= fscript.startY && transform.position.y < fscript.startY + 5 && 
                transform.position.x >= fscript.startX && transform.position.x < fscript.startX + 5){
    		points += gauge;
    		gauge = 0;
    		scoreB.changePoints(points);
            scoreB.changeGauge(gauge);
    	}

        if(prevRepstatus != repel){
            prevRepstatus = repel;
        }

        if(repel){
            if(reptimer < repMax){
                reptimer += Time.deltaTime;
                scoreB.repelStatus(reptimer);
            }else{
                reptimer = 0;
                scoreB.repelStatus(reptimer);
                repel = false;
            }   
        }

     	checkOverlapFlowers();
    }

    void checkOverlapFlowers(){
    	if(gauge <= 100-smallP && (int)(transform.position.y-.5f) >=0 && (int)(transform.position.x-.5f) >=0 && (int)(transform.position.y) < groundtiles.total && (int)(transform.position.x) < groundtiles.total && 
    		(fscript.flowerTiles[(int)(transform.position.y-.5f), (int)(transform.position.x-.5f)] == 1 || fscript.flowerTiles[(int)(transform.position.y), (int)(transform.position.x-.5f)] == 1 ||
    		    		fscript.flowerTiles[(int)(transform.position.y-.5f), (int)(transform.position.x)] == 1 || fscript.flowerTiles[(int)(transform.position.y), (int)(transform.position.x)] == 1)){

    		Vector3Int pos;
    		if(fscript.flowerTiles[(int)(transform.position.y-.5f), (int)(transform.position.x)] == 1){
                pos = new Vector3Int((int)(transform.position.x),(int)(transform.position.y-.5f),0);
            }else if(fscript.flowerTiles[(int)(transform.position.y), (int)(transform.position.x-.5f)] == 1){
                pos = new Vector3Int((int)(transform.position.x-.5f),(int)(transform.position.y),0);
            }else{
                pos = new Vector3Int((int)(transform.position.x-.5f),(int)(transform.position.y-.5f),0);
            }

    		if(flowerRef.GetSprite(pos) == fscript.small[0].sprite || 
    			flowerRef.GetSprite(pos) == fscript.small[1].sprite || 
    			flowerRef.GetSprite(pos) == fscript.small[2].sprite ){
    			flowerRef.SetTile(pos,fscript.unuseable[0]);
    			fscript.flowerNumbers -= 1;
    			fscript.flowerTiles[pos.y,pos.x] = 0;
    			gauge += smallP;
    			scoreB.changeGauge(gauge);
    		}
    		if(gauge <= 100-bigP){
    			if(flowerRef.GetSprite(pos) == fscript.big[0].sprite){
    				changebigFlowers(1,pos);
    			}else if(flowerRef.GetSprite(pos) == fscript.big[1].sprite){
    				changebigFlowers(2,pos);
    			}else if(flowerRef.GetSprite(pos) == fscript.big[2].sprite){
    				changebigFlowers(3,pos);
    			}
    		}
     	}else if(gauge <= 100-bigP && (int)(transform.position.y-.5f)-1 >=0 && (int)(transform.position.x-.5f)-1>=0 && 
     		(fscript.flowerTiles[(int)(transform.position.y-.5f)-1, (int)(transform.position.x-.5f)-1] == 1 || fscript.flowerTiles[(int)(transform.position.y)-1, (int)(transform.position.x-.5f)-1] == 1 ||
     		     		fscript.flowerTiles[(int)(transform.position.y-.5f)-1, (int)(transform.position.x)-1] == 1 || fscript.flowerTiles[(int)(transform.position.y)-1, (int)(transform.position.x)-1] == 1)){
    		Vector3Int pos;
            if(fscript.flowerTiles[(int)(transform.position.y-.5f)-1, (int)(transform.position.x)-1] == 1){
                pos = new Vector3Int((int)(transform.position.x)-1,(int)(transform.position.y-.5f)-1,0);
            }else if(fscript.flowerTiles[(int)(transform.position.y)-1, (int)(transform.position.x-.5f)-1] == 1){
                pos = new Vector3Int((int)(transform.position.x-.5f)-1,(int)(transform.position.y)-1,0);
            }else{
                pos = new Vector3Int((int)(transform.position.x-.5f)-1,(int)(transform.position.y-.5f)-1,0);
            }
    		if(flowerRef.GetSprite(pos) == fscript.big[0].sprite){
    			changebigFlowers(1,pos);
    		}else if(flowerRef.GetSprite(pos) == fscript.big[1].sprite){
    			changebigFlowers(2,pos);
    		}else if(flowerRef.GetSprite(pos) == fscript.big[2].sprite){
    			changebigFlowers(3,pos);
    		}
    	}
    }

    void changebigFlowers(int index, Vector3Int pos){
    	flowerRef.SetTile(pos, fscript.unuseable[index]);
    	fscript.flowerNumbers -= 4;
    	for(int y = 0; y < 2; y++){
    		for(int x = 0; x < 2; x++){
    			fscript.flowerTiles[pos.y + y, pos.x + x] = 0;
    		}
    	}
    	gauge += bigP;
    	scoreB.changeGauge(gauge);
    }
}
