using System.Collections;
using UnityEngine;

public class Fx_SpriteAnimation : BaseFx
{
    [SerializeField] private AnimationController m_animationController = null;

    protected override IEnumerator coPlay()
    {
        this.m_animationController?.Play(base.m_funcOnFinish);
        yield return null;
    }
}