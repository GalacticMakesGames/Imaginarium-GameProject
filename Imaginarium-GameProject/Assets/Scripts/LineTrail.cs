using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//public class LineTrail : MonoBehaviour
//{
//    public LineRenderer lineRenderer;

//    List<Vector2> points;

//    public void UpdateLine(Vector2 position)
//    {
//        if (points == null)
//        {
//            points = new List<Vector2>();
//            SetPoint(position);
//            return;
//        }

//        if (Vector2.Distance(points.Last(), position) > .1f)
//        {
//            SetPoint(position);
//        }
//    }

//    void SetPoint(Vector2 point)
//    {
//        points.Add(point);

//        lineRenderer.positionCount = points.Count; // tells the line renderer how many points you have and where they are
//        lineRenderer.SetPosition(points.Count - 1, point);

//    }
//}

// If you attach this script to an object, Unity will automatically add a LineRenderer for you.
[RequireComponent(typeof(LineRenderer))]
public class LineTrail : MonoBehaviour
{
    // --- VARIABLES (Our Tools & Settings) ---
    [Header("Components")]
    public LineRenderer lineRenderer; // The tool that actually draws the ribbon
    public Camera mainCamera;         // The camera we are looking through

    [Header("Settings")]
    [Tooltip("How far into the 3D world the line is drawn. Must be greater than 0 so the camera can see it!")]
    public float distanceFromCamera = 10f;

    [Tooltip("How far the mouse must move before we drop a new 'dot' to connect.")]
    public float minPointDistance = 0.1f;

    // Keeps track of whether the player's finger is currently held down
    private bool isDrawing = false;

    // --- SETUP ---
    // Start runs exactly once when the game begins
    void Start()
    {
        // Make sure the line starts completely empty (0 dots)
        lineRenderer.positionCount = 0;
    }

    // --- THE MAIN LOOP ---
    // Update checks the mouse every single frame
    void Update()
    {
        // 1. If the player JUST clicked the left mouse button...
        if (Input.GetMouseButtonDown(0))
        {
            isDrawing = true;
            lineRenderer.positionCount = 0; // Erase the old drawing from the screen
            AddPoint(); // Drop the very first dot exactly where the mouse is
        }
        // 2. If the player is HOLDING the button and dragging...
        else if (Input.GetMouseButton(0) && isDrawing)
        {
            // Find out exactly where the mouse is right now in the 3D world
            Vector3 currentPos = GetMouseWorldPosition();

            // Find out where we dropped the VERY LAST dot (positionCount - 1 gets the last item in the list)
            Vector3 lastPos = lineRenderer.GetPosition(lineRenderer.positionCount - 1);

            // Have we moved the mouse far enough away from the last dot to justify dropping a new one?
            // (If we drop a million dots in the exact same spot, the game will lag!)
            if (Vector3.Distance(currentPos, lastPos) > minPointDistance)
            {
                AddPoint(); // Drop a new dot! The LineRenderer will instantly connect it to the previous one.
            }
        }
        // 3. If the player LETS GO of the mouse button...
        else if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false; // Stop dropping dots
        }
    }

    // --- HELPER FUNCTIONS ---

    // Drops a new dot for the Line Renderer to connect
    private void AddPoint()
    {
        Vector3 newPointPosition = GetMouseWorldPosition();

        // Tell the LineRenderer we are adding 1 more dot to our total count
        lineRenderer.positionCount++;

        // Place that newly added dot at our mouse's 3D position
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPointPosition);
    }

    // The Magic Trick: Converting a 2D screen click into a 3D world position
    private Vector3 GetMouseWorldPosition()
    {
        // Get the flat 2D pixel coordinates of the mouse (X and Y)
        Vector3 mouseScreenPos = Input.mousePosition;

        // The mouse doesn't have depth on a flat screen, so Z is 0. 
        // We artificially give it depth by pushing it forward by 'distanceFromCamera'
        mouseScreenPos.z = distanceFromCamera;

        // Ask the camera to translate those modified screen pixels into true 3D space
        return mainCamera.ScreenToWorldPoint(mouseScreenPos);
    }
}
