using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AnimalsManager : MonoBehaviour
{
    // gestion des animaux (spawn, déplacement, despawn)
    
    //public GameManager gameManager;

    public Tilemap tilemap;
    public Transform animalsContainer;
    public float moveSpeed = 2f;

    private List<(GameObject animal, DynamicZone zone)> spawnedAnimals = new List<(GameObject, DynamicZone)>();

    public void SpawnAnimalInZone(DynamicZone zone, int idanimal)
    {
        // spawn l'animal avec le id spécifié dans la zone spécifiée
        if (zone == null || zone.positions == null || zone.positions.Count == 0)
            return;

        InfoManager.Instance.AddEnergy(20 * idanimal);
        Vector3Int spawnPos = zone.positions[Random.Range(0, zone.positions.Count)];
        Vector3 newspawnPos = tilemap.GetCellCenterWorld(spawnPos);
        newspawnPos.z = 1;

        GameObject animal = Instantiate(InfoManager.Instance.animalPrefab[idanimal], newspawnPos, Quaternion.identity, animalsContainer);
        spawnedAnimals.Add((animal, zone));

        if (!InfoManager.Instance.animalInGame.Contains(InfoManager.Instance.animalPrefab[idanimal]))
        {
           //Debug.Log("Animal added to list: " + InfoManager.Instance.animalPrefab[idanimal].name);
            InfoManager.Instance.animalInGame.Add(InfoManager.Instance.animalPrefab[idanimal]);
        }

        Animator animalAnimator = animal.GetComponent<Animator>();
        if (animalAnimator != null)
        {
            animalAnimator.SetBool("left_walk", false);
            animalAnimator.SetBool("right_walk", false);
        }
        StartCoroutine(AnimalBehaviorCycle(animal, zone));
    }

    private IEnumerator AnimalBehaviorCycle(GameObject animal, DynamicZone zone)
    {
        // cycle de déplacement de l'animal (ildle, marche random...)
        while (animal != null)
        {
            Animator animalAnimator = animal.GetComponent<Animator>();

            if (animalAnimator != null)
            {
                animalAnimator.SetBool("left_walk", false);
                animalAnimator.SetBool("right_walk", false);
            }

            yield return new WaitForSeconds(Random.Range(2f, 8f));

            Vector3 targetPosition = GetRandomPositionInZone(zone);

            while (animal != null && Vector3.Distance(animal.transform.position, targetPosition) > 0.1f)
            {
                Vector3 moveDirection = (targetPosition - animal.transform.position).normalized;

                bool isMovingRight = moveDirection.x > 0;

                if (animalAnimator != null)
                {
                    animalAnimator.SetBool("right_walk", isMovingRight); // Marche à droite
                    animalAnimator.SetBool("left_walk", !isMovingRight); // Marche à gauche
                }

                animal.transform.position = Vector3.MoveTowards(animal.transform.position, targetPosition, moveSpeed * Time.deltaTime);

                yield return null;
            }

            if (animalAnimator != null)
            {
                animalAnimator.SetBool("left_walk", false);
                animalAnimator.SetBool("right_walk", false);
            }

            yield return new WaitForSeconds(Random.Range(2f, 8f));
        }
    }

    private Vector3 GetRandomPositionInZone(DynamicZone zone)
    {
        if (zone.positions == null || zone.positions.Count == 0)
        {
            Debug.LogWarning("La zone ne contient aucune position.");
            return Vector3.zero;
        }
        Vector3Int randomTile = zone.positions[Random.Range(0, zone.positions.Count)];
        Vector3 randomWorldPos = tilemap.GetCellCenterWorld(randomTile);
        randomWorldPos.z = 1;
        return randomWorldPos;
    }

    public void DespawnAnimals(DynamicZone zone)
    {
        // Supprime tous les animaux dans la zone spécifiée
        List<(GameObject animal, DynamicZone zone)> animalsToRemove = new List<(GameObject animal, DynamicZone zone)>();

        foreach (var (animal, animalZone) in spawnedAnimals)
        {
            if (animalZone == zone)
            {
                Destroy(animal);
                animalsToRemove.Add((animal, animalZone));
            }
        }

        foreach (var animal in animalsToRemove)
        {
            spawnedAnimals.Remove(animal);
        }


        foreach (var animalPrefab in InfoManager.Instance.animalPrefab)
        {
            bool speciesStillPresent = false;
            foreach (var (animal, animalZone) in spawnedAnimals)
            {
                if (animalPrefab.name == animal.name.Replace("(Clone)", "").Trim())
                {
                    speciesStillPresent = true;
                    break;
                }
            }

            if (!speciesStillPresent)
            {
                //Debug.Log("Animal removed from list: " + animalPrefab.name);
                InfoManager.Instance.animalInGame.Remove(animalPrefab);
            }
        }

        //Debug.Log($"All animals in zone {zone.name} have been removed.");
    }
}
