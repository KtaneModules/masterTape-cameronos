using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinAnimate : MonoBehaviour {

	bool spinning = false;
	private float startTime;
  private float duration = 12f; //12 seconds

	public float speed = 5f; // Speed of rotation
  public float amplitude = 15f; // Amplitude of the sine wave

	//Standard floats for standard men!

	private Quaternion initialRotation;


	// Use this for initialization
	void Start() {
		initialRotation = transform.localRotation;
}

	public void WinAnimation () {
		StartCoroutine(WinCoroutine());
	}

	private System.Collections.IEnumerator WinCoroutine()
{
		float startTime = Time.time;
		while (Time.time - startTime < duration)
		{
			float rotationAngle = Mathf.Sin(Time.time * speed) * amplitude;
			transform.localRotation = initialRotation * Quaternion.Euler(0f, 0f, rotationAngle);
				yield return null; // Wait for the next frame
		}
		RestoreRotation();
}

	void RestoreRotation() {
				transform.localRotation = initialRotation;
			}
			
	// Update is called once per frame
	void Update () {
	}
}
