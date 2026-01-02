using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{

    [SerializeField] private Slider healthBar;
    private Camera mainCamera;
    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void updateHealthBar(float currentValue,float maxValue)
    {
        healthBar.value = currentValue / maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.transform.parent.rotation = mainCamera.transform.rotation;
    }
}
