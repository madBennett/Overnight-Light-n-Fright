using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySceneReset : MonoBehaviour
{
    void Start()
    {
        EffectsManager effectsManager = FindObjectOfType<EffectsManager>();
        if (effectsManager != null)
        {
            effectsManager.ReturnToDefalut(EffectTypes.VISUAL_DISTORTION);
        }
    }
}

