using UnityEngine;

public static class TraitInheritance
{
    public static Color CombineColours(Color parent1Colour, Color parent2Colour, float mutationChance)
    {
      Color childColor = Color.Lerp(parent1Colour, parent2Colour, 0.1f);
        // Apply mutation
        if (Random.value < mutationChance)
        {
           childColor = new Color(Random.value, Random.value, Random.value);
        }
        return childColor;
    }
}
