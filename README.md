# The Stalking Stairs – Modding Guide (C# / BepInEx)

Official community documentation for creating mods for **The Stalking Stairs** using C# and BepInEx.

---

## 📌 Overview

This guide explains how to create and install mods for:

- Game: The Stalking Stairs  
- Engine: Unity 2022.2.3  
- Mod Loader: BepInEx 5.4.23.2  
- Language: C# (.NET Framework 4.7.2)

This documentation is intended for developers with basic C# knowledge.

---

## 📦 Requirements

Before starting, ensure you have:

- Visual Studio 2019 or newer  
- .NET Framework 4.7.2 Developer Pack  
- BepInEx 5.4.23.2 (x64)  
- dnSpy or ILSpy (for inspecting game code)

---

## 🔧 Installing BepInEx

1. Download **BepInEx 5.4.23.2 (x64)**.
2. Extract the contents into the game directory (where the `.exe` is located).
3. Launch the game once.
4. Close the game.

After running once, the following folders will be generated:

```
BepInEx/
BepInEx/plugins/
BepInEx/config/
```

BepInEx is now installed and ready to load plugins.

---

## 🛠 Creating Your First Mod

### Step 1 – Create the Project

1. Open Visual Studio.
2. Select **Create a new project**.
3. Choose **Class Library (.NET Framework)**.
4. Target **.NET Framework 4.7.2**.
5. Name the project (example: `MyFirstMod`).

---

### Step 2 – Add Required References

Right-click **References → Add Reference → Browse**.

Add the following DLL files:

From `BepInEx/core/`:
- `BepInEx.dll`
- `0Harmony.dll`

From `GameName_Data/Managed/`:
- `UnityEngine.dll`
- `UnityEngine.CoreModule.dll`
- `UnityEngine.UI.dll` (if creating UI mods)

Click **OK**.

---

## 🧠 Basic Mod Template

Replace the default class with:

```csharp
using BepInEx;
using BepInEx.Logging;
using UnityEngine;

[BepInPlugin("com.yourname.myfirstmod", "My First Mod", "1.0.0")]
public class MyFirstMod : BaseUnityPlugin
{
    private static ManualLogSource Log;

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
```

---

## 🏗 Building & Installing the Mod

1. Build the project (`Build → Build Solution`).
2. Navigate to:

```
bin/Debug/
```

3. Copy the generated `.dll` file.
4. Paste it into:

```
GameFolder/BepInEx/plugins/
```

5. Launch the game.

If successful, you will see the log message in the BepInEx console window.

---

## 🔎 Inspecting Game Code

To modify or extend game functionality:

1. Locate `Assembly-CSharp.dll` inside:
   ```
   GameName_Data/Managed/
   ```
2. Open it using dnSpy or ILSpy.
3. Search for relevant classes such as:
   - PlayerController
   - GameManager
   - AI-related scripts

This allows you to identify methods to patch using Harmony.

---

## 🔥 Example Harmony Patch

```csharp
using HarmonyLib;

[HarmonyPatch(typeof(PlayerController), "Update")]
class PlayerUpdatePatch
{
    static void Postfix()
    {
        Debug.Log("Player Update executed.");
    }
}
```

To activate patches, add the following inside `Awake()`:

```csharp
using HarmonyLib;

private void Awake()
{
    var harmony = new Harmony("com.yourname.myfirstmod");
    harmony.PatchAll();
}
```

---

## 📁 Recommended Project Structure

```
MyModProject/
│
├── MyFirstMod.cs
├── Properties/
└── References
```

For documentation repositories:

```
Modding-Guide/
│
├── README.md
├── Examples/
│   ├── BasicMod/
│   └── UIModExample/
└── Resources/
```

---

## ⚠ Troubleshooting

**Mod not loading**
- Ensure the project targets .NET Framework 4.7.2.
- Confirm the DLL is inside `BepInEx/plugins/`.
- Check the BepInEx console for errors.

**Missing Unity references**
- Verify you referenced DLLs from the game’s `Managed` folder, not from another Unity installation.

**Harmony patch not working**
- Confirm class and method names match exactly (case-sensitive).

---

## 📜 License

Recommended: MIT License (for open community modding).

---

## 🤝 Contributing

Pull requests improving documentation, examples, or tooling are welcome.

Please ensure examples are clean, documented, and tested before submitting.

---

## Disclaimer

This guide is provided for educational and community purposes.  
Mod responsibly and respect the game’s terms of service.
