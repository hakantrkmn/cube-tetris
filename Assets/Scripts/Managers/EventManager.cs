using System;
using UnityEngine;


public static class EventManager
{
    
    

    #region InputSystem
    public static Func<Vector2> GetInput;
    public static Func<Vector2> GetInputDelta;
    public static Action InputStarted;
    public static Action InputEnded;
    public static Func<bool> IsTouching;
    public static Func<bool> IsPointerOverUI;
    #endregion


    public static Func<Vector3> GetBorders;

    public static Action<bool> PlayerCanClick;

    public static Action CheckForRows;
    public static Action<TetrisCube> SpawnCubeAtColumn;

    public static Action<TetrisCube> CubePainted;
    public static Action SpawnCubeOnColumns;



}