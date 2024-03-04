using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown {

    public int nMaxCharges;


    int nCurCharges;
    float fCurCooldown;

    public Cooldown(int _nMaxCharges = 1) {
        nMaxCharges = _nMaxCharges;
        nCurCharges = nMaxCharges;
        GeneralManager.Get().subUpdate.Subscribe(cbUpdate);
    }

    public virtual void cbUpdate(Object target, params object[] args) {
        fCurCooldown -= Time.fixedDeltaTime;
    }

    public void SetCooldown(float fNewCooldown) {
        
        fCurCooldown = Mathf.Max(fCurCooldown, fNewCooldown);
    }

    public void ResetCooldown() {
        fCurCooldown = 0f;
    }

    public void UseCharge() {
        Debug.Assert(nCurCharges > 0);

        nCurCharges--;
    }

    public void ResetCharges() {
        nCurCharges = nMaxCharges;
    }

    public void AddCharge() {
        nCurCharges++;

        Debug.Assert(nCurCharges <= nMaxCharges);
    }

    public bool CanUse() {
        return nCurCharges > 0 && fCurCooldown <= 0f;
    }

}
