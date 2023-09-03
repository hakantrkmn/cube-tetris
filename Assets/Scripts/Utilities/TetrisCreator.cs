using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class TetrisCreator : MonoBehaviour
{
    public TetrisCube tetrisCubePrefab;
    public GameObject columnPointPrefab;
    public GameObject rowPointPrefab;

    [HideInInspector]public List<TetrisCube> tetrisCubes;
    [HideInInspector]public List<Column> columnPoints;
    [HideInInspector]public List<Transform> rowPositions;

    public Transform ground;
    public Transform spawnPointsParent;
    public Transform tetrisCubesParent;
    public Transform rowParent;

    public Vector2 tetrisSize;

    float tetrisCubeScale;

    public List<Color> colors;

    private void OnValidate()
    {
        for (int i = 0; i < colors.Count; i++)
        {
            colors[i] = Utility.MakeColorF1(colors[i]);
        }
    }

    private void OnEnable()
    {
        EventManager.CheckForRows += CheckForRows;
        EventManager.CubePainted += CubePainted;
    }


    private void CheckForRows()
    {
        var mergeCount = rowPositions.Count(row => !row.GetComponent<Row>().CheckForRow());


        if (mergeCount != rowPositions.Count) return;


        foreach (var col in columnPoints)
        {
            col.GetBoxes();

            col.SpawnCube(tetrisCubePrefab.gameObject, tetrisCubesParent, tetrisCubeScale,
                colors[Random.Range(0, colors.Count)], tetrisCubes);
        }
    }

    private void CubePainted(TetrisCube obj)
    {
        tetrisCubes.Remove(obj);
    }

    private void OnDisable()
    {
        EventManager.CheckForRows -= CheckForRows;
        EventManager.CubePainted -= CubePainted;
    }


    void CreateTetris()
    {
        foreach (var cube in tetrisCubes)
        {
            Destroy(cube);
        }

        tetrisCubes.Clear();


        var border = Vector3.zero;
        border.x = (1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).x - .5f)) / 2;
        border.y = (1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).y - .5f)) / 2;
        border -= new Vector3(.5f, .5f, 0);
        var xScale = (border.x * 2) / tetrisSize.x;
        var yScale = (border.y * 2) / tetrisSize.y;
        if (yScale < xScale)
        {
            xScale = yScale;
        }
        else
        {
            yScale = xScale;
        }

        var bounds = new Vector2(xScale / 2, yScale / 2);

        tetrisCubeScale = xScale;

        var xAmount = tetrisSize.x;
        var yAmount = tetrisSize.y;


        ground.transform.position = new Vector3(ground.transform.position.x, -border.y, ground.transform.position.z);

        CreateRowAndColumnTransforms((int)xAmount, (int)yAmount, border, bounds);


        foreach (var column in columnPoints)
        {
            for (int i = 0; i < column.columnMaxBoxAmount; i++)
            {
                var cube = Instantiate(tetrisCubePrefab.gameObject, Vector3.zero, quaternion.identity,
                    tetrisCubesParent);
                cube.transform.localPosition =
                    new Vector3(column.transform.position.x,
                        column.transform.position.y + (i * bounds.y * 2), 0);
                cube.transform.localScale = new Vector3(xScale, xScale, xScale);
                var color = colors[Random.Range(0, colors.Count)];
                cube.GetComponent<TetrisCube>().color = color;
                cube.GetComponent<TetrisCube>().renderer.material.color = color;

                tetrisCubes.Add(cube.GetComponent<TetrisCube>());
            }
        }
    }

    void CreateRowAndColumnTransforms(int xAmount, int yAmount, Vector3 border, Vector2 bounds)
    {
        for (int i = 0; i < yAmount; i++)
        {
            var row = Instantiate(rowPointPrefab, Vector3.zero, quaternion.identity, rowParent);
            row.transform.SetParent(rowParent);
            row.transform.position = new Vector3(ground.transform.position.x - 10,
                (-border.y + tetrisCubeScale / 2) + i * tetrisCubeScale, ground.transform.position.z);
            rowPositions.Add(row.transform);
            row.GetComponent<Row>().columnMaxBoxAmount = (int)xAmount;
        }

        for (int i = 0; i < xAmount; i++)
        {
            var column = Instantiate(columnPointPrefab, Vector3.zero, quaternion.identity, spawnPointsParent);
            column.name = "Column_Point_" + i;
            column.transform.SetParent(spawnPointsParent);
            column.transform.localPosition = new Vector3((i * bounds.x * 2) - (bounds.x * (xAmount - 1)), border.y, 0);
            columnPoints.Add(column.GetComponent<Column>());
            column.GetComponent<Column>().columnMaxBoxAmount = yAmount;
        }
    }

    private void Start()
    {
        CreateTetris();
    }
}