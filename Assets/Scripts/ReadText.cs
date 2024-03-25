using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ReadText : MonoBehaviour
{
    public enum ObjectType
    {
        Box,
        Ring
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

    private bool isSDFenabled = true;
    private void Start()
    {

        speedSlider.minValue = 0;
        speedSlider.maxValue = maxSpeed;

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

    private float CalculateSDFWithReducedVertexData()
    {
        float minSDF = Mathf.Infinity;
        int minIndex = 999999;
        foreach(Transform vertexTransform in vertexPositions.reducedNumVertexTransforms)
        {
            int curIdx = System.Array.IndexOf(dataManager.wirePositionArray, ConvertPosition(vertexTransform.position));
            float sdf = dataManager.wireSDFArray[curIdx];
            //Debug.Log(sdf);
            if(sdf < minSDF)
            {
                minSDF = sdf;
                minIndex = curIdx;
            }
        }
        return dataManager.wireSDFArray[minIndex];
    }

    
    private Vector3 ConvertPosition(Vector3 pos)
    {
        if(objectType == ObjectType.Box)
        {
            return dataManager.ConvertPositionForTorus(pos);
        }
        else
        {
            return dataManager.ConvertPositionForWire(pos);
        }
    }
    
    private float normalizedSDF()
    {
        float result = Mathf.Abs((currentSDF) / (maxSDF / 4));
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
