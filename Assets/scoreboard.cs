using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreboard : MonoBehaviour
{
	Text score;
	playerControls info;
	string highestScorer = "It's you!";
	//string repel = "Off";
	int currentHighest;
    RectTransform repel;
    RectTransform capacity;
    float fullSizeCap;
    float fullSizeRep;
    // Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("player");
        info = player.GetComponent<playerControls>();

        // mainCamera = Camera.main;

        Transform textObj = transform.Find("Points");
        score = textObj.GetComponent<Text>();

        Transform repImg = transform.Find("Repel");
        RawImage repelraw = repImg.GetComponent<RawImage>();
        repel = repelraw.GetComponent<RectTransform>();
        fullSizeRep = repel.sizeDelta.x;
        repel.sizeDelta = new Vector2(0, repel.sizeDelta.y);

        Transform capImg = transform.Find("Capacity");
        RawImage capacityraw = capImg.GetComponent<RawImage>();
        capacity = capacityraw.GetComponent<RectTransform>();
        fullSizeCap = capacity.sizeDelta.x;
        capacity.sizeDelta = new Vector2(0, capacity.sizeDelta.y);

        Transform endBack = transform.Find("endingBack");
        RawImage endBackImg = endBack.GetComponent<RawImage>();
        endBackImg.enabled = false;

        currentHighest = info.points;
       	score.text = "Highest Scorer: " + highestScorer + "\t\t" + "Highest Score: " + currentHighest + "\t\t" + "Your Score: " + info.points;

    }

    // // Update is called once per frame
    // void Update()
    // {
    //     transform.position = mainCamera.transform.position;
    // }
    public void changePoints(int changedPoints){
    	if(changedPoints > currentHighest){
    		currentHighest = changedPoints;
    	}
    	if(currentHighest == info.points){
    		highestScorer = "It's you!";
    	}else{
    		highestScorer = "Not you!";
    	}
    	score.text = "Highest Scorer: " + highestScorer + "\t\t" + "Highest Score: " + currentHighest + "\t\t" + "Your Score: " + info.points;
    }

    public void changeGauge(int gauge){
        capacity.sizeDelta = new Vector2(fullSizeCap * (gauge/100f), capacity.sizeDelta.y);
    }

    public void repelStatus(float timer){
        if(timer == 0){
            repel.sizeDelta = new Vector2(0, repel.sizeDelta.y);
        }else{
            repel.sizeDelta = new Vector2(fullSizeRep * (((float)info.repMax - timer)/(float)info.repMax), repel.sizeDelta.y);
        }
    }

    public void end(){
        Transform endBack = transform.Find("endingBack");
        RawImage endBackImg = endBack.GetComponent<RawImage>();
        endBackImg.enabled = true;

        Transform textObj = transform.Find("Ending");
        Text ending = textObj.GetComponent<Text>();
    	if(currentHighest == info.points){
    		ending.text = "YOU WIN!! :D";
    	}else{
    		ending.text = "YOU LOSE :(";
    	}
    }
}
