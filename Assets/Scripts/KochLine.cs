using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class KochLine : KochGenerator // inherit from KochGenerator instead of monobehavior to inherit all the protected variables
{
    LineRenderer _lineRenderer;
    // [Range(0,1)]
    // public float _lerpAmount;
    Vector3[] _lerpPosition;
    public float _generateMultiplier;
    private float[] _lerpAudio;


    [Header ("Audio")]
    // public AudioPeer _audioPeer;
    public int[] _audioBand;

    // Start is called before the first frame update
    void Start()
    {
        _lerpAudio = new float[_initiatorPointAmount];
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = true;
        _lineRenderer.useWorldSpace = false;
        _lineRenderer.loop = true;
        _lineRenderer.positionCount = _position.Length;
        _lineRenderer.SetPositions(_position);
        _lerpPosition = new Vector3[_position.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if(_generationCount != 0){
            int count = 0;
            for(int i=0; i<_initiatorPointAmount; i++){
                _lerpAudio[i] = AudioAnalysis.bandBuffer[_audioBand[i]];
                for(int j=0; j<(_position.Length - 1) / _initiatorPointAmount; j++){
                    _lerpPosition[count] = Vector3.Lerp(_position[count], _targetPosition[count], _lerpAudio[i]);
                    count++;
                }
            }
            _lerpPosition[count] = Vector3.Lerp(_position[count], _targetPosition[count], _lerpAudio[_initiatorPointAmount - 1]);

            // for(int i=0; i<_position.Length; i++){
            //     _lerpPosition[i] = Vector3.Lerp(_position[i], _targetPosition[i], AudioAnalysis.bandBuffer[_audioBand]);
            // }
            if(_useBezierCurves){
                _bezierPosition = BezierCurve(_lerpPosition, _bezierVertexCount);
                _lineRenderer.positionCount = _bezierPosition.Length;
                _lineRenderer.SetPositions(_bezierPosition);
            }
            else{
                _lineRenderer.positionCount = _lerpPosition.Length;
                _lineRenderer.SetPositions(_lerpPosition);
            }
        }

        // if(Input.GetKeyUp(KeyCode.O)){
        //     KochGenerate(_targetPosition, true, _generateMultiplier);
        //     _lerpPosition = new Vector3[_position.Length];
        //     _lineRenderer.positionCount = _position.Length;
        //     _lineRenderer.SetPositions(_position);
        //     _lerpAmount = 0;
        // }
        // if(Input.GetKeyUp(KeyCode.I)){
        //     KochGenerate(_targetPosition, false, _generateMultiplier);
        //     _lerpPosition = new Vector3[_position.Length];
        //     _lineRenderer.positionCount = _position.Length;
        //     _lineRenderer.SetPositions(_position);
        //     _lerpAmount = 0;
        // }
    }
}
