using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour
{

    MeshRenderer rend;
    MeshFilter filter;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        filter = GetComponent<MeshFilter>();

        Mesh m = new Mesh();
        Vector3 v1 = new Vector3(0, 0, 0);
        Vector3 v2 = new Vector3(0, 1, 0);
        Vector3 v3 = new Vector3(1, 1, 0);
        List<Vector3> verts = new List<Vector3>();
        verts.Add(v1);
        verts.Add(v2);
        verts.Add(v3);
        m.vertices = verts.ToArray();

        List<int> triangles = new List<int>();
        triangles.Add(0);
        triangles.Add(1);
        triangles.Add(2);
        m.triangles = triangles.ToArray();

        filter.mesh = m;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
