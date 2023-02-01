using MathFighter.GamePlay;
using UnityEngine;

namespace MathFighter.Math.Views
{
    public class MathOperatorExponentView : MathOperatorView
    {
        private const float SUPERSCRIPT_SCALE = 0.6f;


        public MathOperatorExponentView(MathOperatorExponent op, float h, float spacing, Font font)
        {
            // get the textures for our parameters - we'll make the right one a bit smaller as we will be superscripting it
            LogicalRenderTexture leftParamTexture = op.LeftParam.GenerateView(h, spacing, font).Texture;
            LogicalRenderTexture rightParamTexture = op.RightParam.GenerateView(h * SUPERSCRIPT_SCALE, spacing, font).Texture;

            // assemble all the textures into the final visual representation
            _texture = AssembleComponentTextures(spacing * SUPERSCRIPT_SCALE, leftParamTexture, rightParamTexture);

            // cleanup
            LRTPool.Instance.ReleaseLRT(leftParamTexture);
            LRTPool.Instance.ReleaseLRT(rightParamTexture);
        }

        protected LogicalRenderTexture AssembleComponentTextures(float spacing, LogicalRenderTexture leftParamTexture, LogicalRenderTexture rightParamTexture)
        {
            // work out texture size
            float w = leftParamTexture.Width + spacing + rightParamTexture.Width;
            float h = leftParamTexture.Height;

            // create a render target
            LogicalRenderTexture lrt = LRTPool.Instance.AcquireLRT((int)w, (int)h);

            // setup the device stuff
            // TorqueEngineComponent.Instance.Game.GraphicsDevice.SetRenderTarget(0, lrt.RenderTarget);
            // TorqueEngineComponent.Instance.Game.GraphicsDevice.Clear(ClearOptions.Target, Color.TransparentBlack, 0.0f, 0);

            // setup the spritebatch
            // SpriteBatch spriteBatch = Game.SpriteBatch;// new SpriteBatch(TorqueEngineComponent.Instance.Game.GraphicsDevice);

            // render everything to texture
            float posX = 0.0f;

            // spriteBatch.Begin();
            lrt.Draw(leftParamTexture.Texture, new Vector2(posX, (h - leftParamTexture.Height) * 0.5f), leftParamTexture.Region, Color.white, 0.0f, Vector2.zero, 1.0f, 0.0f);
            posX += leftParamTexture.Width + spacing;
            lrt.Draw(rightParamTexture.Texture, new Vector2(posX, 0.0f), rightParamTexture.Region, Color.white, 0.0f, Vector2.zero, 1.0f, 0.0f);
            // spriteBatch.End();

            // reset device stuff
            // TorqueEngineComponent.Instance.Game.GraphicsDevice.SetRenderTarget(0, null);

            // resolve and return the newly rendered texture
            // lrt.ResolveTexture();
            return lrt;
        }
    }
}
