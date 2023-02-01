using UnityEngine;



namespace MathFighter.Math.Views
{
    public class MathOperatorParenthesesInvisibleView : MathOperatorView
    {
        public MathOperatorParenthesesInvisibleView(MathOperatorParenthesis op, float h, float spacing, Font font)
        {
            // get the texture for our parameter - we aren't displaying parentheses in this view
            _texture = op.LeftParam.GenerateView(h, spacing, font).Texture;
        }
    }
}
