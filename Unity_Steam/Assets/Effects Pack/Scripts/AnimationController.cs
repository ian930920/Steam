using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

[System.Serializable]
public class EffectAnimation
{
	public GameObject animationObject;
	public float delay = 0; 
}

public class AnimationController : MonoBehaviour
{
	private static readonly string STR_ANIM = "appear";

	[SerializeField] private List<EffectAnimation> animations = new List<EffectAnimation>();
	private EffectAnimation m_maxAnimation = null;
	private UnityAction m_funcOnFinish = null;

#region DevIan
	public void Play(UnityAction funcOnFinish = null)
    {
		this.m_funcOnFinish = funcOnFinish;

		if(this.m_maxAnimation == null) this.m_maxAnimation = this.GetMaxLengthAnimation();

		for(int i = 0, nMax = this.animations.Count; i < nMax; i++)
		{
			if(this.animations[i].delay > 0) StartCoroutine("DelayPlay", this.animations[i]);
			else this.play(this.animations[i]);
		}
    }
#endregion

	private void play(EffectAnimation animation)
	{
		animation.animationObject.SetActive(true);
		animation.animationObject.GetComponent<Animator>().Play(STR_ANIM);
		StartCoroutine("Disappear", animation);
	}

	private IEnumerator DelayPlay(EffectAnimation animation)
	{
		animation.animationObject.SetActive(false);

		yield return Utility_Time.YieldInstructionCache.WaitForSeconds(animation.delay);

		this.play(animation);
	}

	private IEnumerator Disappear(EffectAnimation animation)
	{
		float fWaitTime = animation.animationObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
		yield return Utility_Time.YieldInstructionCache.WaitForSeconds(fWaitTime);

		if(this.m_funcOnFinish != null)
		{
			this.m_funcOnFinish?.Invoke();
			this.m_funcOnFinish = null;
		}

		animation.animationObject.SetActive (false);
		if(this.m_maxAnimation == animation) this.gameObject.transform.parent.gameObject.SetActive(false);
	}

	/*
	private EffectAnimation GetMaxLengthAnimation()
	{
		EffectAnimation maxAnimation = new EffectAnimation();
		float maxLength = -1;
		for(int i = 0, nMax = this.animations.Count; i < nMax; i++)
		{
			var animator = this.animations[i].animationObject.GetComponent<Animator>();
			AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
			float length = info.length + this.animations[i].delay;
			if(length > maxLength)
			{
				maxLength = length;
				maxAnimation = this.animations[i];
			}
		}
		return maxAnimation;
	}
	*/

	private EffectAnimation GetMaxLengthAnimation()
	{
		EffectAnimation maxAnimation = null;
		float maxLength = -1f;

		for (int i = 0; i < this.animations.Count; i++)
		{
			var animator = this.animations[i].animationObject.GetComponent<Animator>();
			var controller = animator.runtimeAnimatorController;

			if(controller.animationClips.Length == 0) continue;

			// 첫 번째 클립 기준 (보통 하나만 있을 경우)
			var clip = controller.animationClips[0];
			float length = clip.length + this.animations[i].delay;

			if(length > maxLength)
			{
				maxLength = length;
				maxAnimation = this.animations[i];
			}
		}

		return maxAnimation;
	}
}