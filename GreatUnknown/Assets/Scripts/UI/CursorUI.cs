using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CursorUI : MonoBehaviour
{
    private static CursorUI instance; //singleton
    // RectTransform of the Image (the fish sprite), set in Awake automatically
    private RectTransform cursorTransform;
    // The CursorCanvas sitting above this Image in the hierarchy
    private Canvas parentCanvas;
    private RectTransform canvasRectTransform; //parent
    // Null if Screen Space Overlay, otherwise the canvas's assigned camera
    private Camera canvasCamera;

    private void Awake()
    {
        // If a CursorUI already exists in another scene, destroy this duplicate
        if (instance != null && instance != this)
        {
            Destroy(transform.root.gameObject);
            return;
        }
        instance = this;
        
        // Persist the entire CursorCanvas (the root) across scene loads
        // transform.root climbs up to CursorCanvas, so the whole thing survives
        // Persist the root object (the Canvas) across scene loads
        DontDestroyOnLoad(transform.root.gameObject);
        cursorTransform = GetComponent<RectTransform>(); // from current object
        // Find and store canvas references on first run
        RefreshCanvasReferences();
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // After a scene loads, the camera may have changed (or been destroyed).
        // Refresh so we're always pointing at the correct camera.
        RefreshCanvasReferences();
        // Re-position immediately in the new scene for the same reason as OnEnable
        if (Mouse.current != null)
        {
            PositionCursor(Mouse.current.position.ReadValue());
        }
    }

    private void RefreshCanvasReferences()
    {
        // Walk up the hierarchy to find the CursorCanvas 
        parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas != null)
        {
            canvasRectTransform = parentCanvas.GetComponent<RectTransform>();
            canvasCamera = parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : parentCanvas.worldCamera;
        }
    }
    
    void PositionCursor(Vector2 mousePosition)
    {
        // Safety check — both must exist before we try to position anything
        if (cursorTransform == null || canvasRectTransform == null) return;

        // Convert screen-space mouse position into local canvas position
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
         canvasRectTransform, mousePosition, canvasCamera,out var localPoint)){
            cursorTransform.anchoredPosition = localPoint;
        }  
    }

    void Update()
    {
        PositionCursor(Mouse.current.position.ReadValue());
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void SpawnIfMissing()
    {
        // Already exists (e.g. started from Main Menu scene) — do nothing
        if (instance != null) return;

        var prefab = Resources.Load<GameObject>("CursorCanvas");
        if (prefab == null)
        {
            Debug.LogError("CursorUI: Could not find 'CursorCanvas' prefab in a Resources folder!");
            return;
        }
        // Instantiate it — Awake() on the prefab handles the rest
        Instantiate(prefab);
    }
}
