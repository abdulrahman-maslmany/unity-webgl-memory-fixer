
```csharp
using System;
using UnityEngine;

/// <summary>
/// Production WebGL Viewport and Browser Focus Synchronization Adapter.
/// Handles resolution locking, input isolation, and background tab pausing.
/// </summary>
[DisallowMultipleComponent]
public class WebGLResolutionAdapter : MonoBehaviour
{
    public static event Action<bool> OnWindowFocusChanged;

    [Header("Viewport Resolution Settings")]
    [Tooltip("Target fixed render width inside web iframe.")]
    [SerializeField] private int targetWidth = 1280;

    [Tooltip("Target fixed render height inside web iframe.")]
    [SerializeField] private int targetHeight = 720;

    [Header("Performance & Focus")]
    [Tooltip("Automatically pause Time.timeScale and mute AudioListener on tab blur.")]
    [SerializeField] private bool autoPauseOnUnfocus = true;

    private void Awake()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        // Lock internal resolution inside the browser canvas
        Screen.SetResolution(targetWidth, targetHeight, false);

        // Prevent WebGL canvas from propagating keyboard events to the host webpage
        WebGLInput.captureAllKeyboardInput = true;
#endif
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (autoPauseOnUnfocus)
        {
            Time.timeScale = hasFocus ? 1.0f : 0.0f;
            AudioListener.pause = !hasFocus;
        }

        OnWindowFocusChanged?.Invoke(hasFocus);
    }
}
