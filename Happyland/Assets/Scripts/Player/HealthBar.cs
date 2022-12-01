using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public HealthManager healthManager;
    public Slider healthSlider;

    // Start is called before the first frame update
    void Start()
    {
        healthManager = gameObject.GetComponent<HealthManager>();
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = healthManager.currentHealth;
    }
}
