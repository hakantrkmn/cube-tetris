using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Row : MonoBehaviour
{
    public List<TetrisCube> rowBoxes;
    public LayerMask tetrisCubeLayer;
    public int columnMaxBoxAmount;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position,Vector3.right*100);
    }

    public void CheckForRow()
    {
        GetBoxes();
        if (rowBoxes.Count==0)
        {
            return;
        }
        var color = rowBoxes[0].color;
        foreach (var box in rowBoxes)
        {
            if (box.color!=color)
            {
                return;
            }
        }

        if (rowBoxes.Count!=columnMaxBoxAmount)
        {
            return;
        }
        foreach (var box in rowBoxes)
        {
            box.isDestroyed = true;
            EventManager.CubePainted(box);
        }
        foreach (var box in rowBoxes)
        {
            box.DestroyCube();
        }
        


    }
    [Button]
    public void GetBoxes()
    {
        rowBoxes.Clear();
        var temp = Physics.RaycastAll(transform.position, Vector3.right, Mathf.Infinity, tetrisCubeLayer);
        foreach (var hit in temp)
        {
            if (hit.transform.GetComponent<TetrisCube>())
            {
                rowBoxes.Add(hit.transform.GetComponent<TetrisCube>());

            }
        }
    }
}
