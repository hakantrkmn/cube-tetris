using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Column : MonoBehaviour
{
    public List<TetrisCube> columnBoxes;
    public LayerMask tetrisCubeLayer;
    public int columnMaxBoxAmount;

    public int boxAmount;

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
    }

    public void GetBoxes()
    {
        columnBoxes.Clear();
        var temp = Physics.RaycastAll(transform.position + Vector3.up * 10, Vector3.down, Mathf.Infinity,
            tetrisCubeLayer);
        foreach (var hit in temp)
        {
            if (hit.transform.GetComponent<TetrisCube>())
            {
                if (!hit.transform.GetComponent<TetrisCube>().isDestroyed)
                {
                    columnBoxes.Add(hit.transform.GetComponent<TetrisCube>());
                }
            }
        }

        boxAmount = columnBoxes.Count;
    }

    public void SpawnCube(GameObject prefab, Transform parent, float scale, Color color, List<TetrisCube> tetrisCubes)
    {
        var cube = Instantiate(prefab.gameObject, Vector3.zero, quaternion.identity, parent);
        cube.transform.localPosition =
            new Vector3(transform.position.x, transform.position.y + (scale * boxAmount), 0);
        cube.transform.localScale = Vector3.one * scale;
        cube.GetComponent<TetrisCube>().color = color;
        cube.GetComponent<TetrisCube>().renderer.material.color = color;
        columnBoxes.Add(cube.GetComponent<TetrisCube>());
        tetrisCubes.Add(cube.GetComponent<TetrisCube>());
        boxAmount++;


    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, Vector3.down * 100);
    }
}