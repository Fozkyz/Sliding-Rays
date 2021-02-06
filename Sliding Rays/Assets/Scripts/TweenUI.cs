using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenUI : MonoBehaviour
{
	public LeanTweenType in_type;
	public LeanTweenType out_type;
	public float duration;
	public float delay;

	private void OnEnable()
	{
		transform.localScale = Vector3.zero;
		LeanTween.scale(gameObject, Vector3.one, duration).setDelay(delay).setEase(in_type);
	}

	private void OnDisable()
	{
		transform.localScale = Vector3.one;
		LeanTween.scale(gameObject, Vector3.zero, duration).setEase(out_type);
	}
}
