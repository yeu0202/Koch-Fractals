using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snow : MonoBehaviour
{
    public static float maxShift = 20f;
    public static float minShift = -20f;
    public float fallSpeed;
    public GameObject playerObject;

    public Vector3 _rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _rotateSpeed.x = Random.Range(0f, 20f);
        _rotateSpeed.y = Random.Range(0f, 20f);
        _rotateSpeed.z = Random.Range(0f, 20f);
        fallSpeed = Random.Range(2f, 8f);

        float _xShift = Random.Range(-2.5f, 2.5f);
        float _zShift = Random.Range(-2.5f, 2.5f);


        Vector3 shiftPosition = this.transform.position;
        shiftPosition.x += _xShift;
        shiftPosition.z += _zShift;
        this.transform.position = shiftPosition;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 fallPosition = this.transform.position;
        fallPosition.y -= fallSpeed * Time.deltaTime;
        this.transform.position = fallPosition;


        this.transform.Rotate(_rotateSpeed.x * Time.deltaTime,
            _rotateSpeed.y * Time.deltaTime,
            _rotateSpeed.z * Time.deltaTime);


        if(this.transform.position.y < playerObject.transform.position.y + minShift){
            Vector3 _position = this.transform.position;
            _position.y = playerObject.transform.position.y + maxShift;
            this.transform.position = _position;
        }
        else if(this.transform.position.y > playerObject.transform.position.y + maxShift){
            Vector3 _position = this.transform.position;
            _position.y = playerObject.transform.position.y + minShift;
            this.transform.position = _position;
        }

        if(this.transform.position.x < playerObject.transform.position.x + minShift){
            Vector3 _position = this.transform.position;
            _position.x += maxShift * 2;
            this.transform.position = _position;
        }
        else if(this.transform.position.x > playerObject.transform.position.x + maxShift){
            Vector3 _position = this.transform.position;
            _position.x += minShift * 2;
            this.transform.position = _position;
        }

        if(this.transform.position.z < playerObject.transform.position.z + minShift){
            Vector3 _position = this.transform.position;
            _position.z += maxShift * 2;
            this.transform.position = _position;
        }
        else if(this.transform.position.z > playerObject.transform.position.z + maxShift){
            Vector3 _position = this.transform.position;
            _position.z += minShift * 2;
            this.transform.position = _position;
        }


    }
}
