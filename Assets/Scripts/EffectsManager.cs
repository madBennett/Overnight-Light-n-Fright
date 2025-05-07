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
    MOBCHASE,
    MOBDAMAGE,
    INVERT_COLORS
}

public class EffectsManager : MonoBehaviour
{
    [SerializeField] private PlayerBehavior Player;

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

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>();
        PlayerControls = GameObject.FindGameObjectWithTag("Player").GetComponent<ControlManager>();
        PlayerSpriteRenderer = Player.GetComponent<SpriteRenderer>();
        mainCameraBehavior = Camera.main.GetComponent<CameraBehavior>();

        //inmtialize map
        for (int i = 0; i < (int)EffectTypes.NUM_EFFECTS; i++)
        {
            appliedEffects[i] = false;
        }
    }

    public void ApplyEffect(EffectTypes effect, VisualTypes visEffect = VisualTypes.MOBCHASE, int damageAmt = 1)
    {
        if (!appliedEffects[(int)effect])
        {
            switch (effect)
            {
                case EffectTypes.VISUAL_DISTORTION:
                    mainCameraBehavior.currMat = (visualMaterials[(int)visEffect]);
                    MobChaseRenderFeature.Instance?.EnableEffect();
                    break;
                case EffectTypes.REVERSE_CONTROLS:
                    PlayerControls.currMoveState = MovementStates.REVERSE;
                    break;
                case EffectTypes.STUN:
                    PlayerControls.currMoveState = MovementStates.STUN;
                    break;
                case EffectTypes.DAMAGE:
                    mainCameraBehavior.currMat = (visualMaterials[(int)visEffect]);
                    MobChaseRenderFeature.Instance?.EnableEffect();
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
        //
        if (effect == EffectTypes.DEFAULT)
        {
            PlayerSpriteRenderer.color = Color.white;
        }
        else
        {
            PlayerSpriteRenderer.color = Color.red;
        }
    }

    //
    IEnumerator ResetToDefaultTimed(EffectTypes effect)
    {
        //Wait for effect time
        yield return new WaitForSeconds(effectTime);

        ReturnToDefalut(effect);
    }

    public void ReturnToDefalut(EffectTypes effect)
    {
        //reset the applied effect after waiting for the duration
        switch (effect)
        {
            case EffectTypes.VISUAL_DISTORTION:
            case EffectTypes.DAMAGE:
                mainCameraBehavior.currMat = null;
                MobChaseRenderFeature.Instance?.DisableEffect();
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
