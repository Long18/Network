using UnityEngine;

public class FireLight : MonoBehaviour
{
	public AnimationCurve lightCurve;
	public float fireSpeed = 1f;

	private Light lightComp;
	private float initialIntensity;

	private void Awake()
	{
		lightComp = GetComponent<Light>();
		initialIntensity = lightComp.intensity;
	}

	void Update()
	{
		lightComp.intensity = initialIntensity * lightCurve.Evaluate(Time.time * fireSpeed);
	}
}
