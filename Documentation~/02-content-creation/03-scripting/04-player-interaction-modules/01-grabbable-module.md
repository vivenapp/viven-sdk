## 개요

Grabbable Module은 플레이어가 물체를 잡고, 이동하고, 배치하고, 던질 수 있게 하는 상호작용 컴포넌트입니다. VObject에 `SDKGrabbableModule`(또는 `VivenGrabbableModule`)을 붙이면 해당 오브젝트가 Grab 모드와 Place 모드로 상호작용할 수 있습니다. 네트워크를 통해 Rigidbody가 동기화되며, Lua 스크립트로 Grab/Release 시점과 액션 버튼 동작을 정의할 수 있습니다.

## 언제 사용하나요?

- 플레이어가 손으로 들고 옮길 수 있는 도구, 아이템, 가구를 만들 때
- 오브젝트를 원하는 위치에 배치하거나 다른 오브젝트에 붙일 때
- VivenAttachPoint와 함께 활·화살처럼 붙여지는 조합 오브젝트를 만들 때
- 오브젝트를 잡은 상태에서 액션 버튼으로 특정 기능을 실행할 때

## 준비사항

- **VObject**: Grabbable Module을 붙일 오브젝트는 VObject여야 합니다.
- **Rigidbody**: `VivenRigidbodyControlModule`(SDKRigidbody)가 필요합니다.
- **Collider**: 상호작용이 가능한 물체에는 반드시 Collider가 있어야 합니다.
- **VivenBehaviour**: Lua 이벤트를 사용하려면 같은 GameObject에 `VivenLuaBehaviour`가 있어야 합니다.

## 진행 순서

### 1. Grabbable Module 추가

1. VObject가 있는 GameObject를 선택합니다.
2. **Add Component** → **VivenSDK** → **Viven Grabbable Module**을 추가합니다.
3. 필요한 Rigidbody, Collider가 이미 있는지 확인합니다.

### 2. 기본 설정

`SDKGrabbableModule`(또는 `VivenGrabbableModule`) Inspector에서 다음을 설정합니다.

| 필드 | 설명 |
|------|------|
| **Grab Type** | `Kinematic`: 물체가 물리 영향을 받지 않음. `Velocity`: 물리 영향을 받음. |
| **Parent To Hand On Grab** | `true`이면 물체를 잡았을 때 물체가 손(Interactor)의 자식으로 붙습니다. |
| **Hold Time Threshold** | 길게 누른 것으로 인식할 시간(초). 예: 1.5이면 1.5초 이상 눌러야 길게 누른 액션이 실행됩니다. |
| **Throw Force** | 물체를 던질 때 적용되는 힘. |
| **Exclude Layer Objects** | Layer 변경을 방지할 GameObject 목록. |

### 3. Grab Point 설정 (선택)

`grabPoints`에 `VivenGrabPoint`를 추가하면 물체를 잡을 때 특정 위치를 기준으로 잡습니다. 설정하지 않으면 잡았을 때의 위치로 잡힙니다.

### 4. Attach Point 배치 (선택)

다른 오브젝트를 붙일 수 있는 지점이 필요하면 `VivenAttachPoint`를 해당 오브젝트에 추가합니다. `VivenAttachPoint`는 GrabbableModule이 붙은 오브젝트의 hierarchy 내에만 붙일 수 있습니다.

### 5. Lua 이벤트 정의

`VivenLuaBehaviour`가 붙은 같은 GameObject에 Lua 스크립트에서 다음 함수를 정의합니다.

```lua
function onGrab()
    -- 물체를 잡았을 때 실행
end

function onRelease()
    -- 물체를 놓았을 때 실행
end

function objectShortClickAction()
    -- 물체를 잡은 상태에서 짧게 클릭했을 때 실행
end

function objectLongClickAction()
    -- 물체를 잡은 상태에서 길게 클릭했다 뗐을 때 실행
end
```

## 확인 방법

- **Grab 모드**: PC에서 물체를 바라보고 마우스 좌클릭, VR에서 물체에 손을 대고 Grip 버튼 클릭. Grab 가능한 물체는 주황색 Outline이 표시됩니다.
- **Place 모드**: PC에서 마우스 우클릭, VR에서 Trigger 버튼 클릭. 배치 모드로 전환됩니다.
- **던지기**: Release 버튼을 누르면 물체를 놓거나 던질 수 있습니다. `RigidbodyControlModule`의 PhysicsType이 `Physics`일 때만 던지기가 동작합니다.

## 동작 방식

### Grab 모드

- 물체를 잡으면 Grab 모드로 전환됩니다.
- **Kinematic**: 물체가 충돌·중력 없이 손을 따라 움직입니다.
- **Velocity**: 물체가 물리 영향을 받아, 놓을 때 던질 수 있습니다.
- 액션 버튼을 눌러 짧은 클릭/길은 클릭/홀드 동작을 실행할 수 있습니다.

### Place 모드

