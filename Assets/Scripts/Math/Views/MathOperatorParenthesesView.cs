using MathFighter.GamePlay;
using UnityEngine;

namespace MathFighter.Math.Views
{
    public class MathOperatorParenthesesView : MathOperatorView
    {
        public MathOperatorParenthesesView(MathOperatorParenthesis op, float h, float spacing, Font font)
        {
            // get the texture for our parameter
            LogicalRenderTexture leftParamTexture = op.LeftParam.GenerateView(h, spacing, font).Texture;

            // adjust to the height of our parameter's visual representation so the parentheses will wrap it properly
            h = leftParamTexture.Height;

            // render the parentheses
            LogicalRenderTexture parenthesisLeft = DrawStringToTexture("(", h, font);
            LogicalRenderTexture parenthesisRight = DrawStringToTexture(")", h, font);

            // assemble all the textures into the final visual representation
            _texture = AssembleComponentTextures(spacing, parenthesisLeft, leftParamTexture, parenthesisRight);

            // clean up
            LRTPool.Instance.ReleaseLRT(leftParamTexture);
            LRTPool.Instance.ReleaseLRT(parenthesisLeft);
            LRTPool.Instance.ReleaseLRT(parenthesisRight);
        }

        protected LogicalRenderTexture AssembleComponentTextures(float spacing, LogicalRenderTexture parenthesisLeft, LogicalRenderTexture leftParamTexture, LogicalRenderTexture parenthesisRight)
        {
            // work out texture size
            float w = parenthesisLeft.Width + spacing + leftParamTexture.Width + spacing + parenthesisRight.Width;
            float h = leftParamTexture.Height;

            // create a render target
            LogicalRenderTexture lrt = LRTPool.Instance.AcquireLRT((int)w, (int)h);

            // setup the device stuff
            // TorqueEngineComponent.Instance.Game.GraphicsDevice.SetRenderTarget(0, lrt.RenderTarget);
            // TorqueEngineComponent.Instance.Game.GraphicsDevice.Clear(ClearOptions.Target, Color.TransparentBlack, 0.0f, 0);

            // setup the spritebatch
            // SpriteBatch spriteBatch = Game.SpriteBatch;// new SpriteBatch(// TorqueEngineComponent.Instance.Game.GraphicsDevice);

            // render everything to texture
            float posX = 0.0f;

            // spriteBatch.Begin();
            lrt.Draw(parenthesisLeft.Texture, new Vector2(posX, (h - parenthesisLeft.Height) * 0.5f), parenthesisLeft.Region, Color.white, 0.0f, Vector2.zero, 1.0f, 0.0f);
            posX += parenthesisLeft.Width + spacing;
            lrt.Draw(leftParamTexture.Texture, new Vector2(posX, (h - leftParamTexture.Height) * 0.5f), leftParamTexture.Region, Color.white, 0.0f, Vector2.zero, 1.0f, 0.0f);
            posX += leftParamTexture.Width + spacing;
            lrt.Draw(parenthesisRight.Texture, new Vector2(posX, (h - parenthesisRight.Height) * 0.5f), parenthesisRight.Region, Color.white, 0.0f, Vector2.zero, 1.0f, 0.0f);
            // spriteBatch.End();

            // reset device stuff
            // TorqueEngineComponent.Instance.Game.GraphicsDevice.SetRenderTarget(0, null);

            // resolve and return the newly rendered texture
            // lrt.ResolveTexture();
            return lrt;
        }
    }
}
