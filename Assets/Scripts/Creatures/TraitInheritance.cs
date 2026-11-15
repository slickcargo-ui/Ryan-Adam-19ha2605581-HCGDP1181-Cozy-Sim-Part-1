using UnityEngine;

public static class TraitInheritance
{
    private const float ColourMutationStrength = 0.1f;  // max nudge per colour channel
    private const float ValueMutationPercent = 0.1f;    // max +/-10% nudge for stats and scale

    public static Color CombineColours(Color parent1, Color parent2, float mutationChance, out bool mutated)
    {
        Color child = Color.Lerp(parent1, parent2, 0.5f);
        mutated = Random.value < mutationChance;

        if (mutated)
        {
            child.r = Mathf.Clamp01(child.r + Random.Range(-ColourMutationStrength, ColourMutationStrength));
            child.g = Mathf.Clamp01(child.g + Random.Range(-ColourMutationStrength, ColourMutationStrength));
            child.b = Mathf.Clamp01(child.b + Random.Range(-ColourMutationStrength, ColourMutationStrength));
        }

        child.a = 1f;
        return child;
    }

    public static float CombineValues(float parent1, float parent2, float mutationChance, float min, out bool mutated)
    {
        float value = Mathf.Lerp(parent1, parent2, Random.value);
        mutated = Random.value < mutationChance;

        if (mutated)
        {
            value *= 1f + Random.Range(-ValueMutationPercent, ValueMutationPercent);
        }

        return Mathf.Max(value, min);
    }

    public static Trait CombineTraits(Trait parent1, Trait parent2, float mutationChance)
    {
        // Use whichever is larger, in case a serialized MaxValue was never set in the inspector
        float p1 = Mathf.Max(parent1.MaxValue, parent1.Value);
        float p2 = Mathf.Max(parent2.MaxValue, parent2.Value);

        float value = CombineValues(p1, p2, mutationChance, 1f, out _);
        return new Trait(parent1.Name, value, "Child of " + parent1.Name + " and " + parent2.Name);
    }
}
