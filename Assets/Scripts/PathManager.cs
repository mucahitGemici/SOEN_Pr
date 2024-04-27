using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField] private List<TaskPath> pathList;
    public FinishLine placing;
    public AudioSource source;
    //public AudioSource successSource;
    int pathIndex = 0;

    [Header("Colors")]
    [SerializeField] private Color fullTransparentColor;
    [SerializeField] private Color halfTransparentColor;
    [SerializeField] private Color minimumTransparentColor;

    public bool PathIsDone
    {
        get { return pathIndex == pathList.Count - 1; }
    }
    private void Awake()
    {
        foreach(var path in pathList)
        {
            path.onPathArrived += OnPathArrived;
        }

        placing.OnPlacingEnded += OnPlacingEnded;

        Debug.Log(pathList.Count);

    }

    private void Start(){
        pathList[pathIndex].gameObject.SetActive(true);
        pathList[pathIndex].GetComponent<MeshRenderer>().material.color = fullTransparentColor;
        pathList[pathIndex+1].gameObject.SetActive(true);
        pathList[pathIndex+1].GetComponent<MeshRenderer>().material.color = halfTransparentColor;
        pathList[pathIndex+2].gameObject.SetActive(true);
        pathList[pathIndex+2].GetComponent<MeshRenderer>().material.color = minimumTransparentColor;
    }

    private void OnPlacingEnded(float positionDifference, float angle)
    {
        Debug.Log($"Placing ended. PositionDifference: {positionDifference} and AngleDifference: {angle}");
        //successSource.Play();
    }

    

    private void OnPathArrived(Transform reference)
    {
        reference.gameObject.SetActive(false);
        source.Play();
        if(pathIndex + 1 == pathList.Count)
        {
            // finish the path
            // placing.gameObject.SetActive(true);
            return;
        }
        pathIndex++;
        pathList[pathIndex].gameObject.SetActive(true);
        pathList[pathIndex].GetComponent<Collider>().enabled = true;
        pathList[pathIndex].GetComponent<MeshRenderer>().material.color = fullTransparentColor;
        if(pathIndex + 1 > pathList.Count - 1){
            return;
        }
        pathList[pathIndex+1].gameObject.SetActive(true);
        pathList[pathIndex+1].GetComponent<Collider>().enabled = false;
        pathList[pathIndex+1].GetComponent<MeshRenderer>().material.color = halfTransparentColor;
        if(pathIndex + 2 > pathList.Count - 1){
            return;
        }
        pathList[pathIndex+2].gameObject.SetActive(true);
        pathList[pathIndex + 2].GetComponent<Collider>().enabled = false;
        pathList[pathIndex+2].GetComponent<MeshRenderer>().material.color = minimumTransparentColor;
    }

    private void OnDestroy()
    {
        foreach (var path in pathList)
        {
            path.onPathArrived -= OnPathArrived;
        }
    }

}
