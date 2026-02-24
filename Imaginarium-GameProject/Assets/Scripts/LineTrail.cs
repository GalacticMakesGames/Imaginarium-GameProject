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
//[RequireComponent(typeof(LineRenderer))]
//public class LineTrail : MonoBehaviour
//{
//    // --- VARIABLES (Our Tools & Settings) ---
//    [Header("Components")]
//    public LineRenderer lineRenderer; // The tool that actually draws the ribbon
//    public Camera mainCamera;         // The camera we are looking through

//    [Header("Settings")]
//    [Tooltip("How far into the 3D world the line is drawn. Must be greater than 0 so the camera can see it!")]
//    public float distanceFromCamera = 10f;

//    [Tooltip("How far the mouse must move before we drop a new 'dot' to connect.")]
//    public float minPointDistance = 0.1f;

//    // Keeps track of whether the player's finger is currently held down
//    private bool isDrawing = false;

//    // --- SETUP ---
//    // Start runs exactly once when the game begins
//    void Start()
//    {
//        // Make sure the line starts completely empty (0 dots)
//        lineRenderer.positionCount = 0;
//    }

//    // --- THE MAIN LOOP ---
//    // Update checks the mouse every single frame
//    void Update()
//    {
//        // 1. If the player JUST clicked the left mouse button...
//        if (Input.GetMouseButtonDown(0))
//        {
//            isDrawing = true;
//            lineRenderer.positionCount = 0; // Erase the old drawing from the screen
//            AddPoint(); // Drop the very first dot exactly where the mouse is
//        }
//        // 2. If the player is HOLDING the button and dragging...
//        else if (Input.GetMouseButton(0) && isDrawing)
//        {
//            // Find out exactly where the mouse is right now in the 3D world
//            Vector3 currentPos = GetMouseWorldPosition();

//            // Find out where we dropped the VERY LAST dot (positionCount - 1 gets the last item in the list)
//            Vector3 lastPos = lineRenderer.GetPosition(lineRenderer.positionCount - 1);

//            // Have we moved the mouse far enough away from the last dot to justify dropping a new one?
//            // (If we drop a million dots in the exact same spot, the game will lag!)
//            if (Vector3.Distance(currentPos, lastPos) > minPointDistance)
//            {
//                AddPoint(); // Drop a new dot! The LineRenderer will instantly connect it to the previous one.
//            }
//        }
//        // 3. If the player LETS GO of the mouse button...
//        else if (Input.GetMouseButtonUp(0))
//        {
//            isDrawing = false; // Stop dropping dots
//        }
//    }

//    // --- HELPER FUNCTIONS ---

//    // Drops a new dot for the Line Renderer to connect
//    private void AddPoint()
//    {
//        Vector3 newPointPosition = GetMouseWorldPosition();

//        // Tell the LineRenderer we are adding 1 more dot to our total count
//        lineRenderer.positionCount++;

//        // Place that newly added dot at our mouse's 3D position
//        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPointPosition);
//    }

//    // The Magic Trick: Converting a 2D screen click into a 3D world position
//    private Vector3 GetMouseWorldPosition()
//    {
//        // Get the flat 2D pixel coordinates of the mouse (X and Y)
//        Vector3 mouseScreenPos = Input.mousePosition;

//        // The mouse doesn't have depth on a flat screen, so Z is 0. 
//        // We artificially give it depth by pushing it forward by 'distanceFromCamera'
//        mouseScreenPos.z = distanceFromCamera;

//        // Ask the camera to translate those modified screen pixels into true 3D space
//        return mainCamera.ScreenToWorldPoint(mouseScreenPos);
//    }
// }


[RequireComponent(typeof(LineRenderer))]
public class LineTrail : MonoBehaviour
{
    // --- COMPONENTS ---
    [Header("Components")]
    public LineRenderer lineRenderer;
    public Camera mainCamera;

    // --- SETTINGS ---
    [Header("Trail Settings")]
    public float distanceFromCamera = 10f;
    public float minPointDistance = 0.1f;

    [Header("Fading & Colors")]
    [Tooltip("The normal color of the trail when drawing.")]
    public Color defaultColor = Color.white;

