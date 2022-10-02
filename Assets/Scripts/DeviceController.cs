using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceController : MonoBehaviour
{

    public GameObject brain;
    public Material transparentMaterial;
    public float rotationSpeed = 10;

    private Mesh brainMesh;
    private GameObject fluoroscopicImage;

    // Start is called before the first frame update
    void Start()
    {
        brainMesh = brain.transform.GetChild(0).GetComponent<MeshFilter>().mesh;
        //brainMesh = brain.transform.GetComponent<MeshFilter>().mesh;

        fluoroscopicImage = new GameObject();
        fluoroscopicImage.AddComponent<MeshFilter>();
        fluoroscopicImage.AddComponent<MeshRenderer>();
        fluoroscopicImage.GetComponent<Renderer>().material = transparentMaterial;

        fluoroscopicImage.transform.position = new Vector3(0, 0, 0);
        fluoroscopicImage.transform.localScale = brain.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKey(KeyCode.X))
        {
            TookXRay();
        }
        */

        TookXRay();

        if( Input.GetKey(KeyCode.LeftArrow))
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
        fluoroscopicImage.transform.rotation = Quaternion.Euler(Vector3.zero);

        Vector3 direction = Vector3.Normalize(transform.GetChild(0).transform.up);

        Mesh fluoroscopyMesh = new Mesh();

        fluoroscopyMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        List<Vector3> imgVertices = new List<Vector3>();
        List<int> imgTriangles = new List<int>();
        List<Color> imgColors = new List<Color>();

        Vector3[] brainVertieces = brainMesh.vertices;
        int[] brainTriangles = brainMesh.triangles;

        for (int i = 0; i < brainTriangles.Length - 2; i += 3)
        {
            Vector3 p0 = Vector3.Cross(brainVertieces[brainTriangles[i]], direction);
            Vector3 p1 = Vector3.Cross(brainVertieces[brainTriangles[i+1]], direction);
            Vector3 p2 = Vector3.Cross(brainVertieces[brainTriangles[i+2]], direction);

            Plane plane = new Plane(p0, p1, p2);
            Vector3 normal = Vector3.Normalize(plane.normal);

            imgVertices.Add(p0);
            imgVertices.Add(p1);
            imgVertices.Add(p2);
            
            if( normal + direction == Vector3.zero)
            {
                imgTriangles.Add(i + 2);
                imgTriangles.Add(i + 1);
                imgTriangles.Add(i);
            }
            else
            {
                imgTriangles.Add(i);
                imgTriangles.Add(i + 1);
                imgTriangles.Add(i + 2);
            }
        }

        fluoroscopyMesh.vertices = imgVertices.ToArray();
        fluoroscopyMesh.triangles = imgTriangles.ToArray();
        fluoroscopyMesh.colors = imgColors.ToArray();
        fluoroscopyMesh.RecalculateNormals();

        fluoroscopicImage.GetComponent<MeshFilter>().mesh = fluoroscopyMesh;
        fluoroscopicImage.transform.position = Vector3.zero;

        if(direction.z > 0)
        {
            fluoroscopicImage.transform.Rotate(-transform.rotation.eulerAngles.x, 90, 0, Space.World);
        }
        else
        {
            fluoroscopicImage.transform.Rotate(transform.rotation.eulerAngles.x - 180, 90, 0, Space.World);
        }
    }

    private float TriangleArea(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        return Vector3.Distance(v1, v2) * Vector3.Distance(v3, v2) * Mathf.Sin(Vector3.Angle(v1-v2, v3-v2)) / 2;
    }
}