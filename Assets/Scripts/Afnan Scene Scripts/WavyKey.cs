using UnityEngine;

public class WavyKey : MonoBehaviour
{
    public float waveSpeed = 2f;
    public float waveHeight = 0.5f;

    private Vector3 startPos;
    private bool movingRight = true;
    private float moveSpeed = 1f;

    public static bool keyCollected = false;

    [Header("Optional Target")]
    public GameObject objectToHideWhenCollected; // ✅ Assign a sprite you want to disappear

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float waveY = Mathf.Sin(Time.time * waveSpeed) * waveHeight;
        float direction = movingRight ? 1 : -1;
        transform.position += new Vector3(direction * moveSpeed * Time.deltaTime, waveY * Time.deltaTime, 0);

        if (transform.position.x > startPos.x + 2f) movingRight = false;
        if (transform.position.x < startPos.x - 2f) movingRight = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            keyCollected = true;

            if (objectToHideWhenCollected != null)
            {
                objectToHideWhenCollected.SetActive(false); // ✅ Hide the object
            }

            Destroy(gameObject); // Remove the key
            Debug.Log("Key collected!");
        }
    }
}
