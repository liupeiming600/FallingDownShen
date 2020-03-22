using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{

    public GameObject gameObject1;          // Reference to the first GameObject
    public GameObject gameObject2;          // Reference to the second GameObject

    private LineRenderer line;

    // Use this for initialization
    void Start()
    {
        // Add a Line Renderer to the GameObject
        line = this.gameObject.AddComponent<LineRenderer>();
        // Set the width of the Line Renderer
        line.SetWidth(0.05F, 0.05F);
        line.sortingOrder = 1;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.material.color = Color.red;
        // Set the number of vertex fo the Line Renderer
        line.SetVertexCount(2);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the GameObjects are not null
        if (gameObject1 != null && gameObject2 != null)
        {
            // Update position of the two vertex of the Line Renderer
            line.SetPosition(0, gameObject1.transform.position);
            line.SetPosition(1, gameObject2.transform.position);
        }
    }
}
