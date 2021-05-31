using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideObjects : MonoBehaviour
{
    public GameObject objectToHide;
    private bool hidden = false;
    public string hideKey = "0";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(hideKey)){
            if(hidden){
                hidden = false;
                objectToHide.SetActive(true);
            }
            else{
                hidden = true;
                objectToHide.SetActive(false);
            }
        }
    }
}