    [Tooltip("The color the trail turns if a shape is successfully matched!")]
    public Color recognizedColor = Color.green;

    [Tooltip("How long to wait after letting go of the mouse before fading begins.")]
    public float timeBeforeFade = 1f;

    [Tooltip("How long the actual fading animation takes.")]
    public float fadeDuration = 1f;

    // --- PRIVATE MEMORY ---
    private bool isDrawing = false;

    // We keep a reference to our background helper so we can stop it if the player starts drawing again early
    private Coroutine fadeRoutine;

    void Start()
    {
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
        if (mainCamera == null) mainCamera = Camera.main;

        lineRenderer.positionCount = 0;
    }

    void Update()
    {
        // 1. Starting a new drawing
        if (Input.GetMouseButtonDown(0))
        {
            // If our background helper is currently fading an old line, tell it to stop!
            if (fadeRoutine != null)
            {
                StopCoroutine(fadeRoutine);
            }

            ResetLine(); // Clean up the old line and reset the colors

            isDrawing = true;
            AddPoint();
        }
        // 2. Dragging the mouse
        else if (Input.GetMouseButton(0) && isDrawing)
        {
            Vector3 currentPos = GetMouseWorldPosition();
            Vector3 lastPos = lineRenderer.GetPosition(lineRenderer.positionCount - 1);

            if (Vector3.Distance(currentPos, lastPos) > minPointDistance)
            {
                AddPoint();
            }
        }
        // 3. Letting go of the mouse
        else if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;

            // Hire our background helper to start the fading process!
            fadeRoutine = StartCoroutine(FadeAndClear());
        }
    }

    // --- HELPER FUNCTIONS ---

    private void AddPoint()
    {
        Vector3 newPointPosition = GetMouseWorldPosition();
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPointPosition);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = distanceFromCamera;
        return mainCamera.ScreenToWorldPoint(mouseScreenPos);
    }

    // Prepares the LineRenderer for a brand new, fresh drawing
    private void ResetLine()
    {
        lineRenderer.positionCount = 0;

        // Reset the colors back to default, with full solid alpha (transparency)
        lineRenderer.startColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 1f);
        lineRenderer.endColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 1f);
    }

    // --- THE MAGIC CONNECTION ---
    // This is a public function. We will tell our ShapeRecognizer script to press this "button"
    // whenever it successfully finds a match in your library!
    public void ShapeWasRecognized()
    {
        // Instantly snap the line to our success color!
        lineRenderer.startColor = new Color(recognizedColor.r, recognizedColor.g, recognizedColor.b, 1f);
        lineRenderer.endColor = new Color(recognizedColor.r, recognizedColor.g, recognizedColor.b, 1f);
    }

    // --- OUR BACKGROUND HELPER (COROUTINE) ---
    // IEnumerator is the special return type that tells Unity this is a Coroutine.
    private IEnumerator FadeAndClear()
    {
        // Step 1: Just chill and wait for a second so the player can admire their drawing
        // "yield return" means "pause this function right here and give control back to Unity"
        yield return new WaitForSeconds(timeBeforeFade);

        // Step 2: Slowly turn it invisible
        float elapsedTime = 0f; // A stopwatch to track how long we've been fading

        // Remember whatever colors the line currently is (whether default or recognized)
        Color startingStartColor = lineRenderer.startColor;
        Color startingEndColor = lineRenderer.endColor;

        // Loop this block of code until our stopwatch hits our fadeDuration target
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime; // Add the time it took to draw the last frame

            // Mathf.Lerp smoothly slides a number from A to B. 
            // We are sliding the Alpha (transparency) from 1 (solid) to 0 (invisible).
            float currentAlpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

            // Apply the new faded colors
            lineRenderer.startColor = new Color(startingStartColor.r, startingStartColor.g, startingStartColor.b, currentAlpha);
            lineRenderer.endColor = new Color(startingEndColor.r, startingEndColor.g, startingEndColor.b, currentAlpha);

            // Pause the loop here until the next frame, then continue
            yield return null;
        }

        // Step 3: The line is completely invisible now, so delete the points to save memory
        lineRenderer.positionCount = 0;
    }
}
