using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectTypes
{
    DEFAULT,
    VISUAL_DISTORTION,
    REVERSE_CONTROLS,
    STUN,
    DAMAGE,
    NUM_EFFECTS
}

public enum VisualTypes
{
    SNOW
}

public class EffectsManager : MonoBehaviour
{
    [SerializeField] private PlayerBehavior Player;
    [SerializeField] private float effectTime = 2f;

    //visual effects
    [SerializeField] private List<Material> visualMaterials;
    [SerializeField] private Material currentMat;

    //movement effects
    [SerializeField] private ControlManager PlayerControls;

    // Start is called before the first frame update
    void Start()
    {
        currentMat = null;
    }

    public void ApplyEffect(EffectTypes effect, VisualTypes visEffect = VisualTypes.SNOW, int damageAmt = 1)
    {
        switch (effect)
        {
            case EffectTypes.VISUAL_DISTORTION:
                currentMat = visualMaterials[(int)visEffect]; currentMat = visualMaterials[(int)visEffect];
                break;
            case EffectTypes.REVERSE_CONTROLS:
                PlayerControls.currMoveState = MovementStates.REVERSE;
                break;
            case EffectTypes.STUN:
                PlayerControls.currMoveState = MovementStates.STUN;
                break;
            case EffectTypes.DAMAGE:
                Player.DamagePlayer(damageAmt);
                break;
        }

        Invoke("ResetToDefault", effectTime);
    }

    //Apply Visual Effects
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (currentMat != null)
        {
            Graphics.Blit(src, dest, currentMat);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }

    //
    public void ResetToDefault()
    {
        PlayerControls.currMoveState = MovementStates.DEFAULT;
        currentMat = null;
    }
}
