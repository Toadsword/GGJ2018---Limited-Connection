﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Call
{
    private static int ID=0;
    public int id{get;private set;}

    public int size{get;private set;}//nb de edges 

    private Text timerText;

    public Call(){
        id = ID;
        ID++;

        size = 1;

        timerText = GameObject.Find("TimerText").GetComponent<Text>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Debug.Log("Create call");
    }

    //premier chrono en seconde,
    //temps avant que la personne abandonne l'appel (-1 vie)
    private float randomCountDown = Random.Range(10, 20);
    // Temps avant plusieurs appels d'halo
    private const float HALO_COUNTDOWN_BEFORE_NEW = 2.0f;
    private const float HALO_TIME_BETWEEN_TWO = 0.5f;
    private const float HALO_TIME_CONSECUTIVE = 0.1f;
    private const int HALO_COUNT = 3;

    private float HaloCountDownBeforeNew = 0.0f;
    private float HaloTimeBetweenTwo = HALO_TIME_BETWEEN_TWO;
    private float HaloTimeConsecutive = HALO_TIME_CONSECUTIVE;
    private int HaloCurrentCount = 0;

    private bool passedBetweenTwo = false;

    private GameObject messageBox;

    public NodeController caller{get;set;}
    public NodeController reciever { get; set; }

    public enum Status{calling, inCall, interruptedCall};
    public Status status{get;set;}

    GameManager gameManager;

    public float Timers()
    {
        randomCountDown -= Time.deltaTime;

        return randomCountDown;
    }

    public void SetInCall()
    {
        //second chrono en seconde,
        //temps avant que la communication s'achève
        if (status == Status.interruptedCall)
            randomCountDown += 5.0f;
        else
            randomCountDown = Random.Range(10, 20);

        reciever.DisplayMessageBox(false);

        status = Status.inCall;
    }

    public bool Update()
    {
        if (randomCountDown < 0.0f)
        {
            reciever.DisplayMessageBox(false);
            gameManager.LibererDelivrer(caller);
            gameManager.LibererDelivrer(reciever);
            gameManager.EndCall(this);
            return true;
        }
         /*if(status == Status.inCall)
            {
                gameManager.LibererDelivrer(caller);
                gameManager.LibererDelivrer(reciever);
                gameManager.EndCall(true);
                return true;
            } 
            else if (status == Status.calling || status == Status.interruptedCall)
            {
                gameManager.LibererDelivrer(caller);
                gameManager.LibererDelivrer(reciever);
                gameManager.EndCall(false);
                return true;
            }
        }
        else
        {
            if (status == Status.calling)
            {
                HaloManager();
            }
            randomCountDown -= Time.deltaTime;
            //Debug.Log(randomCountDown);
            timerText.text = "Time until call finish : " + randomCountDown.ToString("F1");
        }
        */
        return false;
    }

    private void HaloManager()
    {
        float timePassed = Time.deltaTime;
        HaloCountDownBeforeNew -= timePassed;
        if (HaloCountDownBeforeNew < 0.0f)
        {
            HaloTimeConsecutive -= timePassed;
            // Alors on crée une vague de halo
            if (HALO_COUNT != HaloCurrentCount)
            {
                if(HaloTimeBetweenTwo < 0.0f)
                {
                    HaloTimeConsecutive = HALO_TIME_CONSECUTIVE;
                }
                else if(HaloTimeConsecutive < 0.0f)
                {
                    //Alors on crée un Halo
                    HaloCurrentCount += 1;
                    gameManager.InstantiateHalo(caller);
                    HaloTimeConsecutive = HALO_TIME_CONSECUTIVE;
                }
            }
            else
            {
                HaloTimeBetweenTwo -= timePassed;
                if(HaloTimeBetweenTwo <= 0.0f)
                {
                    HaloTimeBetweenTwo = HALO_TIME_BETWEEN_TWO;
                    HaloCurrentCount = 0;
                    if (!passedBetweenTwo)
                    {
                        passedBetweenTwo = true;
                    }
                    else
                    {
                        passedBetweenTwo = false;
                        HaloCountDownBeforeNew = HALO_COUNTDOWN_BEFORE_NEW;
                    }
                }
            }
        }
    }

    public void setSize(int i)
    {
        size = i;
    }
}
