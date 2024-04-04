using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class PlacingObject : MonoBehaviour
{
    public delegate void PlacingDelegate(float positionDifference, float angle);
    public event PlacingDelegate OnPlacingEnded;

    
    public XROffsetGrabInteractable xrOffsetGrabInteractable;
    public Torus torusRef;
    public Wire wireRef;
    public enum TaskType
    {
        Torus,
        ComplexWire
    }
    public TaskType taskType;
    [SerializeField] private int participantNumber;
    private void OnTriggerEnter(Collider other)
    {
        if(taskType == TaskType.Torus)
        {
            Rigidbody rb = other.transform.parent.parent.GetComponent<Rigidbody>();
            rb.useGravity = false;
        }
        else if(taskType == TaskType.ComplexWire)
        {
            Rigidbody rb = other.transform.parent.parent.parent.GetComponent<Rigidbody>();
            rb.useGravity = false;
        }
        

    }

    private void OnTriggerStay(Collider other)
    {
        if(xrOffsetGrabInteractable.isHolding == false)
        {
            //Debug.Log($"stop the placing! last position is: {xrOffsetGrabInteractable.transform.position}");
            xrOffsetGrabInteractable.GetComponent<Rigidbody>().isKinematic = true;
            float posDiff = Vector3.Distance(transform.position, xrOffsetGrabInteractable.transform.position);
            float angleDiff = Quaternion.Angle(transform.rotation, xrOffsetGrabInteractable.transform.rotation);
            Vector3 finalPos = xrOffsetGrabInteractable.transform.position;
            Vector3 finalRot = xrOffsetGrabInteractable.transform.rotation.eulerAngles;
            if (OnPlacingEnded != null) OnPlacingEnded(posDiff, angleDiff);

            if(taskType == TaskType.Torus)
            {
                RecordData(xrOffsetGrabInteractable.GetTaskTimer, torusRef.GetHitNumber, posDiff, angleDiff, finalPos, finalRot);
            }
            else if(taskType == TaskType.ComplexWire)
            {
                RecordData(xrOffsetGrabInteractable.GetTaskTimer, wireRef.GetHitNumber, posDiff, angleDiff, finalPos, finalRot);
            }
            
            this.gameObject.SetActive(false);
        }
    }

    private void RecordData(float taskTime, int numHits, float positionDifference, float angleDifference, Vector3 finalPos, Vector3 finalRot)
    {
        string data = $"{taskTime}\n{numHits}\n{positionDifference}\n{angleDifference}\n{finalPos.x}\n{finalPos.y}\n{finalPos.z}\n{finalRot.x}\n{finalRot.y}\n{finalRot.z}";

        string path = "";
        if(taskType == TaskType.Torus)
        {
            path = Application.dataPath + $"/Datas/Torus/participant{participantNumber}.txt";
        }
        else if(taskType == TaskType.ComplexWire)
        {
            path = Application.dataPath + $"/Datas/ComplexWire/participant{participantNumber}.txt";
        }

        if(!File.Exists(path))
        {
            File.WriteAllText(path, data);
        }
        else
        {
            File.WriteAllText(path, data);
        }
    }

}
