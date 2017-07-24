using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualMesh : MonoBehaviour
{

    Mesh mesh;

    [SerializeField]
    Transform[] pointObjects;

    Vector3[] points;

    GameObject g;
    [SerializeField]
    Material mat;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateMesh();
    }

    private void UpdateMesh()
    {
        if (g == null)
        {
            CreateObject();
        }

        points = new Vector3[pointObjects.Length + 1];
        for (int i = 0; i < pointObjects.Length; i++)
        {
            Debug.Log("points: " + i);
            points[i] = pointObjects[i].position;
        }


        // point 5 is average:
        Vector3 sum = Vector3.zero;
        int count = 0;
        for (int i = 0; i < points.Length - 1; i++)
        {
            count++;
            sum += points[i];
        }
        points[count] = sum / count;

        mesh.vertices = points;

        int[] tris = new int[] {
            0,1,4,
            1,2,4,
            2,3,4,
            3,0,4
        };
        mesh.triangles = tris;

        mesh.uv = new Vector2[]
        {
            new Vector2(0,0),
            new Vector2(0,1),
            new Vector2(1,1),
            new Vector2(1,0),
            new Vector2(0.5f,0.5f)
        };

    }

    private void CreateObject()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
        }

        g = new GameObject();
        g.AddComponent<MeshFilter>().sharedMesh = mesh;
        g.transform.position = Vector3.zero;
        g.AddComponent<MeshRenderer>().material = mat;
    }
}
