using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{

    public GameObject goFlySprite;

    public float fRoamingDistance;
    public float fMaxArcDistance;
    public float fMinArcDistance;

    public float fMinSegmentTime;
    public float fMaxSegmentTime;
    private float fCurSegmentTime;
    
    private float count, boundaryLeft, boundaryRight, boundaryUp, boundaryDown, xPoint, yPoint;
    private Vector2[] point = new Vector2[3];
    private Vector2 m1, m2;

    // Start is called before the first frame update
    void Start()
    {
        count = 0.0f;
        boundaryLeft =  - fRoamingDistance;
        boundaryRight =  fRoamingDistance;
        boundaryUp = fRoamingDistance;
        boundaryDown = - fRoamingDistance;

        fCurSegmentTime = Random.Range(fMinSegmentTime, fMaxSegmentTime);

        //We need to initialize a starting destination so that our midpoint can shift appropriately
        point[2] = new Vector2(Random.Range(boundaryLeft, boundaryRight), Random.Range(boundaryUp, boundaryDown));

        DefinePoints();
    }

    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime;
        
        if (count >= fCurSegmentTime)
        {
            DefinePoints();

            count = 0;
            fCurSegmentTime = Random.Range(fMinSegmentTime, fMaxSegmentTime);
        } else {
            Movement();
        }
    }

    void DefinePoints() {
        point[0] = goFlySprite.transform.localPosition;

        point[1] = point[2];

        point[2] = new Vector2(Random.Range(boundaryLeft, boundaryRight), Random.Range(boundaryUp, boundaryDown));
        if (Vector2.Distance(point[0], point[2]) > fMaxArcDistance) {
            //If the new destination is too far away, then bring it closer (but in the same direction)
            point[2] = point[0] + (point[2] - point[0]).normalized * fMaxArcDistance;
        } else if (Vector2.Distance(point[0], point[2]) < fMinArcDistance) {
            //If the new destination is too close, then push it away (in the same direction)
            point[2] = point[0] + (point[2] - point[0]).normalized * fMinArcDistance;
        }
    }

    private void Movement()
    {
        float fProgress = 0.5f * (count / fCurSegmentTime);

        m1 = Vector3.Lerp(point[0], point[1], fProgress);
        m2 = Vector3.Lerp(point[1], point[2], fProgress);
        goFlySprite.transform.localPosition = Vector3.Lerp(m1, m2, fProgress);
        
    }
}
