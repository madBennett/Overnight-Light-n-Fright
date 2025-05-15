using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraEffectController : MonoBehaviour
{
    public Material jitterMaterial; // Your JitterEffect material
    private Material currentMaterial;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (currentMaterial != null)
        {
            Graphics.Blit(src, dest, currentMaterial);
        }
        else
        {
            Graphics.Blit(src, dest); // Default rendering (no effect)
        }
    }

    public void ApplyJitter(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ApplyJitterTemporarily(duration));
    }

    private System.Collections.IEnumerator ApplyJitterTemporarily(float duration)
    {
        currentMaterial = jitterMaterial;
        yield return new WaitForSeconds(duration);
        currentMaterial = null; // Return to normal rendering
    }
}
