using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskPath : MonoBehaviour
{
    public delegate void OnPathArrived(Transform reference);
    public event OnPathArrived onPathArrived;
    private void OnTriggerEnter(Collider other)
    {
        if(onPathArrived != null)
        {           
            onPathArrived(this.transform);
        }
    }
}
