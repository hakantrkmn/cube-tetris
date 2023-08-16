using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public List<TetrisCube> sameColorCubes;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            sameColorCubes.Clear();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.GetComponent<TetrisCube>())
                {
                    hit.transform.GetComponent<TetrisCube>().GetSameColorNeighbours(sameColorCubes);
                    foreach (var cube in sameColorCubes)
                    {
                        cube.color = Color.red;
                        cube.boxChecked = false;
                    }
                    sameColorCubes.Clear();
                    hit.transform.GetComponent<TetrisCube>().GetSameColorNeighbours(sameColorCubes);
                    ColorBoxes(sameColorCubes);
                }
            }
        }
    }


    public void ColorBoxes(List<TetrisCube> cubes)
    {
        if (cubes.Count>1)
        {
            Sequence cubePaint = DOTween.Sequence();
            foreach (var cube in cubes)
            {
                cubePaint.Join(cube.renderer.material.DOColor(Color.red, .5f));
            }
            foreach (var cube in cubes)
            {
                cubePaint.AppendCallback(() =>
                {
                    EventManager.CubePainted(cube);
                });
            }
            foreach (var cube in cubes)
            {
                
                    cube.DestroyCube();
      
            }
            cubePaint.AppendCallback(() =>
            {
                EventManager.SpawnCubeOnColumns();
            });
        }
        
    }
}
