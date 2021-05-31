using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTriangle : MonoBehaviour
{
    public int rotateIteration = 0;
    // public float shiftAmount = 0.8660254f;
    // private Vector3 rotateTarget = new Vector3(0f, 0f, 0f);
    private Vector3 rotateTarget;
    // private float yShift = 4f;

    private Vector3 rotateAxisX = new Vector3(1f, 0f, 0f); // for rotating the triangle 
    private Vector3 rotateAxisY = new Vector3(0f, 1f, 0f); // for rotating the triangle to the correct position
    private Vector3 rotateAxisZ = new Vector3(0f, 0f, 1f); 
    // private float rotateAmount = 180.0f - 138.189685f; // 138.189685 is the Dihedral angle of an icosahedron

    // public bool oppositeRotate = false; // true for other half of icosahedron

    private Vector3[] positionArray = new []{
        new Vector3(0f, 4f, 0f),  // 0
        new Vector3(-2.309401f, 2.981424f, -1.333333f),
        new Vector3(-3.736689f, 1.333333f, 0.509288f),  // 2
        new Vector3(-2.309401f, 1.333333f, 2.981424f),
        new Vector3(-6.882551e-08f, 2.981424f, 2.666667f),  // 4
        new Vector3(2.309402f, 1.333333f, 2.981424f),
        new Vector3(3.73669f, 1.333333f, 0.5092877f),  // 6
        new Vector3(2.309401f, 2.981424f, -1.333333f),
        new Vector3(1.427288f, 1.333333f, -3.490712f),  // 8
        new Vector3(-1.427289f, 1.333333f, -3.490712f)
    };

    private Vector3[] rotationArray = new []{
        new Vector3(0f, 0f, 0f),  // 0
        new Vector3(-41.81f, 60f, 0f),
        new Vector3(-131.81f, -15.52197f, 120f),  // 2
        new Vector3(-7.315001f, -130.362f, -70.36201f),
        new Vector3(19.471f, -66.716f, -37.761f),  // 4
        new Vector3(-48.19f, -75.522f, -60f),
        new Vector3(-7.315001f, -10.362f, -70.36201f),  // 6
        new Vector3(-41.81f, -60f, 0f),
        new Vector3(-48.19f, 44.477f, -60f),  // 8
        new Vector3(-7.315001f, 109.638f, -70.36201f)
    };

    // Start is called before the first frame update
    void Start()
    {
        rotateTarget = transform.parent.transform.position;
        // // shift triangle by radius
        // Vector3 shiftPosition = transform.parent.transform.position;
        // shiftPosition.y += yShift;
        // transform.position = shiftPosition;

        // // rotate triangle by appropriate amount

        // int loopLimiter = rotateIteration;

        // loopLimiter = rotateIteration % 5;
        // for(int i=0; i<loopLimiter; i++){
        //     // transform.Rotate(0f, 60f, 0f, Space.Self);
        //     transform.RotateAround (rotateTarget, rotateAxisX, -rotateAmount);
        //     transform.RotateAround(rotateTarget, rotateAxisY, 360f/6f);
        // }
        
        // if(rotateIteration > 5 && rotateIteration <= 10){
        //     transform.RotateAround(rotateTarget, rotateAxisY, 120f);
        // }
        
        // if(rotateIteration > 10 && rotateIteration <= 15){
        //     transform.RotateAround(rotateTarget, rotateAxisY, 240f);
        // }

        // if(oppositeRotate){
        //     transform.RotateAround(rotateTarget, rotateAxisZ, 180f);
        //     transform.RotateAround(rotateTarget, rotateAxisY, 180f);
        // }

        int index = rotateIteration % 10;

        transform.position = positionArray[index];
        transform.Rotate(rotationArray[index].x, rotationArray[index].y, rotationArray[index].z, Space.Self);

        if(rotateIteration > 9){
            transform.RotateAround(rotateTarget, rotateAxisZ, 180f);
            transform.RotateAround(rotateTarget, rotateAxisY, 180f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
    Koch Triangle transform positions and rotations
    (0f, 4f, 0f)
    (0f, 0f, 0f)

    (-2.309401f, 2.981424f, -1.333333f)
    (-41.81f, 60f, 0f)

    (-3.736689f, 1.333333f, 0.509288f)
    (-131.81f, -15.52197f, 120f)

    (-2.309401f, 1.333333f, 2.981424f)
    (-7.315001f, -130.362f, -70.36201f)

    (-6.882551e-08f, 2.981424f, 2.666667f)
    (19.471f, -66.716f, -37.761f)

    (2.309402f, 1.333333f, 2.981424f)
    (-48.19f, -75.522f, -60f)

    (3.73669f, 1.333333f, 0.5092877f)
    (-7.315001f, -10.362f, -70.36201f)

    (2.309401f, 2.981424f, -1.333333f)
    (-41.81f, -60f, 0f)

    (1.427288f, 1.333333f, -3.490712f)
    (-48.19f, 44.477f, -60f)

    (-1.427289f, 1.333333f, -3.490712f)
    (-7.315001f, 109.638f, -70.36201f)


        if(oppositeRotate){
            transform.RotateAround(rotateTarget, rotateAxisZ, 180f);
            transform.RotateAround(rotateTarget, rotateAxisY, 180f);
        }
    **/
}
