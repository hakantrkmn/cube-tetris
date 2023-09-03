using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility 
{
   public static Color MakeColorF1(Color color)
   {
      return color = new Color(float.Parse(color.r.ToString("F1")), float.Parse(color.g.ToString("F1")),
         float.Parse(color.b.ToString("F1")));
   }
}
