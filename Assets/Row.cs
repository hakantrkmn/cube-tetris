using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public bool CheckForRow()
    {
        GetBoxes();
        var falseCount = 0;
        var trueCount = 0;
        if (rowBoxes.Count<2)
        {
            return false;
        }
        var color = rowBoxes[0].color;
        foreach (var box in rowBoxes)
        {
            
            if (box.color!=color)
            {
                falseCount++;
            }
            else
            {
                trueCount++;
            }
        }

        if (trueCount!= rowBoxes.Count)
        {
            return false;
        }
        foreach (var box in rowBoxes)
        {
            box.isDestroyed = true;
            EventManager.CubePainted(box);
        }

        var middlePoint = Vector3.zero;
        foreach (var box in rowBoxes)
        {
            middlePoint += box.transform.position;
        }
        middlePoint /= rowBoxes.Count;
        foreach (var box in rowBoxes)
        {
            box.DestroyCube(middlePoint);
        }

        return true;



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
