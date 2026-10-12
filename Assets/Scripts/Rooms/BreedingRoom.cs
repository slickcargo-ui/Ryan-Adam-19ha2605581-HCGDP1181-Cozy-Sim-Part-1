using UnityEngine;

public class BreedingRoom : Room
{
    [SerializeField] private GameObject offspringPrefab;
    //Added a slider in the inspector to control the mutation chance of the offspring. The value is clamped between 0 and 1.
    [Range(0, 1)]
    [SerializeField] float mutationChance = 0.5f;
    //And this one is for the trait mutation chance, also clamped between 0 and 1.
    [Range(0, 1)]
    [SerializeField] float traitMutationChance = 0.5f;
    // Time in seconds before the Room can breed again
    [Range(0, 50)]
    [SerializeField] float breedingCooldown = 10f;

    // Keeping both the flag and timer,
    // if its only the timer, the room will breed every how ever meany seconds if there are 2 creatures in the room,
    // which is not what we want. We want it to only breed once per pair of creatures.
    private bool hasBredThisPair;
    private float BreedingCooldownTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        BreedingCooldownTimer -= Time.deltaTime;
        BreedingCooldownTimer = Mathf.Clamp(BreedingCooldownTimer, 0, breedingCooldown);

        int roomCount = GetRoomCount();

        if (roomCount == 2 && BreedingCooldownTimer <= 0 && !hasBredThisPair)
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
        BreedingCooldownTimer = breedingCooldown;
        
        Creature parent1 = children[0].GetComponent<Creature>();
        Creature parent2 = children[1].GetComponent<Creature>();

        Creature child = Instantiate(offspringPrefab, movePoint.position, Quaternion.identity).GetComponent<Creature>();

        Color childColour = TraitInheritance.CombineColours(parent1.GetComponent<CreatureGenetics>().GeneColour, parent2.GetComponent<CreatureGenetics>().GeneColour, mutationChance);
        CreatureGenetics childGen = child.GetComponent<CreatureGenetics>();
        childGen.GeneColour = childColour;
        childGen.ApplyColour();

        // Miner problem: The child can not inherit a trait unless both parents have that trait.
        // So if one parent has a trait and the other does not, the child will not inherit that trait.
        // This is a problem because it means that the child can not inherit a trait that is only present in one parent.

        // Set The Speed Value and MaxValue of the child to the combined trait values of the parents
        InheritStat(parent1, parent2, child,"Speed");
        // Set The Health Value and MaxValue of the child to the combined trait values of the parents
       InheritStat(parent1, parent2, child,"Health");
        // Set The Hunger Value and MaxValue of the child to the combined trait values of the parents
        InheritStat(parent1, parent2, child,"Hunger");
        // Set The Charge Value and MaxValue of the child to the combined trait values of the parents
        InheritStat(parent1, parent2, child,"Charge");

        Debug.Log("Breeding " + parent1.GetName() + " and " + parent2.GetName());
    }
    // Inherit the stat from the parents to the child
    private void InheritStat(Creature parent1, Creature parent2, Creature child, string statName)
    {
        // Check if both parents and the child have the stat
        if (parent1.GetStat(statName) != null && parent2.GetStat(statName) != null && child.GetStat(statName) != null)
        {
            Trait childTrait = TraitInheritance.CombineTraits(parent1.GetStat(statName).GetTrait(), parent2.GetStat(statName).GetTrait(), traitMutationChance);
            child.GetStat(statName).GetTrait().Value = childTrait.Value;
            child.GetStat(statName).GetTrait().MaxValue = childTrait.MaxValue;
        }

        else
        {
            Debug.LogWarning("One of the parents or the child does not have the stat: " + statName);
        }
    }
}
