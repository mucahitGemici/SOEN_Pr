using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class FinishLine : MonoBehaviour
{
    public delegate void PlacingDelegate(float positionDifference, float angle);
    public event PlacingDelegate OnPlacingEnded;

    
    public XROffsetGrabInteractable xrOffsetGrabInteractable;
    public Torus torusRef;
    [SerializeField] private PathManager pathManager;
    [SerializeField] private ReadText readText;
    [SerializeField] private Rigidbody rb;
    private string velocityMagnitudeString = "velocityMagnitude\n";
    private string velocityComponentsString = "velocityComponents\n";
    private string closestDistanceString = "closestDistance\n";
    public Wire wireRef;

    [SerializeField] private MeshFilter torusMeshFilter;
    [SerializeField] private MeshFilter objectMeshFilter;
    public enum TaskType
    {
        Torus,
        ComplexWire,
        EasyWire
    }
    public TaskType taskType;
    [SerializeField] private int participantNumber;
    [SerializeField] private int experimentNumber;

    [SerializeField] private AudioSource successSource;

    [SerializeField] private bool recordData;

    private bool canRecordSpeed = false;

    private void FixedUpdate()
    {
        //Debug.Log($"Speed: {rb.velocity}");
        if (xrOffsetGrabInteractable.StartTimerBoolean)
        {
            //Debug.Log($"velocity: {rb.velocity.magnitude}, components: x:{rb.velocity.x}, y:{rb.velocity.y}, z:{rb.velocity.z}");
            //velocityString += $"{rb.velocity.x}\n{rb.velocity.y}\n{rb.velocity.z}\n";
            velocityMagnitudeString += $"{rb.velocity.magnitude}\n";
            velocityComponentsString += $"{rb.velocity.x}\n{rb.velocity.y}\n{rb.velocity.z}\n";
            closestDistanceString += $"{readText.ClosestDistance}\n";
            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(taskType == TaskType.Torus)
        {
            Rigidbody rb = other.transform.parent.parent.GetComponent<Rigidbody>();
            rb.useGravity = false;
        }
        else if(taskType == TaskType.ComplexWire || taskType == TaskType.EasyWire)
        {
            Rigidbody rb = other.transform.parent.parent.parent.GetComponent<Rigidbody>();
            rb.useGravity = false;
        }
        

    }

    private void OnTriggerStay(Collider other)
    {
        /*
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
            else if(taskType == TaskType.ComplexWire || taskType == TaskType.EasyWire)
            {
                RecordData(xrOffsetGrabInteractable.GetTaskTimer, wireRef.GetHitNumber, posDiff, angleDiff, finalPos, finalRot);
            }
            
            this.gameObject.SetActive(false);
        }
        */

        if ((taskType == TaskType.Torus && pathManager.PathIsDone) || (taskType == TaskType.EasyWire) || (taskType == TaskType.ComplexWire))
        {
            //Debug.Log($"stop the placing! last position is: {xrOffsetGrabInteractable.transform.position}");
            //xrOffsetGrabInteractable.GetComponent<Rigidbody>().isKinematic = true;
            float posDiff = Vector3.Distance(transform.position, xrOffsetGrabInteractable.transform.position);
            float angleDiff = Quaternion.Angle(transform.rotation, xrOffsetGrabInteractable.transform.rotation);
            Vector3 finalPos = xrOffsetGrabInteractable.transform.position;
            Vector3 finalRot = xrOffsetGrabInteractable.transform.rotation.eulerAngles;
            if (OnPlacingEnded != null) OnPlacingEnded(posDiff, angleDiff);
            xrOffsetGrabInteractable.StartTimerBoolean = false;

            if (taskType == TaskType.Torus && recordData)
            {
                RecordData(xrOffsetGrabInteractable.GetTaskTimer, torusRef.GetHitNumber, posDiff, angleDiff, finalPos, finalRot);
            }
            else if ((taskType == TaskType.ComplexWire || taskType == TaskType.EasyWire) && recordData)
            {
                RecordData(xrOffsetGrabInteractable.GetTaskTimer, wireRef.GetHitNumber, posDiff, angleDiff, finalPos, finalRot);
            }

            successSource.Play();
            this.gameObject.SetActive(false);
        }
    }

    private void RecordData(float taskTime, int numHits, float positionDifference, float angleDifference, Vector3 finalPos, Vector3 finalRot)
    {
        int sdfEnabled = 0;
        if (readText.IsSdfEnabled)
        {
            sdfEnabled = 1;
        }
        string data = $"participantNumber\n{participantNumber}\nexperimentNumber\n{experimentNumber}\nsdfEnabled\n{sdfEnabled}\ntaskTime\n{taskTime}\nnumHits\n{numHits}\npositionDifference\n{positionDifference}\nangleDifference\n{angleDifference}\nfinalPosX\n{finalPos.x}\nfinalPosY\n{finalPos.y}\nfinalPosZ\n{finalPos.z}\nfinalRotX\n{finalRot.x}\nfinalRotY\n{finalRot.y}\nfinalRotZ\n{finalRot.z}\n";
        data += velocityMagnitudeString;
        data += velocityComponentsString;
        data += closestDistanceString;
        string path = "";
        if(taskType == TaskType.Torus)
        {
            path = Application.dataPath + $"/Datas/Torus/participant{participantNumber}_ex{experimentNumber}_sdf{sdfEnabled}.txt";
        }
        else if(taskType == TaskType.ComplexWire)
        {
            path = Application.dataPath + $"/Datas/ComplexWire/participant{participantNumber}_ex{experimentNumber}_sdf{sdfEnabled}.txt";
        }
        else if(taskType == TaskType.EasyWire)
        {
            path = Application.dataPath + $"/Datas/EasyWire/participant{participantNumber}_ex{experimentNumber}_sdf{sdfEnabled}.txt";
        }
        //

        if(!File.Exists(path))
        {
            File.WriteAllText(path, data);
        }
        else
        {
            File.WriteAllText(path, data);
        }

        
        //Debug.Log("Data is recorded");
    }

}
