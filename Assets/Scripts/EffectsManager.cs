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
    [SerializeField] private bool[] appliedEffects = new bool[(int)EffectTypes.NUM_EFFECTS]; //bool map to verify which effect is curretnly active

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
        mainCameraBehavior = Camera.main.GetComponent<CameraBehavior>();

        //inmtialize map
        for (int i = 0; i < (int)EffectTypes.NUM_EFFECTS; i++)
        {
            appliedEffects[i] = false;
        }
    }

    public void ApplyEffect(EffectTypes effect, VisualTypes visEffect = VisualTypes.SNOW, int damageAmt = 1)
    {
        if (!appliedEffects[(int)effect])
        {
            switch (effect)
            {
                case EffectTypes.VISUAL_DISTORTION:
                    mainCameraBehavior.currMat = (visualMaterials[(int)visEffect]);
                    break;
                case EffectTypes.REVERSE_CONTROLS:
                    PlayerControls.currMoveState = MovementStates.REVERSE;
                    break;
                case EffectTypes.STUN:
                    PlayerControls.currMoveState = MovementStates.STUN;
                    break;
                case EffectTypes.DAMAGE:
                    Player.ChangePlayerHealth(-1 * damageAmt);
                    break;
            }

            appliedEffects[(int)effect] = true;

            StartCoroutine(ResetToDefault(effect));
        }
        

    }

    //
    IEnumerator ResetToDefault(EffectTypes effect)
    {
        //Wait for effect time
        yield return new WaitForSeconds(effectTime);

        //reset the applied effect after waiting for the duration
        switch (effect)
        {
            case EffectTypes.VISUAL_DISTORTION:
                mainCameraBehavior.currMat = null;
                break;
            case EffectTypes.REVERSE_CONTROLS:
            case EffectTypes.STUN:
                PlayerControls.currMoveState = MovementStates.DEFAULT; break;
        }

        appliedEffects[(int)effect] = false;
    }
}
