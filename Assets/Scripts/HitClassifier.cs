using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitClassifier : MonoBehaviour
{
    public Transform[] colliderTransforms;

    public void ClassifyHit(Transform colTransform)
    {
        float[] xValues = new float[colliderTransforms.Length];
        float[] yValues = new float[colliderTransforms.Length];
        float[] zValues = new float[colliderTransforms.Length];

        for(int i = 0; i < colliderTransforms.Length; i++)
        {
            xValues[i] = colliderTransforms[i].transform.position.x;
            yValues[i] = colliderTransforms[i].transform.position.y;
            zValues[i] = colliderTransforms[i].transform.position.z;
        }

        if (colTransform.position.x >= Mathf.Max(xValues))
        {
            Debug.Log($"colliding object has the maximum x! object name: {colTransform.name} => DEPTH MISTAKE");
        }

        else if(colTransform.transform.position.x <= Mathf.Min(xValues))
        {
            Debug.Log($"colliding object has the minumum x! object name: {colTransform.name} => DEPTH MISTAKE");
        }

        /*
        // y
        else if (colTransform.position.y >= Mathf.Max(yValues))
        {
            Debug.Log($"colliding object has the maximum y! object name: {colTransform.name} => UP-DOWN MISTAKE");
        }

        else if (colTransform.transform.position.y <= Mathf.Min(yValues))
        {
            Debug.Log($"colliding object has the minumum y! object name: {colTransform.name} => UP-DOWN MISTAKE");
        }

        // z
        else if (colTransform.position.z >= Mathf.Max(zValues))
        {
            Debug.Log($"colliding object has the maximum z! object name: {colTransform.name} => LEFT-RIGHT MISTAKE");
        }

        else if (colTransform.transform.position.z <= Mathf.Min(zValues))
        {
            Debug.Log($"colliding object has the minumum z! object name: {colTransform.name} => LEFT-RIGHT MISTAKE");
        }
        */
    }
}
