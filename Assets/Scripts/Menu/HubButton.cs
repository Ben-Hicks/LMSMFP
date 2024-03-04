using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubButton : MonoBehaviour {

    public void OnClick() {
        ContScenes.Get().LoadHubWorld();
    }
}
