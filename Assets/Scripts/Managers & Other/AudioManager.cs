using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum AudioClipTypes
{
    FLASHLIGHT,     // Element 0
    HIT_WALL,       // Element 1
    EFFECT_APPLIED, // Element 2
    ENTER_GATE,     // Element 3
    COLLECT_MARKER, // Element 4
    GHOST_IDLE,     // Element 5
    GHOST_HUNT,     // Element 6
    GHOST_FLEE,     // Element 7
    GHOST_HIDE,     // Element 8
    GHOST_DAMAGE,   // Element 9
    TEXT_BOOM,      // Element 10
    GHOST_CHASE,    // Element 11
    GHOST_DEATH,    // Element 12
    NUM_EFFECTS 
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public float volume = 1f;

    [SerializeField] private AudioSource defaultAudioSource;
    [SerializeField] private AudioSource walkingAudioSource;

    [SerializeField] private AudioSource ambientAudioSource;
    [SerializeField] private AudioClip ambientClip;

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
            //find all audio sources in scene
            AudioSource[] audioSources = player.GetComponentsInChildren<AudioSource>(true);

            foreach(AudioSource audioSource in audioSources)
            {
                audioSource.volume = volume;
                if (audioSource.name == "Default Audio Source")
                {
                    defaultAudioSource = audioSource;
                }
                else if (audioSource.name == "Walking Audio Source")
                {
                    walkingAudioSource = audioSource;
                }
            }

            walkingAudioSource.volume = volume;
        }

               // ambientAudioSource
        if (ambientAudioSource == null)
        {
            ambientAudioSource = gameObject.AddComponent<AudioSource>();
        }
        ambientAudioSource.clip = ambientClip;
        ambientAudioSource.loop = true;
        ambientAudioSource.volume = volume * 0.5f; // subtle background noise
        ambientAudioSource.playOnAwake = false;
        ambientAudioSource.Play();
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

    public void HandleWalkAudio(bool enable)
    {
        walkingAudioSource.enabled = enable;
    }

    public void PlayAudio(AudioClipTypes audioClip, AudioSource audioSource = null)
    {
        // TEXT_BOOM is allowed to overlap, so skip the playingAudio map
        if (audioClip == AudioClipTypes.TEXT_BOOM)
        {
            if (audioSource == null)
            {
                defaultAudioSource.PlayOneShot(audioClips[(int)audioClip], volume);
            }
            else
            {
                audioSource.PlayOneShot(audioClips[(int)audioClip], volume);
            }
            return;
        }

        // Other sounds still follow non-overlapping rule
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

            StartCoroutine(RevertMap(audioClip));
        }
    }

    private IEnumerator RevertMap(AudioClipTypes audioClip)
    {
        yield return new WaitForSeconds(audioClips[(int)audioClip].length);

        playingAudio[(int)audioClip] = false;
    }
}
