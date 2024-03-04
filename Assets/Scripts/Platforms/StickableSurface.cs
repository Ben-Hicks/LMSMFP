using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickableSurface : MonoBehaviour {

    public List<Web> lstStuckWebs;

    public void StickWeb(Web newStuckWeb) {
        lstStuckWebs.Add(newStuckWeb);
        newStuckWeb.goAttachedTo = this.gameObject;

        //Set the newly stuck web's parent to be this surface
        newStuckWeb.GetComponent<Transform>().SetParent(this.transform);
    }


}
