using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MobChaseRenderFeature : ScriptableRendererFeature
{
    class MobChasePass : ScriptableRenderPass
    {
        public Material chaseMaterial;
        public bool effectEnabled = false;

        private RenderTargetIdentifier source;
        private RenderTargetHandle tempTexture;

        public void Setup(RenderTargetIdentifier src)
        {
            source = src;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            tempTexture.Init("_TempMobChaseTexture");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (chaseMaterial == null || !effectEnabled)
                return;

            CommandBuffer cmd = CommandBufferPool.Get("MobChaseEffect");

            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.depthBufferBits = 0;

            cmd.GetTemporaryRT(tempTexture.id, opaqueDesc, FilterMode.Bilinear);

            // Blit from source to temp with effect
            Blit(cmd, source, tempTexture.Identifier(), chaseMaterial);
            // Blit back to source
            Blit(cmd, tempTexture.Identifier(), source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            if (tempTexture != RenderTargetHandle.CameraTarget)
            {
                cmd.ReleaseTemporaryRT(tempTexture.id);
            }
        }
    }

    MobChasePass m_ScriptablePass;

    public Material chaseMaterial; // Drag your material here in the inspector

    public static MobChaseRenderFeature Instance; // 🔥 Important for access from scripts!

    public override void Create()
    {
        m_ScriptablePass = new MobChasePass();
        m_ScriptablePass.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        Instance = this; // make it globally accessible
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (chaseMaterial == null)
            return;

        m_ScriptablePass.chaseMaterial = chaseMaterial;
        m_ScriptablePass.Setup(renderer.cameraColorTarget);
        renderer.EnqueuePass(m_ScriptablePass);
    }

    // 🔥 Helper functions you can call easily:
    public void EnableEffect()
    {
        m_ScriptablePass.effectEnabled = true;
    }

    public void DisableEffect()
    {
        m_ScriptablePass.effectEnabled = false;
    }
}
