using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DataManager", menuName = "Scriptable Objects/Data Manager")]
public class DataManager : ScriptableObject
{
    [SerializeField] private TextAsset wirePositionText;
    [SerializeField] private TextAsset wireSDFText;
    [SerializeField] private TextAsset wireClosestPositionText;

    [SerializeField] private TextAsset easyWirePositionText;
    [SerializeField] private TextAsset easyWireSDFText;
    [SerializeField] private TextAsset easyWireClosestPositionText;

    [SerializeField] private TextAsset torusPositionText;
    [SerializeField] private TextAsset torusSDFText;
    [SerializeField] private TextAsset torusClosestPositionText;

    //[HideInInspector] public Vector3[] wirePositionArray = new Vector3[450241];
    [HideInInspector] public Vector3[] wirePositionArray = new Vector3[341901];
    [HideInInspector] public Vector3[] torusPositionArray = new Vector3[665091];
    private Vector3[] easyWirePositionArray = new Vector3[341901];

    public Vector3[] EasyWirePositionArray
    {
        get { return wirePositionArray; }
    }


    //[HideInInspector] public float[] wireSDFArray = new float[450241];
    [HideInInspector] public float[] wireSDFArray = new float[341901];
    [HideInInspector] public float[] torusSDFArray = new float[665091];
    private float[] easyWireSDFArray = new float[341901];

    public float[] EasyWireSDFArray
    {
        get { return easyWireSDFArray; }
    }

    private Vector3[] torusClosestPositionArray = new Vector3[665091];
    public Vector3[] TorusClosestPositionArray
    {
        get { return torusClosestPositionArray; }
    }

    private Vector3[] easyWireClosestPositionArray = new Vector3[341901];
    public Vector3[] EasyWireClosestPositionArray
    {
        get { return easyWireClosestPositionArray; }
    }

    private Vector3[] wireClosestPositionArray = new Vector3[341901];
    public Vector3[] WireClosestPositionArray
    {
        get { return wireClosestPositionArray; }
    }

    [HideInInspector] public float minSDFTorus;
    [HideInInspector] public float maxSDFTorus;
    [HideInInspector] public float minSDFWire;
    [HideInInspector] public float maxSDFWire;
    private float maxSDFeasyWire;
    private float minSDFeasyWire;

    public float MaxSDFeasyWire
    {
        get { return maxSDFeasyWire; }
    }

    public float MinSDFeasyWire
    {
        get { return minSDFeasyWire; }
    }

    public bool IsDataReaded
    {
        get
        {
            if (wirePositionArray[1].magnitude != 0 && torusPositionArray[1].magnitude != 0 && torusClosestPositionArray[1].magnitude != 0)
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
        wirePositionArray = new Vector3[341901];
        torusPositionArray = new Vector3[665091];
        easyWirePositionArray = new Vector3[341901];

        wireSDFArray = new float[341901];
        torusSDFArray = new float[665091];
        easyWireSDFArray = new float[341901];

        torusClosestPositionArray = new Vector3[665091];

        minSDFTorus = 0.0f;
        maxSDFTorus = 0.0f;
        minSDFWire = 0.0f;
        maxSDFWire = 0.0f;
        maxSDFeasyWire = 0.0f;
        minSDFeasyWire = 0.0f;

        ReadWirePositionData();
        ReadWireSDFData();
        ReadEasyWirePositionData();

        ReadTorusPositionData();
        ReadTorusSDFData();
        ReadEasyWireSDFData();

        ReadTorusClosestPositionData();
        ReadWireClosestPositionData();
        ReadEasyWireClosestPositionData();
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

    private void ReadEasyWirePositionData()
    {
        string[] dataLines = easyWirePositionText.text.Split(',');

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
            easyWirePositionArray[i / 3] = pos;
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

    private void ReadTorusClosestPositionData()
    {
        string[] dataLines = torusClosestPositionText.text.Split(',');

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
            torusClosestPositionArray[i / 3] = pos;
        }
    }

    private void ReadEasyWireClosestPositionData()
    {
        string[] dataLines = easyWireClosestPositionText.text.Split(',');

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
            easyWireClosestPositionArray[i / 3] = pos;
        }
    }


    private void ReadWireClosestPositionData()
    {
        string[] dataLines = wireClosestPositionText.text.Split(',');

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
            wireClosestPositionArray[i / 3] = pos;
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

    private void ReadEasyWireSDFData()
    {
        string[] dataLines = easyWireSDFText.text.Split(",");

        for (int i = 0; i < dataLines.Length - 1; i++)
        {
            float val = float.Parse(dataLines[i]);
            easyWireSDFArray[i] = val;
        }

        minSDFeasyWire = Mathf.Min(easyWireSDFArray);
        maxSDFeasyWire = Mathf.Max(easyWireSDFArray);
        Debug.Log($"min sdf for easy wire: {minSDFeasyWire}, max sdf for easy wire: {maxSDFeasyWire}");
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
        // using same position array for both complex and easy wires
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
        float x = Mathf.Round(pos.x * 100f) / 100f;
        float y = Mathf.Round(pos.y * 100f) / 100f;
        float z = Mathf.Round(pos.z * 100f) / 100f;
        x = Mathf.Clamp(x, -0.25f, 0.25f);
        y = Mathf.Clamp(y, -0.4f, 0.4f);
        z = Mathf.Clamp(z, -0.8f, 0.8f);
        return new Vector3(x, y, z);
    }
}
