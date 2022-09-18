using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceControl : MonoBehaviour
{

    public GameObject brain;
    public Material transparentMaterial;
    public float rotationSpeed = 10;
    private Mesh brainMesh;
    private GameObject fluoroscopicImage;

    // Start is called before the first frame update
    void Start()
    {
        brainMesh = brain.GetComponent<MeshFilter>().mesh;

        fluoroscopicImage = new GameObject();
        fluoroscopicImage.AddComponent<MeshFilter>();
        fluoroscopicImage.AddComponent<MeshRenderer>();
        fluoroscopicImage.GetComponent<Renderer>().material = transparentMaterial;

        fluoroscopicImage.transform.position = new Vector3(-1, 0, 0);
        fluoroscopicImage.transform.localScale = brain.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyDown(KeyCode.X))
        {
            TookXRay();
        }

        else if( Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.right * -rotationSpeed * Time.deltaTime);
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        }
    }

    void TookXRay() 
    {
        Vector3 direction = Vector3.Normalize(transform.GetChild(0).transform.up);

        Mesh fluoroscopyImage = new Mesh();

        List<Vector3> imgVertices = new List<Vector3>();
        List<int> imgTriangles = new List<int>();
        List<Color> imgColors = new List<Color>();

        Vector3[] brainVertieces = brainMesh.vertices;
        int[] brainTriangles = brainMesh.triangles;

        for (int i = 0; i < brainTriangles.Length - 2; i += 3)
        {
            /*
            imgVertices.Add(new Vector3(brainVertieces[brainTriangles[i]].x, brainVertieces[brainTriangles[i]].y, 0));
            imgVertices.Add(new Vector3(brainVertieces[brainTriangles[i + 1]].x, brainVertieces[brainTriangles[i + 1]].y, 0));
            imgVertices.Add(new Vector3(brainVertieces[brainTriangles[i + 2]].x, brainVertieces[brainTriangles[i + 2]].y, 0));
            */
            imgVertices.Add( Vector3.Cross(brainVertieces[brainTriangles[i]], direction));
            imgVertices.Add(Vector3.Cross(brainVertieces[brainTriangles[i+1]], direction));
            imgVertices.Add(Vector3.Cross(brainVertieces[brainTriangles[i+2]], direction));

            imgTriangles.Add(i);
            imgTriangles.Add(i + 1);
            imgTriangles.Add(i + 2);

            /*
            imgColors.Add(new Color(color, color, color, transparency));
            imgColors.Add(new Color(color, color, color, transparency));
            imgColors.Add(new Color(color, color, color, transparency));
            */
        }

        fluoroscopyImage.vertices = imgVertices.ToArray();
        fluoroscopyImage.triangles = imgTriangles.ToArray();
        fluoroscopyImage.colors = imgColors.ToArray();

        fluoroscopicImage.GetComponent<MeshFilter>().mesh = fluoroscopyImage;
    }
}
