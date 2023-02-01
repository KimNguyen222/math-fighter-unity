using UnityEngine;
using UnityEngine.U2D;

namespace MathFighter.GamePlay
{
    /// <summary>
    /// A logical render texture is a light wrapper for a RenderTarget2D and its associated Texture2D.
    /// What the LRT adds is the ability to remember a 'logical' region in the actual texture so that
    /// we can easily render and use textures that are smaller than the actual texture being used.  This
    /// is needed when reusing rendertargets in constructing MathExpression visual representations, for 
    /// example, because many arbitrary sized RTs are needed which would otherwise require a potentially
    /// very large pool of RTs - using LRTs we can keep the pool to a reasonable size because we only need
    /// a limited number of fixed size RTs instead of an unspecified number of arbitrarliy sized RTs.
    /// </summary>
    public class LogicalRenderTexture
    {
        //private RenderTarget2D _renderTarget;
        private Texture2D _texture;
        private Rect _region;
        private int _widthIndex;
        private int _heightIndex;

        //public RenderTarget2D RenderTarget
        //{
        //    get { return _renderTarget; }
        //}

        public Texture2D Texture
        {
            get { return _texture; }
        }

        public Rect Region
        {
            get { return _region; }
        }

        /// <summary>
        /// Returns the logical width
        /// </summary>
        public int Width
        {
            get { return (int)_region.width; }
        }

        /// <summary>
        /// Returns the logical height
        /// </summary>
        public int Height
        {
            get { return (int)_region.height; }
        }

        public int WidthIndex
        {
            get { return _widthIndex; }
        }

        public int HeightIndex
        {
            get { return _heightIndex; }
        }


        /// <summary>
        /// Specifies width by power instead of actual width as this is internally faster and also less
        /// prone to errors since only power of 2 width and heights are acceptable.  (note: width and height
        /// do not need to be the same value, however).
        /// </summary>
        /// <param name="widthPower2">The power to raise 2 to, in order to calculate the width</param>
        /// <param name="heightPower2">The power to raise 2 to, in order to calculate the height</param>
        public LogicalRenderTexture(int widthPower2, int heightPower2)
        {
            _widthIndex = widthPower2 - LRTPool.MIN_WIDTHPOWER;
            _heightIndex = heightPower2 - LRTPool.MIN_HEIGHTPOWER;

            int width = 1 << widthPower2;
            int height = 1 << heightPower2;

            //_renderTarget = new RenderTarget2D(Game.Instance.GraphicsDevice, width, height, 1, SurfaceFormat.Color);
            _region = new Rect(0, 0, width, height);
        }

        /// <summary>
        /// For code that needs to work with LRT instances, but when no actual pooled LRT is involved, we
        /// can create an LRT that doesn't have a render target.
        /// </summary>
        /// <param name="texture"></param>
        public LogicalRenderTexture(Texture2D texture)
        {
            _region.width = texture.width;
            _region.height = texture.height;
            _texture = texture;
        }

        public void SetLogicalSize(int logicalWidth, int logicalHeight)
        {
            _region.width = logicalWidth;
            _region.height = logicalHeight;
        }
        public void Draw(Texture2D sourceTexture, Vector2 position, Rect sourceRectangle, Color color,
            float rotation, Vector2 origin, float scale, float layerDepth)
        {
            Texture2D newTex = new Texture2D(_texture.width, _texture.height, _texture.format, false);

            int startX = (int)position.x;
            int endX = (int)(position.x + sourceRectangle.width);
            int startY = (int)position.y;
            int endY = (int)(position.y + sourceRectangle.height);


            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    if (x >= startX && y >= startY && x < origin.x && y < origin.y)
                    {
                        Color bgColor = _texture.GetPixel(x, y);
                        Color wmColor = sourceTexture.GetPixel(x - startX, y - startY);

                        Color final_color = Color.Lerp(bgColor, wmColor, wmColor.a / 1.0f);

                        newTex.SetPixel(x, y, final_color);
                    }
                    else
                        newTex.SetPixel(x, y, _texture.GetPixel(x, y));
                }
            }

