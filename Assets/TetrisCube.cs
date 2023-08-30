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

    public bool isDestroyed;
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
    public bool painted;

    public void DestroyCube()
    {
        //gameObject.SetActive(false);
        rb.isKinematic = true;
        collider.enabled = false;
        transform.DOScale(0, .2f).OnComplete(() => { gameObject.SetActive(false); });
        
    }

    [Button]
    public void GetSameColorNeighbours(List<TetrisCube> sameColorCubes,Color paintColor)
    {
        if (boxChecked) return;

        renderer.material.DOColor(paintColor, .3f);
        //transform.DOPunchScale(Vector3.one * .1f, .3f);
        boxChecked = true;

        if (!sameColorCubes.Contains(this))
        {
            sameColorCubes.Add(this);
        }

        GetNeighbours();

        foreach (var neighbour in neighbours)
        {
            if (neighbour.color == color)
            {
                sameColorNeighbours.Add(neighbour);
                if (!sameColorCubes.Contains(neighbour))
                {
                    sameColorCubes.Add(neighbour);
                }
            }
        }
        color=paintColor;


        DOVirtual.DelayedCall(.1f, () =>
        {
            foreach (var cube in sameColorNeighbours)
            {
                if (!cube.boxChecked)
                {
                    cube.GetSameColorNeighbours(sameColorCubes,paintColor);
                }
            }
        }).OnComplete(() =>
        {
            if (sameColorCubes.Last()==this)
            {
                EventManager.PlayerCanClick(true);
                
                EventManager.CheckForRows();
                

            }

        });
    }

    public void GetNeighbours()
    {
        neighbours.Clear();

        foreach (var direction in directions)
        {
            var tempCube = GetBoxOnDirection(direction);
            if (tempCube && tempCube != this)
            {
                neighbours.Add(tempCube);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<TetrisCube>())
        {
            //EventManager.CheckForRows();
        }
        
    }

    private void Update()
    {
        if (rb.IsSleeping())
        {
            rb.WakeUp();
        }
    }

    public TetrisCube GetBoxOnDirection(Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, transform.localScale.x, tetrisCubeLayer))
        {
            return hit.transform.GetComponent<TetrisCube>();
        }
        else
        {
            return null;
        }
    }
}