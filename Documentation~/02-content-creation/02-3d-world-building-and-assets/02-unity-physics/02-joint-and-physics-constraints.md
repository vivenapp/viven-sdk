> **⚠️ 임시 문서**  
> 이 문서는 임시 문서입니다. 다른 문서나 작업에서 참조하지 마세요.

# Joint와 물리 제약

## 개요

Unity의 **Joint** 컴포넌트를 사용해 Rigidbody가 붙은 오브젝트들을 서로 연결하거나, 고정점에 묶을 수 있습니다. 문, 레버, 체인, 래그돌 등 물리 기반 연결 구조를 만들 때 사용합니다.

## 언제 사용하나요?

- 문·레버처럼 한 축을 기준으로 회전하는 오브젝트를 만들 때
- 체인·로프처럼 여러 물체가 연결된 구조를 만들 때
- 래그돌 캐릭터를 구성할 때
- 오브젝트의 이동·회전 범위를 제한하고 싶을 때

## 준비사항

- 연결할 각 오브젝트에 **Rigidbody**가 있어야 합니다.
- Joint는 한쪽 Rigidbody를 다른 Rigidbody(또는 고정점)에 연결합니다.
- Viven 네트워크 동기화가 필요한 경우, 루트 오브젝트에 `SDKNetworkObject`와 `VivenRigidbodyView`가 있어야 합니다.

## 진행 순서

### 1. Joint 종류 선택

| Joint | 용도 |
|-------|------|
| **Fixed Joint** | 두 물체를 완전히 고정. 부서질 수 있음. |
| **Hinge Joint** | 한 축 기준 회전 (문, 레버) |
| **Spring Joint** | 스프링처럼 당기고 밀리는 연결 |
| **Configurable Joint** | 이동·회전 축별로 제약을 세밀하게 설정 |

### 2. Joint 추가 및 연결

1. 연결할 오브젝트(자식 쪽)를 선택합니다.
2. **Add Component** → 원하는 Joint 타입을 추가합니다.
3. **Connected Body**에 연결할 부모 Rigidbody를 할당합니다. 비워 두면 월드 고정점에 연결됩니다.
4. **Anchor**와 **Axis**를 조정해 회전축·피벗을 설정합니다.

### 3. Hinge Joint 예시 (문)

1. 문 오브젝트에 Rigidbody와 Collider를 추가합니다.
2. **Hinge Joint**를 추가합니다.
3. **Connected Body**에 문틀(또는 벽)의 Rigidbody를 할당합니다.
4. **Anchor**를 문의 회전축(힌지 위치)에 맞춥니다.
5. **Axis**를 회전축 방향(예: Y축)으로 설정합니다.
6. **Use Limits**를 체크해 열림 각도를 제한할 수 있습니다.

### 4. Configurable Joint로 제약 세분화

Configurable Joint는 X/Y/Z 이동과 X/Y/Z 회전을 각각 **Free**, **Limited**, **Locked**로 설정할 수 있습니다.

- **Free**: 해당 축에서 자유롭게 움직임
- **Limited**: **Limit**으로 범위 지정
- **Locked**: 해당 축 고정

## Viven에서 Joint 사용 시 유의사항

- **네트워크 동기화**: Joint로 연결된 오브젝트는 보통 **루트 Rigidbody 하나**만 네트워크 동기화하고, 자식들은 물리 시뮬레이션으로 따라가게 합니다. 루트에 `VivenRigidbodyView`를 두고, 자식들은 로컬 물리만 사용하는 구조가 일반적입니다.
- **순간이동**: Joint가 붙은 물체를 `TeleportObject`로 이동할 때는 **루트 VObject**에 대해 호출합니다. 자식 Joint들은 부모와 함께 이동합니다.
- **성능**: Joint 수가 많을수록 물리 연산 부하가 커집니다. VR 환경에서는 필요한 만큼만 사용하는 것이 좋습니다.

## 확인 방법

- **단일 클라이언트**: Play 모드에서 Joint가 의도대로 동작하는지 확인합니다.
- **멀티플레이어**: 루트 물체를 그랩하거나 텔레포트했을 때, Joint로 연결된 전체 구조가 함께 움직이는지 확인합니다.

## 자주 일어나는 실수

- **Connected Body 미설정**: 비워 두면 월드에 고정되므로, 다른 물체와 연결하려면 반드시 할당해야 합니다.
- **Anchor 위치 오류**: Anchor가 잘못된 위치에 있으면 물체가 비정상적으로 꼬이거나 튕깁니다.
- **과도한 Joint**: 복잡한 체인·래그돌은 물리 부하가 크므로, 간소화된 구조를 권장합니다.

## 관련 문서

- [Unity Physics 개요](00-overview.md)
- [Rigidbody와 Collider](01-rigidbody-and-collider.md)
- [동기화 시스템](../../01-project-management/06-viven-architecture/02-synchronization-system.md)
