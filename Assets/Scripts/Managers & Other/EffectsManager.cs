using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private AudioManager AM;

    [SerializeField] private float effectTime = 2f;
    [SerializeField] private bool switchMode = false;
    [SerializeField] private bool[] appliedEffects = new bool[(int)EffectTypes.NUM_EFFECTS]; //bool map to verify which effect is curretnly active

    //color effects
    [SerializeField] private SpriteRenderer PlayerSpriteRenderer;
    [SerializeField] private Image UserStatusImage;
    [SerializeField] private Material noiseMaterial;

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
        AM = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        UserStatusImage = GameObject.FindGameObjectWithTag("UserStatusImage").GetComponent<Image>();

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
            AM.PlayAudio(AudioClipTypes.EFFECT_APPLIED);

            switch (effect)
            {
                case EffectTypes.VISUAL_DISTORTION:
                    mainCameraBehavior.currMat = (visualMaterials[(int)visEffect]);
                    MobChaseRenderFeature.Instance?.EnableEffect();
                    UserStatusImage.material = noiseMaterial;
                    break;
                case EffectTypes.REVERSE_CONTROLS:
                    PlayerControls.currMoveState = MovementStates.REVERSE;
                    UserStatusImage.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                    break;
                case EffectTypes.STUN:
                    PlayerControls.currMoveState = MovementStates.STUN;
                    UserStatusImage.color = Color.cyan;
                    break;
                case EffectTypes.DAMAGE:
                    mainCameraBehavior.currMat = (visualMaterials[(int)visEffect]);
                    MobChaseRenderFeature.Instance?.EnableEffect();
                    UserStatusImage.color = Color.red;
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
                UserStatusImage.material = null;
                mainCameraBehavior.currMat = null;
                MobChaseRenderFeature.Instance?.DisableEffect();
                break;
            case EffectTypes.DAMAGE:
                UserStatusImage.color = Color.white;
                mainCameraBehavior.currMat = null;
                MobChaseRenderFeature.Instance?.DisableEffect();
                break;
            case EffectTypes.REVERSE_CONTROLS:
                UserStatusImage.rectTransform.rotation = Quaternion.Euler(Vector3.zero);
                PlayerControls.currMoveState = MovementStates.DEFAULT;
                break;
            case EffectTypes.STUN:
                PlayerControls.currMoveState = MovementStates.DEFAULT;
                UserStatusImage.color = Color.white;
                break;
        }

        ChangePlayerColor(EffectTypes.DEFAULT);
        appliedEffects[(int)effect] = false;
    }
}
