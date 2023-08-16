using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public List<TetrisCube> columnBoxes;
    public LayerMask tetrisCubeLayer;
    public int columnMaxBoxAmount;

    private void OnEnable()
    {
        EventManager.CubePainted += CubePainted;
    }

    private void OnDisable()
    {
        EventManager.CubePainted -= CubePainted;
    }

    private void CubePainted(TetrisCube cube)
    {
        GetBoxes();
        if (columnBoxes.Contains(cube))
        {
            columnBoxes.Remove(cube);
        }
    }

    public void GetBoxes()
    {
        columnBoxes.Clear();
        var temp = Physics.RaycastAll(transform.position + Vector3.up*10, Vector3.down, Mathf.Infinity, tetrisCubeLayer);
        foreach (var hit in temp)
        {
            if (hit.transform.GetComponent<TetrisCube>())
            {
                columnBoxes.Add(hit.transform.GetComponent<TetrisCube>());

            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, Vector3.down * 100);
    }
}
