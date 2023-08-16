using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class TetrisCube : MonoBehaviour
{
    public Collider collider;
    public Rigidbody rb;
    public Renderer renderer;
    public Color color;
    public Vector3 bounds;

    public List<TetrisCube> neighbours;
    public List<TetrisCube> sameColorNeighbours;

    public LayerMask tetrisCubeLayer;

    public List<Vector3> directions;
    [Button]
    public void SetColor()
    {
        color = renderer.material.color;

    }
    [Button]
    public void RandomColor()
    {
        color = Random.ColorHSV();
            renderer.material.color = color;
    }
    [Button]
    public void SetBound()
    {
        bounds = collider.bounds.size;
    }

    public bool boxChecked;

    public void DestroyCube()
    {
        rb.isKinematic = true;
        collider.enabled = false;
        transform.DOScale(0, .2f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
    [Button]
    public void GetSameColorNeighbours(List<TetrisCube> sameColorCubes)
    {
        if (boxChecked) return;
        
        boxChecked = true;
        
        if (!sameColorCubes.Contains(this))
        {
            sameColorCubes.Add(this);
        }
        
        GetNeighbours();
            
        foreach (var neighbour in neighbours)
        {
            if (neighbour.color==color)
            {
                sameColorNeighbours.Add(neighbour);
                if (!sameColorCubes.Contains(neighbour))
                {
                    sameColorCubes.Add(neighbour);
                }
            }
        }

        foreach (var cube in sameColorNeighbours)
        {
            if (!cube.boxChecked)
            {
                cube.GetSameColorNeighbours(sameColorCubes);

            }
        }

    }

    public void GetNeighbours()
    {
        neighbours.Clear();

        foreach (var direction in directions)
        {
            var tempCube = GetBoxOnDirection(direction);
            if (tempCube && tempCube!=this)
            {
                neighbours.Add(tempCube);
            }
        }
    }

    private void Update()
    {
        if (rb.IsSleeping()) {
            rb.WakeUp();
        }
    }

    public TetrisCube GetBoxOnDirection(Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity,tetrisCubeLayer))
        {
            return hit.transform.GetComponent<TetrisCube>();
        }
        else
        {
            return null;
        }
    }
    
    
    
}
