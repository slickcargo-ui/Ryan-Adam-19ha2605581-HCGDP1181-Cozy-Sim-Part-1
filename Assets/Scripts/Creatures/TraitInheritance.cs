using UnityEngine;

public static class TraitInheritance
{
    public static Color CombineColours(Color parent1Colour, Color parent2Colour, float mutationChance)
    {
      Color childColor = Color.Lerp(parent1Colour, parent2Colour, 0.5f);// Average the two parent colors. also i changed the float here from 0.1 to 0.5 to make the child color more balanced between the two parents
        // Apply mutation
        if (Random.value < mutationChance)
        {
           childColor = new Color(Random.value, Random.value, Random.value);
        }
        return childColor;
    }

    public static Trait CombineTraits(Trait parent1Trait, Trait parent2Trait, float traitMutationChance)
    {
        string childName = parent1Trait.Name;
        // Average the max values of the two parent traits to determine the child's trait value
        float childValue = (parent1Trait.MaxValue + parent2Trait.MaxValue) / 2f;
        // Apply mutation
        if (Random.value < traitMutationChance)
        {
            childValue += Random.Range(-10f, 10f); // Randomly adjust the value
            childValue = Mathf.Clamp(childValue, 1f, Mathf.Max(parent1Trait.MaxValue + parent2Trait.MaxValue)); // Ensure the value stays within bounds
        }
        return new Trait(childName, childValue, "Child of " + parent1Trait.Name + " and " + parent2Trait.Name);
    }
}
