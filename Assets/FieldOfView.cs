using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    // Start is called before the first frame update
    private Mesh mesh;
    private Vector3 origin;
    [SerializeField] private float fov;
    [SerializeField] private float viewDistance;
    private float startingAngle;
    [SerializeField] private LayerMask layerMask;
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        SetOrigin(Vector3.zero);
    }
    
    void LateUpdate(){
        int rayCount = 50;
        float angle = startingAngle + 90f;
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;
    
        int vertexIndex = 1;
        int triangleIndex = 0;
        for(int i = 0; i <= rayCount; i++){
            Vector3 vertex;
            //RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, GetVectorFromAngle(angle), viewDistance, layerMask);
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);
            if(raycastHit2D.collider == null){
                //no hit
                vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            }
            else {
                //Hit
                //vertex = transform.InverseTransformPoint(raycastHit2D.point);
                vertex = origin;
            }
            vertices[vertexIndex] = vertex;

            if(i > 0){
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }           

            angle -= angleIncrease;
            vertexIndex++;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;  
    }

    public static Vector3 GetVectorFromAngle(float angle){
        // angle = 0 -> 360
        float angleRad = angle * (Mathf.PI/180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public static float GetAngleFromVectorFloat(Vector3 dir){
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if( n < 0) n += 360;

        return n;
    }

    public void SetOrigin(Vector3 inOrigin){
        origin = inOrigin;
    }

    public void SetAimDirection(Vector3 aimDirection){
        startingAngle = GetAngleFromVectorFloat(aimDirection) - fov/2f;
    }

    public float GetAimDirection(){
        return startingAngle;
    }

    public void SetFoV(float inFOV){
        fov = inFOV;
    }

    public void SetViewDistance(float viewDistance){
        this.viewDistance = viewDistance;
    }

}
