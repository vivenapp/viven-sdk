# Unity Physics 개요

## 개요

Unity의 물리 엔진(PhysX)을 사용해 Viven 월드에 중력, 충돌, 관성 등 물리 기반 동작을 적용할 수 있습니다. 이 섹션에서는 Rigidbody, Collider, Joint 설정과 **Viven 네트워크 환경에서 물체 위치를 안전하게 변경하는 방법**을 설명합니다.

## 언제 사용하나요?

- 오브젝트가 중력·충돌에 반응해야 할 때
- 그랩 가능한 물체, 던질 수 있는 물체를 만들 때
- 문, 레버처럼 Joint로 연결된 오브젝트를 만들 때
- 스크립트로 물체를 **순간이동**시켜야 할 때

## Viven에서 물리 사용 시 핵심 원칙

Viven은 **네트워크를 통해 물체 위치를 동기화**합니다. 따라서:

- **`transform.position`으로 직접 위치를 변경하면** 다른 클라이언트에서 위치가 Lerping되거나, Rigidbody 속도가 이상하게 보이는 문제가 발생할 수 있습니다.
- **물체를 순간이동시킬 때는 반드시 `VObject.TeleportObject`를 사용**해야 합니다. 이 메서드는 RPC를 통해 네트워크에 전파되므로 모든 클라이언트에서 일관된 위치와 속도(0으로 초기화)를 유지합니다.

## 준비사항

- Unity 프로젝트에 Viven SDK가 포함되어 있어야 합니다.
- 물리 동기화가 필요한 오브젝트에는 `SDKNetworkObject`(VObject)와 `VivenRigidbodyView` 또는 `VivenTransformView`가 있어야 합니다.

## 진행 순서

1. [Rigidbody와 Collider 설정](01-rigidbody-and-collider.md) — 물체에 질량·충돌·마찰 적용
2. [Joint와 물리 제약](02-joint-and-physics-constraints.md) — 연결된 오브젝트 구성

## 확인 방법

- **단일 물체**: Play 모드에서 중력·충돌이 정상 동작하는지 확인합니다.
- **네트워크**: 멀티플레이어로 두 클라이언트를 실행해, 한쪽에서 물체를 이동·텔레포트했을 때 다른 클라이언트에서도 동일하게 보이는지 확인합니다.

## 자주 일어나는 실수

- `transform.position`으로 직접 이동 → 다른 클라이언트에서 Lerping·속도 이상 → `TeleportObject` 사용
- `Collider` 없이 `Rigidbody`만 사용 → 충돌이 안 됨
- `Rigidbody` 없이 `Collider`만 사용 → 충돌은 되지만 물체가 움직이지 않음 (정적 오브젝트만 해당)

## 관련 문서

- [Rigidbody와 Collider](01-rigidbody-and-collider.md)
- [Joint와 물리 제약](02-joint-and-physics-constraints.md)
- [동기화 시스템](../../01-project-management/06-viven-architecture/02-synchronization-system.md)
- [3D 월드 빌딩 개요](../00-overview.md)
