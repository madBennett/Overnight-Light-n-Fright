using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public static AudioManager Instance;

    public float volume = 1f;

    private float timeBuffer = 1f;

    [SerializeField] private AudioSource defaultAudioSource;

    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();

    [SerializeField] private bool[] playingAudio = new bool[(int)AudioClipTypes.NUM_EFFECTS]; //bool map to verify which effect is curretnly active

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make this object persistent
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reacquire the player and its AudioSource
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            defaultAudioSource = player.GetComponent<AudioSource>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
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
