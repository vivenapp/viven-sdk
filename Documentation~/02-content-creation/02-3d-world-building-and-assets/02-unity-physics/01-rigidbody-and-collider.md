# Rigidbody와 Collider

## 개요

Unity의 **Rigidbody**와 **Collider**를 사용해 Viven 월드에 물리 기반 오브젝트를 만들 수 있습니다. Rigidbody는 질량·중력·관성을, Collider는 충돌 영역을 정의합니다. 이 문서는 기본 설정과 **Viven 네트워크 환경에서 물체를 안전하게 순간이동시키는 방법**을 설명합니다.

## 언제 사용하나요?

- 던지거나 굴릴 수 있는 물체를 만들 때
- 그랩 가능한 오브젝트(Grabbable)를 만들 때
- 중력·충돌에 반응하는 환경 오브젝트를 만들 때
- 스크립트로 물체를 **순간이동**시켜야 할 때

## 준비사항

- 물리 동기화가 필요한 오브젝트에는 `VObject` 컴포넌트와 `VivenRigidbodyView` 또는 `VivenTransformView`가 있어야 합니다.
- 그랩 가능한 물체는 `SDKGrabbableModule` 등 Grabbable 관련 컴포넌트가 필요합니다.

## 진행 순서

### 1. Rigidbody 설정

1. 오브젝트를 선택한 뒤 **Add Component** → **VivenRigidbodyView**를 추가합니다. 필요한 컴포넌트들이 자동으로 추가됩니다.
2. Inspector에서 다음 항목을 확인합니다.
   - **Mass**: 질량 (기본 1). 너무 크면 다른 물체와 충돌 시 비현실적으로 튕깁니다.
   - **Drag**: 선형 저항. 공기 저항처럼 속도를 줄입니다.
   - **Angular Drag**: 회전 저항.
   - **Use Gravity**: 중력 적용 여부.
   - **Is Kinematic**: 체크 시 물리 시뮬레이션에 의해 움직이지 않습니다. 스크립트로만 이동할 때 사용합니다.
   - **Interpolate**: 물체가 떨릴 때 **Interpolate** 또는 **Extrapolate**로 부드럽게 보이도록 설정할 수 있습니다.

### 2. Collider 설정

1. **Add Component** → **Box Collider**, **Sphere Collider**, **Capsule Collider** 중 하나를 추가합니다.
2. **Mesh Collider**는 복잡한 형태에 사용하되, 동적 물체에는 **Convex** 체크가 필요합니다. 성능상 Box·Sphere·Capsule이 더 유리합니다.
3. **Material**에 Physics Material을 지정하면 마찰·반발력을 조절할 수 있습니다.
   - **Assets** → **Create** → **Physics Material**로 생성
   - **Dynamic Friction**, **Static Friction**, **Bounciness** 설정

### 3. Viven 네트워크용 View 추가

1. `VObject`컴포넌트가 붙은 오브젝트에 **VivenRigidbodyView**(Rigidbody 사용 시) 또는 **VivenTransformView**(Transform만 사용 시)를 추가합니다.
2. 이 View가 없으면 `TeleportObject`가 동작하지 않습니다.

## Viven에서 물체를 순간이동시키는 방법

### 왜 `transform.position`을 쓰면 안 되나요?

Viven은 **네트워크를 통해 물체의 위치·회전·속도를 동기화**합니다. `transform.position`으로 직접 위치를 바꾸면:

- **다른 클라이언트**에서는 이전 위치와 새 위치 사이를 Lerping(보간)하면서 이동하는 것처럼 보입니다.
- **Rigidbody**가 붙어 있으면 속도(velocity)가 갑자기 바뀌지 않아 물리 시뮬레이션이 꼬이거나, 이상한 궤적으로 움직일 수 있습니다.

따라서 **네트워크 동기화가 필요한 물체**를 순간이동시킬 때는 반드시 `VObject.TeleportObject`를 사용해야 합니다.

### `TeleportObject` 사용법

`VObject.TeleportObject(position, rotation, force)`는 다음을 수행합니다.

- **위치·회전**을 즉시 설정
- **속도·각속도**를 0으로 초기화 (관성 제거)
- **RPC**를 통해 다른 클라이언트에도 동일하게 적용

**Lua 예시 (Viven Lua API 사용 시):**

```lua
-- vObject 참조를 얻은 뒤 (예: LuaBehaviour에서 self.gameObject 또는 이벤트로 전달된 오브젝트)
local vObject = VObject.Get(gameObject)
if vObject then
    vObject:TeleportObject(Vector3(0, 2, 0), Quaternion.identity, false)
end
```

**파라미터:**

| 파라미터 | 설명 |
|----------|------|
| `position` | 목표 위치 (월드 좌표) |
| `rotation` | 목표 회전 |
| `force` | `true`면 인터랙션 중이어도 강제로 이동 (기본 `false`) |

### `TeleportObject`의 동작 방식

- **오너(Owner)인 경우**: 즉시 `ExecuteTeleport`가 실행되어 로컬에 적용되고, RPC로 다른 클라이언트에 전파됩니다.
- **오너가 아닌 경우**: 현재 오너에게 RPC로 `RequestTeleportFromCurrentOwner`를 보내고, 오너가 실행 후 결과를 다른 클라이언트에 전파합니다.

### 네트워크 딜레이

`TeleportObject`는 **RPC**를 사용하므로 네트워크 지연이 있을 수 있습니다. 오너가 아닌 클라이언트에서 호출하면, 오너에게 요청이 도달하고 실행된 뒤 결과가 돌아오기까지 시간이 걸립니다. 실시간 반응이 중요한 경우 오너십을 먼저 확보하거나, 호출 시점을 고려해 설계하세요.

## 확인 방법

- **물리**: Play 모드에서 중력·충돌·마찰이 의도대로 동작하는지 확인합니다.
- **텔레포트**: 멀티플레이어로 두 클라이언트를 실행해, 한쪽에서 `TeleportObject`를 호출했을 때 다른 클라이언트에서도 즉시 같은 위치에 나타나는지 확인합니다.

## 자주 일어나는 실수

- `transform.position`으로 직접 이동 → 다른 클라이언트에서 Lerping·속도 이상 → `TeleportObject` 사용
- Collider 없이 Rigidbody만 사용 → 충돌이 일어나지 않음
- `VivenRigidbodyView` 없이 `TeleportObject` 호출 → RPC 전파 실패, `VivenTransformView`도 없으면 경고 로그 출력
- `force: true`로 인터랙션 중 강제 이동 → 그랩 중인 손에서 물체가 갑자기 사라질 수 있음

## 관련 문서

- [Unity Physics 개요](00-overview.md)
- [Joint와 물리 제약](02-joint-and-physics-constraints.md)
- [동기화 시스템](../../01-project-management/06-viven-architecture/02-synchronization-system.md)
- [네트워크 소유권](../../01-project-management/06-viven-architecture/03-network-ownership.md)
