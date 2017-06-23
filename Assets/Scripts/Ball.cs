﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour {
    public float speed = 25;
    private Rigidbody2D rigidBody;
    private AudioSource audioSource;
	// Use this for initialization
	void Start () {

        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = Vector2.right * speed; 
		
	}

    private void OnCollisionEnter2D(Collision2D col)
    {
        //left or right paddle
        if((col.gameObject.name=="LeftPaddle") || (col.gameObject.name == "RightPaddle"))
        {
            HandlePaddleHit(col);
        }

        //Left or Right Goal
        if ((col.gameObject.name == "LeftGoal") || (col.gameObject.name == "RightGoal"))
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.goalBloop);
            //Score UI
            if (col.gameObject.name == "LeftGoal")
            {
               IncreaseTextUIScore("RightScoreUI");

            }
            else if (col.gameObject.name == "RightGoal")
            {
                IncreaseTextUIScore("LeftScoreUI");

            }

            transform.position = new Vector2(0, 25.5f);
        }

        //Top or Bottom Wall
        if ((col.gameObject.name == "TopWall") || (col.gameObject.name == "BottomWall"))
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.wallBloop);
        }
    }
    void HandlePaddleHit(Collision2D col)
    {
        float y = BallHitPaddleWhere(transform.position, 
            col.transform.position,
            col.collider.bounds.size.y);

        Vector2 dir = new Vector2();

        if(col.gameObject.name == "LeftPaddle")
        {
            dir = new Vector2(1, y).normalized;
        }
        if (col.gameObject.name == "RightPaddle")
        {
            dir = new Vector2(-1, y).normalized;
        }
        rigidBody.velocity = dir * speed;
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.hitPaddleBloop);
    }

    float BallHitPaddleWhere(Vector2 ball, Vector2 paddle, float paddleHeight)
    {
        return (ball.y - paddle.y) / paddleHeight;
    }

    void IncreaseTextUIScore(string textUIName)
    {
        var textUIComp = GameObject.Find(textUIName).GetComponent<Text>();
        int score = int.Parse(textUIComp.text);
        score++;
        textUIComp.text = score.ToString();
    }
}