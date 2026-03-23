---
trigger: always_on
---

# Coding Guidelines — Scene & Project Organization

## Scene Hierarchy

Every scene follows this GameObject structure:

```
[Scene Root]
├── _Systems          # Managers, services, event buses (no visuals)
├── _UI               # All Canvas objects
├── Environment       # Static world geometry
├── Dynamic           # Runtime-spawned objects
└── Lighting          # Light2D, Global Light, etc.
```

- Prefix non-visual system objects with `_` to sort them to the top.
- Never put logic on the scene root object itself.
- Keep scenes **small and composable** — use Additive Scene Loading for distinct areas.

---

## Prefab Rules
- Every reusable object is a Prefab. No exceptions.
- Use **Prefab Variants** for specializations instead of duplicating prefabs.
- Nested Prefabs allowed — keep nesting to 2 levels max.
- Do not leave unintentional overrides on Prefab instances — they become maintenance debt.
- All prefabs must be self-contained: no scene-object references baked in.

---

## Asset Folder Structure

Organize by **feature/domain**, not by asset type.

```
Assets/
├── Features/
│   └── [FeatureName]/
│       ├── Prefabs/
│       ├── Scripts/
│       ├── ScriptableObjects/
│       └── Sprites/
├── Shared/
│   ├── Scripts/
│   │   ├── Events/
│   │   ├── Extensions/
│   │   └── Utilities/
│   ├── Prefabs/
│   └── UI/
├── Art/
│   ├── Sprites/
│   ├── Tilemaps/
│   └── Animations/
└── Settings/
    ├── InputActions/
    ├── RenderPipelineAsset/
    └── Physics2DSettings/
```

Apply Step 2 regularly — delete folders and assets that no longer serve a purpose.