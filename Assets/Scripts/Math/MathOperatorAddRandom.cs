
using MathFighter.Math.Views;
using UnityEngine;

namespace MathFighter.Math
{
    /// <summary>
    /// Represents an operator that (when evaluated) will generate a random number in the range
    /// given by it's left and right params.  The range is from inclusive min to *inclusive* max
    /// and will produce integers only (this differs from Random.Next() which uses an exclusive
    /// max, but it makes math expressions much more readable if we use an inclusive max).
    /// </summary>
    public class MathOperatorRandom : MathOperator
    {
        public override EnumPemdasLevel PemdasLevel()
        {
            return EnumPemdasLevel.Rnd;
        }

        public override string TextRepresentation()
        {
            return LeftParam.TextRepresentation() + ApplyInsertionString("RND ") + RightParam.TextRepresentation();
        }

        public override MathVariableNumber Evaluate()
        {
            MathVariableNumber leftVar = EvaluateLeftParam();

            if (leftVar == null) return null;

            MathVariableNumber rightVar = EvaluateRightParam();

            if (rightVar == null) return null;
            System.Random rnd = new System.Random();
            return new MathVariableNumber((float)rnd.Next((int)(leftVar as MathVariableNumber).Value, (int)(rightVar as MathVariableNumber).Value + 1));
        }

        public override MathElementView GenerateView(float charHeight, float spacing, Font font)
        {
            Debug.Log("The RND operator is not viewable - it is only intended for use in evaluation of expressions");
            return null;    // not a viewable operator
        }
    }
}
