using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web : MonoBehaviour {

    public enum CollisionType { NONE, STICKABLE, NONSTICKABLE };
    public enum WebType { SWINGING, LAUNCHING };
    public WebType webType;

    public float fDespawnDelay;
    public float fFadeoutTime;
    public float fDistTargetTolerance = 0.05f;

    public float fMaxWebShotLength;

    public GameObject goAttachedTo;

    public Vector2 v2Target;
    public Vector2 v2ShotDirection;

    public float fSpeed;
    public GameObject goOwner;
    LineRenderer lineRenderer;

    enum Despawn { ACTIVE, WAITING, FADEOUT };
    Despawn curDespawn;
    float fDespawnTimer;

    public List<Transform> lstWebNodes;

    public Rigidbody2D rb;

    public bool bOverrideMovement;

    public void Awake() {

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // for the websource and the owner

        lstWebNodes.Add(this.transform);
    }

    public void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetTarget(Vector2 _v2Target, float _fMaxWebShotLength) {
        fMaxWebShotLength = _fMaxWebShotLength;

        //Get the direction from the owner to the target
        v2ShotDirection = new Vector2(_v2Target.x - this.transform.position.x, _v2Target.y - this.transform.position.y);
        v2ShotDirection.Normalize();

        //Our target will be extended in that direction up to the maximum length
        v2Target = (Vector2)this.transform.position + v2ShotDirection * fMaxWebShotLength;

    }

    public void SetOwner(GameObject _goOwner) {
        if (goOwner != null) lstWebNodes.RemoveAt(lstWebNodes.Count - 1);

        goOwner = _goOwner;
        lstWebNodes.Add(goOwner.transform);

        UpdateWebVisuals();
    }


    public void UpdateWebVisuals() {

        for(int i=0; i<lstWebNodes.Count; i++) {
            lineRenderer.SetPosition(i, lstWebNodes[i].position);
        }
    }

    //Add a weight to the web so that it can dangle
    public void AddWeight(GameObject pfWeight) {
        
        GameObject goWeight = Instantiate(pfWeight, goOwner.transform.position, Quaternion.identity, this.transform);
        goWeight.GetComponent<DistanceJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();

        //Have weight continue in the same direction as the owner
        goWeight.GetComponent<Rigidbody2D>().velocity = goOwner.GetComponent<Rigidbody2D>().velocity / 2;

        //Set our owner to be the weight, instead of the previous owner
        SetOwner(goWeight);

        curDespawn = Despawn.WAITING;
    }

    public CollisionType ReachedSurface() {

        int nLayerPlatforms = 1 << LayerMask.NameToLayer("Platforms");
        
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, v2ShotDirection, fSpeed * Time.fixedDeltaTime, nLayerPlatforms);

        //If we haven't collided with anything, return no collision
        if (hit == false) {
            return CollisionType.NONE;
        }

        //Check if we've reached a Stickable Surface
        StickableSurface stuck = hit.rigidbody.gameObject.GetComponent<StickableSurface>();

        //If we collided with something stickable
        if (stuck != null) {
            //Make the web stick to the projected point of collision
            rb.transform.position = hit.point;
            stuck.StickWeb(this);

            return CollisionType.STICKABLE;
        }

        return CollisionType.NONSTICKABLE;

    }

    public bool HasReachedMaximumLength() {
        //Check if we are sufficiently close to the target (which will be at the maximum length)

        return Vector2.Distance(this.transform.position, v2Target) <= fDistTargetTolerance;
    }

    public void MoveToTarget() {

        if (Vector2.Distance(this.transform.position, v2Target) < fSpeed * Time.fixedDeltaTime) {
            //If we're close enough, just move directly to the target
            rb.MovePosition(v2Target);
            rb.velocity = Vector2.zero;
        } else {
            if (bOverrideMovement == false) {
                rb.MovePosition((Vector2)transform.position + v2ShotDirection * fSpeed * Time.fixedDeltaTime);
            } else {
                bOverrideMovement = false;
            }
        }
    }

    void HandleDespawn() {
        switch (curDespawn) {
            case Despawn.ACTIVE:
                return;
            case Despawn.WAITING:
                fDespawnTimer += Time.deltaTime;
                if(fDespawnTimer >= fDespawnDelay) {
                    fDespawnTimer = 0f;
                    curDespawn = Despawn.FADEOUT;
                }
                break;
            case Despawn.FADEOUT:
                fDespawnTimer += Time.deltaTime;

                float fFadeoutProgress = Mathf.Lerp(1, 0, fDespawnTimer / fFadeoutTime);

                Color startColour = lineRenderer.startColor;
                startColour.a = fFadeoutProgress;
                lineRenderer.startColor = startColour;

                Color endColour = lineRenderer.endColor;
                endColour.a = fFadeoutProgress;
                lineRenderer.endColor = endColour;

                if (fDespawnTimer >= fFadeoutTime) {
                    goAttachedTo.GetComponent<StickableSurface>().lstStuckWebs.Remove(this);
                    GameObject.Destroy(this.gameObject);
                }
                break;

        }
    }

    // Update is called once per frame
    void Update() {

        UpdateWebVisuals();
        HandleDespawn();
    }


}
