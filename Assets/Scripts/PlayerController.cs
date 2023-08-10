using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public List<TetrisCube> sameColorCubes;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.GetComponent<TetrisCube>())
                {
                    hit.transform.GetComponent<TetrisCube>().GetNeighbours(sameColorCubes);
                }
            }
        }
    }
}
