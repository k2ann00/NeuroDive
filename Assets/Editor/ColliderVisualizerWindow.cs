#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ColliderVisualizerWindow : EditorWindow
{
    [Serializable]
    private class TrackedCollider
    {
        public Collider collider;
        public Color color = Color.green;
        public bool drawSolid = false;
        [Range(0f, 1f)] public float solidAlpha = 0.1f;
        public bool enabled = true;
    }

    private List<TrackedCollider> colliders = new();
    private bool drawInPlayMode = true;
    private bool autoClearMissing = true;

    [MenuItem("Tools/Collider Visualizer")]
    public static void ShowWindow()
    {
        GetWindow<ColliderVisualizerWindow>("Collider Visualizer");
    }

    private void OnGUI()
    {
        drawInPlayMode = EditorGUILayout.ToggleLeft("Draw in Play Mode", drawInPlayMode);
        autoClearMissing = EditorGUILayout.ToggleLeft("Auto Clear Missing Colliders", autoClearMissing);

        EditorGUILayout.Space();

        if (GUILayout.Button("Add Selected Colliders"))
        {
            foreach (var go in Selection.gameObjects)
            {
                var cols = go.GetComponentsInChildren<Collider>(true);
                foreach (var col in cols)
                {
                    if (colliders.Exists(tc => tc.collider == col)) continue;

                    colliders.Add(new TrackedCollider
                    {
                        collider = col,
                        color = UnityEngine.Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f),
                        drawSolid = false,
                        solidAlpha = 0.1f,
                        enabled = true
                    });
                }
            }
        }

        if (GUILayout.Button("Clear All")) colliders.Clear();

        if (GUILayout.Button("Clear Nulls"))
            colliders.RemoveAll(c => c.collider == null);

        EditorGUILayout.Space(10);

        for (int i = 0; i < colliders.Count; i++)
        {
            var tc = colliders[i];
            if (tc == null) continue;

            using (new EditorGUILayout.VerticalScope("box"))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    tc.enabled = EditorGUILayout.Toggle(tc.enabled, GUILayout.Width(18));
                    tc.collider = (Collider)EditorGUILayout.ObjectField(tc.collider, typeof(Collider), true);
                    tc.color = EditorGUILayout.ColorField(GUIContent.none, tc.color, true, true, false, GUILayout.Width(60));

                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        colliders.RemoveAt(i);
                        break;
                    }
                }

                using (new EditorGUI.IndentLevelScope())
                {
                    tc.drawSolid = EditorGUILayout.ToggleLeft("Draw Solid", tc.drawSolid);
                    if (tc.drawSolid)
                    {
                        tc.solidAlpha = EditorGUILayout.Slider("Solid Alpha", tc.solidAlpha, 0f, 1f);
                    }
                }
            }
        }
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (!drawInPlayMode && Application.isPlaying) return;

        foreach (var tc in colliders)
        {
            if (tc == null || !tc.enabled || tc.collider == null) continue;
            DrawOne(tc);
        }

        if (autoClearMissing)
            colliders.RemoveAll(c => c.collider == null);
    }

    private void OnEnable() => SceneView.duringSceneGui += OnSceneGUI;
    private void OnDisable() => SceneView.duringSceneGui -= OnSceneGUI;

    private void DrawOne(TrackedCollider tc)
    {
        var col = tc.color;
        var cWire = col;
        var cSolid = new Color(col.r, col.g, col.b, tc.solidAlpha);
        var c = tc.collider;

        if (c is BoxCollider box)
        {
            Matrix4x4 m = Matrix4x4.TRS(
                box.transform.TransformPoint(box.center),
                box.transform.rotation,
                box.transform.lossyScale
            );

            using (new HandlesColorScope(cWire))
            using (new HandlesMatrixScope(m))
            {
                Handles.DrawWireCube(Vector3.zero, box.size);
                if (tc.drawSolid)
                {
                    Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
                    Handles.DrawSolidRectangleWithOutline(GetBoxVerts(box.size), cSolid, Color.clear);
                }
            }
        }
        else if (c is SphereCollider sphere)
        {
            float radius = sphere.radius * MaxAbsComponent(sphere.transform.lossyScale);
            Vector3 center = sphere.transform.TransformPoint(sphere.center);

            using (new HandlesColorScope(cWire))
            {
                Handles.DrawWireDisc(center, sphere.transform.right, radius);
                Handles.DrawWireDisc(center, sphere.transform.up, radius);
                Handles.DrawWireDisc(center, sphere.transform.forward, radius);

                if (tc.drawSolid)
                    Handles.SphereHandleCap(0, center, Quaternion.identity, radius * 2f, EventType.Repaint);
            }
        }
        else if (c is CapsuleCollider cap)
        {
            DrawCapsuleCollider(cap, cWire, cSolid, tc.drawSolid);
        }
        else if (c is MeshCollider meshCol && meshCol.sharedMesh != null)
        {
            var mesh = meshCol.sharedMesh;
            Matrix4x4 m = meshCol.transform.localToWorldMatrix;

            using (new HandlesColorScope(cWire))
            using (new HandlesMatrixScope(m))
            {
                Graphics.DrawMeshNow(mesh, m);
            }
        }
    }

    private static Vector3[] GetBoxVerts(Vector3 size)
    {
        Vector3 hs = size * 0.5f;
        return new Vector3[]
        {
            new Vector3(-hs.x, -hs.y, -hs.z),
            new Vector3(hs.x, -hs.y, -hs.z),
            new Vector3(hs.x, hs.y, -hs.z),
            new Vector3(-hs.x, hs.y, -hs.z)
        };
    }

    private static void DrawCapsuleCollider(CapsuleCollider cap, Color wireColor, Color solidColor, bool drawSolid)
    {
        Transform t = cap.transform;
        Vector3 center = t.TransformPoint(cap.center);

        int dir = cap.direction;
        Vector3 axis = (dir == 0 ? t.right : dir == 1 ? t.up : t.forward).normalized;

        float r = cap.radius * GetCapsuleRadiusScale(t.lossyScale, dir);
        float h = cap.height * Mathf.Abs(GetAxisScale(t.lossyScale, dir));
        float cylinderHalf = Mathf.Max(0, (h * 0.5f) - r);

        Vector3 p1 = center + axis * cylinderHalf;
        Vector3 p2 = center - axis * cylinderHalf;

        Vector3 right, up;
        GetPerpendicularVectors(axis, out right, out up);

        using (new HandlesColorScope(wireColor))
        {
            Handles.DrawWireDisc(p1, right, r);
            Handles.DrawWireDisc(p1, up, r);
            Handles.DrawWireDisc(p2, right, r);
            Handles.DrawWireDisc(p2, up, r);

            Handles.DrawLine(p1 + right * r, p2 + right * r);
            Handles.DrawLine(p1 - right * r, p2 - right * r);
            Handles.DrawLine(p1 + up * r, p2 + up * r);
            Handles.DrawLine(p1 - up * r, p2 - up * r);

            if (drawSolid)
            {
                Handles.SphereHandleCap(0, p1, Quaternion.identity, r * 2f, EventType.Repaint);
                Handles.SphereHandleCap(0, p2, Quaternion.identity, r * 2f, EventType.Repaint);
            }
        }
    }

    private static void GetPerpendicularVectors(Vector3 axis, out Vector3 right, out Vector3 up)
    {
        if (Mathf.Abs(axis.y) < 0.99f)
            right = Vector3.Cross(axis, Vector3.up).normalized;
        else
            right = Vector3.Cross(axis, Vector3.right).normalized;
        up = Vector3.Cross(axis, right).normalized;
    }

    private static float GetAxisScale(Vector3 scale, int dir) =>
        dir == 0 ? scale.x : dir == 1 ? scale.y : scale.z;

    private static float GetCapsuleRadiusScale(Vector3 scale, int dir) => dir switch
    {
        0 => Mathf.Max(scale.y, scale.z),
        1 => Mathf.Max(scale.x, scale.z),
        _ => Mathf.Max(scale.x, scale.y),
    };

    private static float MaxAbsComponent(Vector3 v) => Mathf.Max(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));

    private readonly struct HandlesColorScope : IDisposable
    {
        private readonly Color old;
        public HandlesColorScope(Color c) { old = Handles.color; Handles.color = c; }
        public void Dispose() => Handles.color = old;
    }

    private readonly struct HandlesMatrixScope : IDisposable
    {
        private readonly Matrix4x4 old;
        public HandlesMatrixScope(Matrix4x4 m) { old = Handles.matrix; Handles.matrix = m; }
        public void Dispose() => Handles.matrix = old;
    }
}
#endif
