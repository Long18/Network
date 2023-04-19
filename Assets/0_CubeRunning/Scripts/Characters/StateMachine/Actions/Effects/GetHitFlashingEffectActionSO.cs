using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "GetHitFlashingEffectAction", menuName = "State Machines/Actions/Get Hit Flashing Effect")]
public class GetHitFlashingEffectActionSO : StateActionSO
{
    protected override StateAction CreateAction() => new GetHitFlashingEffectAction();
}

public class GetHitFlashingEffectAction : StateAction
{
    private float getHitFlashingDuration;
    private float getHitFlashingSpeed;
    private Color flashingColor;

    private Material material;
    private Color baseTintColor;
    private float innerFlashingTime;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        Damageable attackableEntity = stateMachine.GetComponent<Damageable>();
        // GetHitEffectConfigSO getHitEffectConfig = attackableEntity.GetHitEffectConfig;

        // Take the last one if many.
        material = attackableEntity.MainMeshRenderer.materials[attackableEntity.MainMeshRenderer.materials.Length - 1];
        // getHitFlashingDuration = getHitEffectConfig.GetHitFlashingDuration;
        // getHitFlashingSpeed = getHitEffectConfig.GetHitFlashingSpeed;
        baseTintColor = material.GetColor("_MainColor");
        // innerFlashingTime = getHitEffectConfig.GetHitFlashingDuration;
        // flashingColor = getHitEffectConfig.GetHitFlashingColor;
    }

    public override void OnUpdate()
    {
        ApplyHitEffect();
    }

    public override void OnStateEnter()
    {
        innerFlashingTime = getHitFlashingDuration;
    }

    public override void OnStateExit()
    {
        material.SetColor("_MainColor", baseTintColor);
    }

    public void ApplyHitEffect()
    {
        if (innerFlashingTime > 0)
        {
            Color tintingColor = computeGetHitTintingColor();
            material.SetColor("_MainColor", tintingColor);
            innerFlashingTime -= Time.deltaTime;
        }
    }

    private Color computeGetHitTintingColor()
    {
        Color finalTintingColor = Color.Lerp(baseTintColor, flashingColor, flashingColor.a);
        float tintingTiming = (getHitFlashingDuration - innerFlashingTime) * getHitFlashingSpeed /
                              getHitFlashingDuration;
        return Color.Lerp(baseTintColor, finalTintingColor, (-Mathf.Cos(Mathf.PI * 2 * tintingTiming) + 1) / 2);
    }
}