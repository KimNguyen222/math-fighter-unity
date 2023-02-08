using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
            Debug.Log("Draw1: " + color);
            Texture2D newTex = new Texture2D(_texture.width, _texture.height, _texture.format, false);

            int startX = (int)position.x;
            int endX = (int)(position.x + sourceRectangle.width);
            int startY = (int)position.y;
            int endY = (int)(position.y + sourceRectangle.height);
            for (int x = 0; x < _texture.width; x++)
            {
                for (int y = 0; y < _texture.height; y++)
                {
                    newTex.SetPixel(x, y, _texture.GetPixel(x, y));
                }
            }

            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    //if (x >= startX && y >= startY && x < origin.x && y < origin.y)
                    //{
                        Color bgColor = _texture.GetPixel(x, y);
                        Color srcColor = sourceTexture.GetPixel(x - startX, y - startY);

                        Color final_color = Color.Lerp(bgColor, srcColor, srcColor.a / 1.0f);

                        newTex.SetPixel(x, y, srcColor);
                    //}
                    //else
                    //    newTex.SetPixel(x, y, _texture.GetPixel(x, y));
                }
            }

            newTex.Apply();
            ApplyTexture(newTex);
            //_texture.Apply();

        }
        public void Draw(Texture2D sourceTexture, Vector2 position, Rect sourceRectangle, Color color)
        {
            Debug.Log("Draw2: " + color);
            Texture2D newTex = new Texture2D(_texture.width + sourceTexture.width, _texture.height + 
                sourceTexture.height, _texture.format, false);

            int startX = (int)position.x;
            int endX = (int)(position.x + sourceRectangle.width);
            int startY = (int)position.y;
            int endY = (int)(position.y + sourceRectangle.height);
            for (int x = 0; x < _texture.width; x++)
            {
                for (int y = 0; y < _texture.height; y++)
                {
                    newTex.SetPixel(x, y, _texture.GetPixel(x, y));
                }
            }

            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    //if (x >= startX && y >= startY && x < origin.x && y < origin.y)
                    //{
                    Color bgColor = _texture.GetPixel(x, y);
                    Color srcColor = sourceTexture.GetPixel(x - startX, y - startY);

                    Color final_color = Color.Lerp(bgColor, srcColor, srcColor.a / 1.0f);

                    newTex.SetPixel(x, y, srcColor);
                    //}
                    //else
                    //    newTex.SetPixel(x, y, _texture.GetPixel(x, y));
                }
            }

            newTex.Apply();
            Debug.Log("_Texture Size1: " + _texture.width + ", " + _texture.height);
            //ApplyTexture(newTex);
            _texture = newTex;
            Debug.Log("_Texture Size2: " + _texture.width + ", " + _texture.height);
            //_texture.Apply();

        }
        public void Draw(Texture2D sourceTexture, Rect destinationRectangle, Rect sourceRectangle, Color color)
        {
            Debug.Log("Draw3: " + color);
            Texture2D newTex = new Texture2D(_texture.width, _texture.height, _texture.format, false);

            int startX = (int)destinationRectangle.x;
            int endX = (int)(destinationRectangle.x + destinationRectangle.width);
            int startY = (int)destinationRectangle.y;
            int endY = (int)(destinationRectangle.y + destinationRectangle.height);

            for (int x = 0; x < _texture.width; x++)
            {
                for (int y = 0; y < _texture.height; y++)
                {
                    newTex.SetPixel(x, y, _texture.GetPixel(x, y));
                }
            }

            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    //if (x >= startX && y >= startY && x < origin.x && y < origin.y)
                    //{
                    Color bgColor = _texture.GetPixel(x, y);
                    Color srcColor = sourceTexture.GetPixel(x - startX, y - startY);

                    Color final_color = Color.Lerp(bgColor, srcColor, srcColor.a / 1.0f);

                    newTex.SetPixel(x, y, srcColor);
                    //}
                    //else
                    //    newTex.SetPixel(x, y, _texture.GetPixel(x, y));
                }
            }

            newTex.Apply();

            ApplyTexture(newTex);
            //_texture.Apply();
        }
        public void DrawString(Font font, string text, Vector2 position, Color color, float rotation, 
            Vector2 origin, float scale, float layerDepth)
        {
            // Request characters.
            font.RequestCharactersInTexture(text);

            // Set up mesh.
            Mesh mesh = new Mesh();
            GameObject go = new GameObject();
            go.transform.SetParent(GameObject.Find("Question Bar").transform);
            go.transform.localScale = Vector3.one * 0.05f;
            go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>();
            go.GetComponent<MeshFilter>().mesh = mesh;
            go.GetComponent<MeshRenderer>().material = font.material;

            // Generate font mesh.
            RebuildMesh(text, font, mesh);


            RenderTexture renderTexture = new RenderTexture(_texture.width, _texture.height, 24);
            renderTexture.Create();
            Material material = go.GetComponent<MeshRenderer>().material;
            RenderTexture currentTexture = RenderTexture.active;
            RenderTexture.active = renderTexture;
            GL.Clear(false, true, Color.black, 1.0f);
            material.SetPass(0);
            Graphics.DrawMeshNow(mesh, Vector3.zero, Quaternion.identity);
            _texture.ReadPixels(new Rect(0, 0, _texture.width, _texture.height), 0, 0);
            //_texture = rotateTexture(_texture, true);
            //_texture = rotateTexture(_texture, true);
            RenderTexture.active = currentTexture;
            
            renderTexture.Release();
            //Destroy(go);
            //Debug.Log("Mesh Color Count: " + mesh.colors.Length);
            //for (int h = 0; h < Height; h++)
            //    for (int w = 0; w < Width; w++)
            //    {
            //        _texture.SetPixel(w, h, mesh.colors[h * Width + w]);
            //        Debug.Log(mesh.colors[h * Width + w]);
            //    }
            //_texture = (Texture2D)go.GetComponent<MeshRenderer>().material.mainTexture;
            _texture.Apply();


            //CharacterInfo ci;
            //char[] cText = text.ToCharArray();

            //Material fontMat = font.material;
            //Texture2D fontTx = (Texture2D)fontMat.mainTexture;
            //Debug.Log("Text: " + text);
            //Debug.Log("FontTX: " + fontTx.width + ", " + fontTx.height);
            //Debug.Log("_texture: " + _texture.width + ", " + _texture.height);
            //int x, y, w, h;
            //int posX = 10;

            //for (int i = 0; i < cText.Length; i++)
            //{
            //    bool isCharacter = font.GetCharacterInfo(cText[i], out ci);

            //    x = (int)((float)fontTx.width * ci.uvBottomLeft.x);
            //    y = (int)((float)fontTx.height * ci.uvTopLeft.y);
            //    w = (int)((float)fontTx.width * (ci.uvBottomRight.x - ci.uvBottomLeft.x));
            //    h = (int)((float)fontTx.height * (-(ci.uvTopLeft.y - ci.uvBottomLeft.y)));


            //    x = (int)(ci.minX);
            //    y = (int)(ci.minY);
            //    w = (int)(ci.maxX - ci.minX);
            //    h = (int)((ci.maxY - ci.minY));
            //    Debug.Log("Gylph: " + ci.glyphWidth + ", " + (ci.uvBottomRight - ci.uvBottomLeft) + "," +
            //        (ci.uvTopRight - ci.uvTopLeft));
            //    Debug.Log("x, y, w, h: " + x + "," + y + "," + w + "," + h);
            //    _texture = new Texture2D(fontTx.width, fontTx.height);
            //    Color[] cChar = fontTx.GetPixels(0, 0, _texture.width, _texture.height);

            //    _texture.SetPixels(0, 0, _texture.width, _texture.height, cChar);
            //    _texture.Apply();
            //    break;
            //    posX += (int)(ci.uvBottomRight.x - ci.uvBottomLeft.x);
            //    Debug.Log(i + "posX: " + posX + ", W: " + (ci.uvBottomRight.x - ci.uvBottomLeft.x));
            //}
            //_texture.Apply();
            //string str64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(text));
            //byte[] imageBytes = Convert.FromBase64String(str64);
            //Debug.Log(text + ": " + imageBytes[0]);
            //_texture.LoadImage(imageBytes);




            //Texture2D output = new Texture2D(_texture.width, _texture.height);
            //RenderTexture renderTexture = new RenderTexture(_texture.width, _texture.height, 24);
            //GameObject tempObject = new GameObject("tempObject");
            //tempObject.transform.position = position;

            //Camera myCamera = new GameObject("TextCamera").AddComponent<Camera>();
            //myCamera.transform.SetParent(tempObject.transform, false);
            //myCamera.backgroundColor = new Color(0, 0, 0, 0);
            //myCamera.clearFlags = CameraClearFlags.Color;
            //myCamera.cullingMask = 1 << LayerMask.NameToLayer("TEXT");
            //myCamera.targetTexture = renderTexture;

            //// Canvas


            //Canvas myCanvas = new GameObject("Canvas", typeof(RectTransform)).AddComponent<Canvas>();
            //myCanvas.transform.SetParent(myCamera.transform, false);
            //myCanvas.gameObject.AddComponent<CanvasScaler>();
            //myCanvas.renderMode = RenderMode.WorldSpace;
            //myCanvas.worldCamera = myCamera;
            //var canvasRectTransform = myCanvas.GetComponent<RectTransform>();
            //canvasRectTransform.anchoredPosition3D = new Vector3(0, 0, 3);
            //canvasRectTransform.sizeDelta = Vector2.one;

            //// Text
            //Text UIText = new GameObject("Text", typeof(RectTransform)).AddComponent<Text>();
            //UIText.transform.SetParent(myCanvas.transform, false);
            //var textRectTransform = UIText.GetComponent<RectTransform>();
            //textRectTransform.localScale = Vector3.one * 0.001f;
            //textRectTransform.sizeDelta = new Vector2(2000, 2000);

            //UIText.font = font;
            //UIText.fontStyle = FontStyle.Bold;
            //UIText.alignment = TextAnchor.MiddleCenter;
            //Debug.Log("color: " + color);
            //UIText.color = Color.white;
            //UIText.fontSize = 300;
            //UIText.horizontalOverflow = HorizontalWrapMode.Wrap;
            //UIText.verticalOverflow = VerticalWrapMode.Overflow;

            //myCanvas.gameObject.layer = LayerMask.NameToLayer("TEXT");
            //UIText.gameObject.layer = LayerMask.NameToLayer("TEXT");

            //UIText.text = text;

            //// Text position
            ////myText.transform.position = position;

            //myCamera.Render();
            //RenderTexture.active = renderTexture;
            //_texture.ReadPixels(new Rect(0, 0, _texture.width, _texture.height), 0, 0);
            //_texture.Apply();

            //RenderTexture.active = null;
            ////Destroy(tempObject);
        }
        private Texture2D rotateTexture(Texture2D originalTexture, bool clockwise)
        {
            Color32[] original = originalTexture.GetPixels32();
            Color32[] rotated = new Color32[original.Length];
            int w = originalTexture.width;
            int h = originalTexture.height;

            int iRotated, iOriginal;

            for (int j = 0; j < h; ++j)
            {
                for (int i = 0; i < w; ++i)
                {
                    iRotated = (i + 1) * h - j - 1;
                    iOriginal = clockwise ? original.Length - 1 - (j * w + i) : j * w + i;
                    rotated[iRotated] = original[iOriginal];
                }
            }

            Texture2D rotatedTexture = new Texture2D(h, w);
            rotatedTexture.SetPixels32(rotated);
            rotatedTexture.Apply();
            return rotatedTexture;
        }
        private void RebuildMesh(string str, Font font, Mesh mesh)
        {
            // Generate a mesh for the characters we want to print.
            var vertices = new Vector3[str.Length * 4];
            var triangles = new int[str.Length * 6];
            var uv = new Vector2[str.Length * 4];
            Vector3 pos = Vector3.zero;
            for (int i = 0; i < str.Length; i++)
            {
                // Get character rendering information from the font
                CharacterInfo ch;
                font.GetCharacterInfo(str[i], out ch);

                vertices[4 * i + 0] = pos + new Vector3(ch.minX, ch.maxY, 0);
                vertices[4 * i + 1] = pos + new Vector3(ch.maxX, ch.maxY, 0);
                vertices[4 * i + 2] = pos + new Vector3(ch.maxX, ch.minY, 0);
                vertices[4 * i + 3] = pos + new Vector3(ch.minX, ch.minY, 0);

                //uv[4 * i + 0] = ch.uvTopLeft;
                //uv[4 * i + 1] = ch.uvTopRight;
                //uv[4 * i + 2] = ch.uvBottomRight;
                //uv[4 * i + 3] = ch.uvBottomLeft;

                uv[4 * i + 0] = ch.uvBottomLeft;
                uv[4 * i + 1] = ch.uvBottomRight;
                uv[4 * i + 2] = ch.uvTopRight;
                uv[4 * i + 3] = ch.uvTopLeft;

                triangles[6 * i + 0] = 4 * i + 0;
                triangles[6 * i + 1] = 4 * i + 1;
                triangles[6 * i + 2] = 4 * i + 2;

                triangles[6 * i + 3] = 4 * i + 0;
                triangles[6 * i + 4] = 4 * i + 2;
                triangles[6 * i + 5] = 4 * i + 3;

                // Advance character position
                pos += new Vector3(ch.advance, 0, 0);
            }
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            Color32[] colors = new Color32[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
                colors[i] = Color.Lerp(Color.white, Color.white, vertices[i].y);

            mesh.colors32 = colors;
        }


        private void ApplyTexture(Texture2D src)
        {
            // Get a copy of the color data from the source Texture2D, in high-precision float format.
            // Each element in the array represents the color data for an individual pixel.
            int sourceMipLevel = 0;
            Color[] pixels = src.GetPixels(sourceMipLevel);

            // Set the pixels of the destination Texture2D.
            int destinationMipLevel = 0;
            _texture = new Texture2D(src.width, src.height);
            _texture.SetPixels(pixels, destinationMipLevel);

            // Apply changes to the destination Texture2D, which uploads its data to the GPU.
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
            _texture = new Texture2D((int)_region.width, (int)_region.height, TextureFormat.RGB24, false);
            //_renderTarget.Dispose();
        }
    }
}
