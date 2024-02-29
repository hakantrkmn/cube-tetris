using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CubePool : MonoBehaviour
{
    public TetrisCube cubePrefab;

    public int poolSize;
    public ObjectPool<TetrisCube> pool;

    public TetrisCreator tetrisCreator;
    public Transform tetrisCubesParent;

    private void OnEnable()
    {
        EventManager.GetCubeFromPool += GetCubeFromPool;
        EventManager.DestroyCube += DestroyCube;
    }

    private TetrisCube GetCubeFromPool()
    {
        return pool.Get();
    }

    private void DestroyCube(TetrisCube obj)
    {
        pool.Release(obj);
    }

    private void OnDisable()
    {
        EventManager.GetCubeFromPool -= GetCubeFromPool;
        EventManager.DestroyCube -= DestroyCube;
    }

    private void Start()
    {
        poolSize = (int)(tetrisCreator.tetrisSize.x * tetrisCreator.tetrisSize.y) * 2;
        pool = new ObjectPool<TetrisCube>(CreateCube, GetCube, ReleaseCube, OnDestroyCube, false, poolSize, poolSize);

        tetrisCreator.CreateTetris();
    }

    public TetrisCube CreateCube()
    {
        return Instantiate(cubePrefab, tetrisCubesParent);
    }

    public void OnDestroyCube(TetrisCube cube)
    {
        Destroy(cube.gameObject);
    }

    public void GetCube(TetrisCube cube)
    {
        cube.gameObject.SetActive(true);
    }

    public void ReleaseCube(TetrisCube cube)
    {
        Debug.Log("sdgsdf");
        cube.rb.angularVelocity = Vector3.zero;
        cube.rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ |
                              RigidbodyConstraints.FreezePositionX;
        cube.rb.velocity = Vector3.zero;
        cube.collider.enabled = true;
        cube.transform.rotation = Quaternion.identity;
        cube.isDestroyed = false;
        cube.boxChecked = false;
        cube.transform.position = new Vector3(30, 0, 0);
        cube.gameObject.SetActive(false);
        
    }
}