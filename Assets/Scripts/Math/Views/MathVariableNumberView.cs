using UnityEngine;

namespace MathFighter.Math.Views
{
    public class MathVariableNumberView : MathVariableView
    {
        public MathVariableNumberView(MathVariableNumber variable, float h, float spacing, Font font)
        {
            _texture = DrawStringToTexture(variable.Value.ToString(), h, font);
        }
    }
}
