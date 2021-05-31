using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TimeController : MonoBehaviour
{

    public string hideKey = "p";
    private bool timeFlowing = true;

    public CinemachineBrain _camera;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(hideKey)){
            if(timeFlowing){
                Time.timeScale = 0;
                timeFlowing = false;
                _camera.m_UpdateMethod = CinemachineBrain.UpdateMethod.LateUpdate;
            }
            else{
                Time.timeScale = 1;
                timeFlowing = true;
                _camera.m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;
            }
        }
    }
}
