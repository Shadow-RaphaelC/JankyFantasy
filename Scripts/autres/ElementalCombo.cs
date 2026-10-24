using UnityEngine;

public static class ElementalCombo
{
    public static float GetMultiplier(Element a, Element b)
    {
        if (a == b) return 1f;

        if (IsPair(a, b, Element.Fire, Element.Ice)) return 2.0f;
        if (IsPair(a, b, Element.Ice, Element.Lightning)) return 1.6f;
        if (IsPair(a, b, Element.Lightning, Element.Fire)) return 1.8f;

        return 1f;
    }

    private static bool IsPair(Element a, Element b, Element e1, Element e2)
    {
        return (a == e1 && b == e2) || (a == e2 && b == e1); // faire en sorte que l'ordre n'importe pas (Fire + Ice = Ice + Fire)
    }
}
