using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShapeRecognizer : MonoBehaviour
{
    // --- DATA CONTAINERS ---
    [System.Serializable]
    public class ShapeTemplate
    {
        public string shapeName;
        public List<Vector2> points = new List<Vector2>();

        public string requiredTargetTag;

        [Tooltip("Leave empty if this shape is always unlocked. Otherwise, type the exact name of the item required in the inventory.")]
        public string requiredItemName; // NEW: The name of the unlockable ability item!

        public UnityEvent onShapeRecognized;
    }

    [Header("Components")]
    public Camera mainCamera;
    public InventoryManager inventoryManager;

    [Header("Settings")]
    public float maxMatchDistance = 0.3f;
    public float minPointDistance = 5f;
    public int resampleCount = 64;

    [Header("Templates Library")]
    public List<ShapeTemplate> templates = new List<ShapeTemplate>();

    [Header("Recording Mode")]
    public bool isRecordingMode = false;
    public string newTemplateName = "New Shape";

    private List<Vector2> currentStroke = new List<Vector2>();
    private bool isDrawing = false;

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentStroke.Clear();
            isDrawing = true;
            AddPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) && isDrawing)
        {
            Vector2 mousePos = Input.mousePosition;
            if (Vector2.Distance(currentStroke[currentStroke.Count - 1], mousePos) > minPointDistance)
            {
                AddPoint(mousePos);
            }
        }
        else if (Input.GetMouseButtonUp(0) && isDrawing)
        {
            isDrawing = false;
            ProcessStroke();
        }
    }

    private void AddPoint(Vector2 point)
    {
        currentStroke.Add(point);
    }

    private void ProcessStroke()
    {
        if (currentStroke.Count < 5) return;

        List<Vector2> normalizedStroke = NormalizeStroke(currentStroke);

        if (isRecordingMode)
        {
            SaveTemplate(normalizedStroke);
            return;
        }

        RecognizeShape(normalizedStroke);
    }

    private List<Vector2> NormalizeStroke(List<Vector2> stroke)
    {
        List<Vector2> resampled = Resample(stroke, resampleCount);
        ScaleToBox(resampled);
        TranslateToOrigin(resampled);
        return resampled;
    }

    // --- COMPARING & CHECKING CONTEXT ---
    private void RecognizeShape(List<Vector2> normalizedStroke)
    {
        float bestDistance = float.MaxValue;
        ShapeTemplate bestMatch = null;

        foreach (var template in templates)
        {
            if (template.points.Count == 0) continue;

            float distance = CalculateDistance(normalizedStroke, template.points);

            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestMatch = template;
            }
        }

        // 1. Did they draw a recognized shape?
        if (bestMatch != null && bestDistance < maxMatchDistance)
        {
            // 2. NEW: Do they have the required item unlocked in their inventory?
            if (!string.IsNullOrEmpty(bestMatch.requiredItemName))
            {
                // If they DON'T have the item, stop immediately.
                if (CheckInventoryForAbility(bestMatch.requiredItemName) == false)
                {
                    Debug.Log($"Shape '{bestMatch.shapeName}' recognized, but you don't have the '{bestMatch.requiredItemName}' item yet!");
                    return;
                }
            }

            // 3. Does this shape require a specific target?
            if (string.IsNullOrEmpty(bestMatch.requiredTargetTag) || CheckIfDrawnOverTarget(currentStroke, bestMatch.requiredTargetTag))
            {
                Debug.Log($"Success! Cast {bestMatch.shapeName} on target.");
                bestMatch.onShapeRecognized?.Invoke();
            }
            else
            {
                Debug.Log($"Shape '{bestMatch.shapeName}' recognized, but was not drawn over a '{bestMatch.requiredTargetTag}'.");
            }
        }
        else
        {
            Debug.Log("Shape not recognized.");
        }
    }

    // --- THE INVENTORY BRIDGE ---
    private bool CheckInventoryForAbility(string itemName)
    {
        // Ask the InventoryManager our Yes/No question!
        return inventoryManager.CheckIfHasItem(itemName);
    }

    // --- MATH, RAYCASTING & SAVING HELPERS (Unchanged) ---
    private bool CheckIfDrawnOverTarget(List<Vector2> rawStroke, string requiredTag)
    {
        int step = Mathf.Max(1, rawStroke.Count / 20);
        for (int i = 0; i < rawStroke.Count; i += step)
        {
            Ray laser = mainCamera.ScreenPointToRay(rawStroke[i]);
            RaycastHit hit;
            if (Physics.Raycast(laser, out hit, 1000f))
            {
                if (hit.collider.CompareTag(requiredTag)) return true;
            }
        }
        return false;
    }

    private List<Vector2> Resample(List<Vector2> points, int n)
    {
        float interval = PathLength(points) / (n - 1); float D = 0;
        List<Vector2> resampled = new List<Vector2> { points[0] };
        for (int i = 1; i < points.Count; i++)
        {
            float d = Vector2.Distance(points[i - 1], points[i]);
            if ((D + d) >= interval)
            {
                float qx = points[i - 1].x + ((interval - D) / d) * (points[i].x - points[i - 1].x);
                float qy = points[i - 1].y + ((interval - D) / d) * (points[i].y - points[i - 1].y);
                Vector2 q = new Vector2(qx, qy); resampled.Add(q); points.Insert(i, q); D = 0;
            }
            else { D += d; }
        }
        while (resampled.Count < n) resampled.Add(points[points.Count - 1]);
        return resampled;
    }

    private float PathLength(List<Vector2> points)
    {
        float d = 0; for (int i = 1; i < points.Count; i++) d += Vector2.Distance(points[i - 1], points[i]); return d;
    }

    private void ScaleToBox(List<Vector2> points)
    {
        float minX = float.MaxValue, maxX = float.MinValue, minY = float.MaxValue, maxY = float.MinValue;
        foreach (var p in points) { if (p.x < minX) minX = p.x; if (p.x > maxX) maxX = p.x; if (p.y < minY) minY = p.y; if (p.y > maxY) maxY = p.y; }
        float width = maxX - minX; float height = maxY - minY;
        if (width == 0) width = 0.01f; if (height == 0) height = 0.01f;
        for (int i = 0; i < points.Count; i++) points[i] = new Vector2((points[i].x - minX) / width, (points[i].y - minY) / height);
    }

    private void TranslateToOrigin(List<Vector2> points)
    {
        Vector2 centroid = Vector2.zero;
        foreach (var p in points) centroid += p; centroid /= points.Count;
        for (int i = 0; i < points.Count; i++) points[i] -= centroid;
    }

    private float CalculateDistance(List<Vector2> p1, List<Vector2> p2)
    {
        float distance = 0;
        for (int i = 0; i < p1.Count; i++) distance += Vector2.Distance(p1[i], p2[i]);
        return distance / p1.Count;
    }

    private void SaveTemplate(List<Vector2> normalizedStroke)
    {
        ShapeTemplate newTemplate = new ShapeTemplate { shapeName = newTemplateName, points = new List<Vector2>(normalizedStroke), requiredTargetTag = "", requiredItemName = "" };
        templates.Add(newTemplate);
        isRecordingMode = false;
        Debug.Log($"Successfully recorded template: {newTemplateName}. Remember to copy component values or disable recording mode!");
    }
}