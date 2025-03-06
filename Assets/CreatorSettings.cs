using System;
using UnityEngine;
using UnityEngine.UI;

public class CreatorSettings : MonoBehaviour
{
    [SerializeField] private InputField _sizeInput;
    [SerializeField] private GridField _grid;


    public void Submit()
    {
        int size = Int32.Parse(_sizeInput.text);
        _grid.CreateGrid(size);
    }
}
