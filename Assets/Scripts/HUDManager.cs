using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class HUDManager : MonoBehaviour
{
    public Text volumeText;
    public Text xSpeedText;
    public Text ySpeedText;

    public CinemachineFreeLook _camera;
    private string xSpeed = "450";
    private string ySpeed = "4";

    // variable to change
    private string volumeString = "70";

    // Start is called before the first frame update
    void Start()
    {
        volumeText.text = volumeString + "%";
        xSpeedText.text = xSpeed;
        ySpeedText.text = ySpeed;
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void changeNumber(float number){
        number = number * 100;
        number = Mathf.Round(number);
        volumeString = number.ToString();
        volumeText.text = volumeString + "%";
    }

    public void changeXSpeed(float number){
        number = Mathf.Round(number);
        _camera.m_XAxis.m_MaxSpeed = number;

        xSpeed = number.ToString();
        xSpeedText.text = xSpeed;
    }

    public void changeYSpeed(float number){
        number = Mathf.Round(number);
        _camera.m_YAxis.m_MaxSpeed = number;

        ySpeed = number.ToString();
        ySpeedText.text = ySpeed;
    }
}
