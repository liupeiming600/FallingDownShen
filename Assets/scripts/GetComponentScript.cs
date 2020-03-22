using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetComponentScript : MonoBehaviour {

    public PassTransform1[] passer1;
    public PassTransform2[] passer2;

	// Use this for initialization
	void Start () {
        passer1 = GetComponentsInChildren<PassTransform1>();
        passer2 = GetComponentsInChildren<PassTransform2>();
	}
	
	// Update is called once per frame
	void Update () {
        if (this.name == "bunshin")
        {
            DisableChildComponents1();
            DisableChildComponents2();
        }
        
	}

    void DisableChildComponents1()
    {
        foreach (PassTransform1 pass in passer1)
        {
            pass.enabled = false;
        }
    }

    void DisableChildComponents2()
    {
        foreach (PassTransform2 pass in passer2)
        {
            pass.enabled = false;
        }
    }

}
