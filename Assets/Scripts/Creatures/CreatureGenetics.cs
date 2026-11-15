using UnityEngine;

public class CreatureGenetics : MonoBehaviour
{
    [SerializeField] private Color geneColour = Color.white;
    [SerializeField] private float geneScale = 1f;
    [SerializeField] private int generation = 0;
    [SerializeField] private string mutations = "None";

    private const float MinScale = 0.7f;
    private const float MaxScale = 1.4f;
    public string Mutations { get { return mutations; } }

    private Vector3 baseScale;

    public Color GeneColour { get { return geneColour; } set { geneColour = value; } }
    public float GeneScale { get { return geneScale; } }
    public int Generation { get { return generation; } }

    private void Awake()
    {
        baseScale = transform.localScale;
    }

    public void InheritFrom(CreatureGenetics parent1, CreatureGenetics parent2, float mutationChance)
    {
        bool colourMutated, scaleMutated;

        geneColour = TraitInheritance.CombineColours(parent1.geneColour, parent2.geneColour, mutationChance, out colourMutated);
        geneScale = Mathf.Clamp(
            TraitInheritance.CombineValues(parent1.geneScale, parent2.geneScale, mutationChance, MinScale, out scaleMutated),
            MinScale, MaxScale);

        mutations = BuildMutationSummary(colourMutated, scaleMutated);
        generation = Mathf.Max(parent1.generation, parent2.generation) + 1;
        ApplyGenes();
    }

    private static string BuildMutationSummary(bool colour, bool scale)
    {
        if (colour && scale) return "Colour, Scale";
        if (colour) return "Colour";
        if (scale) return "Scale";
        return "None";
    }

    public string GetInfo()
    {
        return "Generation: " + generation +
               "\nScale: " + geneScale.ToString("0.00") +
               "\nColour: #" + ColorUtility.ToHtmlStringRGB(geneColour) +
               "\nMutations: " + mutations;
    }

    public void ApplyGenes()
    {
        ApplyColour();
        transform.localScale = baseScale * geneScale;
    }

    public void ApplyColour()
    {
        GetComponent<Renderer>().material.color = geneColour;
    }
}
