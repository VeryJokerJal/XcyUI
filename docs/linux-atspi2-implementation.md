# Linux AT-SPI2 accessibility implementation

## Goal

Expose the existing `XAccessibilityNode` tree to Linux assistive technologies through the desktop-standard AT-SPI2 protocol, without taking a dependency on GTK, Qt, ATK, libdbus, a specific compositor, or a specific Linux distribution.

The supported boundary is **desktop Linux sessions that provide AT-SPI2 and a user D-Bus session**. This includes X11 and Wayland sessions on GNOME, KDE Plasma, Cinnamon, MATE, Xfce, LXQt and similar desktops. Console-only/headless systems and desktop sessions that intentionally disable AT-SPI2 degrade cleanly and keep the UI running.

## Architecture

### 1. Process-wide service

`LinuxAtSpiBridge` owns one accessibility-bus connection for the process. Windows register and unregister lightweight descriptors with that bridge. The exported tree has this shape:

```text
/org/a11y/atspi/accessible/root                   Application
  /window/1                                      Frame
    /node/...
  /window/2                                      Frame
    /node/...
```

This matches AT-SPI2's single application-root requirement and avoids one registry application per native window.

### 2. Distribution-independent transport

Use `Tmds.DBus.Protocol` and its compile-time generator:

- managed D-Bus implementation
- NativeAOT and trimming compatible
- no `libdbus-1.so` P/Invoke
- no distro-specific package manager assumptions
- works with glibc and musl when the application's .NET/runtime/native rendering assets support that target

Startup sequence:

1. Connect to the user session bus.
2. Call `org.a11y.Bus.GetAddress` at `/org/a11y/bus`.
3. Connect to the returned accessibility-bus address.
4. Export `/org/a11y/atspi/accessible/root` and all child paths.
5. Call `org.a11y.atspi.Socket.Embed` on the registry.
6. Emit object/focus/children/property events while windows are alive.

Failures are logged only when diagnostics are enabled and never abort normal window creation.

### 3. Stable snapshots and paths

The bridge converts each live `XAccessibilityNode` tree to an immutable snapshot before serving D-Bus requests. Paths remain stable for the lifetime of an `XView` by assigning IDs through reference identity. Removed views are retired after a structure refresh.

D-Bus calls execute on the bridge thread. Actions that mutate UI state are marshalled to the GLFW main thread through `ExecuteOnMainThread`.

### 4. Exported interfaces

| Interface | Scope | XcyUI mapping |
|---|---|---|
| `org.a11y.atspi.Accessible` | every node | role, name, description, parent/children, state, attributes |
| `org.a11y.atspi.Application` | application root | toolkit/version/application ID |
| `org.a11y.atspi.Component` | visible nodes | bounds, hit testing, focus |
| `org.a11y.atspi.Action` | activatable nodes | click/default action |
| `org.a11y.atspi.Text` | text and input nodes | content, ranges, caret and selection |
| `org.a11y.atspi.EditableText` | writable `XInput` | set/insert/delete text |
| `org.a11y.atspi.Value` | slider/progress/value roles | numeric range and current value |
| `org.a11y.atspi.Selection` | selectable containers | selected children and selection actions |
| `org.a11y.atspi.Event.Object` | event source | focus, state, property, bounds and children changes |

Unsupported mutating operations return `false` rather than pretending they succeeded.

### 5. Role and state mapping

`XAccessibilityRole` is mapped to standard `AtspiRole` values. State is returned as AT-SPI2's two-word bitset and includes, where applicable:

- enabled / sensitive
- visible / showing
- focusable / focused
- editable / read-only
- checked / selected / expanded / pressed
- required / invalid
- single-line / multi-line

Heading level, position in set, set size, live-region priority and hints are exported through attributes.

### 6. Coordinates

- window-relative coordinates are always available
- screen coordinates add the current GLFW window position
- parent-relative coordinates subtract the parent bounds
- on Wayland compositors that do not expose global window positions, screen coordinates gracefully fall back to window-relative coordinates, matching the protocol limitation rather than inventing coordinates

### 7. Events

Initial event sources:

- `XEvent.AccessibilityFocusChanged`
- `XAccessibility.StructureChanged`
- explicit text/value mutation paths used by AT-SPI actions

The bridge compares old and new snapshots after invalidation and emits:

- `StateChanged:focused`
- `ChildrenChanged:add/remove`
- `PropertyChange:accessible-name/description/value`
- `BoundsChanged`
- `TextChanged:insert/delete`
- `TextCaretMoved`

Event emission is best-effort and isolated from the render/input loop.

## Compatibility and validation matrix

### Desktop/session matrix

- GNOME Wayland + Orca
- GNOME X11 + Orca
- KDE Plasma Wayland
- KDE Plasma X11
- Cinnamon / MATE / Xfce X11

### Distribution matrix

- Ubuntu LTS x64 and arm64
- Debian stable x64 and arm64
- Fedora current x64 and arm64
- Arch Linux x64
- openSUSE Tumbleweed x64
- Alpine Linux x64 and arm64, using a musl-compatible application publish

### Automated checks

- build normal, trimmed and NativeAOT Linux targets
- introspection with `busctl`
- tree inspection with `accerciser` where available
- keyboard/focus/action tests through D-Bus calls
- process teardown and repeated window creation
- no-session-bus and AT-SPI-disabled startup tests

## Delivery sequence

1. Add managed D-Bus transport and generated protocol definitions.
2. Add the process-wide bridge, snapshots, role/state mapping and lifecycle.
3. Wire Linux window registration into `XWindow`.
4. Add text-editing accessibility hooks to `XInput`.
5. Add events and snapshot diffing.
6. Add demo semantics and Linux validation scripts.
7. Run the matrix above in CI runners/containers where the desktop requirement can be satisfied.
