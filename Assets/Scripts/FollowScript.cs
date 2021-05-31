using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScript : MonoBehaviour
{
    public GameObject followTarget;
    public float yShift = -14.26f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = followTarget.transform.position;
        Vector3 positionEdit = this.transform.position;
        positionEdit.y = yShift;
        this.transform.position = positionEdit;
    }
}
