using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class NewRF : ScriptableRendererFeature
{
    class CustomPostProcessPass : ScriptableRenderPass
    {
        private Material _material;

        public CustomPostProcessPass(Material material)
        {
            _material = material;
            // ����������������Pass����Ⱦ�¼��������ڲ�͸����͸������֮��
            renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            // ��ȡһ��CommandBuffer����ִ����Ⱦ����
            CommandBuffer cmd = CommandBufferPool.Get("CustomPostProcessPass");

            // ��RenderingData��ȡ��ȾĿ���ʶ��
            RenderTargetIdentifier source = renderingData.cameraData.renderer.cameraColorTarget;

            // ������ʱ��ȾĿ��
            cmd.GetTemporaryRT(Shader.PropertyToID("_TempRenderTarget"), renderingData.cameraData.cameraTargetDescriptor);
            RenderTargetIdentifier temp = new RenderTargetIdentifier(Shader.PropertyToID("_TempRenderTarget"));

            // ����Դ������ʱ��ȾĿ�꣬Ӧ�ú������
            cmd.Blit(source, temp, _material);
            cmd.Blit(temp, source);

            // ִ������
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            // ������ʱ��ȾĿ��
            cmd.ReleaseTemporaryRT(Shader.PropertyToID("_TempRenderTarget"));

            // ����CommandBuffer
            CommandBufferPool.Release(cmd);
        }
    }

    // ����CustomPostProcessPassʵ���ı���
    CustomPostProcessPass customPostProcessPass;

    // ������ʹ����ĺ������
    public Material material;

    // ��Create�����д������Render Passʵ��
    public override void Create()
    {
        customPostProcessPass = new CustomPostProcessPass(material);
        // �������pass��ִ��ʱ��
        customPostProcessPass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    }

    // ��AddRenderPasses�����н����pass��ӵ�renderer�Ķ�����
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(customPostProcessPass);
    }
}

