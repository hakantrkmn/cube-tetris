using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public List<TetrisCube> sameColorCubes;
    public bool canClick = true;

    public Color currentColor;

    private void OnEnable()
    {
        EventManager.PlayerCanClick += b => canClick = b;
    }

    private void OnDisable()
    {
        EventManager.PlayerCanClick -= b => canClick = b;
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
                    canClick = false;
                    foreach (var cube in hit.transform.GetComponentInParent<TetrisCreator>().tetrisCubes)
                    {
                        cube.boxChecked = false;
                    }

                    hit.transform.GetComponent<TetrisCube>().GetSameColorNeighbours(sameColorCubes, currentColor);
                    /*foreach (var cube in sameColorCubes)
                    {
                        cube.color = Color.red;
                        cube.boxChecked = false;
                    }
                    sameColorCubes.Clear();
                    */
                    //hit.transform.GetComponent<TetrisCube>().GetSameColorNeighbours(sameColorCubes,.1f);
                    //ColorBoxes(sameColorCubes);
                }
            }
        }
    }


    public void ColorBoxes(List<TetrisCube> cubes)
    {
        if (cubes.Count > 1)
        {
            Sequence cubePaint = DOTween.Sequence();
            foreach (var cube in cubes)
            {
                cubePaint.Join(cube.renderer.material.DOColor(Color.red, .5f));
            }

            foreach (var cube in cubes)
            {
                cubePaint.AppendCallback(() => { cube.DestroyCube(); });
            }

            foreach (var cube in cubes)
            {
                cubePaint.AppendCallback(() => { EventManager.CubePainted(cube); });
            }

            cubePaint.AppendCallback(() => { EventManager.SpawnCubeOnColumns(); });
        }
    }
}