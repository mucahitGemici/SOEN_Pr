using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ReadText : MonoBehaviour
{
    public enum ObjectType
    {
        TorusSceneObject,
        ComplexWireSceneObject,
        EasyWireSceneObject
    }
    public ObjectType objectType;
    [SerializeField] private DataManager dataManager;
    [SerializeField] private VertexPositions vertexPositions;
    public XROffsetGrabInteractable xrOffsetGrabInteractable;
    public TMPro.TMP_Text screenText;
    public TMPro.TMP_Text speedText;

    private float currentSDF;
    private float maxSDF;
    private float minSDF;

    //private float movementSpeed;
    public Slider speedSlider;
    private float maxSpeed = 1f;
    private float minSpeed = 0.05f;

    Vector3 lastConvertedVertexPos;
    Vector3 minPos;

    private bool isSDFenabled = false;
    public bool IsSdfEnabled
    {
        get { return isSDFenabled; }
    }
    private void Start()
    {

        speedSlider.minValue = 0;
        speedSlider.maxValue = maxSpeed;

        dataManager.ReadData();
        if (dataManager.IsDataReaded == false)
        {
            dataManager.ReadData();
        }

    }

    private void Update()
    {
        if (isSDFenabled == true && dataManager.IsDataReaded == true)
        {
            //Debug.Log("sdf is enabled");
            PrintCurrentSDF();
            //movementSpeed = Mathf.Lerp(0, maxSpeed, normalizedSDF());
            //speedText.text = $"SPEED: {movementSpeed}";
            //Debug.Log($"movementSpeed: {movementSpeed}");
            screenText.text = $"SDF = {currentSDF}";
        }
        else
        {
            //movementSpeed = maxSpeed;
            setSpeed(maxSpeed);
            screenText.text = $"SDF Disabled";
        }
       

    }

    private void PrintCurrentSDF()
    {
        Vector3 cPos = Vector3.zero;
        /*
        if(objectType == ObjectType.Box)
        {
            cPos = ConvertPosition(transform.position);
            int idx = System.Array.IndexOf(dataManager.torusPositionArray, cPos);
            currentSDF = dataManager.torusSDFArray[idx];
            maxSDF = dataManager.maxSDFTorus;
            //Debug.Log($"sdf: {currentSDF}");
        }
        else
        {
            //currentSDF = CalculateSdfWithVertexData();
            currentSDF = CalculateSDFWithReducedVertexData();
            maxSDF = dataManager.maxSDFWire;
            //Debug.Log($"sdf: {currentSDF}");
        }
        */

        if(objectType == ObjectType.TorusSceneObject)
        {
            currentSDF = CalculateSDFWithReducedVertexDataTorus();
            maxSDF = dataManager.maxSDFTorus;
        }
        else if(objectType == ObjectType.ComplexWireSceneObject)
        {
            currentSDF = CalculateSDFWithReducedVertexDataComplexWire();
            maxSDF = dataManager.maxSDFWire;
        }
        else if(objectType == ObjectType.EasyWireSceneObject)
        {
            currentSDF = CalculateSDFWithReducedVertexDataEasyWire();
            maxSDF = dataManager.MaxSDFeasyWire;
        }

        normalizedSDF();
    }

    /*
    private float CalculateSdfWithVertexData()
    {

        float minSDF = Mathf.Infinity;
        int minIndex = 999999;
        foreach (Vector3 vertexPos in vertexPositions.verticesWorldPositions)
        {
            int index = System.Array.IndexOf(posArray, ConvertPosition(vertexPos));
            if (sdfArray[index] < minSDF)
            {
                minSDF = sdfArray[index];
                minIndex = index;
            }
        }
        return sdfArray[minIndex];
    }
    */

    private float CalculateSDFWithReducedVertexDataComplexWire()
    {
        float minSDF = Mathf.Infinity;
        int minIndex = 999999;
        minPos = Vector3.one * 999999;
        foreach(Transform vertexTransform in vertexPositions.reducedNumVertexTransforms)
        {
            Vector3 convertedPos = ConvertPosition(vertexTransform.position);
            int curIdx = System.Array.IndexOf(dataManager.wirePositionArray, convertedPos);
            float sdf = dataManager.wireSDFArray[curIdx];
            //Debug.Log(sdf);
            if(sdf < minSDF)
            {
                minSDF = sdf;
                minIndex = curIdx;
                minPos = vertexTransform.position;
                lastConvertedVertexPos = convertedPos;
            }
        }

        if(minPos.x < -0.1f || minPos.x > 0.1f || minPos.y > 0.4f || minPos.y < -0.4f || minPos.z < -1f || minPos.z > 1f)
        {
            return 99;
        }
        else
        {
            return dataManager.wireSDFArray[minIndex];
        }
    }

    private float CalculateSDFWithReducedVertexDataEasyWire()
    {
        float minSDF = Mathf.Infinity;
        int minIndex = 999999;
        minPos = Vector3.one * 999999;
        foreach (Transform vertexTransform in vertexPositions.reducedNumVertexTransforms)
        {
            Vector3 convertedPos = ConvertPosition(vertexTransform.position);
            int curIdx = System.Array.IndexOf(dataManager.EasyWirePositionArray, convertedPos);
            float sdf = dataManager.EasyWireSDFArray[curIdx];
            //Debug.Log(sdf);
            if (sdf < minSDF)
            {
                minSDF = sdf;
                minIndex = curIdx;
                minPos = vertexTransform.position;
                lastConvertedVertexPos = convertedPos;
            }
        }

        if (minPos.x < -0.1f || minPos.x > 0.1f || minPos.y > 0.4f || minPos.y < -0.4f || minPos.z < -1f || minPos.z > 1f)
        {
            return 99;
        }
        else
        {
            return dataManager.EasyWireSDFArray[minIndex];
        }
    }

    private float CalculateSDFWithReducedVertexDataTorus()
    {
        float minSDF = Mathf.Infinity;
        int minIndex = 999999;
        minPos = Vector3.one * 999999;
        foreach (Transform vertexTransform in vertexPositions.reducedNumVertexTransforms)
        {
            Vector3 convertedPos = ConvertPosition(vertexTransform.position);
            int curIdx = System.Array.IndexOf(dataManager.torusPositionArray, convertedPos);
            float sdf = dataManager.torusSDFArray[curIdx];
            //Debug.Log(sdf);
            if (sdf < minSDF)
            {
                minSDF = sdf;
                minIndex = curIdx;
                minPos = vertexTransform.position;
                lastConvertedVertexPos = convertedPos;
            }
        }

        if (minPos.x < -0.25f || minPos.x > 0.25f || minPos.y > 0.4f || minPos.y < -0.4f || minPos.z < -0.8f || minPos.z > 0.8f)
        {
            return 99;
        }
        else
        {
            return dataManager.torusSDFArray[minIndex];
        }
    }

    private Vector3 ConvertPosition(Vector3 pos)
    {
        if(objectType == ObjectType.TorusSceneObject)
        {
            return dataManager.ConvertPositionForTorus(pos);
        }
        else if(objectType == ObjectType.ComplexWireSceneObject)
        {
            return dataManager.ConvertPositionForWire(pos);
        }
        else
        {
            return dataManager.ConvertPositionForWire(pos);
        }
    }

    
    private float normalizedSDF()
    {
        if(currentSDF == 99)
        {
            if(objectType == ObjectType.ComplexWireSceneObject || objectType == ObjectType.EasyWireSceneObject)
            {
                float distane = Vector3.Distance(lastConvertedVertexPos, minPos);
                float cSpeed = Mathf.Lerp(0.5f, maxSpeed, distane);
                setSpeed(cSpeed);
                return currentSDF;
            }
            else if(objectType == ObjectType.TorusSceneObject)
            {
                setSpeed(1);
                return currentSDF;
            }
        }

        float divideMax = 4f;
        if(objectType == ObjectType.ComplexWireSceneObject || objectType == ObjectType.EasyWireSceneObject)
        {
            divideMax = 4f;
        }
        else if (objectType == ObjectType.TorusSceneObject)
        {
            divideMax = 3f;
        }

        float result = Mathf.Abs((currentSDF) / (maxSDF/ divideMax));
        //xrOffsetGrabInteractable.desiredVelocity = result;
        float speed = Mathf.Lerp(minSpeed, maxSpeed, result);
        setSpeed(speed);
        return result;
    }

    private void setSpeed(float speed)
    {
        xrOffsetGrabInteractable.desiredVelocity = speed;
        speedSlider.value = speed;
        speedText.text = $"SPEED: {speed}";
    }



}
