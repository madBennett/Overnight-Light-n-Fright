using UnityEngine;

public class MobShaderController : MonoBehaviour
{
    public Material chaseMaterial;
    public Material damageMaterial;
    private int chasingGhostCount = 0;
    private bool chaseShaderActive = false;

    private EffectsManager effectsManager;

    private void Start()
    {
        effectsManager = FindObjectOfType<EffectsManager>();
    }

    public void ApplyDamageShader()
    {
        if (effectsManager != null)
        {
            Debug.Log("[MobShaderController] Activating Damage Shader");
            effectsManager.ApplyEffect(EffectTypes.VISUAL_DISTORTION, VisualTypes.MOBCHASE);
        }
    }

    public void RemoveDamageShader()
    {
        if (effectsManager != null)
        {
            Debug.Log("[MobShaderController] Deactivating Damage Shader");
            effectsManager.ReturnToDefalut(EffectTypes.VISUAL_DISTORTION);
        }
    }

    public void AddChasingGhost()
    {
        chasingGhostCount = Mathf.Max(0, chasingGhostCount + 1);
        //Debug.Log($"[MobShaderController] AddChasingGhost. Count: {chasingGhostCount}");

        if (chasingGhostCount == 1 && !chaseShaderActive)
        {
            chaseShaderActive = true;
            if (effectsManager != null)
            {
                //Debug.Log("[MobShaderController] Activating Chase Shader");
                effectsManager.ApplyEffect(EffectTypes.VISUAL_DISTORTION, VisualTypes.MOBCHASE);
            }
        }
    }

    public void RemoveChasingGhost()
    {
        chasingGhostCount = Mathf.Max(0, chasingGhostCount - 1);
        //Debug.Log($"[MobShaderController] RemoveChasingGhost. Count: {chasingGhostCount}");

        if (chasingGhostCount == 0 && chaseShaderActive)
        {
            chaseShaderActive = false;
            if (effectsManager != null)
            {
                //Debug.Log("[MobShaderController] Deactivating Chase Shader");
                effectsManager.ReturnToDefalut(EffectTypes.VISUAL_DISTORTION);
            }
        }
    }

    private void OnDestroy()
    {
        if (chaseShaderActive && effectsManager != null)
        {
            effectsManager.ReturnToDefalut(EffectTypes.VISUAL_DISTORTION);
        }
    }
}