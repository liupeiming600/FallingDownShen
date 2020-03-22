using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectController : MonoBehaviour {

    SpriteRenderer MainSpriteRenderer;
    // publicで宣言し、inspectorで設定可能にする
    public Sprite StandbySprite;
    public Sprite HoldSprite;
    public Sprite SlashSprite;

    Vector3 screenPoint;
    Vector3 offset;

    Rigidbody2D rb;
    private PolygonCollider2D polygonCollider;
    private Sprite sprite;

    public const string LAYER_NAME = "TopLayer";
    public int sortingOrder = 8;
    private SpriteRenderer spriteRenderer;

    private Vector3 position_now;
    private Vector3 position_prev;
    private Quaternion rotation_now;
    private Quaternion rotation_prev;
    private float distPos = 0f;
    private float simiRot = 0f;
    private float timePrev;
    private Texture2D texture;
    private Sprite image;
    public bool IsMove = true;
    public bool IsThrew = false;
    public bool IsHold = false;
    public bool WasHold = false;
    public float timeThrew = 0f;
    public float timeFromThrew = 0f;
    public float thresholdDistPos = 0.001f;
    public float thresholdSimiRot = 0.99f;

    public Texture2D tex;
    public Texture2D tex2;

    SnapshotCameraTest Snapshotter;

    int playerTurn = 1;

    // Use this for initialization
    void Start() {
        // paste the image which you write
        //Sprite image = Resources.Load<Sprite>("images/players/human");

        if (playerTurn == 1)
        {
            Snapshotter = GameObject.Find("Sub Camera").GetComponent<SnapshotCameraTest>();
            playerTurn = 2;
        }

        else if (playerTurn == 2)
        {
            Snapshotter = GameObject.Find("Sub Camera 2").GetComponent<SnapshotCameraTest>();
            playerTurn = 1;
        }

        //texture = new Texture2D(Snapshotter.texture.width, Snapshotter.texture.height);

        //Graphics.CopyTexture(Snapshotter.texture, texture);

        /*
        texture = new Texture2D(Snapshotter.texture.width, Snapshotter.texture.height);
        texture.SetPixels(Snapshotter.texture.GetPixels());
        */

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer)
        {
            spriteRenderer.sortingOrder = sortingOrder;
            spriteRenderer.sortingLayerName = LAYER_NAME;
        }


        tex = Snapshotter.texture;
        tex2 = new Texture2D(tex.width, tex.height);
        Color[] colors = tex.GetPixels();// (0, 0, tex.width, tex.height);
        tex2.SetPixels(colors);
        tex2.Apply();
        //Sprite image = Sprite.Create(tex2, new Rect(0, 0, tex.width, tex.height), new Vector2(0.0f, 0.0f), 1.0f);

        //texture = Snapshotter.texture;
        texture = tex2;
        Sprite image = Sprite.Create(
           texture: texture,
           rect: new Rect(0, 0, texture.width, texture.height),
           pivot: new Vector2(0.5f, 0.5f),
           pixelsPerUnit: 100.0f
         );


        this.GetComponent<SpriteRenderer>().sprite = image;

        rb = this.GetComponent<Rigidbody2D>();
        ResetPolygonCollider2D();
        position_now = this.transform.position;
        rotation_now = this.transform.rotation;
        UpdateTranform();
        timePrev = Time.time;

        rb.gravityScale = 0.75f;
        IsThrew = true;

    }

    void Update() {
        //ResetPolygonCollider2D();
        
        if (Time.time - timePrev > 3f) {
            UpdateTranform();
        }
        DetectMotionsOfPics();
        

        /*
        // Process when the moment that mouse button up
        if (WasHold == true && IsHold == false) {
            IsThrew = true;
            timeThrew = Time.time;
        }

        if (IsThrew) {
            rb.gravityScale = 0.75f;
            timeFromThrew = Time.time - timeThrew;
        }

        WasHold = IsHold;
        */
    }

    /*
    void OnMouseDown() {
        if (!IsThrew) {
           this.screenPoint = Camera.main.WorldToScreenPoint(transform.position);
           this.offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
           IsHold = true;
        }
    }

    void OnMouseUp() {
        if (!IsThrew) {
           IsHold = false;
        }
    }
    
    void OnMouseDrag() {
        if (!IsThrew) {
            Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint) + this.offset;
            transform.position = new Vector3(currentPosition.x, transform.position.y, transform.position.z);
        }
    }
    */



    void ResetPolygonCollider2D() {
        //Debug.Log("ResetPolygonCollider2D");
        polygonCollider = GetComponent<PolygonCollider2D>();
        Destroy(polygonCollider);
        this.gameObject.AddComponent<PolygonCollider2D>();

        sprite = this.GetComponent<SpriteRenderer>().sprite;
        for (int i = 0; i < polygonCollider.pathCount; i++) polygonCollider.SetPath(i, null);
        polygonCollider.pathCount = sprite.GetPhysicsShapeCount();

        List<Vector2> path = new List<Vector2>();
        for (int i = 0; i < polygonCollider.pathCount; i++) {
            path.Clear();
            sprite.GetPhysicsShape(i, path);
            polygonCollider.SetPath(i, path.ToArray());
        }
    }

    void DetectMotionsOfPics() {
        distPos = Vector3.Distance(position_now, position_prev);
        simiRot = CosSimilarity(rotation_now, rotation_prev);
        if (distPos > thresholdDistPos || simiRot < thresholdSimiRot) {
            IsMove = true;
        } else {
            IsMove = false;
        }
    }

    float CosSimilarity(Quaternion a, Quaternion b) {
        float aa = Mathf.Sqrt(Quaternion.Dot(a, a));
        float bb = Mathf.Sqrt(Quaternion.Dot(b, b));
        return Quaternion.Dot(a, b) / (aa * bb);
    }

    void UpdateTranform() {
        position_prev = new Vector3(position_now.x, position_now.y, position_now.z);
        position_now = this.transform.position;
        rotation_prev = new Quaternion(rotation_now.x, rotation_now.y, rotation_now.z, rotation_now.w);
        rotation_now = this.transform.rotation;

        timePrev = Time.time;
    }

    Sprite GetAtRandom(Sprite[] list) {
        if (list.Length == 0) {
            Debug.LogError("リストが空です！");
        }
        return list[UnityEngine.Random.Range(0, list.Length)];
    }

}

