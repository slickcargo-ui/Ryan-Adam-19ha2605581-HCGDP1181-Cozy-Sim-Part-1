using UnityEngine;

public class CreatureGenetics : MonoBehaviour
{
    [SerializeField] private Color geneColour = Color.white;
    [SerializeField] private float geneScale = 1f;
    [SerializeField] private int generation = 0;

    private const float MinScale = 0.7f;
    private const float MaxScale = 1.4f;

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
        geneColour = TraitInheritance.CombineColours(parent1.geneColour, parent2.geneColour, mutationChance);
        geneScale = Mathf.Clamp(
            TraitInheritance.CombineValues(parent1.geneScale, parent2.geneScale, mutationChance, MinScale),
            MinScale, MaxScale);
        generation = Mathf.Max(parent1.generation, parent2.generation) + 1;
        ApplyGenes();
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
