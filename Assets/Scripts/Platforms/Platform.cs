using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Platform : MonoBehaviour {

    public GameObject goFloor;
    public GameObject goCeiling;
    public GameObject goLeftWall;
    public GameObject goRightWall;

    public GameObject pfPlatformCollider;

    public Rigidbody2D rb;

    Bounds bounds;

    public float fCOLLIDERTHICKNESS = 0.04f;

    public Dictionary<Rigidbody2D, int> dictrb2dStandingOn;

    public Subject subAddedStandingOn;
    public Subject subRemovedStandingOn;

    public void PlaceEdgeColliders() {
        bounds = GetComponent<Renderer>().bounds;

        goFloor = Instantiate(pfPlatformCollider);
        goFloor.transform.localScale = new Vector3(bounds.size.x, fCOLLIDERTHICKNESS, 1f);
        goFloor.transform.localPosition = new Vector2(this.transform.position.x, this.transform.position.y + bounds.extents.y - fCOLLIDERTHICKNESS / 2);
        goFloor.transform.SetParent(this.transform);
        
        goFloor.layer = LayerMask.NameToLayer("Floor");


        goCeiling = Instantiate(pfPlatformCollider);
        goCeiling.transform.localScale = new Vector3(bounds.size.x, fCOLLIDERTHICKNESS, 1f);
        goCeiling.transform.localPosition = new Vector2(this.transform.position.x, this.transform.position.y - bounds.extents.y + fCOLLIDERTHICKNESS / 2);
        goCeiling.transform.SetParent(this.transform);

        goCeiling.layer = LayerMask.NameToLayer("Ceiling");


        goLeftWall = Instantiate(pfPlatformCollider);
        goLeftWall.transform.localScale = new Vector3(fCOLLIDERTHICKNESS, bounds.size.y - (fCOLLIDERTHICKNESS * 2), 1f);
        goLeftWall.transform.localPosition = new Vector2(this.transform.position.x - bounds.extents.x + fCOLLIDERTHICKNESS / 2, this.transform.position.y);
        goLeftWall.transform.SetParent(this.transform);

        goLeftWall.layer = LayerMask.NameToLayer("Wall");

        goRightWall = Instantiate(pfPlatformCollider);
        goRightWall.transform.localScale = new Vector3(fCOLLIDERTHICKNESS, bounds.size.y - (fCOLLIDERTHICKNESS * 2), 1f);
        goRightWall.transform.localPosition = new Vector2(this.transform.position.x + bounds.extents.x - fCOLLIDERTHICKNESS / 2, this.transform.position.y);
        goRightWall.transform.SetParent(this.transform);

        goRightWall.layer = LayerMask.NameToLayer("Wall");
    }

    public void Awake() {
        subAddedStandingOn = new Subject();
        subRemovedStandingOn = new Subject();
        dictrb2dStandingOn = new Dictionary<Rigidbody2D, int>();
    }

    public void Start() {

        PlaceEdgeColliders();
        rb = GetComponent<Rigidbody2D>();
    }


    public void OnTriggerEnter2D(Collider2D collision) {

        Rigidbody2D rbCollided = collision.attachedRigidbody;

        if (rbCollided == null) return; //If our collision somehow doesn't have a rigidbody (shouldn't be possible), then we're done

        if (dictrb2dStandingOn.ContainsKey(rbCollided)) {
            dictrb2dStandingOn[rbCollided]++;
            return; //If we're already tracking this as being on us, then we're done
        }

        dictrb2dStandingOn.Add(rbCollided, 1);

        subAddedStandingOn.NotifyObs(rbCollided);

    }

    public void OnTriggerStay2D(Collider2D collision) {
        
    }

    public void OnTriggerExit2D(Collider2D collision) {

        Rigidbody2D rbCollided = collision.attachedRigidbody;

        if (rbCollided == null) return; //If our collision somehow doesn't have a rigidbody (shouldn't be possible), then we're done

        if(dictrb2dStandingOn.ContainsKey(rbCollided) == false) {
            Debug.LogErrorFormat("Somehow exited the trigger for {0}, but we don't have a record of us colliding with it", rbCollided.gameObject.name);
            return;
        }
        dictrb2dStandingOn[rbCollided]--;

        if (dictrb2dStandingOn[rbCollided] == 0) {
            dictrb2dStandingOn.Remove(rbCollided);

            subRemovedStandingOn.NotifyObs(rbCollided);
        }

    }


}
