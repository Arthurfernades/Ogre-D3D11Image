using org.ogre;
using System;
using System.Windows;
using System.Windows.Interop;

namespace OgreEngine
{
    public partial class Ogre : Microsoft.Wpf.Interop.DirectX.D3D11Image
    {
        private RenderTarget renderTarget;

        private TexturePtr texturePtr;

        private Viewport viewport;

        public int TextureWidth {  get; set; }

        public int TextureHeight { get; set; }

        public void CreateTexture9()
        {
            texturePtr = TextureManager.getSingleton().createManual(
                            "Ogre Render",
                            ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                            TextureType.TEX_TYPE_2D,
                            (uint)TextureWidth,
                            (uint)TextureHeight,
                            32,
                            0,
                            PixelFormat.PF_R8G8B8A8,
                            0x20);
        }

        public void CreateTexture11(IntPtr surface)
        {
            texturePtr = TextureManager.getSingleton().createManualWithSurface(
                            "Ogre Render",
                            ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                            TextureType.TEX_TYPE_2D_WITH_SURFACE,
                            (uint)TextureWidth,
                            (uint)TextureHeight,
                            32,
                            0,
                            PixelFormat.PF_B8G8R8A8,
                            0x20,
                            surface);
        }

        public void InitRenderTarget()
        {
            renderTarget = texturePtr.getBuffer().getRenderTarget();

            //Transparent Background
            renderTarget.removeAllViewports();
            viewport = renderTarget.addViewport(cam);
            viewport.setBackgroundColour(new ColourValue(0f, 0f, 0f, 0f));
            viewport.setClearEveryFrame(true);
            viewport.setOverlaysEnabled(false);
        }

        public void DisposeRenderTarget()
        {
            if (renderTarget != null)
            {
                renderTarget.removeAllListeners();
                renderTarget.removeAllViewports();
                renderSystem.destroyRenderTarget(renderTarget.getName());
                renderTarget = null;
            }

            if (texturePtr != null)
            {
                TextureManager.getSingleton().remove(texturePtr.getHandle());
                GC.SuppressFinalize(texturePtr);
                texturePtr = null;
            }
        }

        //Only DirectX 9
        public void AttachRenderTarget()
        {
            IntPtr surface = IntPtr.Zero;

            Lock();
            try
            {
                renderTarget.getCustomAttribute("DDBACKBUFFER", out surface);
                SetBackBuffer(D3DResourceType.IDirect3DSurface9, surface, true);
            }            
            finally
            {
                Unlock();
            }
        }

        public void DetachRenderTarget()
        {
            Lock();
            SetBackBuffer(D3DResourceType.IDirect3DSurface9, IntPtr.Zero);
            Unlock();
        }

        //Only DirectX 11
        public void DoFlush()
        {
            renderTarget.doFlush();
        }

        public void ReInitRendering11()
        {

        }

        public void ReInitRendering9()
        {
            DetachRenderTarget();
            DisposeRenderTarget();
            CreateTexture9();
            InitRenderTarget();
            AttachRenderTarget();
        }

        public void RenderOneFrame()
        {
            if (root != null)
            {
                root.renderOneFrame();
            }

            if (DX == Engine.DX9)
            {
                Lock();
                AddDirtyRect(new Int32Rect(0, 0, (int)TextureWidth, (int)TextureHeight));
                Unlock();
            }
        }

        public void DisposeOgre()
        {
            DisposeRenderTarget();
            root.queueEndRendering();
            CompositorManager.getSingleton().removeAll();
            GC.Collect();
        }

        public void SaveRender()
        {
            string vArquivo = @"C:\Users\Admin\Pictures\output_new.png";

            //Escreve o arquivo no disco
            renderTarget.writeContentsToFile(vArquivo);

        }
    }
}
