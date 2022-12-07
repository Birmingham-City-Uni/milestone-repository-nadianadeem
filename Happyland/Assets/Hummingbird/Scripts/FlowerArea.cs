using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Manages a collection of flower plants and attached flowers.
/// </summary>
public class FlowerArea : MonoBehaviour
{
    public const float areaDiameter = 20f;

    //The list of all flower plants in the flower area (Flower plants have multiple flowers.
    private List<GameObject> flowerPlants;

    //A look up dictionary for looking up a flower from a nectar collider.
    private Dictionary<Collider, Flower> nectarFlowerDictionary;

    /// <summary>
    /// List of all flowers in flower area.
    /// </summary>
    public List<Flower> flowers { get; private set; }

    /// <summary>
    /// Reset flowers and flower plants
    /// </summary>
    public void ResetFlowers()
    {
        //Rotate each flower plant around the Y axis and subtly on X and Z.
        foreach(GameObject flowerPlant in flowerPlants)
        {
            float xRotation = UnityEngine.Random.Range(-5f, 5f);
            float yRotation = UnityEngine.Random.Range(-180f, 180f);
            float zRotation = UnityEngine.Random.Range(-5f, 5f);
            flowerPlant.transform.localRotation = Quaternion.Euler(xRotation, yRotation, zRotation);
        }

        //Reset each flower
        foreach (Flower flower in flowers)
        {
            flower.ResetFlower();
        }
    }

    /// <summary>
    /// Gets the <see cref="Flower"/> that a nectar collider belongs to.
    /// </summary>
    /// <param name="collider"> The nectar collider.</param>
    /// <returns>The matching flower.</returns>
    public Flower GetFlowerFromNectar(Collider collider)
    {
        return nectarFlowerDictionary[collider];
    }

    private void Awake()
    {
        flowerPlants = new List<GameObject>();
        nectarFlowerDictionary = new Dictionary<Collider, Flower>();
        flowers = new List<Flower>();
    }

    private void Start()
    {
        //Find all flowers that are children of this GameObject.
        FindChildFlowers(transform);
    }

    /// <summary>
    /// Recursively finds all flowers and flower plants that are children of the parent transform.
    /// </summary>
    /// <param name="parent">The parent of the child to check.</param>
    private void FindChildFlowers(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            if (child.CompareTag("flower_plant"))
            {
                // Found a flower plant, add it to flower plants list.
                flowerPlants.Add(child.gameObject);

                //Look for flowers in flower plant.
                FindChildFlowers(child);
            }
            else
            {
                //Not a flower plant, look for a flower component.
                Flower flower = child.GetComponent<Flower>();
                if(flower != null)
                {
                    //Found flower add it to flowers list.
                    flowers.Add(flower);

                    //Add the collider to dictionary.
                    nectarFlowerDictionary.Add(flower.nectarCollider, flower);

                    //Note: there are no flowers that are children of other flowers.
                }
                else
                {
                    //Flower component not found, check children.
                    FindChildFlowers(child);
                }
            }
        }
    }
}
