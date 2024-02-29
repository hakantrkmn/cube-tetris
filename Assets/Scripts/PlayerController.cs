using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public List<TetrisCube> sameColorCubes;
    public bool canClick = true;

    public List<Color> colors;
    public Color currentColor;
    private int colorIndex;
    private void OnEnable()
    {
        EventManager.PlayerCanClick += b => canClick = b;
    }

    private void OnDisable()
    {
        EventManager.PlayerCanClick -= b => canClick = b;
    }

    [Button]
    public void Test()
    {
        foreach (var cube in sameColorCubes)
        {
            EventManager.SpawnCubeAtColumn(cube);
        }
    }
    private void OnValidate()
    {
        for (int i = 0; i < colors.Count; i++)
        {
            colors[i] = Utility.MakeColorF1(colors[i]);
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canClick)
        {
            sameColorCubes.Clear();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.GetComponent<TetrisCube>())
                {
                    foreach (var cube in hit.transform.GetComponentInParent<TetrisCreator>().tetrisCubes)
                    {
                        cube.boxChecked = false;
                        cube.sameColorNeighbours.Clear();
                    }

                    canClick = false;
                    hit.transform.GetComponent<TetrisCube>().GetSameColorNeighbours(sameColorCubes, currentColor);
                    colorIndex++;
                    if (colorIndex >= colors.Count)
                    {
                        colorIndex = 0;
                    }
                    currentColor = colors[colorIndex];
                }
            }
        }
    }
}