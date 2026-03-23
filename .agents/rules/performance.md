---
trigger: always_on
---

# Coding Guidelines — Performance (Unity 6 2D)

## General Rules
- **Profile first, optimize second.** Use the Unity Profiler and Frame Debugger before making any optimization decision.
- Establish a frame time budget early and protect it.
- Avoid per-frame allocations — no `new` inside `Update`, no LINQ in hot paths, no string concatenation.

---

## Physics 2D
- Use `Rigidbody2D.linearVelocity` (Unity 6) — not the deprecated `.velocity`.
- Set `Collision Detection` to `Continuous` only on fast-moving objects that need it.
- Use `Physics2D.queriesHitTriggers = false` globally and opt-in selectively.
- Prefer `OverlapCircleNonAlloc` / `RaycastNonAlloc` over allocating variants.
- Tighten your collision matrix — most objects should not interact with most layers.

---

## Rendering (URP 2D)
- Use **Sprite Atlas** for all sprites — one atlas per feature/scene to minimize draw calls.
- Enable **GPU Instancing** on shared materials (enemies, projectiles).
- Use `TilemapRenderer` chunk mode; keep tilemaps on dedicated sorting layers.
- Never call `Camera.main` in hot paths — cache it once in `Awake`.
- Light2D is expensive: budget your lights, use `Shadow Caster 2D` sparingly, bake static lights where possible.

---

## Object Pooling
Never instantiate or destroy GameObjects at runtime for frequently-spawned objects.
Use **Unity 6's built-in `ObjectPool<T>`**.

```csharp
public class BulletPool : MonoBehaviour
{
    [SerializeField] private Bullet _prefab;

    private ObjectPool<Bullet> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Bullet>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: b => b.gameObject.SetActive(true),
            actionOnRelease: b => b.gameObject.SetActive(false),
            actionOnDestroy: b => Destroy(b.gameObject),
            defaultCapacity: 20,
            maxSize: 100
        );
    }

    public Bullet Get() => _pool.Get();
    public void Release(Bullet bullet) => _pool.Release(bullet);
}
```

---

## Jobs System & Burst Compiler
Use for CPU-intensive simulations (pathfinding, large particle logic, procedural generation) — **not as a default for everything**.

- Schedule jobs in `Update`, complete them in `LateUpdate` to maximize parallelism.
- Use `NativeArray<T>` / `NativeList<T>` for job data — dispose in `OnDestroy`.
- Tag job structs with `[BurstCompile]` and avoid managed types inside them.
- Do not use Jobs for logic that runs once per frame on a handful of objects.

```csharp
[BurstCompile]
public struct MoveEnemiesJob : IJobParallelFor
{
    public NativeArray<float2> Positions;
    public NativeArray<float2> Directions;
    public float Speed;
    public float DeltaTime;

    public void Execute(int index)
    {
        Positions[index] += Directions[index] * Speed * DeltaTime;
    }
}
```