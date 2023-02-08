using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] LayerMask solidObjectsLayer;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask detectLayer;
    [SerializeField] LayerMask doorLayer;

    public static GameLayers i { get; set; }

    private void Awake()
    {
        i = this;
    }

    public LayerMask SolidLayer
    {
        get => solidObjectsLayer;
    }

    public LayerMask InteractableLayer
    {
        get => interactableLayer;
    }
    public LayerMask DetectLayer
    {
        get => detectLayer;
    }
    public LayerMask DoorLayer
    {
        get => doorLayer;
    }

    public LayerMask TriggerableLayers
    {
        get => detectLayer | doorLayer;
    }
}
