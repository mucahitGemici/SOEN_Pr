using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ReadText : MonoBehaviour
{
    public TextAsset positionFile;
    public TextAsset sdfFile;
    public Vector3[] posArray = new Vector3[68921];
    private float[] sdfArray = new float[68921];
    public Transform objectReference;
    public TMPro.TMP_Text screenText;

    private bool toLeft = true;

    private float currentSDF;
    private float maxSDF;
    private float minSDF;

    private float movementSpeed;
    public Slider speedSlider;
    private float maxSpeed = 3f;

    public Toggle sdfToggle;
    private bool isSdfEnabled;
    private void Start()
    {
        ReadPositionText();
        ReadSDFText();

        speedSlider.minValue = 0;
        speedSlider.maxValue = maxSpeed;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PrintCurrentSDF();
        }

        Movement();

        if (isSdfEnabled)
        {
            PrintCurrentSDF();
            movementSpeed = Mathf.Lerp(0, maxSpeed, normalizedSDF());
            screenText.text = $"SDF = {currentSDF}";
        }
        else
        {
            movementSpeed = maxSpeed;
        }
        
        //Debug.Log($"movementSpeed: {movementSpeed}");

        speedSlider.value = movementSpeed;

    }

    private void ReadPositionText()
    {
        string[] dataLines = positionFile.text.Split(',');
        
        for(int i = 0; i <= dataLines.Length - 3; i += 3) 
        {
            string stringX = dataLines[i];
            string stringY = dataLines[i + 1];
            string stringZ = dataLines[i + 2];
            

            Vector3 pos = new Vector3(float.Parse(stringX), float.Parse(stringY), float.Parse(stringZ));
            RuntimePlatform platform = Application.platform;
            if(platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer)
            {
                pos /= 100;
            }
            //Debug.Log(pos);
            posArray[i/3] = pos;
        }
        //Debug.Log(posArray[1]);


        //Debug.Log(posArray[posArray.Length - 1]);
    }

    private void ReadSDFText()
    {
        string[] dataLines = sdfFile.text.Split(",");

        for(int i = 0;i < dataLines.Length - 1; i++)
        {
            float val = float.Parse(dataLines[i]);
            sdfArray[i] = val;
        }

        maxSDF = sdfArray.Max();
        Debug.Log($"maxSDF: {maxSDF}");
        minSDF = sdfArray.Min();
        Debug.Log($"minSDF: {minSDF}");
        //Debug.Log(sdfArray.Length);
        //Debug.Log(sdfArray[sdfArray.Length-1]);
    }

    private void PrintCurrentSDF()
    {
        Vector3 cPos = GetPosition();
        //Vector3 cPos = new Vector3(-2, -1.8f, -0.9f);
        int idx = System.Array.IndexOf(posArray, cPos);
        currentSDF = sdfArray[idx];
        //Debug.Log(sdfArray[idx]);

        normalizedSDF();
    }

    private Vector3 GetPosition()
    {
        Vector3 pos = transform.position;
        float x = Mathf.Round(pos.x * 10f) / 10f;
        float y = Mathf.Round(pos.y * 10f) / 10f;
        float z = Mathf.Round(pos.z * 10f) / 10f;
        pos = new Vector3(x, y, z);
        //Debug.Log(pos);
        return pos;
    }

    private void Movement()
    {
        Vector3 newPos = transform.position;
        if (toLeft)
        {
            newPos = transform.position + new Vector3(0,0,1) * Time.deltaTime * movementSpeed;
            if(newPos.z >= 1.99f)
            {
                toLeft = false;
                return;
            }
            transform.position = newPos;
        }
        else
        {
            newPos = transform.position - new Vector3(0, 0, 1) * Time.deltaTime * movementSpeed;
            if (newPos.z <= -1.99f)
            {
                toLeft = true;
                return;
            }
            transform.position = newPos;
        }
    }

    private float normalizedSDF()
    {
        float result = (currentSDF) / (maxSDF-minSDF-0.4f);
        //Debug.Log($"normalizedSDF: {result}");
        return result;
    }

    public void UP()
    {
        if(transform.position.y + 0.1f <= 1.9f)
        {
            transform.position += Vector3.up * 0.1f;
        }
        
    }

    public void DOWN()
    {
        if (transform.position.y - 0.1f >= -1.9f)
        {
            transform.position += Vector3.down * 0.1f;
        }
    }

    public void OnToggleStateChange()
    {
        if (sdfToggle.isOn) isSdfEnabled = true;
        else
        {
            isSdfEnabled = false;
        }
    }
}
