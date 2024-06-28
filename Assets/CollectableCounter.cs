using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CollectableCounter : MonoBehaviour
{
    [SerializeField] private float _maxAmount = 10f;
    [SerializeField] private Scrollbar _counterSlider;

    private Collectable collectable;


    void Start()
    {
        collectable = FindObjectOfType<Collectable>();
    }

    private void Update()
    {
        UpdateScrollbar();
    }

    void UpdateScrollbar()
    {
        int collectableCount = collectable.GetCollectableCount();

        float progress = (float)collectableCount / _maxAmount;

        _counterSlider.value = progress;
    }
}

