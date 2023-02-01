using UnityEngine;

namespace MathFighter.Math.Views
{
    public class MathVariableLetterView : MathVariableView
    {
        public MathVariableLetterView(MathVariableLetter variable, float h, float spacing, Font font)
        {
            _texture = DrawStringToTexture(variable.StringToken, h, font);
        }
    }
}
