﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour
{

    private static GameMusic _instance;
    public static GameMusic Instance { get { return _instance; } }

    [Range(0.01f, 1f)]
    public float StartPlayingAtSec;

    private bool inMenu = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        for (int i = 0; i < GameMusicLevels.Length; i++)
        {
            GameMusicLevels[i].volume = .9f;
            GameMusicLevels[i].priority = 0;
        }

        GameMusicLevels[0].Play();
    }
    public void SetInMenu(bool b)
    {
        inMenu = b;
        currentLevel = 0;
        this.Update();
        
    }
    public void SetGameMusicLevel(int activeRocketCount)
    {

        // play mello intro music if there are no rockets
        if (activeRocketCount == 0 || inMenu)
        {
            currentLevel = 0;
            return;
        }

        currentLevel = activeRocketCount / 3;

        if (currentLevel < 1)
        {
            currentLevel = 1;
        }
        else if (currentLevel >= Instance.GameMusicLevels.Length)
        {
            currentLevel = Instance.GameMusicLevels.Length - 1;
        }
    }

    public int currentLevel;
    public AudioSource[] GameMusicLevels;

    public void Update()
    {
        SetGameMusicLevel(RocketFactory.GetActiveRocketCount());

        if (currentLevel == 0)
        {
            // playing intro music which doesn't get layered
            
            if (!GameMusicLevels[0].isPlaying)
            {
                // stop all the other music layers
                for (int i = 1; i < GameMusicLevels.Length; i++)
                {
                    GameMusicLevels[i].Stop();
                }
                GameMusicLevels[0].Play();
            }
        }
        else
        {
            // playing in play music which is layered so loop through the layers to turn the right ones on
            GameMusicLevels[0].Stop();

            if (!GameMusicLevels[1].isPlaying) { GameMusicLevels[1].Play(); }
            
            if (GameMusicLevels[1].time > StartPlayingAtSec)
            {
                return;
            }

            // do turn on/off correct music level at the beginning of the music loop
            for (int i = 2; i < GameMusicLevels.Length; i++)
            {
                if (i <= currentLevel)
                {
                    if (!GameMusicLevels[i].isPlaying)
                    {
                        GameMusicLevels[i].Play();
                        GameMusicLevels[i].time = GameMusicLevels[1].time;
                    }
                }
                else
                {
                    GameMusicLevels[i].Stop();
                }
            }
        }
    }
}