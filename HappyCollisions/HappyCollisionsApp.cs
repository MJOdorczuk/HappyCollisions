using HappyCollisions.Forms;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.DXGI;
using SharpDX.Windows;
using System;
using D3D11 = SharpDX.Direct3D11;
using D2D1 = SharpDX.Direct2D1;

namespace HappyCollisions
{
    class HappyCollisionsApp : IDisposable
    {
        private readonly int maxWidth;
        private readonly int maxHeight;
        private readonly MainForm mainForm = new MainForm();
        private readonly D3D11.Device d3dDevice;
        private readonly D3D11.DeviceContext d3dDeviceContext;
        private readonly SwapChain swapChain;
        private readonly D3D11.RenderTargetView renderTargetView;
        private readonly Vector3[] vertices = new Vector3[]
        {
            new Vector3(-0.5f, 0.5f, 0.0f),
            new Vector3(0.5f, 0.5f, 0.0f),
            new Vector3(0.0f, -0.5f, 0.0f)
        };
        private D3D11.Buffer triangleVertexBuffer;

        public HappyCollisionsApp(int maxWidth, int maxHeight)
        {
            this.maxWidth = maxWidth;
            this.maxHeight = maxHeight;
            var backBufferDescription = new ModeDescription(maxWidth, 
                                                        maxHeight, 
                                                        new Rational(60, 1), 
                                                        Format.R8G8B8A8_UNorm);
            var swapChainDescription = new SwapChainDescription()
            {
                ModeDescription = backBufferDescription,
                SampleDescription = new SampleDescription(1, 0),
                Usage = Usage.RenderTargetOutput,
                BufferCount = 1,
                OutputHandle = mainForm.Handle,
                IsWindowed = true
            };
            D3D11.Device.CreateWithSwapChain(DriverType.Hardware,
                                             D3D11.DeviceCreationFlags.None,
                                             swapChainDescription,
                                             out d3dDevice,
                                             out swapChain);
            d3dDeviceContext = d3dDevice.ImmediateContext;
            using (D3D11.Texture2D backBuffer = swapChain.GetBackBuffer<D3D11.Texture2D>(0))
            {
                renderTargetView = new D3D11.RenderTargetView(d3dDevice, backBuffer);
            }
            InitializeTriangle();
        }

        public void Dispose()
        {
            triangleVertexBuffer.Dispose();
            renderTargetView.Dispose();
            swapChain.Dispose();
            d3dDevice.Dispose();
            d3dDeviceContext.Dispose();
            mainForm.Dispose();
        }

        public void Run()
        {
            RenderLoop.Run(mainForm, RenderCallback);
        }

        private void RenderCallback()
        {
            Draw();
        }

        private void Draw()
        {
            d3dDeviceContext.OutputMerger.SetRenderTargets(renderTargetView);
            d3dDeviceContext.ClearRenderTargetView(renderTargetView, new SharpDX.Color(32, 103, 178));
            swapChain.Present(1, PresentFlags.None);
        }

        private void InitializeTriangle()
        {
            triangleVertexBuffer = D3D11.Buffer.Create<Vector3>(d3dDevice, 
                                                                D3D11.BindFlags.VertexBuffer, 
                                                                vertices);
        }
    }
}
