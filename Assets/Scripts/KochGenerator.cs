﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KochGenerator : MonoBehaviour
{
    /*
        Most code is copied and adjusted from code by Peer Play at https://youtu.be/fkRnUoXacHM
    */

    protected enum _axis{
        XAxis,
        YAxis,
        ZAxis
    };
    [SerializeField]
    protected _axis axis = new _axis();

    protected enum _initiator{
        Triangle,
        Square,
        Pentagon,
        Hexagon,
        Heptagon,
        Octagon,
        Nonagon,
        Decagon
    };

    public struct LineSegment{ // keeps track of the lines
        public Vector3 StartPosition {get; set;}
        public Vector3 EndPosition {get; set;}
        public Vector3 Direction {get; set;}
        public float Length {get; set;}
    }

    [SerializeField]
    protected _initiator initiator = new _initiator();
    [SerializeField]
    protected AnimationCurve _generator;

    [System.Serializable]
    public struct StartGen{
        public bool outwards;
        public float scale;
    }

    public StartGen[] _startGen;

    protected Keyframe[] _keys;
    [SerializeField]
    protected bool _useBezierCurves;
    [SerializeField]
    [Range(8,24)]
    protected int _bezierVertexCount;

    protected int _generationCount;

    protected int _initiatorPointAmount;
    private Vector3[] _initiatorPoint;
    private Vector3 _rotateVector;
    private Vector3 _rotateAxis;
    private float _initialRotation;
    [SerializeField]
    protected float _initiatorSize;


    protected Vector3[] _position;
    protected Vector3[] _targetPosition;
    protected Vector3[] _bezierPosition;
    private List<LineSegment> _lineSegment;

    protected Vector3[] BezierCurve(Vector3[] points, int vertexCount){
        var pointList = new List<Vector3>();
        for(int i=0; i<points.Length; i+=2){// skip every other point
            if(i+2 <= points.Length - 1){
                for(float ratio = 0f; ratio<= 1f; ratio += 1.0f / vertexCount){
                    var tangentLineVertex1 = Vector3.Lerp(points[i], points[i+1], ratio);
                    var tangentLineVertex2 = Vector3.Lerp(points[i+1], points[i+2], ratio);
                    var bezierpoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
                    pointList.Add(bezierpoint);
                }
            }
        }
        return pointList.ToArray();
    }

    private void Awake(){
        GetInitiatorPoints();
        // ssign lists and arrays
        _position = new Vector3[_initiatorPointAmount + 1]; // +1 is for the last line segment to return to the start point
        _targetPosition = new Vector3[_initiatorPointAmount + 1];
        _lineSegment = new List<LineSegment>();
        _keys = _generator.keys;

        _rotateVector = Quaternion.AngleAxis(_initialRotation, _rotateAxis) * _rotateVector;
        for (int i=0; i<_initiatorPointAmount; i++){
            _position[i] = _rotateVector * _initiatorSize;
            _rotateVector = Quaternion.AngleAxis(360 / _initiatorPointAmount, _rotateAxis) * _rotateVector; // rotates the sides
        }
        _position[_initiatorPointAmount] = _position[0];

        _targetPosition = _position;

        for(int i=0; i<_startGen.Length; i++){
            KochGenerate(_targetPosition, _startGen[i].outwards, _startGen[i].scale);
        }
    }

    protected void KochGenerate(Vector3[] positions, bool outwards, float generatorMultiplier){ // generatorMultiplier is the multiplier applied to the Koch extrusion
        // creating line segments
        _lineSegment.Clear(); // clears the lists of line segments to make sure it is empty
        for(int i=0; i<positions.Length - 1; i++){
            LineSegment line = new LineSegment();
            line.StartPosition = positions[i];
            if(i == positions.Length - 1){
                line.EndPosition = positions[0];
            }
            else{
                line.EndPosition = positions[i+1];
            }
            line.Direction = (line.EndPosition - line.StartPosition).normalized;
            line.Length = Vector3.Distance(line.EndPosition, line.StartPosition);
            _lineSegment.Add(line);
        }

        // add the line segment points to a point array
        List<Vector3> newPos = new List<Vector3>();
        List<Vector3> targetPos = new List<Vector3>();
        for(int i=0; i<_lineSegment.Count; i++){
            newPos.Add(_lineSegment[i].StartPosition);
            targetPos.Add(_lineSegment[i].StartPosition);

            for(int j=1; j<_keys.Length - 1; j++){
                float moveAmount = _lineSegment[i].Length * _keys[j].time;
                float heightAmount = (_lineSegment[i].Length * _keys[j].value) * generatorMultiplier;
                Vector3 movePos = _lineSegment[i].StartPosition + (_lineSegment[i].Direction * moveAmount);
                Vector3 Dir;
                if(outwards){
                    Dir = Quaternion.AngleAxis(-90, _rotateAxis) * _lineSegment[i].Direction;
                }
                else{
                    Dir = Quaternion.AngleAxis(90, _rotateAxis) * _lineSegment[i].Direction;
                }
                newPos.Add(movePos);
                targetPos.Add(movePos + (Dir * heightAmount));
            }
        }
        newPos.Add(_lineSegment[0].StartPosition);
        targetPos.Add(_lineSegment[0].StartPosition);
        _position = new Vector3[newPos.Count];
        _targetPosition = new Vector3[targetPos.Count];
        _position = newPos.ToArray();
        _targetPosition = targetPos.ToArray();
        _bezierPosition = BezierCurve(_targetPosition, _bezierVertexCount);
        _generationCount++;
    }

    public float _lengthOfSides;

    private void OnDrawGizmos(){
        GetInitiatorPoints();
        _initiatorPoint = new Vector3[_initiatorPointAmount];

        _rotateVector = Quaternion.AngleAxis(_initialRotation, _rotateAxis) * _rotateVector;
        for (int i=0; i<_initiatorPointAmount; i++){
            _initiatorPoint[i] = _rotateVector * _initiatorSize;
            _rotateVector = Quaternion.AngleAxis(360 / _initiatorPointAmount, _rotateAxis) * _rotateVector; // rotates the sides
        }

        for (int i=0; i<_initiatorPointAmount; i++){
            Gizmos.color = Color.white;
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale); // allows transforms of parent to affect transforms of gizmos
            Gizmos.matrix = rotationMatrix;
            if(i < _initiatorPointAmount - 1){ // draws the lines
                Gizmos.DrawLine(_initiatorPoint[i], _initiatorPoint[i+1]);
            }
            else{
                Gizmos.DrawLine(_initiatorPoint[i], _initiatorPoint[0]);
            }
        }
        _lengthOfSides = Vector3.Distance(_initiatorPoint[0], _initiatorPoint[1]) * 0.5f;
    }

    private void GetInitiatorPoints(){
        switch(initiator){ // choose number of sides
            case _initiator.Triangle:
                _initiatorPointAmount = 3;
                break;
            case _initiator.Square:
                _initiatorPointAmount = 4;
                break;
            case _initiator.Pentagon:
                _initiatorPointAmount = 5;
                break;
            case _initiator.Hexagon:
                _initiatorPointAmount = 6;
                break;
            case _initiator.Heptagon:
                _initiatorPointAmount = 7;
                break;
            case _initiator.Octagon:
                _initiatorPointAmount = 8;
                break;
            case _initiator.Nonagon:
                _initiatorPointAmount = 9;
                break;
            case _initiator.Decagon:
                _initiatorPointAmount = 10;
                break;
            default:
                _initiatorPointAmount = 3;
                break;
        };
        _initialRotation = 360f/_initiatorPointAmount/2f; // finds the rotation amount for each shape needed

        switch (axis){ // choose axis of shape
            case _axis.XAxis:
                _rotateVector = new Vector3(1, 0, 0);
                _rotateAxis = new Vector3(0, 0, 1);
                break;
            case _axis.YAxis:
                _rotateVector = new Vector3(0, 1, 0);
                _rotateAxis = new Vector3(1, 0, 0);
                break;
            case _axis.ZAxis:
                _rotateVector = new Vector3(0, 0, 1);
                _rotateAxis = new Vector3(0, 1, 0);
                break;
            default:
                _rotateVector = new Vector3(0, 1, 0);
                _rotateAxis = new Vector3(1, 0, 0);
                break;
        }
    }

}
