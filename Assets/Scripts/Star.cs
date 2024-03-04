using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour{

    public Animator anim;

    public Image imgStar;

    public Sprite sprGoldStar;

    void Start(){

    }

    public void SetGold() {

        imgStar.sprite = sprGoldStar;

    }

    public void PauseAnimation() {
        anim.SetTrigger("Pause");
        Debug.Log("Pausing");
    }

    public void UnPauseAnimation() {
        anim.SetTrigger("Unpause");
    }

    public void NewlyEarnedStar() {

        anim.SetTrigger("NewlyEarned");

    }

    public void HideStar() {

        gameObject.SetActive(false);

    }

}
