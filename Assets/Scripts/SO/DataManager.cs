using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DataManager", menuName = "Scriptable Objects/Data Manager")]
public class DataManager : ScriptableObject
{
    [SerializeField] private TextAsset wirePositionText;
    [SerializeField] private TextAsset wireSDFText;

    [SerializeField] private TextAsset torusPositionText;
    [SerializeField] private TextAsset torusSDFText;

    //[HideInInspector] public Vector3[] wirePositionArray = new Vector3[450241];
    [HideInInspector] public Vector3[] wirePositionArray = new Vector3[341901];
    [HideInInspector] public Vector3[] torusPositionArray = new Vector3[226981];


    //[HideInInspector] public float[] wireSDFArray = new float[450241];
    [HideInInspector] public float[] wireSDFArray = new float[341901];
    [HideInInspector] public float[] torusSDFArray = new float[226981];

    [HideInInspector] public float minSDFTorus;
    [HideInInspector] public float maxSDFTorus;
    [HideInInspector] public float minSDFWire;
    [HideInInspector] public float maxSDFWire;

    public bool IsDataReaded
    {
        get
        {
            if (wirePositionArray[1].magnitude != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void ReadData()
    {
        ReadWirePositionData();
        ReadWireSDFData();

        ReadTorusPositionData();
        ReadTorusSDFData();
    }

    private void ReadWirePositionData()
    {
        string[] dataLines = wirePositionText.text.Split(',');

        for (int i = 0; i <= dataLines.Length - 3; i += 3)
        {
            string stringX = dataLines[i];
            string stringY = dataLines[i + 1];
            string stringZ = dataLines[i + 2];


            Vector3 pos = new Vector3(float.Parse(stringX), float.Parse(stringY), float.Parse(stringZ));
            RuntimePlatform platform = Application.platform;
            if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer)
            {
                pos /= 100;
            }
            wirePositionArray[i / 3] = pos;
        }
    }

    private void ReadTorusPositionData()
    {
        string[] dataLines = torusPositionText.text.Split(',');

        for (int i = 0; i <= dataLines.Length - 3; i += 3)
        {
            string stringX = dataLines[i];
            string stringY = dataLines[i + 1];
            string stringZ = dataLines[i + 2];


            Vector3 pos = new Vector3(float.Parse(stringX), float.Parse(stringY), float.Parse(stringZ));
            RuntimePlatform platform = Application.platform;
            if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer)
            {
                pos /= 100;
            }
            torusPositionArray[i / 3] = pos;
        }
    }

    private void ReadWireSDFData()
    {
        string[] dataLines = wireSDFText.text.Split(",");

        for (int i = 0; i < dataLines.Length - 1; i++)
        {
            float val = float.Parse(dataLines[i]);
            wireSDFArray[i] = val;
        }

        minSDFWire = Mathf.Min(wireSDFArray);
        maxSDFWire = Mathf.Max(wireSDFArray);
        Debug.Log($"min sdf for wire: {minSDFWire}, max sdf for wire: {maxSDFWire}");
    }

    private void ReadTorusSDFData()
    {
        string[] dataLines = torusSDFText.text.Split(",");

        for (int i = 0; i < dataLines.Length - 1; i++)
        {
            float val = float.Parse(dataLines[i]);
            torusSDFArray[i] = val;
        }

        minSDFTorus = Mathf.Min(torusSDFArray);
        maxSDFTorus = Mathf.Max(torusSDFArray);
        Debug.Log($"min sdf for torus: {minSDFTorus}, max sdf for torus: {maxSDFTorus}");
    }

    public Vector3 ConvertPositionForWire(Vector3 pos)
    {
        float x = Mathf.Round(pos.x * 100f) / 100f;
        float y = Mathf.Round(pos.y * 100f) / 100f;
        float z = Mathf.Round(pos.z * 100f) / 100f;
        x = Mathf.Clamp(x, -0.1f, 0.1f);
        y = Mathf.Clamp(y, -0.4f, 0.4f);
        z = Mathf.Clamp(z, -1f, 1f);
        return new Vector3(x, y, z);
    }


    public Vector3 ConvertPositionForTorus(Vector3 pos)
    {
        float x = Mathf.Round(pos.x * 10f) / 10f;
        float y = Mathf.Round(pos.y * 10f) / 10f;
        float z = Mathf.Round(pos.z * 10f) / 10f;
        x = Mathf.Clamp(x, -3, 3);
        y = Mathf.Clamp(y, -3, 3);
        z = Mathf.Clamp(z, -3, 3);
        return new Vector3(x, y, z);
    }
}
