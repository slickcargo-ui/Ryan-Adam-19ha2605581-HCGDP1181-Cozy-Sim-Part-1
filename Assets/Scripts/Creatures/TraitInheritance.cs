using UnityEngine;

public static class TraitInheritance
{
    private const float ColourMutationStrength = 0.1f;  // max nudge per colour channel
    private const float ValueMutationPercent = 0.1f;    // max +/-10% nudge for stats and scale

    public static Color CombineColours(Color parent1, Color parent2, float mutationChance)
    {
        Color child = Color.Lerp(parent1, parent2, 0.5f);

        if (Random.value < mutationChance)
        {
            child.r = Mathf.Clamp01(child.r + Random.Range(-ColourMutationStrength, ColourMutationStrength));
            child.g = Mathf.Clamp01(child.g + Random.Range(-ColourMutationStrength, ColourMutationStrength));
            child.b = Mathf.Clamp01(child.b + Random.Range(-ColourMutationStrength, ColourMutationStrength));
        }

        child.a = 1f;
        return child;
    }

    // Random blend between the two parents, then a small mutation nudge.
    public static float CombineValues(float parent1, float parent2, float mutationChance, float min)
    {
        float value = Mathf.Lerp(parent1, parent2, Random.value);

        if (Random.value < mutationChance)
        {
            value *= 1f + Random.Range(-ValueMutationPercent, ValueMutationPercent);
        }

        return Mathf.Max(value, min);
    }

    public static Trait CombineTraits(Trait parent1, Trait parent2, float mutationChance)
    {
        float value = CombineValues(parent1.MaxValue, parent2.MaxValue, mutationChance, 1f);
        return new Trait(parent1.Name, value, "Child of " + parent1.Name + " and " + parent2.Name);
    }
}
