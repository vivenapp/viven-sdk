# VivenScript 글로벌 심볼 레퍼런스

## 개요

VivenScript 환경에서는 자주 사용하는 C# 타입이 Lua 글로벌에 미리 등록되어 있습니다. 이 타입들은 `CS.UnityEngine.` 등의 prefix 없이 바로 사용할 수 있습니다.

> [!CAUTION]
> 아래 목록에 포함된 타입은 **`CS.`를 붙이지 마세요.** 동작은 하지만 불필요하게 길어지고, 코드 가독성이 떨어집니다.

```lua
-- ✅ 글로벌 심볼 사용 (권장)
local pos = Vector3(1, 2, 3)
local obj = GameObject.Find("MyObject")
local dt = Time.deltaTime

-- ❌ 불필요한 CS. 접근
local pos = CS.UnityEngine.Vector3(1, 2, 3)
```

---

## Viven API

| 심볼 | 설명 | 문서 |
|------|------|------|
| `Player` | 플레이어 정보, 이동 제어, 텔레포트 | [PlayerAPI](https://sdkdoc.viven.app/api/VivenAPI/TwentyOz.VivenSDK.Scripts.Core.VivenAPI.PlayerAPI.html) |
| `Room` | 방 정보, 입장/퇴장 이벤트, RoomProperty | [RoomAPI](https://sdkdoc.viven.app/api/VivenAPI/TwentyOz.VivenSDK.Scripts.Core.VivenAPI.RoomAPI.html) |
| `VivenSystem` | 시스템 제어 | [SystemAPI](https://sdkdoc.viven.app/api/VivenAPI/TwentyOz.VivenSDK.Scripts.Core.VivenAPI.SystemAPI.html) |
| `Debug` | 로그 출력 (Debug.Log / LogWarning / LogError) | — |
| `VivenUtil` | 유틸리티 함수 | [VivenUtilAPI](https://sdkdoc.viven.app/api/VivenAPI/TwentyOz.VivenSDK.Scripts.Core.VivenAPI.VivenUtilAPI.html) |
| `UI` | Fader, 마우스 커서, Dock | [UIAPI](https://sdkdoc.viven.app/api/VivenAPI/TwentyOz.VivenSDK.Scripts.Core.VivenAPI.UIAPI.html) |
| `TextChat` | 텍스트 채팅 | [ChatAPI](https://sdkdoc.viven.app/api/VivenAPI/TwentyOz.VivenSDK.Scripts.Core.VivenAPI.ChatAPI.html) |
| `XR` | VR/XR 디바이스 | [XRAPI](https://sdkdoc.viven.app/api/VivenAPI/TwentyOz.VivenSDK.Scripts.Core.VivenAPI.XRAPI.html) |
| `HandTracking` | 핸드트래킹 | — |
| `Web` | 웹 요청 | — |
| `ScreenRecording` | 화면 녹화 | — |
| `Locale` | 로케일 정보 | — |

---

## Viven 컴포넌트

| 심볼 | 설명 |
|------|------|
| `VivenLuaBehaviour` | Lua 스크립트 컴포넌트 |
| `VivenGrabbableModule` | 물체 잡기 모듈 |
| `VObject` | Viven 오브젝트 |
| `VivenCustomSyncView` | 커스텀 동기화 뷰 |
| `VivenRigidbodyControlModule` | Rigidbody 제어 모듈 |
| `OutlineModule` | 아웃라인 모듈 |
| `VivenCustomAnimationModule` | 커스텀 애니메이션 모듈 |
| `VivenAudioEventInstance` | 오디오 이벤트 인스턴스 (FMOD) |
| `VivenWebView` / `VivenLocalWebView` | 웹뷰 |
| `ECanvas` | 전자 캔버스 |
| `ElectronicBlackboard` | 전자 칠판 |
| `YoutubeViewer` | 유튜브 뷰어 |
| `RPCSendOption` | RPC 전송 옵션 (All, Others, Host 등) |
| `VivenUIPointerEvents` | UI 포인터 이벤트 |

---

## Unity 핵심

| 심볼 | 심볼 | 심볼 |
|------|------|------|
| `Object` | `GameObject` | `Transform` |
| `Vector3` | `Vector2` | `Quaternion` |
| `Time` | `Mathf` | `Random` |
| `Application` | `LayerMask` | `Resources` |
| `Camera` | `Color` | `ColorBlock` |
| `Screen` | `Light` | `PlayerPrefs` |
| `CharacterController` | `SceneManager` | `Scene` |

---

## Coroutine / Yield

| 심볼 | 설명 |
|------|------|
| `Coroutine` | 코루틴 핸들 타입 |
| `WaitForSeconds(초)` | 지정 시간 대기 |
| `WaitForSecondsRealtime(초)` | TimeScale 무관 대기 |
| `WaitForEndOfFrame()` | 프레임 렌더링 종료 대기 |
| `WaitForFixedUpdate()` | 다음 FixedUpdate 대기 |
| `WaitUntil(func)` | 조건이 true가 될 때까지 대기 |
| `WaitWhile(func)` | 조건이 true인 동안 대기 |

---

## UI 컴포넌트

| 심볼 | 심볼 | 심볼 |
|------|------|------|
| `Button` | `Text` | `TMP_Text` |
| `TMP_InputField` | `InputField` | `Dropdown` |
| `TMP_Dropdown` | `Image` | `RawImage` |
| `Slider` | `Toggle` | `Canvas` |
| `CanvasGroup` | `RectTransform` | `Rect` |
| `GraphicRaycaster` | `ScrollRect` | `LayoutRebuilder` |
| `ContentSizeFitter` | | |

---

## 렌더링

| 심볼 | 심볼 | 심볼 |
|------|------|------|
| `Renderer` | `MeshRenderer` | `SkinnedMeshRenderer` |
| `SpriteRenderer` | `LineRenderer` | `ParticleSystem` |
| `Material` | `Shader` | `Texture` / `Texture2D` |
| `RenderTexture` | `Sprite` | `Gradient` |
| `Animator` | `RenderTextureFormat` | |

---

## 물리

| 심볼 | 심볼 | 심볼 |
|------|------|------|
| `Rigidbody` | `Rigidbody2D` | `Collider` / `Collider2D` |
| `Physics` | `Physics2D` | `Ray` |
| `RaycastHit` | `RaycastHit2D` | |
| `Joint` | `FixedJoint` | `SpringJoint` |
| `HingeJoint` | `CharacterJoint` | `ConfigurableJoint` |

> 2D Joint 계열 (`Joint2D`, `FixedJoint2D`, `SpringJoint2D`, `HingeJoint2D`, `SliderJoint2D`, `WheelJoint2D`, `DistanceJoint2D`)도 사용 가능합니다.

---

## 오디오

| 심볼 | 심볼 | 심볼 |
|------|------|------|
| `AudioSource` | `AudioClip` | `AudioListener` |
| `AudioRolloffMode` | `AudioReverbPreset` | `AudioReverbZone` |

> 오디오 필터 계열 (`AudioLowPassFilter`, `AudioHighPassFilter`, `AudioDistortionFilter`, `AudioEchoFilter`, `AudioChorusFilter`, `AudioReverbFilter`)도 사용 가능합니다.

---

## 입력 (Input System)

| 심볼 | 설명 |
|------|------|
| `PlayerInput` | Input System 플레이어 입력 |
| `Keyboard` | 키보드 디바이스 |
| `Key` | 키 열거형 |
| `Mouse` | 마우스 디바이스 |
| `Touchscreen` | 터치스크린 디바이스 |
| `KeyCode` | 키코드 (레거시) |
| `Touch` / `TouchPhase` | 터치 입력 (레거시) |

> `Input` (구 InputManager)은 **제거**되었습니다. Input System 패키지를 사용하세요.

---

## AI (NavMesh)

| 심볼 | 심볼 |
|------|------|
| `NavMesh` | `NavMeshAgent` |
| `NavMeshObstacle` | `NavMeshPath` |
| `NavMeshPathStatus` | `NavMeshHit` |

---

## 직렬화

| 심볼 | 설명 |
|------|------|
| `JsonUtility` | Unity 내장 JSON |
| `JsonConvert` | Newtonsoft.Json |
| `JToken` / `JObject` / `JArray` / `JProperty` | Newtonsoft.Json.Linq |

---

## 기타

| 심볼 | 설명 |
|------|------|
| `System` / `DateTime` / `DateTimeOffset` | .NET 기본 타입 |
| `VivenTweenUtil` | DoTween 확장 유틸 |
| `LoopType` / `Ease` | DoTween 열거형 |
| `PlayableDirector` / `PlayableAsset` | Timeline |
| `AsyncOperation` | 비동기 작업 핸들 |
| `WWW` | 네트워크 요청 (레거시) |

---

## CS. 접근이 필요한 경우

위 목록에 **없는** 타입은 `CS.` prefix가 필요합니다.

```lua
-- 글로벌에 없는 타입 → CS. 필요
local encoding = CS.System.Text.Encoding.UTF8
local regex = CS.System.Text.RegularExpressions.Regex("pattern")
```

- Lua는 대소문자를 구분합니다. `CS.UnityEngine`과 `CS.unityengine`은 다릅니다.

## 관련 문서

- [Viven API](02-viven-api.md)
- [VivenScript와 LuaBehaviour](../01-viven-lua-behaviour.md)
- [비동기 프로그래밍 — 코루틴](../06-asynchronous-programming/01-unity-coroutines.md)
