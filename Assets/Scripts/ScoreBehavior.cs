﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBehavior : MonoBehaviour {
    
    float timer_destruction = 0.8f;
    Vector3 direction;
    float speed = 0.5f;
	// Use this for initialization
	void Start () {
        //direction = new Vector3(Random.Range(-300,300)/100.0f, Random.Range(-300,300)/100.0f);
        Vector3 center = GameObject.Find("GameManager").GetComponent<GameManager>().centreLevel();
        center += new Vector3(0, 0, transform.position.z) - new Vector3(0, 0, center.z);
        Debug.Log("START SCORE");
        direction = center - transform.position;
        direction = direction.normalized*speed;
	    Destroy(gameObject, timer_destruction);

        GetComponent<MeshRenderer>().sortingOrder = 4;
    }
	
	// Update is called once per frame
	void Update () {
		//déplacement
        transform.position += direction;

        direction = direction*0.9f;
	}
}
