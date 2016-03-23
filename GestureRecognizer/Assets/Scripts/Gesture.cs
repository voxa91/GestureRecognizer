using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gesture {
    public List<Vector2> points;

    private const int NumPoints = 64;
    private const float SquareSize = 250f;

    public Gesture(List<Vector2> _points) {
        //name = _name;
        points = _points;
        points = Resample();
        points = ScaleToSquadSize();
        points = TranslateToCenter();
    }

    public List<Vector2> Resample() {
        float length = 0;
        for (int i = 1; i < points.Count; i++) {
            length += Vector2.Distance(points[i - 1], points[i]);
        }
        float I = length / (NumPoints - 1);
        float D = 0.0f;

        List<Vector2> resampledPoints = new List<Vector2>();
        resampledPoints.Add(points[0]);

        for (int i = 1; i < points.Count; i++) {
            float d = Vector2.Distance(points[i - 1], points[i]);
            if (D + d >= I) {
                float x = points[i - 1].x + ((I - D) / d) * (points[i].x - points[i - 1].x);
                float y = points[i - 1].y + ((I - D) / d) * (points[i].y - points[i - 1].y);
                Vector2 q = new Vector2(x, y);
                resampledPoints.Add(q);
                this.points.Insert(i, q);
                D = 0.0f;

            } else {
                D += d;
            }
        }
        return resampledPoints;
    }

    private Vector2 CalculateScale() {
        Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 max = new Vector2(float.MinValue, float.MinValue);
        for (int i = 0; i < points.Count; i++) {
            min = Vector2.Min(min, points[i]);
            max = Vector2.Max(max, points[i]);
        }
        Vector2 scale = Vector2.one;
        if (max.x - min.x != 0) { 
            scale.x = SquareSize / (max.x - min.x);
        }
        if (max.y - min.y != 0) {
            scale.y = SquareSize / (max.y - min.y);
        }
        return scale;
    }

    public List<Vector2> ScaleToSquadSize() {
        Vector2 scale = CalculateScale();
        List<Vector2> scaledPoints = new List<Vector2>();
        for (int i = 0; i < points.Count; i++) {
            float x = points[i].x * scale.x;
            float y = points[i].y * scale.y;
            scaledPoints.Add(new Vector2(x, y));
        }
        return scaledPoints;
    }

    public List<Vector2> TranslateToCenter() {
        Vector2 center = Vector2.zero;
        for (int i = 0; i < points.Count; i++) {
            center += points[i];
        }
        center /= points.Count;
        List<Vector2> translatedPoints = new List<Vector2>();
        for (int i = 0; i < points.Count; i++) {
            float x = points[i].x - center.x;
            float y = points[i].y - center.y;
            translatedPoints.Add(new Vector2(x, y));
        }
        return translatedPoints;
    }

    private static List<Vector2> ShiftPoints(List<Vector2> points, int shift) { 
        List<Vector2> newPoints = new List<Vector2> ();
        for (int i = shift; i < points.Count; i++) {
            newPoints.Add(points[i]);
        }
        for (int i = 0; i < shift; i++) {
            newPoints.Add(points[i]);
        }
        return newPoints;
    }

    public static float GetDistanceBetweenPaths(List<Vector2> path_1, List<Vector2> path_2) {
        float distance = 0;
        int count = Mathf.Min(path_1.Count, path_2.Count);
        for (int i = 0; i < count; i++) {
            distance += Vector2.Distance(path_1[i], path_2[i]);
        }
        return distance / count;
    }

    public static float CompareGestures(Gesture gesture_1, Gesture gesture_2) {
        float d = Gesture.GetDistanceBetweenPaths(gesture_1.points, gesture_2.points);
        int shift = 3;
        List<Vector2> shiftPoints = gesture_2.points;
        for (int i = 0; i < gesture_2.points.Count / shift; i++) {
            shiftPoints = Gesture.ShiftPoints(shiftPoints, shift);
            d = Mathf.Min(d, Gesture.GetDistanceBetweenPaths(gesture_1.points, shiftPoints));
        }

        float Diagonal = Mathf.Sqrt(SquareSize * SquareSize + SquareSize * SquareSize);
        float HalfDiagonal = Diagonal * 0.5f;
        return 1f - d / HalfDiagonal;
    }
}
