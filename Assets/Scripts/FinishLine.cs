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
    private string timeStamps = "timeStamps\n";
    private string taskTypeString = "taskType\n";
    private string taskTypeFilename;
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
    [SerializeField] private int repetitionNum;

    [SerializeField] private AudioSource successSource;

    [SerializeField] private bool recordData;

    private bool canRecordSpeed = false;

    public struct NewDataLine
    {
        public string participantNumber;
        public string task;
        public string sdfState;
        public string repetition;
        public string timeStamp;
        public string numHits;
        public string timeToCompleteTask;
        public string instanteneousVelocityMag;
        public string instanteneousVelocityX;
        public string instanteneousVelocityY;
        public string instanteneousVelocityZ;
        public string closestDistance;

        public NewDataLine(string ParticipantNumber, string Task, string SdfState,string Repetition, string TimeStamp, string NumHits, string TimeToCompleteTask,
            string InstanteneousVelocityMag, string InstanteneousVelocityX, string InstanteneousVelocityY, string InstanteneousVelocityZ, string ClosestDistance)
        {
            this.participantNumber = ParticipantNumber;
            this.task = Task;
            this.sdfState = SdfState;
            this.repetition = Repetition;
            this.timeStamp = TimeStamp;
            this.numHits = NumHits;
            this.timeToCompleteTask = TimeToCompleteTask;
            this.instanteneousVelocityMag = InstanteneousVelocityMag;
            this.instanteneousVelocityX = InstanteneousVelocityX;
            this.instanteneousVelocityY = InstanteneousVelocityY;
            this.instanteneousVelocityZ = InstanteneousVelocityZ;
            this.closestDistance = ClosestDistance;
        }
    }
    private List<NewDataLine> newDataLinesList = new List<NewDataLine>();

    private void Start()
    {
        switch (taskType)
        {
            case TaskType.Torus:
                taskTypeString += "torus\n";
                taskTypeFilename = "torus";
                break;
            case TaskType.ComplexWire:
                taskTypeString += "complexWire\n";
                taskTypeFilename = "complexWire";
                break;
            case TaskType.EasyWire:
                taskTypeString += "easyWire\n";
                taskTypeFilename = "easyWire";
                break;

        }

        NewDataLine firstLine = new NewDataLine(ParticipantNumber: "ParticipantNumber", Task: "Task", SdfState: "SdfState", Repetition: "Repetition", TimeStamp: "TimeStamp",
            NumHits: "NumHits", TimeToCompleteTask: "TimeToCompleteTask", InstanteneousVelocityMag: "InstanteneousVelocityMag", InstanteneousVelocityX: "InstanteneousVelocityX",
            InstanteneousVelocityY: "InstanteneousVelocityY", InstanteneousVelocityZ: "InstanteneousVelocityZ", ClosestDistance: "ClosestDistance");
        newDataLinesList.Add(firstLine);

    }
    private void Update()
    {
        if (xrOffsetGrabInteractable.StartTimerBoolean)
        {
            //Debug.Log($"velocity: {rb.velocity.magnitude}, components: x:{rb.velocity.x}, y:{rb.velocity.y}, z:{rb.velocity.z}");
            //velocityString += $"{rb.velocity.x}\n{rb.velocity.y}\n{rb.velocity.z}\n";
            velocityMagnitudeString += $"{rb.velocity.magnitude}\n";
            velocityComponentsString += $"{rb.velocity.x}\n{rb.velocity.y}\n{rb.velocity.z}\n";
            closestDistanceString += $"{readText.ClosestDistance}\n";
            timeStamps += $"{Time.time}\n";

            NewDataLine newLine = new NewDataLine(ParticipantNumber: "-", Task: $"{taskTypeFilename}", SdfState: "-", Repetition: "-", TimeStamp: $"{Time.time}",
            NumHits: "-", TimeToCompleteTask: "-", InstanteneousVelocityMag: $"{rb.velocity.magnitude}", InstanteneousVelocityX: $"{rb.velocity.x}",
            InstanteneousVelocityY: $"{rb.velocity.y}", InstanteneousVelocityZ: $"{rb.velocity.z}", ClosestDistance: $"{readText.ClosestDistance}");
            newDataLinesList.Add(newLine);
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
        string data = $"participantNumber\n{participantNumber}\nrepetitionNum\n{repetitionNum}\nsdfEnabled\n{sdfEnabled}\ntaskTime\n{taskTime}\nnumHits\n{numHits}\npositionDifference\n{positionDifference}\nangleDifference\n{angleDifference}\nfinalPosX\n{finalPos.x}\nfinalPosY\n{finalPos.y}\nfinalPosZ\n{finalPos.z}\nfinalRotX\n{finalRot.x}\nfinalRotY\n{finalRot.y}\nfinalRotZ\n{finalRot.z}\n";

        for(int i = 1; i < newDataLinesList.Count; i++)
        {
            // for the new data format
            NewDataLine line = newDataLinesList[i];
            line.participantNumber = $"{participantNumber}";
            line.sdfState = $"{sdfEnabled}";
            line.repetition = $"{repetitionNum}";
            line.numHits = $"{numHits}";
            line.timeToCompleteTask = $"{taskTime}";
            newDataLinesList[i] = line;
        }

        data += taskTypeString;
        data += velocityMagnitudeString;
        data += velocityComponentsString;
        data += closestDistanceString;
        data += timeStamps;
        string path = "";
        string newDataFormatPath = "";
        if(taskType == TaskType.Torus)
        {
            path = Application.dataPath + $"/Datas/Torus/participant{participantNumber}_rep{repetitionNum}_sdf{sdfEnabled}_task{taskTypeFilename}.txt";
            newDataFormatPath = Application.dataPath + $"/Datas/Torus/participant{participantNumber}_rep{repetitionNum}_sdf{sdfEnabled}_task{taskTypeFilename}_NewFormat.txt";
        }
        else if(taskType == TaskType.ComplexWire)
        {
            path = Application.dataPath + $"/Datas/ComplexWire/participant{participantNumber}_rep{repetitionNum}_sdf{sdfEnabled}_task{taskTypeFilename}.txt";
            newDataFormatPath = Application.dataPath + $"/Datas/ComplexWire/participant{participantNumber}_rep{repetitionNum}_sdf{sdfEnabled}_task{taskTypeFilename}_NewFormat.txt";
        }
        else if(taskType == TaskType.EasyWire)
        {
            path = Application.dataPath + $"/Datas/EasyWire/participant{participantNumber}_rep{repetitionNum}_sdf{sdfEnabled}_task{taskTypeFilename}.txt";
            newDataFormatPath = Application.dataPath + $"/Datas/EasyWire/participant{participantNumber}_rep{repetitionNum}_sdf{sdfEnabled}_task{taskTypeFilename}_NewFormat.txt";
        }
        //

        if(!File.Exists(path))
        {
            File.WriteAllText(path, data);
            StreamWriter writer = new StreamWriter(newDataFormatPath, true);
            foreach(NewDataLine dataLine in newDataLinesList)
            {
                writer.WriteLine($"{dataLine.participantNumber};{dataLine.task};{dataLine.sdfState};{dataLine.repetition};{dataLine.timeStamp};{dataLine.numHits};{dataLine.timeToCompleteTask};{dataLine.instanteneousVelocityMag};{dataLine.instanteneousVelocityX};{dataLine.instanteneousVelocityY};{dataLine.instanteneousVelocityZ};{dataLine.closestDistance}");
            }
            writer.Close();
        }
        else
        {
            File.WriteAllText(path, data);
            StreamWriter writer = new StreamWriter(newDataFormatPath, true);
            foreach (NewDataLine dataLine in newDataLinesList)
            {
                writer.WriteLine($"{dataLine.participantNumber};{dataLine.task};{dataLine.sdfState};{dataLine.repetition};{dataLine.timeStamp};{dataLine.numHits};{dataLine.timeToCompleteTask};{dataLine.instanteneousVelocityMag};{dataLine.instanteneousVelocityX};{dataLine.instanteneousVelocityY};{dataLine.instanteneousVelocityZ};{dataLine.closestDistance}");
            }
            writer.Close();
        }

        
        //Debug.Log("Data is recorded");
    }

}
