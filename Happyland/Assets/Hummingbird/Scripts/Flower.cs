using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a single flower with nectar.
/// </summary>

public class Flower : MonoBehaviour
{
    [Tooltip("The color when the flower is full")]
    public Color fullFlowerColor = new Color(1f, 0f, 0.3f);

    [Tooltip("The color when the flower is empty")]
    public Color emptyFlowerColor = new Color(0.5f, 0f, 1f);

    /// <summary>
    /// The trigger collider representing the nectar.
    /// </summary>
    [HideInInspector]
    public Collider nectarCollider;

    // The solider collider representing the flower petals;
    private Collider flowerCollider;

    //The flower's material.
    private Material flowerMaterial;

    /// <summary>
    /// Vector poiting straight out of the flower.
    /// </summary>
    public Vector3 FlowerUpVector
    {
        get
        {
            return nectarCollider.transform.up;
        }
    }

    /// <summary>
    /// The center position of the nectar collider.
    /// </summary>
    public Vector3 FlowerCenterPosition
    {
        get
        {
            return nectarCollider.transform.position;
        }
    }

    /// <summary>
    /// The amount of nectar remaining in the flower.
    /// </summary>
    public float NectarAmount { get; private set; }

    public bool HasNectar
    {
        get
        {
            return NectarAmount > 0;
        }
    }


    /// <summary>
    /// Attempts to remove nectar from the flower.
    /// </summary>
    /// <param name="amount"> The amount of nectar to remove.</param>
    /// <returns> The actual amount successfully removed.</returns>
    public float Feed(float amount)
    {
        //Track how much nectar was actually removed (cannot take more than available).
        float nectarTaken = Mathf.Clamp(amount, 0f, NectarAmount);

        //Subtract the nectar
        NectarAmount -= amount;

        if(!HasNectar)
        {
            //No nectar left
            NectarAmount = 0;

            //Disable flower and nectar colliders
            flowerCollider.gameObject.SetActive(false);
            nectarCollider.gameObject.SetActive(false);

            //Change flower colour to show emptiness.
            flowerMaterial.SetColor("_BaseColor", emptyFlowerColor);
        }

        //Return nectar amount taken.
        return nectarTaken;
    }

    public void ResetFlower()
    {
        //Refill nectar
        NectarAmount = 1f;

        //Enabled flower and nectar colliders.
        flowerCollider.gameObject.SetActive(true);
        nectarCollider.gameObject.SetActive(true);

        //Change flower colour to show fullness.
        flowerMaterial.SetColor("_BaseColor", fullFlowerColor);
    }

    /// <summary>
    /// Called when the flower wakes up.
    /// </summary>
    private void Awake()
    {
        //Find flower's mesh renderer and get the main material.
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        flowerMaterial = meshRenderer.material;

        //Find flower and nectar colliders.
        flowerCollider = transform.Find("FlowerCollider").GetComponent<Collider>();
        nectarCollider = transform.Find("FlowerNectarCollider").GetComponent<Collider>();

        ResetFlower();
    }
}
