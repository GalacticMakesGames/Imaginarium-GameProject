using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // This allows us to trigger actions in the Unity Inspector

public class ShapeRecognizer : MonoBehaviour
{
    // --- DATA CONTAINERS ---
    // Think of this class as a "Folder" that holds all the information for a single shape.
    [System.Serializable] // This tag makes the folder visible in the Unity Inspector
    public class ShapeTemplate
    {
        public string shapeName; // The name of the shape (e.g., "Fireball")
        public List<Vector2> points = new List<Vector2>(); // The 64 normalized points that make up the perfect shape
        public UnityEvent onShapeRecognized; // The button we press in Unity to trigger an action (like attacking)
    }

    // --- VARIABLES (Our Settings) ---
    [Header("Settings")]
    [Tooltip("How much leeway the player gets. Lower = must be more accurate. 0.3 is a good starting point.")]
    public float maxMatchDistance = 0.3f; // The highest "error score" allowed for a drawing to still count

    [Tooltip("Minimum distance mouse must move to register a new point.")]
    public float minPointDistance = 5f; // Stops the script from recording a million points if the player holds the mouse still

    public int resampleCount = 64; // The exact number of points EVERY drawing will be converted into

    [Header("Templates Library")]
    public List<ShapeTemplate> templates = new List<ShapeTemplate>(); // Our library of saved shapes

    [Header("Recording Mode")]
    [Tooltip("Enable to save your next drawn shape into the template library with this name.")]
    public bool isRecordingMode = false; // A switch to flip when we want to teach the game a new shape
    public string newTemplateName = "New Shape";

    // These variables keep track of the current drawing being made right now
    private List<Vector2> currentStroke = new List<Vector2>();
    private bool isDrawing = false;

    // --- THE MAIN LOOP ---
    // Update is called by Unity every single frame (often 60+ times a second)
    void Update()
    {
        // 1. If the player JUST pressed the left mouse button down...
        if (Input.GetMouseButtonDown(0))
        {
            currentStroke.Clear(); // Erase the old drawing memory
            isDrawing = true;      // Remember that we are currently drawing
            AddPoint(Input.mousePosition); // Record the starting point
        }
        // 2. If the player is HOLDING the mouse button and dragging...
        else if (Input.GetMouseButton(0) && isDrawing)
        {
            Vector2 mousePos = Input.mousePosition;
            // Only record a new point if the mouse has moved far enough away from the last point
            if (Vector2.Distance(currentStroke[currentStroke.Count - 1], mousePos) > minPointDistance)
            {
                AddPoint(mousePos);
            }
        }
        // 3. If the player LETS GO of the mouse button...
        else if (Input.GetMouseButtonUp(0) && isDrawing)
        {
            isDrawing = false; // Stop drawing
            ProcessStroke();   // Send the finished drawing to be analyzed
        }
    }

    // A simple helper function to add a point to our list
    private void AddPoint(Vector2 point)
    {
        currentStroke.Add(point);
    }

    // --- PROCESSING THE DRAWING ---
    // This is the "manager" function that directs traffic after the player lifts their mouse.
    private void ProcessStroke()
    {
        // If the player barely clicked and didn't draw a line, ignore it
        if (currentStroke.Count < 5) return;

        // Send the raw, messy drawing to the "cleanup crew" (NormalizeStroke)
        List<Vector2> normalizedStroke = NormalizeStroke(currentStroke);

        // If we are just trying to save a new shape for our game...
        if (isRecordingMode)
        {
            SaveTemplate(normalizedStroke);
            return; // Stop here, don't try to recognize it
        }

        // Otherwise, figure out what the player just drew
        RecognizeShape(normalizedStroke);
    }

    // This is the "cleanup crew". It runs the raw drawing through three filters.
    private List<Vector2> NormalizeStroke(List<Vector2> stroke)
    {
        List<Vector2> resampled = Resample(stroke, resampleCount); // Filter 1: Make it exactly 64 points
        ScaleToBox(resampled); // Filter 2: Make it a standard size
        TranslateToOrigin(resampled); // Filter 3: Center it on the screen
        return resampled;
    }

    // --- COMPARING TO OUR LIBRARY ---
    // This function grades the player's cleaned-up drawing against our saved templates.
    private void RecognizeShape(List<Vector2> normalizedStroke)
    {
        float bestDistance = float.MaxValue; // Start with an infinitely bad score
        ShapeTemplate bestMatch = null;      // Start with no match

        // Loop through every shape in our library one by one
        foreach (var template in templates)
        {
            if (template.points.Count == 0) continue; // Skip empty templates

            // Calculate the "error score" (distance) between the drawing and the template
            float distance = CalculateDistance(normalizedStroke, template.points);

            // If this score is better (lower) than our previous best score, remember it!
            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestMatch = template;
            }
        }

