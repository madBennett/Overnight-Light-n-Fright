using System.Collections;
using System.Collections.Generic;
using UnityEngine;


EffectsManager1.ApplyEffect(EffectTypes.VISUAL_DISTORTION, VisualTypes.RESPAWN_JITTER);

public class EffectsManager1 : MonoBehaviour
{
    [SerializeField] private PlayerBehavior Player;
    [SerializeField] private AudioManager AM;

    [SerializeField] private float effectTime = 2f;
    [SerializeField] private bool switchMode = false;
    [SerializeField] private bool[] appliedEffects = new bool[(int)EffectTypes.NUM_EFFECTS]; //bool map to verify which effect is curretnly active

    //color effects
    [SerializeField] private SpriteRenderer PlayerSpriteRenderer;

    //visual effects
    [SerializeField] private List<Material> visualMaterials;
    [SerializeField] private CameraBehavior mainCameraBehavior;

    //movement effects
    [SerializeField] private ControlManager PlayerControls;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>();
        PlayerControls = GameObject.FindGameObjectWithTag("Player").GetComponent<ControlManager>();
        PlayerSpriteRenderer = Player.GetComponent<SpriteRenderer>();
        mainCameraBehavior = Camera.main.GetComponent<CameraBehavior>();
        AM = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        for (int i = 0; i < (int)EffectTypes.NUM_EFFECTS; i++)
        {
            appliedEffects[i] = false;
        }
    }

    public void ApplyEffect(EffectTypes effect, VisualTypes visEffect = VisualTypes.MOBCHASE, int damageAmt = 1)
    {
        if (!appliedEffects[(int)effect])
        {
            AM.PlayAudio(AudioClipTypes.EFFECT_APPLIED);

            switch (effect)
            {
                case EffectTypes.VISUAL_DISTORTION:
                    mainCameraBehavior.currMat = visualMaterials[(int)visEffect];

                    // ✅ Only enable MobChaseRenderFeature for mob-related visual types
                    if (visEffect == VisualTypes.MOBCHASE || visEffect == VisualTypes.MOBDAMAGE)
                    {
                        MobChaseRenderFeature.Instance?.EnableEffect();
                    }
                    break;

                case EffectTypes.REVERSE_CONTROLS:
                    PlayerControls.currMoveState = MovementStates.REVERSE;
                    break;

                case EffectTypes.STUN:
                    PlayerControls.currMoveState = MovementStates.STUN;
                    break;

                case EffectTypes.DAMAGE:
                    mainCameraBehavior.currMat = visualMaterials[(int)visEffect];

                    if (visEffect == VisualTypes.MOBCHASE || visEffect == VisualTypes.MOBDAMAGE)
                    {
                        MobChaseRenderFeature.Instance?.EnableEffect();
                    }
                    break;
            }

            appliedEffects[(int)effect] = true;
            ChangePlayerColor(effect);

            if (!switchMode)
            {
                StartCoroutine(ResetToDefaultTimed(effect));
            }
        }
    }

    private void ChangePlayerColor(EffectTypes effect)
    {
        if (effect == EffectTypes.DEFAULT)
        {
            PlayerSpriteRenderer.color = Color.white;
        }
        else
        {
            PlayerSpriteRenderer.color = Color.red;
        }
    }

    private IEnumerator ResetToDefaultTimed(EffectTypes effect)
    {
        yield return new WaitForSeconds(effectTime);
        ReturnToDefalut(effect);
    }

    public void ReturnToDefalut(EffectTypes effect)
    {
        switch (effect)
        {
            case EffectTypes.VISUAL_DISTORTION:
            case EffectTypes.DAMAGE:
                // ✅ Only disable render feature if it was used
                if (mainCameraBehavior.currMat == visualMaterials[(int)VisualTypes.MOBCHASE] ||
                    mainCameraBehavior.currMat == visualMaterials[(int)VisualTypes.MOBDAMAGE])
                {
                    MobChaseRenderFeature.Instance?.DisableEffect();
                }

                mainCameraBehavior.currMat = null;
                break;

            case EffectTypes.REVERSE_CONTROLS:
            case EffectTypes.STUN:
                PlayerControls.currMoveState = MovementStates.DEFAULT;
                break;
        }

        ChangePlayerColor(EffectTypes.DEFAULT);
        appliedEffects[(int)effect] = false;
    }
}
