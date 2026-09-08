using UnityEngine;

public class BreedingRoom : Room
{
    [SerializeField] private GameObject offspringPrefab;
    private bool hasBredThisPair;  
    // Update is called once per frame
    void Update()
    {
        int roomCount = GetRoomCount();

        if (roomCount == 2 && !hasBredThisPair)
        {
            Breed();
            hasBredThisPair = true;
        }
        
        else if (roomCount != 2)
        {
            hasBredThisPair = false;
        }

    }

    private void Breed()
    {
        Creature parent1 = children[0].GetComponent<Creature>();
        Creature parent2 = children[1].GetComponent<Creature>();

        Color childColour = TraitInheritance.CombineColours(parent1.GetComponent<CreatureGenetics>().GeneColour, parent2.GetComponent<CreatureGenetics>().GeneColour, 0.1f);
        Creature child = Instantiate(offspringPrefab, movePoint.position, Quaternion.identity).GetComponent<Creature>();
        CreatureGenetics childGen = child.GetComponent<CreatureGenetics>();
        childGen.GeneColour = childColour;
        childGen.ApplyColour();
        Debug.Log("Breeding " + parent1.GetName() + " and " + parent2.GetName());
    }
}
