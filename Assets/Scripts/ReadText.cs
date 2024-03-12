using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadText : MonoBehaviour
{
    public TextAsset positionFile;
    public TextAsset sdfFile;
    private Vector3[] posArray = new Vector3[68921];
    private float[] sdfArray = new float[68921];
    private void Start()
    {
        ReadPositionText();
        ReadSDFText();
        PrintCurrentSDF();
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
            pos /= 100;
            //Debug.Log(pos);
            posArray[i/3] = pos;
        }
        //Debug.Log(posArray[1]);


        Debug.Log(posArray[posArray.Length - 1]);
    }

    private void ReadSDFText()
    {
        string[] dataLines = sdfFile.text.Split(",");

        for(int i = 0;i < dataLines.Length - 1; i++)
        {
            float val = float.Parse(dataLines[i]);
            sdfArray[i] = val;
        }

        Debug.Log(sdfArray.Length);
        Debug.Log(sdfArray[sdfArray.Length-1]);
    }

    private void PrintCurrentSDF()
    {

    }
}
