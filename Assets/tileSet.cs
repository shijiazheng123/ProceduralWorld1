using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class tileSet : MonoBehaviour
{
	public Tile grass;
	public Tile dirt;
	public int areamax = 25;
	public int areamin = 10;
	public int dirtColumn = 3;
	public int total = 80;
	public int[,] grid;
    // Start is called before the first frame update
    void Start()
    {
        Tilemap map = GetComponent<Tilemap>();
        grid = new int[total,total];
        //map.SetTile(new Vector3Int(0,0,0),grass);
        
        for(int y = 0; y < total; y++){
        	for(int x = 0; x < total; x++){
        		grid[y,x] = 1;
        	}
        }
        //"dirt tiles" for large areas of flowers
        // for(int i = 0; i < dirtColumn; i++){

        // }
        int start = 5;
        int end = 11;
        int minX = Random.Range(start, end);
        int minY = Random.Range(start, end);
        int maxX = Random.Range(minX + areamin, minX + areamax);
        int maxY = Random.Range(minY + areamin, minY + areamax);
        int count = 0;
        int highestX = maxX;
        int highestminX = minX;
        int curXs = start;
        int curXe = end;
        while(count < dirtColumn && maxX < total){
        	while(maxY < total && maxX < total){
        	int[] area = areaGen(minX, minY, maxX, maxY, minX + ((maxX - minX) / 2), minY + ((maxY - minY) / 2));
        		for(int y = area[2]; y < area[3]; y++){
        			for(int x = area[0]; x < area[1]; x++){
        				if(y < total && x < total){
                            grid[y,x] = 0;
                        }
        			}
        		}
        		minY = Random.Range(maxY + start + 10, maxY + end + 10);
        		maxY = Random.Range(minY + areamin, minY + areamax);
        		minX = Random.Range(curXs, curXe);
        		maxX = Random.Range(minX + areamin, minX + areamax);
        		if(highestX < maxX){
        			highestX = maxX;
        		}
        		
        	}
        	minY = Random.Range(start,end);
        	maxY = Random.Range(minY + areamin, minY + areamax);
        	minX = Random.Range(highestX + start, highestX + end);
        	maxX = Random.Range(minX + areamin, minX + areamax);
        	curXs = minX;
        	curXe = minX + end - start;
        	count +=1;
        }
        
        
        //sets the tiles
        for(int y = 0; y < total; y++){
        	for(int x = 0; x < total; x++){
        		if(grid[y,x] == 0){
        			map.SetTile(new Vector3Int(x,y,0), dirt);
        		}else{
        			map.SetTile(new Vector3Int(x,y,0), grass);
        		}
        	}
        }
    }

    int[] areaGen(int minX, int minY, int maxX, int maxY, int midX, int midY){
    	int xmin = Random.Range(minX, midX -2);
    	int xmax = Random.Range(midX, maxX);
    	int ymin = Random.Range(minY, midY -2);
    	int ymax = Random.Range(midY, maxY);
    	int[] area = new int[4]{xmin,xmax,ymin,ymax};
    	return area;
    }


    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