        // We checked all templates. Did the best match score lower than our strict max leeway?
        if (bestMatch != null && bestDistance < maxMatchDistance)
        {
            Debug.Log($"Shape Recognized: {bestMatch.shapeName} (Score: {bestDistance})");
            // Trigger the Unity event! This is what tells the game "Shoot the fireball!"
            bestMatch.onShapeRecognized?.Invoke();
        }
        else
        {
            // The drawing was too messy or didn't match anything closely enough.
            Debug.Log($"Shape not recognized. Closest was {(bestMatch != null ? bestMatch.shapeName : "None")} with distance {bestDistance}.");
        }
    }

    // --- THE MATH FILTERS ---

    // Filter 1: Resampling. 
    // Imagine the drawing is a piece of string. We want to place exactly 64 beads on this string, 
    // perfectly spaced out from start to finish, regardless of how fast the player drew it.

    private List<Vector2> Resample(List<Vector2> points, int n)
    {
        float interval = PathLength(points) / (n - 1); // Calculate the exact spacing between beads
        float D = 0;
        List<Vector2> resampled = new List<Vector2> { points[0] };

        for (int i = 1; i < points.Count; i++)
        {
            float d = Vector2.Distance(points[i - 1], points[i]);
            if ((D + d) >= interval)
            {
                // Calculate the exact coordinate for the new bead
                float qx = points[i - 1].x + ((interval - D) / d) * (points[i].x - points[i - 1].x);
                float qy = points[i - 1].y + ((interval - D) / d) * (points[i].y - points[i - 1].y);
                Vector2 q = new Vector2(qx, qy);
                resampled.Add(q);
                points.Insert(i, q);
                D = 0;
            }
            else
            {
                D += d;
            }
        }

        // Just in case the computer's decimal math is slightly off, ensure we have exactly 'n' points
        while (resampled.Count < n)
            resampled.Add(points[points.Count - 1]);

        return resampled;
    }

    // Calculates the total length of the drawing (how long the string is)
    private float PathLength(List<Vector2> points)
    {
        float d = 0;
        for (int i = 1; i < points.Count; i++)
            d += Vector2.Distance(points[i - 1], points[i]);
        return d;
    }

    // Filter 2: Scaling.
    // Imagine taking the drawing and squishing or stretching it until it perfectly fits inside a 1x1 square box.
    // This makes drawing a giant circle the exact same as drawing a tiny circle.

    private void ScaleToBox(List<Vector2> points)
    {
        // First, find the furthest edges of the drawing (top, bottom, left, right)
        float minX = float.MaxValue, maxX = float.MinValue, minY = float.MaxValue, maxY = float.MinValue;
        foreach (var p in points)
        {
            if (p.x < minX) minX = p.x;
            if (p.x > maxX) maxX = p.x;
            if (p.y < minY) minY = p.y;
            if (p.y > maxY) maxY = p.y;
        }

        float width = maxX - minX;
        float height = maxY - minY;

        // Prevent math errors if the player drew a perfectly straight horizontal or vertical line
        if (width == 0) width = 0.01f;
        if (height == 0) height = 0.01f;

        // Resize every point based on the width and height we found
        for (int i = 0; i < points.Count; i++)
        {
            points[i] = new Vector2((points[i].x - minX) / width, (points[i].y - minY) / height);
        }
    }

    // Filter 3: Translating.
    // This moves the drawing so that its exact center is at coordinate (0,0). 
    // This makes drawing in the top-left of the screen the same as drawing in the bottom-right.
    private void TranslateToOrigin(List<Vector2> points)
    {
        Vector2 centroid = Vector2.zero;
        foreach (var p in points) centroid += p; // Add up all the points
        centroid /= points.Count; // Divide to find the average center

        // Shift every point by that center amount
        for (int i = 0; i < points.Count; i++)
        {
            points[i] -= centroid;
        }
    }

    // The grader. This compares the distance between point 1 of the drawing and point 1 of the template, 
    // then point 2 to point 2, and so on, averaging out the difference.
    private float CalculateDistance(List<Vector2> p1, List<Vector2> p2)
    {
        float distance = 0;
        for (int i = 0; i < p1.Count; i++)
        {
            distance += Vector2.Distance(p1[i], p2[i]);
        }
        return distance / p1.Count;
    }

    // --- SAVING ---
    // Bundles up the cleaned 64 points and saves them to our list in the Inspector.
    private void SaveTemplate(List<Vector2> normalizedStroke)
    {
        ShapeTemplate newTemplate = new ShapeTemplate
        {
            shapeName = newTemplateName,
            points = new List<Vector2>(normalizedStroke)
        };
        templates.Add(newTemplate); // Add it to the library!
        isRecordingMode = false; // Turn off recording mode automatically
        Debug.Log($"Successfully recorded template: {newTemplateName}. Remember to copy component values or disable recording mode!");
    }
}