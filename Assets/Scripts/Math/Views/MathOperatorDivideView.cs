using MathFighter.GamePlay;
using UnityEngine;


namespace MathFighter.Math.Views
{
    public class MathOperatorDivideFractionView : MathOperatorView
    {
        public MathOperatorDivideFractionView(MathOperatorDivide op, float h, float spacing, Font font)
        {
            // get the textures for our parameters
            LogicalRenderTexture leftParamTexture = op.LeftParam.GenerateView(h, spacing, font).Texture;
            LogicalRenderTexture rightParamTexture = op.RightParam.GenerateView(h, spacing, font).Texture;

            // workout how wide the horizontal divider line needs to be
            float w;

            if (rightParamTexture.Width > leftParamTexture.Width)
            {
                w = rightParamTexture.Width;
            }
            else
            {
                w = leftParamTexture.Width;
            }

            w *= 1.25f; // extend the line a bit further

            // make our horizontal line texture
            LogicalRenderTexture opTexture = DrawOperatorToTexture(w, h, font);

            // assemble all the textures into the final visual representation
            _texture = AssembleComponentTextures(w, leftParamTexture.Height + rightParamTexture.Height + (h * 0.25f), opTexture, leftParamTexture, rightParamTexture);

            // cleanup
            LRTPool.Instance.ReleaseLRT(leftParamTexture);
            LRTPool.Instance.ReleaseLRT(rightParamTexture);
            LRTPool.Instance.ReleaseLRT(opTexture);
        }

        protected LogicalRenderTexture DrawOperatorToTexture(float w, float h, Font font)
        {
            // render the three parts of the long division symbol (the ascii symbols used here have been replaced in the font by the art we need)
            LogicalRenderTexture leftCap = DrawStringToTexture("&", h, font);
            LogicalRenderTexture middle = DrawStringToTexture("}", h, font);
            LogicalRenderTexture rightCap = DrawStringToTexture("~", h, font);


            // the operator symbol characters will have been antialiased when they were rendered by DrawStringg() so need to trim off the left/right ends or we will get artifacts (looking like gaps) when trying to stitch them together seamlessly
            GUIStyle s_guiStyle = new GUIStyle();
            s_guiStyle.font = font;
            Vector2 textSize = s_guiStyle.CalcSize(new GUIContent("@"));

            //int trim = (int)System.Math.Ceiling(h / font.MeasureString("@").Y);
            int trim = (int)System.Math.Ceiling(h / textSize.y);

            // create a render target
            LogicalRenderTexture lrt = LRTPool.Instance.AcquireLRT((int)w, (int)h);

            // setup the device stuff
            // TorqueEngineComponent.Instance.Game.GraphicsDevice.SetRenderTarget(0, lrt.RenderTarget);
            // TorqueEngineComponent.Instance.Game.GraphicsDevice.Clear(ClearOptions.Target, Color.TransparentBlack, 0.0f, 0);

            // setup the spritebatch
            // SpriteBatch spriteBatch = Game.SpriteBatch;// new SpriteBatch(TorqueEngineComponent.Instance.Game.GraphicsDevice);

            // render the 3 parts of the fraction division line (the middle part is stretched and the ends are just capping it)
            int middleWidth = (int)w - leftCap.Width - rightCap.Width + trim * 6;

            if (middleWidth < 1)
            {
                middleWidth = 1;
            }

            // spriteBatch.Begin();
            lrt.Draw(leftCap.Texture, new Rect(0, 0, leftCap.Width - trim * 2, leftCap.Height), new Rect(leftCap.Region.x + trim, leftCap.Region.y, leftCap.Region.width - trim * 2, leftCap.Region.height), Color.white);
            lrt.Draw(middle.Texture, new Rect(leftCap.Width - trim * 2, 0, middleWidth, middle.Height), new Rect(middle.Region.x + trim, middle.Region.y, middle.Region.width - trim * 2, middle.Region.height), Color.white);
            lrt.Draw(rightCap.Texture, new Rect((int)w - (rightCap.Width - trim * 2), 0, rightCap.Width - trim * 2, rightCap.Height), new Rect(rightCap.Region.x + trim, rightCap.Region.y, rightCap.Region.width - trim * 2, rightCap.Region.height), Color.white);
            // spriteBatch.End();

            // reset device stuff
            // TorqueEngineComponent.Instance.Game.GraphicsDevice.SetRenderTarget(0, null);

            // cleanup
            LRTPool.Instance.ReleaseLRT(leftCap);
            LRTPool.Instance.ReleaseLRT(middle);
            LRTPool.Instance.ReleaseLRT(rightCap);

            // resolve and return the newly rendered texture
            // lrt.ResolveTexture();
            return lrt;
        }

        protected LogicalRenderTexture AssembleComponentTextures(float w, float h, LogicalRenderTexture opTexture, LogicalRenderTexture leftParamTexture, LogicalRenderTexture rightParamTexture)
        {
            // create a render target
            LogicalRenderTexture lrt = LRTPool.Instance.AcquireLRT((int)w, (int)h);

            // setup the device stuff
            // TorqueEngineComponent.Instance.Game.GraphicsDevice.SetRenderTarget(0, lrt.RenderTarget);
            // TorqueEngineComponent.Instance.Game.GraphicsDevice.Clear(ClearOptions.Target, Color.TransparentBlack, 0.0f, 0);

            // setup the spritebatch
            // SpriteBatch spriteBatch = Game.SpriteBatch;// new SpriteBatch(TorqueEngineComponent.Instance.Game.GraphicsDevice);

            // render everything to texture
            // spriteBatch.Begin();
                lrt.Draw(opTexture.Texture, new Vector2(0.0f, (float)leftParamTexture.Height), opTexture.Region, Color.white, 0.0f, Vector2.zero, 1.0f, 0.5f);
                lrt.Draw(leftParamTexture.Texture, new Vector2((w - (float)leftParamTexture.Width) * 0.5f, 0.0f), leftParamTexture.Region, Color.white, 0.0f, Vector2.zero, 1.0f, 0.0f);
                lrt.Draw(rightParamTexture.Texture, new Vector2((w - (float)rightParamTexture.Width) * 0.5f, h - (float)rightParamTexture.Height), rightParamTexture.Region, Color.white, 0.0f, Vector2.zero, 1.0f, 0.0f);
            // spriteBatch.End();

            // reset device stuff
            // TorqueEngineComponent.Instance.Game.GraphicsDevice.SetRenderTarget(0, null);

            // resolve and return the newly rendered texture
            // lrt.ResolveTexture();
            return lrt;
        }
    }
}
