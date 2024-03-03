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
            // 这里你可以设置这个Pass的渲染事件（比如在不透明或透明物体之后）
            renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            // 获取一个CommandBuffer用来执行渲染命令
            CommandBuffer cmd = CommandBufferPool.Get("CustomPostProcessPass");

            // 从RenderingData获取渲染目标标识符
            RenderTargetIdentifier source = renderingData.cameraData.renderer.cameraColorTarget;

            // 设置临时渲染目标
            cmd.GetTemporaryRT(Shader.PropertyToID("_TempRenderTarget"), renderingData.cameraData.cameraTargetDescriptor);
            RenderTargetIdentifier temp = new RenderTargetIdentifier(Shader.PropertyToID("_TempRenderTarget"));

            // 复制源纹理到临时渲染目标，应用后处理材质
            cmd.Blit(source, temp, _material);
            cmd.Blit(temp, source);

            // 执行命令
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            // 回收临时渲染目标
            cmd.ReleaseTemporaryRT(Shader.PropertyToID("_TempRenderTarget"));

            // 回收CommandBuffer
            CommandBufferPool.Release(cmd);
        }
    }

    // 创建CustomPostProcessPass实例的变量
    CustomPostProcessPass customPostProcessPass;

    // 在这里使用你的后处理材质
    public Material material;

    // 在Create方法中创建你的Render Pass实例
    public override void Create()
    {
        customPostProcessPass = new CustomPostProcessPass(material);
        // 设置你的pass的执行时机
        customPostProcessPass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    }

    // 在AddRenderPasses方法中将你的pass添加到renderer的队列中
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(customPostProcessPass);
    }
}

