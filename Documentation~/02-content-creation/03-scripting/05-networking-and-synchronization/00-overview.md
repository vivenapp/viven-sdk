# 네트워크 및 동기화 (Networking and Synchronization) 개요

## 개요

Viven 플랫폼의 멀티플레이어 환경에서 모든 플레이어가 동일한 월드 상태를 공유하고 상호작용할 수 있도록 하는 네트워크 시스템을 설명합니다. DTS 서버를 기반으로 한 동기화 메커니즘을 통해 복잡한 멀티플레이어 로직을 손쉽게 구현할 수 있습니다.

## 주요 특징

- **자동 상태 동기화**: `SyncView`와 `SyncVar`를 통해 위치, 회전, 물리 상태 및 사용자 정의 변수를 자동으로 동기화합니다.
- **원격 프로시저 호출 (RPC)**: 일회성 이벤트나 특정 시점의 동작을 모든 클라이언트 또는 특정 대상에게 즉시 전달합니다.
- **소유권(Ownership) 관리**: 상호작용 시 오브젝트의 제어 권한을 동적으로 변경하여 충돌을 방지합니다.
- **방 프로퍼티(Room Property)**: 현재 방의 전역적인 상태나 데이터를 저장하고 공유합니다.

## 학습 순서

1. **[원격 프로시저 호출 (RPC)](./01-remote-procedure-calls.md)**: 특정 시점에 발생하는 이벤트를 다른 유저들에게 전달하는 방법을 배웁니다.
2. **[네트워크 변수 (Network Variables)](./02-network-variables.md)**: 점수, 체력 등 지속적으로 변화하는 데이터를 자동으로 동기화하는 방법을 익힙니다.
3. **[동기화 뷰 (Sync View)](./03-sync-view.md)**: `VivenCustomSyncView`를 통해 오브젝트의 물리 상태와 사용자 데이터를 관리하는 상세 기법을 학습합니다.
4. **[방 프로퍼티 (Room Property)](./04-room-property.md)**: 방 단위의 전역 데이터를 저장하고 조회하며, 방 입장/퇴장 시의 상태 관리를 배웁니다.

## 준비사항

- VObject가 부착된 게임 오브젝트
- `VivenCustomSyncView` 컴포넌트
- `VivenLuaBehaviour`를 통한 Lua 스크립트 연결 환경

## 관련 문서

- [스크립팅 개요](../00-overview.md)
- [Host/Client 서버 모델](../../01-project-management/06-viven-architecture/01-host-client-server-model.md)
- [네트워크 소유권 (Network Ownership)](../../01-project-management/06-viven-architecture/03-network-ownership.md)
