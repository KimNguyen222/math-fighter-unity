using MathFighter.GamePlay;
using UnityEngine;

namespace MathFighter.Math.Views
{
    /// <summary>
    /// Base class for the view of a math element
    /// </summary>
    public abstract class MathElementView
    {
        //protected static Vector4 _backgroundColor;
        protected LogicalRenderTexture _texture;

        public LogicalRenderTexture Texture
        {
            get { return _texture; }
        }

        //// call this to setup render settings such as the surface format that will be used
        //public static void InitializeRenderSettings(Vector4 backgroundColor)
        //{
        //    _backgroundColor = backgroundColor;
        //}
        
        public static Vector3 GetTextSize(string text, float h, Font font)
        {
            if (text == null && text.Length == 0)
                Debug.Log("Trying to create a math expression element view without any content");

            //Vector2 textSize = font.MeasureString(text);
            GUIStyle s_guiStyle = new GUIStyle();
            s_guiStyle.font = font;
            Vector2 textSize = s_guiStyle.CalcSize(new GUIContent(text));

            float textScale = h / textSize.y;

            return new Vector3(textSize.x * textScale, textSize.y * textScale, textScale);
        }

        // rendering one element at a time is not the most efficient way to do this stuff, but
        // we only need to generate the math expression visual representation once and it
        // won't change after that, so we'll just keep things simple.
        public static LogicalRenderTexture DrawStringToTexture(string text, float h, Font font)
        {
            // work out how much space the text will take up when rendered
            Vector3 textSize = GetTextSize(text, h, font);
            
            // create a render target
            LogicalRenderTexture lrt = LRTPool.Instance.AcquireLRT((int)textSize.x, (int)textSize.y);

            // setup the device stuff
            // TorqueEngineComponent.Instance.Game.GraphicsDevice.SetRenderTarget(0, lrt.RenderTarget);
            // TorqueEngineComponent.Instance.Game.GraphicsDevice.Clear(ClearOptions.Target, Color.TransparentBlack, 0.0f, 0);

            // setup the spritebatch
            // SpriteBatch spriteBatch = Game.SpriteBatch;// new SpriteBatch(TorqueEngineComponent.Instance.Game.GraphicsDevice);

            // render text to texture
            // spriteBatch.Begin();
            lrt.DrawString(font, text, Vector2.zero, Color.white, 0.0f, Vector2.zero, textSize.z, 0.0f);
            // spriteBatch.End();

            // reset device stuff
            // TorqueEngineComponent.Instance.Game.GraphicsDevice.SetRenderTarget(0, null);

            // resolve and return the newly rendered texture
            // lrt.ResolveTexture();
            return lrt;
        }
    }
}
