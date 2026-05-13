# 플레이어 상호작용 모듈 (Player Interaction) 개요

## 개요

Viven 플랫폼에서 플레이어가 월드 내 오브젝트와 상호작용하는 다양한 방식(잡기, 앉기, 클릭 등)을 설명합니다. `Grabbable`, `Sittable` 등 미리 정의된 모듈을 활용하여 복잡한 물리 상호작용과 네트워크 동기화를 손쉽게 구현할 수 있습니다.

## 주요 특징

- **Grabbable Module**: 물체를 잡고, 옮기고, 던지며, 액션 버튼을 통해 기능을 실행할 수 있습니다.
- **Sittable Module**: 의자나 소파 등에 아바타를 앉히고, 시점 전환 및 애니메이션을 제어합니다.
- **네트워크 동기화 통합**: 상호작용 시 소유권(Ownership) 변경과 Rigidbody 동기화가 자동으로 처리됩니다.
- **Lua 이벤트 연결**: `onGrab`, `onRelease`, `objectShortClickAction` 등 다양한 상호작용 시점을 Lua로 제어합니다.

## 학습 순서

1. **[Grabbable Module (물체 잡기)](./01-grabbable-module.md)**: 물체를 잡고 던지며, 액션 버튼으로 기능을 실행하는 방법을 배웁니다.
2. **[Sittable Module (의자 앉기)](./02-sittable-module.md)**: 아바타를 특정 위치에 앉히고 상호작용을 처리하는 방법을 익힙니다.
3. **[상호작용 이벤트 처리](./03-interaction-event-handling.md)**: 클릭, 홀드, 트리거 진입 등 다양한 상호작용 이벤트를 Lua로 처리하는 상세 기법을 학습합니다.

## 준비사항

- VObject로 설정된 게임 오브젝트
- `Rigidbody` 및 `Collider` 컴포넌트
- `VivenLuaBehaviour`를 통한 Lua 스크립트 연결 환경

## 관련 문서

- [스크립팅 개요](../00-overview.md)
- [Viven API 레퍼런스](../03-viven-services-and-api/02-viven-api.md)
- [네트워크 소유권 (Network Ownership)](../../01-project-management/06-viven-architecture/03-network-ownership.md)
