using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Wire : MonoBehaviour
{
    public TMP_Text hitText;
    private int hitNum;
    public int GetHitNumber
    {
        get { return hitNum; }
    }

    public AudioSource beepSource;
    public FinishLine placing;
    public AudioSource successSource;

    private float hitTimer;

    private void Awake()
    {
        placing.OnPlacingEnded += OnPlacingEnded;
    }

    private void Update()
    {
        hitTimer -= Time.deltaTime;

    }

    private void OnPlacingEnded(float positionDifference, float angle)
    {
        successSource.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6 && hitTimer <= 0)
        {
            hitNum++;
            hitTimer = 0.2f;
            //Debug.Log(collision.collider.gameObject.name);
            //Debug.Log(collision.gameObject.name);
            MeshRenderer mr = collision.collider.transform.parent.GetComponentsInChildren<MeshRenderer>()[1];
            collision.gameObject.GetComponent<XROffsetGrabInteractable>().setHitColor(mr);
            hitText.text = hitNum.ToString();
            beepSource.Play();
        }
    }

}
