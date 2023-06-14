using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateReels : MonoBehaviour {
	
	public static int spinSpeed = 0;
	public int speedToUse = spinSpeed * -1;
	bool spinning = false;
	private float startTime;
  private float duration = 30f;
	private float shortduration = 10f;
	private float ffduration = 3f;
	private float yeatduration = 12f;


	// Use this for initialization
	private Quaternion initialRotation;

	void Start() {
		initialRotation = transform.localRotation;
	}

	public void RestoreRotation() {
				transform.localRotation = initialRotation;
			}

	public void FastForward () {
		StartCoroutine(FastForwardCoroutine());
	}

	public void WinSpin () {
		StartCoroutine(WinSpinCoroutine());
	}

	public void Spin () {
		StartCoroutine(SpinCoroutine());
	}

	public void ShortSpin () {
		StartCoroutine(ShortSpinCoroutine());
	}

	private System.Collections.IEnumerator FastForwardCoroutine()
{
		float startTime = Time.time;
		float insaneSpeed = 1000f;
		while (Time.time - startTime < ffduration)
		{
				transform.Rotate(0, insaneSpeed * Time.deltaTime, 0);
				yield return null; // Wait for the next frame
		}
		RestoreRotation();
}

private System.Collections.IEnumerator WinSpinCoroutine()
{
	float startTime = Time.time;
	float regularhappy = 350f;
	while (Time.time - startTime < yeatduration)
	{
			transform.Rotate(0, regularhappy * Time.deltaTime, 0);
			yield return null; // Wait for the next frame
	}
	RestoreRotation();
}

	private System.Collections.IEnumerator SpinCoroutine()
{
		float startTime = Time.time;
		while (Time.time - startTime < duration)
		{
				transform.Rotate(0, speedToUse * Time.deltaTime, 0);
				yield return null; // Wait for the next frame
		}
}

private System.Collections.IEnumerator ShortSpinCoroutine()
{
	float startTime = Time.time;
	while (Time.time - startTime < shortduration)
	{
			transform.Rotate(0, speedToUse * Time.deltaTime, 0);
			yield return null; // Wait for the next frame
	}

}

	// Update is called once per frame
	void Update () {
	}
}
