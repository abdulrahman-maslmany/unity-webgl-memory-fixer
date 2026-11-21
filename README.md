# 🚀 Unity WebGL Memory & Canvas Optimization Utility

[![Unity Version](https://img.shields.io/badge/Unity-2021.3%20%7C%202022.3%20%7C%20Unity%206-blue.svg)](https://unity.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![Platform](https://img.shields.io/badge/Platform-WebGL%20%7C%20itch.io-green.svg)]()

A lightweight, zero-dependency C# adapter engineered for **Unity WebGL** builds hosted on platforms like **itch.io**, **Poki**, or custom web servers. 

It permanently solves the three most frustrating WebGL release engineering defects:
1. **Webpage Scrolling on Input:** Traps arrow and spacebar keys to avoid page scroll.
2. **Background Audio & Execution Leaks:** Auto-pauses `Time.timeScale` and mutes audio when visitors blur/switch browser tabs.
3. **Canvas Viewport Distortion:** Locks target render resolution within responsive iframes.

---

## 📖 Deep-Dive Engineering Documentation

For exhaustive benchmarks, WebAssembly heap diagnostic analysis, and publishing tutorials:
- 🛠️ **[Fixing Unity WebGL Crashes: Memory & Black Screen Guide](https://www.gameunity.dev/2026/10/Fixing-Unity-WebGL-Crashes-Memory-Black-Screen-Guide.html)** — Comprehensive manual on memory allocation, Gzip decompression fallbacks, and browser console debugging.
- 📦 **[How to Publish a Unity Game on itch.io: WebGL & ZIP Setup](https://www.gameunity.dev/2026/09/How-to-Publish-Your-Unity-Game-on-itchio.html)** — Step-by-step root-level packaging guide to avoid the "Shows Executable instead of Web Play" defect.

---

## ⚡ Quick Start

1. Add `WebGLResolutionAdapter.cs` to your Unity project's `Scripts/` directory.
2. Attach the component to a persistent `GameObject` in your initial bootstrapping scene (e.g., GameManager).
3. In your **Build Settings & Player Settings**:
   - Set **Compression Format** to **Gzip**.
   - Check **Decompression Fallback** to ensure compatibility across servers without custom HTTP headers.
   - Tune **Initial WebGL Memory Size** between `256MB` and `512MB`.

---

## 💻 Core Implementation (`WebGLResolutionAdapter.cs`)

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
📄 License
This project is licensed under the MIT License.
Code
---
