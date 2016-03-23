using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using LitJson;

public class Gestures {
    private string jsonName;
    private string persistentJsonPath;
    private List<Gesture> gestures = new List<Gesture>();

    public List<Gesture> GesturesList { get { return gestures; } }

    public Gestures(string _jsonName) {
        jsonName = _jsonName;
        persistentJsonPath = System.IO.Path.Combine(Application.persistentDataPath, jsonName + ".json");
        if (!File.Exists(persistentJsonPath)) {
            string fileContents = Resources.Load<TextAsset>(jsonName).text;
            File.WriteAllText(persistentJsonPath, fileContents);
        }
        LoadJson();
    }

    public void LoadJson() {
        //string jsonString = Resources.Load<TextAsset>(jsonName).text;
        string jsonString = File.ReadAllText(persistentJsonPath, System.Text.Encoding.Default);
        JsonReader reader = new JsonReader(jsonString);
        JsonData data = JsonMapper.ToObject(reader);
        for (int i = 0; i < data.Count; i++) {
            List<Vector2> gesturePoints = new List<Vector2>();
            for (int j = 0; j < data[i].Count; j++) {
                Vector2 gesturePoint = new Vector2();
                gesturePoint.x = (float)System.Convert.ToDouble(data[i][j]["x"].ToString ());
                gesturePoint.y = (float)System.Convert.ToDouble(data[i][j]["y"].ToString ());
                gesturePoints.Add(gesturePoint);
            }
            Gesture gesture = new Gesture(gesturePoints);
            gestures.Add(gesture);
        }
    }

    public void SaveJson() {
        JsonWriter writer = new JsonWriter();
        writer.WriteArrayStart();
        for (int i = 0; i < gestures.Count; i++) {
            List<Vector2> points = gestures[i].points;
            writer.WriteArrayStart();
            for (int j = 0; j < points.Count; j++) {
                writer.WriteObjectStart();
                writer.WritePropertyName("x");
                writer.Write(points[j].x);
                writer.WritePropertyName("y");
                writer.Write(points[j].y);
                writer.WriteObjectEnd();
            }
            writer.WriteArrayEnd();
        }
        writer.WriteArrayEnd();

        try {
            File.WriteAllText(persistentJsonPath, writer.ToString());
        } catch (Exception e) {
            Debug.Log(e.Message);
        }
    }
}