            newTex.Apply();
            _texture = newTex;

        }
        public void Draw(Texture2D sourceTexture, Vector2 position, Rect sourceRectangle, Color color)
        {
            Texture2D newTex = new Texture2D(_texture.width, _texture.height, _texture.format, false);

            int startX = (int)position.x;
            int endX = (int)(position.x + sourceRectangle.width);
            int startY = (int)position.y;
            int endY = (int)(position.y + sourceRectangle.height);


            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    if (x >= startX && y >= startY && x < sourceRectangle.x && y < sourceRectangle.y)
                    {
                        Color bgColor = _texture.GetPixel(x, y);
                        Color wmColor = sourceTexture.GetPixel(x - startX, y - startY);

                        Color final_color = Color.Lerp(bgColor, wmColor, wmColor.a / 1.0f);

                        newTex.SetPixel(x, y, final_color);
                    }
                    else
                        newTex.SetPixel(x, y, _texture.GetPixel(x, y));
                }
            }

            newTex.Apply();
            _texture = newTex;

        }
        public void Draw(Texture2D sourceTexture, Rect destinationRectangle, Rect sourceRectangle, Color color)
        {
            Texture2D newTex = new Texture2D(_texture.width, _texture.height, _texture.format, false);

            int startX = (int)destinationRectangle.x;
            int endX = (int)(destinationRectangle.x + destinationRectangle.width);
            int startY = (int)destinationRectangle.y;
            int endY = (int)(destinationRectangle.y + destinationRectangle.height);


            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    if (x >= startX && y >= startY && x < sourceTexture.width && y < sourceTexture.height)
                    {
                        Color bgColor = _texture.GetPixel(x, y);
                        Color wmColor = sourceTexture.GetPixel(x - startX, y - startY);

                        Color final_color = Color.Lerp(bgColor, wmColor, wmColor.a / 1.0f);

                        newTex.SetPixel(x, y, final_color);
                    }
                    else
                        newTex.SetPixel(x, y, _texture.GetPixel(x, y));
                }
            }

            newTex.Apply();
            _texture = newTex;
        }
        public void DrawString(Font font, string text, Vector2 position, Color color, float rotation, 
            Vector2 origin, float scale, float layerDepth)
        {
            CharacterInfo ci;
            char[] cText = text.ToCharArray();

            Material fontMat = font.material;
            Texture2D fontTx = (Texture2D)fontMat.mainTexture;
            int x, y, w, h;
            int posX = 10;

            for (int i = 0; i < cText.Length; i++)
            {
                bool isCharacter = font.GetCharacterInfo(cText[i], out ci);

                x = (int)((float)fontTx.width * ci.uvBottomLeft.x);
                y = (int)((float)fontTx.height * ci.uvTopLeft.y);
                w = (int)((float)fontTx.width * (ci.uvBottomRight.x - ci.uvBottomLeft.x));
                h = (int)((float)fontTx.height * (-(ci.uvTopLeft.y - ci.uvBottomLeft.y)));

                Color[] cChar = fontTx.GetPixels(x, y, w, h);

                _texture.SetPixels(posX, 10, w, h, cChar);
                posX += (int)(ci.uvBottomRight.x - ci.uvBottomLeft.x);
                Debug.Log(i + "posX: " + posX + ", W: " + (ci.uvBottomRight.x - ci.uvBottomLeft.x));
            }
            _texture.Apply();
        }
        /// <summary>
        /// Call this to put the rendered texture in the LRT's Texture property so it can actually be used
        /// </summary>
        //public void ResolveTexture()
        //{
        //    //_texture = _renderTarget.GetTexture();
        //}

        public void Dispose()
        {
            _texture = new Texture2D((int)_region.width, (int)_region.height);
            //_renderTarget.Dispose();
        }
    }
}
