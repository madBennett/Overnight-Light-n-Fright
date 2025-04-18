using UnityEngine;

[ExecuteInEditMode]
public class SnowEffectController : MonoBehaviour
{
    public Material snowMaterial;
    public bool isActive = false;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (isActive && snowMaterial != null)
        {
            Graphics.Blit(src, dest, snowMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}