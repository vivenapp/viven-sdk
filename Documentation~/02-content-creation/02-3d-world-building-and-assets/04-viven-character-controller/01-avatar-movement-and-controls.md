# 아바타 이동 및 조작

## 개요

Viven 플레이어는 1인칭, 3인칭, VR(XR) 시점에 따라 전방(Forward)과 움직임이 달라집니다. `Player.Mine.TeleportPlayer`로 플레이어를 지정 위치로 순간이동할 수 있습니다.

## 언제 사용하나요?

- 플레이어 이동·회전·텔레포트를 Lua 스크립트에서 제어할 때
- 시점(PointOfView)에 따른 Forward·움직임 차이를 이해하고 싶을 때
- VR 콘텐츠에서 멀미를 고려한 이동 방식을 설계할 때

## 준비사항

- Viven SDK가 적용된 월드
- Lua 스크립트 또는 C# API 접근 권한

## 시점(PointOfView)별 전방과 움직임

Viven은 `PointOfView` 열거형으로 시점을 관리합니다.

| 시점 | 설명 | Forward 기준 | 움직임 특성 |
|------|------|--------------|-------------|
| **1인칭 (First)** | PC 기본 시점 | 카메라(머리) 방향 | 마우스로 시선 회전 → 캐릭터가 따라 회전. 이동 시 캐릭터와 머리가 함께 회전 |
| **3인칭 (Third)** | PC·모바일 시점 | 3인칭 카메라가 바라보는 방향 | 카메라가 붙어 있으면 마우스 회전 시 캐릭터도 회전. 떨어져 있으면 카메라만 회전 |
| **VR (Xr)** | HMD 기반 시점 | HMD 트래킹 + 조이스틱 스냅 회전 | HMD 방향으로 몸이 회전. 조이스틱 좌우로 15° 스냅 회전. 이동 중 HMD 트래킹으로 자연스러운 보행 |

### 1인칭·3인칭 (PC)

- `Forward`는 `LocomotionController.PlayerTransform.forward`를 따릅니다.
- 카메라가 먼저 회전하고, 일정 각도(Yaw Limit)를 넘으면 캐릭터가 회전합니다.
- 이동 입력은 캐릭터의 `forward`·`right` 기준으로 적용됩니다.

### VR (XR)

- `Forward`는 HMD 트래킹 공간과 조이스틱 스냅 회전이 합쳐진 방향입니다.
- HMD 위치·회전이 `TrackingSpace`를 통해 실시간 반영됩니다.
- 이동 중이 아닐 때는 HMD 트래킹으로 인한 이동이 적용됩니다(머리 움직임에 따른 자연스러운 보행).

## 텔레포트

플레이어를 지정 위치·회전으로 순간이동하려면 `VivenPlayer.TeleportPlayer`를 사용합니다. Lua에서는 `Player.Mine.TeleportPlayer`로 호출합니다.

### 사용 예시

```lua
-- Lua: 위치와 회전으로 텔레포트
Player.Mine.TeleportPlayer(Vector3.New(0, 0, 0), Quaternion.identity)
```

```csharp
// C#: VivenPlayer 인스턴스 사용
VivenPlayer.Instance.TeleportPlayer(position, rotation);
```

### 동작 방식

1. 카메라와 머리 회전을 초기화합니다.
2. `LocomotionController.TeleportTo`로 CharacterController를 즉시 이동합니다.
3. 텔레포트 중에는 `IsTeleporting`이 true가 되어 이동·회전이 잠깁니다.
4. 네트워크 환경에서는 RPC로 다른 클라이언트에 동기화됩니다.

자세한 VR 텔레포트·로코모션 내용은 [VR 텔레포트 및 로코모션](02-vr-teleport-and-locomotion.md)을 참고하세요.

## 진행 순서

1. Lua 스크립트 또는 C#에서 `Player.Mine.TeleportPlayer(pos, rot)` 호출
2. 목표 `Transform`이 있다면 `transform.position`, `transform.rotation` 전달
3. 이벤트 `EventName.Common.Rig.Teleport`를 사용해 `Transform` 기반 텔레포트도 가능

## 확인 방법

- 플레이 모드에서 해당 위치·회전으로 플레이어가 이동하는지 확인
- VR 모드에서 HMD 회전과 조이스틱 스냅 회전이 의도대로 동작하는지 확인

## 자주 일어나는 실수

- **VR 멀미**: VR에서는 연속 이동(스틱 이동)보다 **텔레포트**가 멀미를 줄이는 데 유리합니다. 급격한 회전, 높이 변화, 시선과 이동 방향 불일치를 피하세요.
- **텔레포트 직후 입력**: `IsTeleporting`이 해제될 때까지 한 프레임 대기하므로, 직후 입력은 무시될 수 있습니다.
- **Forward 혼동**: `VivenPlayer.Forward`는 `Rig.Forward`와 동일하며, CharacterController의 `forward`를 따릅니다. 1인칭/3인칭에서는 카메라 회전이 반영되지만, VR에서는 HMD 트래킹과 조이스틱 회전이 반영됩니다.

## 관련 문서

- [VR 텔레포트 및 로코모션](02-vr-teleport-and-locomotion.md)
- [Viven 카메라 API](../03-viven-camera-api.md)
- [PC·VR 플랫폼 개발](../../01-project-management/05-pc-vr-platform-development.md)
