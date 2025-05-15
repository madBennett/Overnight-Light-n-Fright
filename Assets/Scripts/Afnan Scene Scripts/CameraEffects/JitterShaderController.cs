using UnityEngine;

public class JitterShaderController : MonoBehaviour
{
    public float jitterDuration = 0.5f; // How long the effect lasts
    private EffectsManager effectsManager;

    private void Start()
    {
        effectsManager = FindObjectOfType<EffectsManager>();
    }

    public void TriggerJitter()
    {
        if (effectsManager != null)
        {
            Debug.Log("[JitterShaderController] Activating jitter shader");
            effectsManager.ApplyEffect(EffectTypes.VISUAL_DISTORTION, VisualTypes.MOBCHASE);
            Invoke(nameof(StopJitter), jitterDuration);
        }
    }

    private void StopJitter()
    {
        if (effectsManager != null)
        {
            Debug.Log("[JitterShaderController] Stopping jitter shader");
            effectsManager.ReturnToDefalut(EffectTypes.VISUAL_DISTORTION);
        }
    }
}
