using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CurrencySlotGroup : MonoBehaviour
{
    private UI_CurrencySlot[] ArrCurrencySlot { get; set; } = null;

    private void setCurrencySlot()
    {
        this.ArrCurrencySlot = this.transform.GetComponentsInChildren<UI_CurrencySlot>();
    }

    public void RefreshSlot()
    {
        if(this.ArrCurrencySlot == null) this.setCurrencySlot();

        for(int i = 0, nMax = this.ArrCurrencySlot.Length; i < nMax; ++i)
        {
            if(this.ArrCurrencySlot[i].gameObject.activeSelf == false) continue;

            this.ArrCurrencySlot[i].RefreshSlot();
        }
    }

    public void InitSlot()
    {
        if(this.ArrCurrencySlot == null) this.ArrCurrencySlot = this.transform.GetComponentsInChildren<UI_CurrencySlot>();

        for(int i = 0, nMax = this.ArrCurrencySlot.Length; i < nMax; ++i)
        {
            this.ArrCurrencySlot[i].InitSlot();
        }
    }
}