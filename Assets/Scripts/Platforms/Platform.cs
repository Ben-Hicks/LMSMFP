using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public GameObject goFloor;
    public GameObject goCeiling;
    public GameObject goLeftWall;
    public GameObject goRightWall;

    public GameObject pfPlatformCollider;

    public Rigidbody2D rb;

    Bounds bounds;

    public float fCOLLIDERTHICKNESS = 0.04f;

    public Vector3 v3MovingForce;
    //TODO:: Maybe extend this to be able to carry multiple things?
    public int nCollisionCounts;
    public Rigidbody2D rbStandingOn;

    public void PlaceEdgeColliders() {
        bounds = GetComponent<Renderer>().bounds;

        goFloor = Instantiate(pfPlatformCollider);
        goFloor.transform.localScale = new Vector2(bounds.size.x, fCOLLIDERTHICKNESS);
        goFloor.transform.localPosition = new Vector2(this.transform.position.x, this.transform.position.y + bounds.extents.y - fCOLLIDERTHICKNESS / 2);
        goFloor.transform.SetParent(this.transform);
        
        goFloor.layer = LayerMask.NameToLayer("Floor");


        goCeiling = Instantiate(pfPlatformCollider);
        goCeiling.transform.localScale = new Vector2(bounds.size.x, fCOLLIDERTHICKNESS);
        goCeiling.transform.localPosition = new Vector2(this.transform.position.x, this.transform.position.y - bounds.extents.y + fCOLLIDERTHICKNESS / 2);
        goCeiling.transform.SetParent(this.transform);

        goCeiling.layer = LayerMask.NameToLayer("Ceiling");


        goLeftWall = Instantiate(pfPlatformCollider);
        goLeftWall.transform.localScale = new Vector2(fCOLLIDERTHICKNESS, bounds.size.y - (fCOLLIDERTHICKNESS * 2));
        goLeftWall.transform.localPosition = new Vector2(this.transform.position.x - bounds.extents.x + fCOLLIDERTHICKNESS / 2, this.transform.position.y);
        goLeftWall.transform.SetParent(this.transform);

        goLeftWall.layer = LayerMask.NameToLayer("Wall");

        goRightWall = Instantiate(pfPlatformCollider);
        goRightWall.transform.localScale = new Vector2(fCOLLIDERTHICKNESS, bounds.size.y - (fCOLLIDERTHICKNESS * 2));
        goRightWall.transform.localPosition = new Vector2(this.transform.position.x + bounds.extents.x - fCOLLIDERTHICKNESS / 2, this.transform.position.y);
        goRightWall.transform.SetParent(this.transform);

        goRightWall.layer = LayerMask.NameToLayer("Wall");
    }

    public void Start() {

        PlaceEdgeColliders();
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetMovingForce(Vector3 _v3MovingForce) {
        CounteractMovingForce();
        v3MovingForce = _v3MovingForce;
        ApplyMovingForce();

    }

    public void ApplyMovingForce() {
        if(rbStandingOn != null) {

            rbStandingOn.AddForce(rbStandingOn.mass * v3MovingForce, ForceMode2D.Impulse);
            rbStandingOn.GetComponent<ContFriction>().fTargetVelocityX = v3MovingForce.x;
        } 
    }

    public void CounteractMovingForce() {
        //Only apply a counteracting force if you are either movng the platform down or, not at all vertically
        // - this means that only if you are moving up, will you continue to be flung in that direction
        if (rbStandingOn != null && v3MovingForce.y <= 0.1) {
            rbStandingOn.AddForce(rbStandingOn.mass * -v3MovingForce, ForceMode2D.Impulse);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        //Either we aren't carrying anything currently, or its the same thing we're already carrying - also it better have a Player component
        if ((rbStandingOn == null && collision.gameObject.GetComponent<Player>() != null) || collision.attachedRigidbody == rbStandingOn)  {
            rbStandingOn = collision.attachedRigidbody;
            nCollisionCounts++;
            if(nCollisionCounts == 1) {
                //Debug.Log("This was our first collision");
                ApplyMovingForce();
            } else {
                //Debug.Log("This was our second collision so we shouldn't apply any extra force");
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collision) {
        
    }

    public void OnTriggerExit2D(Collider2D collision) {

        if (rbStandingOn != null && collision.attachedRigidbody == rbStandingOn) {
            nCollisionCounts--;

            if(nCollisionCounts == 0) {
                //Debug.Log("Lost all collisions");
                rbStandingOn.GetComponent<ContFriction>().fTargetVelocityX = 0;

                rbStandingOn = null;
            }
        } 
    }


}
