using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffector : MonoBehaviour {

    public AudioClip audioClipBGM;
    public AudioClip audioClipDrag;
    public AudioClip audioClipFinish;
    public AudioClip audioClipDrop;

    private AudioSource audioSourceBGM;
    private AudioSource audioSourceEffect;

    private JudgeBreakdown judgeGame;
    private GameObject[] players;
    private ObjectController playerLatest;

    // Use this for initialization
    void Start () {
        audioSourceBGM = GameObject.Find("BGM").GetComponent<AudioSource>();
        audioSourceEffect = GameObject.Find("Effect").GetComponent<AudioSource>();
        PlayBGM(audioClipBGM);
    }
	
	// Update is called once per frame
	void Update () {
        players = GameObject.FindGameObjectsWithTag("Player");
        judgeGame = GameObject.FindGameObjectWithTag("GameController").GetComponent<JudgeBreakdown>();

        //Debug.Log(String.Format("{0}", players.Length));
        if (players.Length > 0) {
            playerLatest = players[players.Length - 1].GetComponent<ObjectController>();
            if (playerLatest.WasHold == false && playerLatest.IsHold == true) {
                PlayEffect(audioClipDrag);
            }
            if (playerLatest.WasHold == true && playerLatest.IsHold == false) {
                PlayEffect(audioClipDrop);
            }
        }
 
        if (judgeGame.IsFinish) {
            PlayEffect(audioClipFinish);
        }
	}

    void PlayBGM(AudioClip audioClip) {
        audioSourceBGM.clip = audioClip;
        audioSourceBGM.Play();
    }

    void PlayEffect(AudioClip audioClip) {
        audioSourceEffect.clip = audioClip;
        audioSourceEffect.Play();
    }
}
