using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField] private List<TaskPath> pathList;
    public PlacingObject placing;
    public AudioSource source;
    public AudioSource successSource;
    int pathIndex = 0;

    private void Awake()
    {
        foreach(var path in pathList)
        {
            path.onPathArrived += OnPathArrived;
        }

        placing.OnPlacingEnded += OnPlacingEnded;

        Debug.Log(pathList.Count);
    }

    private void OnPlacingEnded(float positionDifference, float angle)
    {
        Debug.Log($"Placing ended. PositionDifference: {positionDifference} and AngleDifference: {angle}");
        successSource.Play();
    }

    private void OnPathArrived(Transform reference)
    {
        reference.gameObject.SetActive(false);
        source.Play();
        if(pathIndex + 1 == pathList.Count)
        {
            // finish the path
            placing.gameObject.SetActive(true);
            return;
        }
        pathIndex++;
        pathList[pathIndex].gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        foreach (var path in pathList)
        {
            path.onPathArrived -= OnPathArrived;
        }
    }
}
