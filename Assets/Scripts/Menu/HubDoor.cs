using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HubDoor : MonoBehaviour {

    public int nWorld;
    public int nFliesRequired;
    public float fDelayBeforeTransition;
    public Text txtFliesRequired;

    public bool bUnlocked;
    public float fTimeOnTrigger;

    private void Start() {
        fTimeOnTrigger = 0f;

        cbCheckUnlock(null);
        UpdateFliesRequiredPanel();
    }

    public void cbCheckUnlock(Object target, params object[] args) {
        Debug.Log("Currently earned " + GeneralManager.nSavedTotalStarsEarned + " stars in total");
        bUnlocked = GeneralManager.nSavedTotalStarsEarned >= nFliesRequired;
        UpdateFliesRequiredPanel();
    }

    void UpdateFliesRequiredPanel() {
        if (bUnlocked) {
            txtFliesRequired.text = "Unlocked";
        } else {
            txtFliesRequired.text = nFliesRequired.ToString();
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision) {

        if(bUnlocked == false) {
            fTimeOnTrigger = 0f;
            return;
        }

        fTimeOnTrigger += Time.deltaTime;
        
        if(fTimeOnTrigger >= fDelayBeforeTransition) {
            ActivateDoor();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        fTimeOnTrigger = 0f;
    }

    void ActivateDoor() {
        //Load the menu level (0th scene) for the world we represent
        ContScenes.Get().LoadScene(0, nWorld);
    }

}
