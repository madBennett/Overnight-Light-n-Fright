using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AudioClipTypes
{
    WALK,
    FLASHLIGHT,
    HIT_WALL,
    EFFECT_APPLIED,
    ENTER_GATE,
    COLLECT_MARKER,
    GHOST_IDLE,
    GHOST_HUNT,
    GHOST_FLEE,
    GHOST_HIDE,
    GHOST_DAMAGE,
    NUM_EFFECTS
}
public class AudioManager : MonoBehaviour
{
    public float volume = 1f;

    private float timeBuffer = 1f;

    [SerializeField] private AudioSource defaultAudioSource;

    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();

    [SerializeField] private bool[] playingAudio = new bool[(int)AudioClipTypes.NUM_EFFECTS]; //bool map to verify which effect is curretnly active



    // Start is called before the first frame update
    void Start()
    {
        defaultAudioSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();

        //inmtialize map
        for (int i = 0; i < (int)AudioClipTypes.NUM_EFFECTS; i++)
        {
            playingAudio[i] = false;
        }
    }

    public void PlayAudio(AudioClipTypes audioClip, AudioSource audioSource = null)
    {
        if (!playingAudio[(int)audioClip])
        {
            playingAudio[(int)audioClip] = true;

            if (audioSource == null)
            {
                defaultAudioSource.PlayOneShot(audioClips[(int)audioClip], volume);
            }
            else
            {
                audioSource.PlayOneShot(audioClips[(int)audioClip], volume);
            }

        }

        StartCoroutine(RevertMap(audioClip));
    }

    private IEnumerator RevertMap(AudioClipTypes audioClip)
    {
        yield return new WaitForSeconds(audioClips[(int)audioClip].length);

        playingAudio[(int)audioClip] = false;
    }
}
