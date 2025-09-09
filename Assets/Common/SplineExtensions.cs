using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Common
{
    public static class SplineExtensions
    {
        public static Vector3[] GetPoints(this Spline spline)
        {
            var points = new List<Vector3>();

            for (var i = 0; i < spline.GetPointCount(); i++)
            {
                points.Add(spline.GetPosition(i));
            }
        
            return points.ToArray();
        }
    }
}