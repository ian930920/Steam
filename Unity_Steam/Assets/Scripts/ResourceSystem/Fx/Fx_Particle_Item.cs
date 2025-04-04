using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fx_Particle_Item : Fx_Particle
{
    [SerializeField] private ParticleSystemRenderer m_renderer = null;

    private int m_nItemID = 0;

    public void SetItem(int nItemID, Vector3 vecPos)
    {
        if(this.m_nItemID != nItemID)
        {
            this.m_nItemID = nItemID;
            //TODO
            //this.m_renderer.material = ProjectManager.Instance.Resource.GetMaterialByItemID(nItemID);
        }
        
        //TODO Sound 사운드 재생
        //if(TableData.TableItem.IsGold(nItemID) == true) ProjectManager.Instance.ObjectPool.PlayEffectSound(BaseSound.eID.ItemDrop, 0.3f);
        //else ProjectManager.Instance.ObjectPool.PlayEffectSound(BaseSound.eID.ItemDrop_Special);

        base.Play(vecPos);
    }
}