using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class DissolveHelper : MonoBehaviour
{
	[FormerlySerializedAs("_dissolveParticlesPrefab")] [SerializeField] ParticleSystem dissolveParticlesPrefab = default;
	[FormerlySerializedAs("_dissolveDuration")] [SerializeField] float dissolveDuration = 1f;

	private MeshRenderer meshRenderer;
	private ParticleSystem particules;

	private MaterialPropertyBlock materialPropertyBlock;

	[ContextMenu("Trigger Dissolve")]
	public void TriggerDissolve()
	{
		if (materialPropertyBlock == null)
		{
			materialPropertyBlock = new MaterialPropertyBlock();
		}
		InitParticleSystem();
		StartCoroutine(DissolveCoroutine());
	}

	[ContextMenu("Reset Dissolve")]
	private void ResetDissolve()
	{
		materialPropertyBlock.SetFloat("_Dissolve", 0);
		meshRenderer.SetPropertyBlock(materialPropertyBlock);
	}

	private void InitParticleSystem()
	{
		particules = GameObject.Instantiate(dissolveParticlesPrefab, transform);

		meshRenderer = GetComponent<MeshRenderer>();
		ParticleSystem.ShapeModule shapeModule = particules.shape;
		shapeModule.meshRenderer = meshRenderer;
		shapeModule.enabled = true;

		ParticleSystem.MainModule mainModule = particules.main;
		mainModule.duration = dissolveDuration;
	}

	public IEnumerator DissolveCoroutine()
	{
		float normalizedDeltaTime = 0;

		particules.Play();

		while (normalizedDeltaTime < dissolveDuration)
		{
			normalizedDeltaTime += Time.deltaTime;
			float remappedValue = VFXUtil.RemapValue(normalizedDeltaTime, 0, dissolveDuration, 0, 1);
			materialPropertyBlock.SetFloat("_Dissolve", remappedValue);
			meshRenderer.SetPropertyBlock(materialPropertyBlock);

			yield return null;
		}
		GameObject.Destroy(particules.gameObject);
	}
}