- 물체를 원하는 위치에 배치하거나 다른 오브젝트의 Attach Point에 붙일 수 있습니다.
- Place 모드에서는 Grabbable Event가 호출되지 않습니다.
- VObject로 생성한 오브젝트만 삭제할 수 있습니다. 맵에 포함된 오브젝트는 삭제할 수 없습니다.

### Layer 변경

- 상호작용 과정에서 GrabbableModule이 붙은 오브젝트의 Layer가 변경됩니다.
- Layer 변경을 막고 싶은 GameObject는 `excludeLayerObjects`에 등록합니다.

## 자주 일어나는 실수

- **HoldAction과 ClickAction 동시 사용**: `objectShortClickAction`과 `objectLongClickAction`, `objectHoldActionStart`/`objectHoldActionEnd`를 같이 사용하면 실행 순서가 보장되지 않습니다. 권장하지 않습니다.
- **던지기가 안 됨**: `RigidbodyControlModule`의 PhysicsType이 `Physics`가 아니면 던지기가 동작하지 않습니다.
- **Outline이 보이지 않음**: Collider가 없거나, Layer가 잘못 설정되어 있으면 Grab 가능한 Outline이 표시되지 않을 수 있습니다.
- **Attach Point 연결 실패**: 붙여지는 오브젝트도 GrabbableModule을 포함해야 합니다. `attachablePrefabs`/`notAttachablePrefabs`로 ContentId 기반 화이트리스트·블랙리스트를 설정할 수 있습니다.

## 스크립팅 가이드

### Grabbable Event Function (Lua)

| 함수명 | 호출 시점 |
|--------|-----------|
| `onGrab()` | 물체를 잡았을 때 |
| `onRelease()` | 물체를 놓았을 때 |
| `objectShortClickAction()` | 물체를 잡은 상태에서 짧게 클릭했을 때 |
| `objectLongClickAction()` | 물체를 잡은 상태에서 길게 클릭했다 뗐을 때 |
| `objectHoldActionStart()` | 길게 누르는 것이 인식되는 순간 |
| `objectHoldActionEnd()` | `objectHoldActionStart` 이후 액션 버튼을 뗐을 때 |

### 액션 버튼 2·3 (고급)

여러 액션 버튼을 구분하려면 다음 함수를 사용합니다.

| 함수명 | 설명 |
|--------|------|
| `ShortClickAction1`, `LongClickAction1`, `HoldActionStart1`, `HoldActionEnd1` | 버튼 1 |
| `ShortClickAction2`, `LongClickAction2`, `HoldActionStart2`, `HoldActionEnd2` | 버튼 2 |
| `ShortClickAction3`, `LongClickAction3`, `HoldActionStart3`, `HoldActionEnd3` | 버튼 3 |

### Lua 예시

```lua
function onGrab()
    -- 잡았을 때 로그 출력
    print("오브젝트를 잡았습니다.")
end

function onRelease()
    -- 놓았을 때 처리
    print("오브젝트를 놓았습니다.")
end

function objectShortClickAction()
    -- 짧은 클릭: 사용
    print("짧은 클릭 - 사용")
end

function objectLongClickAction()
    -- 길게 클릭: 상세 모드
    print("길게 클릭 - 상세 모드")
end

function objectHoldActionStart()
    -- 길게 누르기 시작
    print("길게 누르기 시작")
end

function objectHoldActionEnd()
    -- 길게 누르기 종료
    print("길게 누르기 종료")
end
```

### Player API (Lua)

Lua에서 `Player.Mine`을 통해 플레이어가 GrabbableModule을 잡도록 시도할 수 있습니다.

```lua
-- Player.Mine.TryGrab(grabbableModule, isLeft, isForce, interpolation)
-- grabbableModule: 잡을 GrabbableModule
-- isLeft: true면 왼손, false면 오른손
-- isForce: true면 기존에 잡고 있는 오브젝트를 놓고 새로 잡음
-- 반환: Task<bool> (성공 여부)

local success = await(Player.Mine.TryGrab(grabbableModule, false, false))
if success then
    print("잡기 성공")
end
```

### Release API

물체를 놓을 때는 GrabbableModule의 `Release()` 메서드를 사용합니다.

```lua
-- 현재 상호작용 중인 오브젝트를 놓고 싶을 때
grabbableModule:Release()
```

### 추가 데이터 동기화

GrabbableModule은 Rigidbody를 네트워크로 동기화합니다. 추가적인 데이터를 동기화하려면 `VivenBehaviour`에서 RPC를 사용합니다.

## 관련 문서

- [Lua 함수 및 이벤트](../08-lua-reference/06-functions-and-events.md)
- [Sittable Module (의자 앉기)](02-sittable-module.md)
- [상호작용 이벤트 처리](03-interaction-event-handling.md)
- [Unity Rigidbody와 Collider](../../02-3d-world-building-and-assets/02-unity-physics/01-rigidbody-and-collider.md)
- [플레이어 상호작용 모듈 개요](00-overview.md) — VivenAttachPoint 관련 내용
