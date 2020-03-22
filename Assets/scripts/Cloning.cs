using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class Cloning : MonoBehaviour {

    public GameObject WholeBody;
    public GameObject Face;

    private float timeleft = 3.0f;
    private Vector3 positionNow;
    private Vector3 positionPrev;
    public float ThresholdMotion = 0.1f;
    public bool flagImageSaved = false;

    SnapshotCameraTest Snapshotter;

    private BodySourceManager bodyManager;
    private Body[] bodies;


    // Use this for initialization
    void Start()
    {
        WholeBody = GameObject.Find("HumanBody");
        Face = GameObject.Find("HumanBody/Head");
        positionPrev = new Vector3(Face.transform.position.x,
                                   Face.transform.position.y,
                                   Face.transform.position.z);

        Snapshotter = GameObject.Find("Sub Camera").GetComponent<SnapshotCameraTest>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InstantiateObject();
        }
        */


        timeleft -= Time.deltaTime;

        if (timeleft <= 0.0f)
        {
            
            //Debug.Log(String.Format("{0}", bodies));
            Debug.Log(String.Format("{0}", JudgeHumanMotion(Face)));

            //if(bodies != null)
            //{ 
            if (JudgeHumanMotion(Face))
            {
                //InstantiateObject();
                SaveImage();
                flagImageSaved = true;
            }
            //}

            timeleft = 3.0f;
        }

    }

    bool JudgeHumanMotion(GameObject bodyPart)
    {
        positionNow = new Vector3(bodyPart.transform.position.x,
                                  bodyPart.transform.position.y,
                                  bodyPart.transform.position.z);
        float distanceHumanMotion = Vector3.Distance(positionNow, positionPrev);
        positionPrev = positionNow;
        return (distanceHumanMotion < ThresholdMotion);
    }

    private void InstantiateObject()
    {
        GameObject bunshin = Instantiate(WholeBody) as GameObject;
        bunshin.name = "bunshin";
    }

    private void SaveImage()
    {
        Snapshotter.UpdatePreview();
        System.IO.FileInfo fi = SnapshotCamera.SavePNG(Snapshotter.texture);
        Debug.Log(string.Format("Snapshot {0} saved to {1}", "human.png", fi.DirectoryName));
    }

}
