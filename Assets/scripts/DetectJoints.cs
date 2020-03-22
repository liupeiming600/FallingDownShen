using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class DetectJoints : MonoBehaviour
{

    public GameObject BodySrcManager;
    public JointType TrackedJoint;
    private BodySourceManager bodyManager;
    private Body[] bodies;
    public float multiplier = 10f;

    public GameObject BodyParts1;
    public GameObject BodyParts2;

    public ulong bodyID1 = 0;
    public ulong bodyID2 = 0;
    public ulong trackID1 = 0;
    public ulong trackID2 = 0;

    // Use this for initialization
    void Start()
    {
        if (BodySrcManager == null)
        {
            Debug.Log("Assign GameObject with Body Source Manager");
        }
        else
        {
            bodyManager = BodySrcManager.GetComponent<BodySourceManager>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (bodyManager == null)
        {
            return;
        }
        bodies = bodyManager.GetData();

        if (bodies == null)
        {
            return;
        }


        /*
         foreach (var bodyText in bodies)
         {

             if (bodyText.IsTracked)
             {
                 if (bodyCounter == 0)
                 {
                     trackID1 = bodyText.TrackingId;
                     bodyCounter = 1;
                     continue;
                 }
                 else if (bodyCounter == 1)
                 {
                     trackID2 = bodyText.TrackingId;
                     bodyCounter = 2;
                     continue;
                 }

             }

          }
             */

        int bodiesNull = 0;
        int k = 0;

        foreach (var bodyText in bodies)
        {
            if (bodyText.TrackingId == 0)
            {
                bodiesNull++;
            }
            else if (bodyText.TrackingId != 0 && k == 0)
            {
                bodyID1 = bodyText.TrackingId;
                k = 1;
            }
            else if (bodyText.TrackingId != 0 && k == 1)
            {
                bodyID2 = bodyText.TrackingId;
                k = 0;
            }

        }

        if (bodiesNull == 4)
        {
            trackID1 = bodyID1;
            trackID2 = bodyID2;

        }

        foreach (var body in bodies)
        {
            if (body.TrackingId == trackID1)
            {
                var pos = body.Joints[TrackedJoint].Position;
                BodyParts1.transform.position = new Vector3(pos.X * multiplier, (pos.Y * multiplier) + 1);
            }

            else if (body.TrackingId == trackID2)
            {
                var pos = body.Joints[TrackedJoint].Position;
                BodyParts2.transform.position = new Vector3(pos.X * multiplier, (pos.Y * multiplier) + 1);
            }

        }
       
        /*
        foreach (var body in bodies)
        {
            if (body == null)
            {
                continue;
            }
            if (body.IsTracked)
            {
                currTrackingId = body.TrackingId;
                var pos = body.Joints[TrackedJoint].Position;
                gameObject.transform.position = new Vector3(pos.X * multiplier, (pos.Y * multiplier)+1);
                
            }

            if (TrackingIdFirst != currTrackingId)
            {
                TrackingIdFirst = currTrackingId;
                continue;
            }



            if (TrackingIdSecond != currTrackingId && TrackingIdSecond != TrackingIdFirst)
            {
                TrackingIdFirst = currTrackingId;
                continue;
            }


        }
        */
    }
}
