using UnityEngine;

namespace MathFighter.Math.Views
{
    public class MathOperatorExpressionRootView : MathOperatorView
    {
        public MathOperatorExpressionRootView(MathOperatorExpressionRoot op, float h, float spacing, Font font)
        {
            _texture = op.LeftParam.GenerateView(h, spacing, font).Texture;
        }
    }
}
