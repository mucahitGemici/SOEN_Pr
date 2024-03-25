using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Wire : MonoBehaviour
{
    public TMP_Text hitText;
    private int hitNum;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 6)
        {
            hitNum++;
            hitText.text = hitNum.ToString();
        }
    }
}
