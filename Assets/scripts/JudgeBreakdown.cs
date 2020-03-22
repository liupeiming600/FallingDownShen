using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JudgeBreakdown : MonoBehaviour {

    private GameObject[] players;
    private bool isMove = false;
    private bool isThrew = false;
    private PolygonCollider2D playerCollider;
    private ObjectController objectLatest;
    private Vector3 screenPoint;
    private float WidthShift = 6f;
    private float dy;

    public bool allMove = false;
    public bool IsFinish = false;
    public Vector3 initialPlayerPosition = new Vector3(-4.0f, 3.0f, 0.0f);
    public float YThreshold = 0.0f;

    Cloning cloning;

    // Use this for initialization
    void Start() {
        cloning = GameObject.Find("cloneManager").GetComponent<Cloning>();
        //MakeNewObject();
    }

    // Update is called once per frame
    void Update() {
        players = GameObject.FindGameObjectsWithTag("Player");
        // Bahavior when the game finish
        if (IsFinish) {
            Debug.Log("Game Finish!!");
            //GameObject.FindWithTag("AudioController").GetComponents<SoundEffector>.;

            ResetGame();
            MakeNewObject();

        } else {
            if (players.Length == 0)
            {
                MakeNewObject();
            }

            allMove = false;
            for (int i = 0; i < players.Length; i++) {
                isMove = players[i].GetComponent<ObjectController>().IsMove;
                if (isMove) {
                    allMove = true;
                }
            }
            //Debug.Log(String.Format("allMove：{0}", allMove));
            if (players.Length > 0) {
                objectLatest = players[players.Length - 1].GetComponent<ObjectController>();
                isThrew = objectLatest.IsThrew;
            }

            //Debug.Log(String.Format("allMove: {0}, isThrew: {1}, timeFromThrew: {2}",
            //    allMove, isThrew, objectLatest.timeFromThrew));
            //if (!allMove && isThrew && objectLatest.timeFromThrew > 3f) {
            if (!allMove) {
                //UpperCameraPosition();
                MakeNewObject();
            }

            // Detect the finishing of game.
            for (int i = 0; i < players.Length; i++) {
                playerCollider = players[i].GetComponent<PolygonCollider2D>();
                if (this.GetComponent<PolygonCollider2D>().IsTouching(playerCollider)) {
                    IsFinish = true;
                }
            }
        }
    }

    void MakeNewObject()
    {
        if (cloning.flagImageSaved)
        {
            GameObject obj = (GameObject)Resources.Load("prefabs/player_prefab");
            Instantiate(obj);
            //Instantiate(obj, initialPlayerPosition, Quaternion.identity);
            cloning.flagImageSaved = false;
        }
    }

    void UpperCameraPosition() {
        // Search the max position of the each collider
        players = GameObject.FindGameObjectsWithTag("Player");
        float yPolyMax = -Mathf.Infinity;
        for (int i = 0; i < players.Length; i++) {
            float yPoly = players[i].GetComponent<PolygonCollider2D>().bounds.max.y;
            if (yPoly > yPolyMax) {
                yPolyMax = yPoly;
            }
        }
        //Debug.Log(String.Format("yPolyMax: {0}, YTreshold: {1}", yPolyMax, YThreshold));
        // Update initialPosition for object appering position
        if (yPolyMax > YThreshold) {
            int N = 60;
            dy = ((yPolyMax + WidthShift) - initialPlayerPosition.y) / N;
            float waitTime = 0.5f / (float)N;
            StartCoroutine(IncrementCameraPosition(waitTime, N));
            YThreshold += yPolyMax;
            initialPlayerPosition.y = (yPolyMax + WidthShift);
        }
    }

    IEnumerator IncrementCameraPosition(float waitTime, int N) {
        for (int i = 0; i < N; i++) {
            yield return new WaitForSeconds(waitTime);
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,
                                                         Camera.main.transform.position.y + dy,
                                                         Camera.main.transform.position.z);
        }
    }

    void DeleteAllObject() {
        players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i<players.Length; i++) {
            GameObject.Destroy(players[i]);
        }
    }

    void ResetGame() {
        DeleteAllObject();
        initialPlayerPosition = new Vector3(0.0f, 3f, 0.0f);
        Camera.main.transform.position = new Vector3(0f, 0f, -1f);
        YThreshold = 0.0f;
        IsFinish = false;
    }
}
