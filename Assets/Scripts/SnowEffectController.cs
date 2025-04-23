using UnityEngine;

[ExecuteInEditMode]
public class SnowEffectController : MonoBehaviour
{
    public Material snowMaterial;
    private int chasingGhostCount = 0;

    public void AddChasingGhost() => chasingGhostCount++;
    public void RemoveChasingGhost() => chasingGhostCount--;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (chasingGhostCount > 0 && snowMaterial != null)
        {
            Graphics.Blit(src, dest, snowMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}