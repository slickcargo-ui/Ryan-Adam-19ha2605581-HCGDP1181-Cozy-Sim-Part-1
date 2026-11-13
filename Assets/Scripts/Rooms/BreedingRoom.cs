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

        else if (roomCount < 2)
        {
            hasBredThisPair = false;
        }

    }

    private void Breed()
    {
        BreedingCooldownTimer = breedingCooldown;

        Creature parent1 = children[0].GetComponent<Creature>();
        Creature parent2 = children[1].GetComponent<Creature>();

        Creature child = Instantiate(offspringPrefab, movePoint.position, Quaternion.identity, transform).GetComponent<Creature>();

        child.GetComponent<CreatureGenetics>().InheritFrom(
            parent1.GetComponent<CreatureGenetics>(),
            parent2.GetComponent<CreatureGenetics>(),
            mutationChance);

        foreach (Stat stat in child.GetStats())
        {
            InheritStat(parent1, parent2, child, stat.GetTrait().Name);
        }
    }

    private void InheritStat(Creature parent1, Creature parent2, Creature child, string statName)
    {
        Stat p1 = parent1.GetStat(statName);
        Stat p2 = parent2.GetStat(statName);
        Stat c = child.GetStat(statName);

        if (c == null) return;

        Trait childTrait;

        if (p1 != null && p2 != null)
        {
            childTrait = TraitInheritance.CombineTraits(p1.GetTrait(), p2.GetTrait(), traitMutationChance);
        }
        else if (p1 != null || p2 != null)
        {
            // Only one parent has this stat: inherit from that parent, still allowing mutation
            Trait only = (p1 != null ? p1 : p2).GetTrait();
            childTrait = TraitInheritance.CombineTraits(only, only, traitMutationChance);
        }
        else
        {
            return; // neither parent has it, so the child keeps its prefab default
        }

        c.GetTrait().MaxValue = childTrait.MaxValue;
        c.GetTrait().Value = childTrait.Value;
    }
}
