using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassTransform2 : MonoBehaviour {

    public GameObject Track1;
    public GameObject Track2;
    float xMid, yMid, zRot, yDiff, xDiff;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        xMid = (Track1.transform.position.x + Track2.transform.position.x) / 2f;
        yMid = (Track1.transform.position.y + Track2.transform.position.y) / 2f;
        yDiff = Track1.transform.position.y - Track2.transform.position.y;
        xDiff = Track1.transform.position.x - Track2.transform.position.x;
        zRot = Mathf.Atan2(yDiff, xDiff) * Mathf.Rad2Deg;

        this.transform.position = new Vector3(xMid - 5, yMid, Track1.transform.position.z);
        this.transform.rotation = Quaternion.Euler(0, 0, zRot-90);

    }
}
