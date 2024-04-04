using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Torus : MonoBehaviour
{
    public TMP_Text hitText;
    private int hitNum;
    public int GetHitNumber
    {
        get { return hitNum; }
    }
    public AudioSource beepSource;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            hitNum++;
            //Debug.Log(collision.collider.gameObject.name);
            //Debug.Log(collision.gameObject.name);
            collision.gameObject.GetComponent<XROffsetGrabInteractable>().setHitColor(collision.collider.GetComponent<MeshRenderer>());
            hitText.text = hitNum.ToString();
            beepSource.Play();
        }
    }
}
