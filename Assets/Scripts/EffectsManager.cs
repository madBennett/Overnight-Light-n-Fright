using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectTypes
{
    DEFAULT,
    VISUAL_DISTORTION,
    REVERSE_CONTROLS,
    STUN,
    TELEPORT,
    DAMAGE,
    NUM_EFFECTS
}

public enum VisualTypes
{
    SNOW,
    TILE
}

public class EffectsManager : MonoBehaviour
{
    [SerializeField] private PlayerBehavior Player;

    //visual effects
    [SerializeField] private List<Material> visualMaterials;
    [SerializeField] private Material currentMat;

    //movement effects
    [SerializeField] private ControlManager PlayerControls;

    // Start is called before the first frame update
    void Start()
    {
        currentMat = visualMaterials[0];
    }

    // Update is called once per frame
    void Update()
    {
        switch (Player.currEffect)
        {
            case EffectTypes.REVERSE_CONTROLS:
                ReverseControls();
                break;
            case EffectTypes.STUN:
                StunPlayer();
                break;
        }
    }

    //Apply Visual Effects
    public void ApplyVisualDistortion(VisualTypes visEffect)
    {
        currentMat = visualMaterials[(int)visEffect];
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (Player.currEffect == EffectTypes.VISUAL_DISTORTION)
        {
            Graphics.Blit(src, dest, currentMat);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }

    //Apply Movement Effects
    public void ReverseControls()
    {
        //TODO FIX
        PlayerControls.movement = PlayerControls.movement * - 1;
    }

    public void StunPlayer()
    {
        PlayerControls.currSpeed = 0;
        //TODO PREVENT FLASHLIGHT
    }

    public void TeleportPlayerr()
    {
        //TODO IMPLEMENT
    }

    //
    public void DamagePlayer()
    {
        //TODO IMPLEMENT
    }
}
