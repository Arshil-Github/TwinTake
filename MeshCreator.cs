using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCreator : MonoBehaviour
{
    private Mesh mesh;
    private Vector3 ori;
    private float startingAngle;
    public float fov = 50f;
    public float visionDistance = 2f;
    public float zPosition;


    private bool tempPlayerBool;
    [HideInInspector] public bool playerBool;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
    }
    private void LateUpdate()
    {
        Vector3 origin = ori;
        int rayCount = 50;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;


        Vector3[] vertices = new Vector3[rayCount + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = Vector3.zero;
        int vertexIndex = 1;
        int triangleIndex = 0;

        for (int i = 0; i <= rayCount; i++)
        {
            float angleRad = angle * (Mathf.PI / 180f);
            Vector3 dirVec = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
            Vector3 vertex;

            RaycastHit2D raycast = Physics2D.Raycast(origin, dirVec, visionDistance);

            if (raycast.collider == null)
            {
                //Run the original code
                vertex = Vector3.zero + dirVec * visionDistance;

            }
            else
            {
                //Vertex at the point of collision
                vertex = transform.InverseTransformPoint(raycast.point);
           
                if(raycast.collider.CompareTag("Player"))
                {
                    tempPlayerBool = true;
                }

                //Debug.Log(raycast.point);
            }
           

            vertices[vertexIndex] = vertex;


            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            uv[vertexIndex] = new Vector2(0.5f, 0.5f);

            angle -= angleIncrease;
            vertexIndex++;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;


        gameObject.GetComponent<MeshFilter>().mesh = mesh;
    }
    private void Update()
    {
        playerBool = tempPlayerBool;
        tempPlayerBool = false;
        transform.position = new Vector3(transform.position.x, transform.position.y, zPosition);

    }
    public void SetOrigin(Vector3 origin) {
        this.ori = origin;
    }
    public void SetDirection(Vector3 dir)
    {
        dir = dir.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        startingAngle = angle + fov / 2f;
    }
}
