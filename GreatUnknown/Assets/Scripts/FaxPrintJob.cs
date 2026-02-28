using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Fax/Fax Print Job")]
public class FaxPrintJob : ScriptableObject
{
    [SerializeField] private Sprite2D sprite;
    [SerializeField] private String header;
    [SerializeField] private String body;
}
