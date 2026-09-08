using UnityEngine;

public class CreatureGenetics : MonoBehaviour
{
    [SerializeField] private Color geneColour;
   
    public Color GeneColour { get { return geneColour; } set { geneColour = value; } }

   public void ApplyColour()
    {
       GetComponent<Renderer>().material.color = geneColour;
    }
}
