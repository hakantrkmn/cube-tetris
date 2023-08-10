using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Transform border;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnEnable()
    {
        EventManager.GetBorders += GetBorders;
    }

    private void OnDisable()
    {
        EventManager.GetBorders -= GetBorders;
    }

    [Button]
    public Vector3 GetBorders()
    {
        Ray ray = Camera.main.ScreenPointToRay(border.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
