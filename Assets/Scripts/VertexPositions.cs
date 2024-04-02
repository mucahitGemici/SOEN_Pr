using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexPositions : MonoBehaviour
{
    //[SerializeField] MeshFilter mf;
    public Vector3[] verticesWorldPositions;

    public Transform[] reducedNumVertexTransforms;

    public bool updateRealVertexPositions;


    private void Start()
    {
        //verticesWorldPositions = mf.mesh.vertices;
        //Debug.Log(verticesWorldPositions.Length);
    }

    private void Update()
    {
        if (updateRealVertexPositions)
        {
            //transform.TransformPoints(mf.mesh.vertices, verticesWorldPositions);
        }
    }

}
