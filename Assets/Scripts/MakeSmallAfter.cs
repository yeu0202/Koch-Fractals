using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSmallAfter : MonoBehaviour
{
    // public static float scaleSpeed = 0.0009f;
    // private Vector3 scaleChange = new Vector3(scaleSpeed, scaleSpeed, scaleSpeed);
    public float scale = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExecuteAfterTime(0.05f));
    }

    // Update is called once per frame
    void Update()
    {
        // transform.localScale += scaleChange;

        // if (transform.localScale.y < 0.09f || transform.localScale.y > 0.2f)
        // {
        //     scaleChange = -scaleChange;
        // }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
    
        // Code to execute after the delay
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
