# The Stalking Stairs – Complete Modding Guide (C# / BepInEx)

Official community documentation for creating mods for **The Stalking Stairs** using C# and BepInEx.

---

# Overview

This guide explains how to create, debug, and distribute mods for:

- **Game:** The Stalking Stairs  
- **Engine:** Unity 2022.2.3  
- **Mod Loader:** BepInEx 5.4.23.2 (x64)  
- **Patching Library:** Harmony  
- **Language:** C#  
- **Target Framework:** .NET Framework 4.7.2  

This documentation assumes basic knowledge of C# and Unity fundamentals.

---

# Requirements

Before starting, install:

- Visual Studio 2022 or newer  
- .NET Framework 4.7.2 Developer Pack  
- BepInEx 5.4.23.2 (x64)  
- dnSpy or ILSpy  

---

# Installing BepInEx

1. Download **BepInEx 5.4.23.2 (x64)**.
2. Extract all contents into the game directory (where the `.exe` is located).
3. Launch the game once.
4. Close the game.

After first launch, these folders will be created:

    BepInEx/
    BepInEx/plugins/
    BepInEx/config/
    BepInEx/logs/

BepInEx is now installed and ready to load plugins.

---

# Creating Your First Mod

## Step 1 – Create Project

1. Open Visual Studio.
2. Click **Create a new project**.
3. Choose **Class Library (.NET Framework)**.
4. Select **.NET Framework 4.7.2**.
5. Name your project (example: `MyFirstMod`).

---

## Step 2 – Add Required References

Right-click **References → Add Reference → Browse**.

Add the following DLLs:

### From `BepInEx/core/`:
- `BepInEx.dll`
- `0Harmony.dll`

### From `GameName_Data/Managed/`:
- `UnityEngine.dll`
- `UnityEngine.CoreModule.dll`
- `UnityEngine.UI.dll` (if creating UI mods)
- `Assembly-CSharp.dll` (to access game classes)

Click **OK**.

---

# Basic Plugin Template

Replace the default class with:

    using BepInEx;
    using BepInEx.Logging;
    using UnityEngine;

    [BepInPlugin("com.yourname.myfirstmod", "My First Mod", "1.0.0")]
    public class MyFirstMod : BaseUnityPlugin
    {
        internal static ManualLogSource Log;

        private void Awake()
        {
            Log = Logger;
            Log.LogInfo("My First Mod loaded successfully.");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                Log.LogInfo("F5 key pressed.");
            }
        }
    }

---

# Building & Installing

1. Click **Build → Build Solution**
2. Navigate to:

    bin/Debug/

3. Copy the generated `.dll`.
4. Paste into:

    GameFolder/BepInEx/plugins/

5. Launch the game.

If successful, your log will appear in the BepInEx console window.

---

# Adding Configuration Support

Example config system:

    using BepInEx;
    using BepInEx.Configuration;
    using UnityEngine;

    [BepInPlugin("com.yourname.configmod", "Config Mod", "1.0.0")]
    public class ConfigMod : BaseUnityPlugin
    {
        private ConfigEntry<bool> enableFeature;
        private ConfigEntry<float> speedMultiplier;

        private void Awake()
        {
            enableFeature = Config.Bind("General",
                                        "EnableFeature",
                                        true,
                                        "Enable or disable the feature.");

            speedMultiplier = Config.Bind("Gameplay",
                                          "SpeedMultiplier",
                                          2.0f,
                                          "Player speed multiplier.");

            Logger.LogInfo("Config Mod Loaded");
        }

        private void Update()
        {
            if (!enableFeature.Value)
                return;

            if (Input.GetKeyDown(KeyCode.F6))
            {
                Logger.LogInfo($"Speed Multiplier: {speedMultiplier.Value}");
            }
        }
    }

Config file will appear in:

    BepInEx/config/

---

# Harmony Patch Example (Postfix)

    using BepInEx;
    using HarmonyLib;
    using UnityEngine;

    [BepInPlugin("com.yourname.harmonypatch", "Harmony Patch Example", "1.0.0")]
    public class HarmonyPatchExample : BaseUnityPlugin
    {
        private void Awake()
        {
            Harmony harmony = new Harmony("com.yourname.harmonypatch");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(PlayerController), "Update")]
    class PlayerUpdatePatch
    {
        static void Postfix()
        {
            Debug.Log("Player Update executed.");
        }
    }

---

# Harmony Prefix Example (Modify Behavior)

    [HarmonyPatch(typeof(PlayerController), "TakeDamage")]
    class TakeDamagePatch
    {
        static bool Prefix(ref int damage)
        {
            damage = 0; // Cancel all damage
            Debug.Log("Damage prevented!");
            return true; // Continue original method
        }
    }

---

# Example: Spawning an Object

    private void SpawnCube()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(0, 2, 0);
        cube.transform.localScale = Vector3.one * 2f;
    }

Call inside Update:

    if (Input.GetKeyDown(KeyCode.F7))
    {
        SpawnCube();
    }

---

# Example: Creating Simple UI Text

    using UnityEngine.UI;

    private void CreateText()
    {
        GameObject canvasObj = new GameObject("ModCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        GameObject textObj = new GameObject("ModText");
        textObj.transform.SetParent(canvasObj.transform);

        Text text = textObj.AddComponent<Text>();
        text.text = "Mod Loaded!";
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 24;
        text.color = Color.red;
    }

---

# Inspecting Game Code

1. Navigate to:

    GameName_Data/Managed/

2. Open `Assembly-CSharp.dll` in dnSpy or ILSpy.
3. Search for relevant classes:
   - PlayerController
   - GameManager
   - AIController

Use this to identify methods to patch using Harmony.

---

For documentation repositories:

    Modding-Guide/
    │
    ├── README.md
    ├── Examples/
    │   ├── BasicMod/
    │   ├── ConfigExample/
    │   └── HarmonyExample/
    └── Resources/

---

# Debugging Tips

- Check `BepInEx/logs/LogOutput.log`
- Use `Logger.LogInfo()` frequently
- Confirm target framework is 4.7.2
- Ensure x64 architecture matches the game

---

# Releasing Your Mod

Recommended:

- Include compiled `.dll`
- Add README
- Add version number
- Use semantic versioning (1.0.0)
- Provide changelog

Optional:
- Publish on GitHub
- Use MIT License

---

# ⚠ Troubleshooting

### Mod not loading
- Ensure DLL is in `BepInEx/plugins/`
- Confirm target framework is 4.7.2
- Check console for errors

### Missing references
- Verify DLLs are from the game’s Managed folder

### Harmony patch not working
- Method names must match exactly (case-sensitive)
- Confirm the method exists and is not renamed/obfuscated

---

# 📜 License

Recommended: MIT License for open community modding.

---

# 🤝 Contributing

Pull requests improving documentation, examples, or tooling are welcome.

Please ensure:
- Code compiles
- Examples are tested
- Documentation is clear

---

# Disclaimer

This guide is provided for educational and community purposes.  
Mod responsibly and respect the game’s terms of service.
