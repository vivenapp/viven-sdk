# 동기화 시스템

## 개요

Viven 콘텐츠의 네트워크 동기화는 **RPC**, **NetworkVariable**, **SyncView**를 통해 이루어집니다. 이 문서에서는 동기화의 개념, 원리, 동작 방식을 설명합니다. API 사용법과 설정 절차는 [네트워크 및 동기화](../../03-scripting/05-networking-and-synchronization/) 섹션에서 다룹니다.

## 동기화의 세 가지 방식

| 방식 | 용도 | 특징 |
|------|------|------|
| **RPC** | 원격 프로시저 호출 | 이벤트성 호출, 단방향 메시지 전달 |
| **NetworkVariable** | 개별 변수 동기화 | Lua 스크립트에서 `VivenCustomSyncView`를 사용한 필드 |
| **SyncView** | Transform·Rigidbody 등 연속 상태 | 위치, 회전, 물리 상태를 주기적으로 동기화 |

세 가지 방식은 함께 사용되며, `SDKNetworkObject`(VObject)를 루트로 하는 오브젝트에 컴포넌트로 부착됩니다.

## 핵심 컴포넌트: SDKNetworkObject

`SDKNetworkObject`는 Viven SDK에서 **네트워크 동기화와 상호작용이 가능한 오브젝트**를 만들 때 사용하는 컴포넌트입니다.

- **SDKdisplayName**: 오브젝트 표시 이름
- **SDKobjectId**: 네트워킹 시 사용하는 오브젝트 ID
- **SDKcontentType**: 콘텐츠 타입 (Prepared, VObject 등)
- **SDKobjectSyncType**: 동기화 방식 (아래 `VivenSDKSyncType` 참고)

`SDKNetworkObject`가 붙은 오브젝트는 DTS 서버를 통해 다른 플레이어와 상태를 공유하며, Grabbable, Sittable 등 상호작용 모듈과 함께 동작합니다.

## 동기화 타입: VivenSDKSyncType

`SDKSyncType.cs`에 정의된 `VivenSDKSyncType`은 **언제 동기화 데이터를 송신할지**를 결정합니다.

| 값 | 설명 | 적합한 상황 |
|----|------|-------------|
| **Continuous** | 매 프레임(또는 고정 주기) 송신 | Transform, Rigidbody처럼 연속적으로 변하는 상태 |
| **Manual** | 명시적 호출 시에만 송신 | 사용자가 `Sync()` 등을 호출할 때만 전송하고 싶을 때 |
| **OnChanged** | 값이 변경되었을 때만 송신 | 변경 빈도가 낮은 변수, 대역폭 절약 |

- **Continuous**: 물체를 들고 이동하는 Grabbable, 실시간 위치 추적이 필요한 오브젝트
- **Manual**: 보드 그리기, 턴제 게임처럼 “저장” 시점에만 동기화하는 경우
- **OnChanged**: 점수, 상태 플래그처럼 가끔만 바뀌는 데이터

## 동작 원리

1. **소유권 기반 송신**: 각 VObject는 한 명의 소유자(Owner)를 가집니다. **소유자만** 자신의 오브젝트 상태를 DTS 서버로 송신합니다.
2. **서버 중계**: DTS 서버가 송신된 데이터를 수신해, 같은 Room의 다른 클라이언트들에게 전달합니다.
3. **수신 측 적용**: 비소유자는 서버에서 받은 데이터를 로컬 오브젝트에 적용해, 모든 플레이어가 동일한 상태를 보도록 합니다.
4. **RPC**: View와 달리 연속 동기화가 아닌 **단발성 원격 호출**입니다. Lua 함수를 지정해 특정 플레이어(들)에게 실행을 요청합니다.

자세한 Host/Client 구조와 소유권은 [Host/Client 서버 모델](./01-host-client-server-model.md), [네트워크 소유권](./03-network-ownership.md)을 참고하세요.

## 네트워크 동기화 시 유의할 점

### 자주 발생하는 실수

1. **소유권 없이 송신 기대**
   - 오브젝트를 소유하지 않은 클라이언트에서 상태 변경을 해도 다른 플레이어에게 전달되지 않습니다.
   - 상호작용(그랩 등)으로 소유권을 얻은 뒤 동기화가 이루어집니다.

2. **동기화 타입 선택 오류**
   - 거의 변하지 않는 데이터에 `Continuous`를 쓰면 불필요한 대역폭을 사용합니다.
   - 실시간으로 움직이는 오브젝트에 `Manual`만 쓰면 끊기는 것처럼 보일 수 있습니다.

3. **RPC와 SyncView 혼동**
   - RPC: “이벤트를 한 번 보낸다” (예: 버튼 클릭, 액션 트리거)
   - SyncView/NetworkVariable: “상태를 지속적으로 맞춘다” (예: 위치, 회전, 점수)
   - 이벤트성 동작은 RPC, 연속 상태는 SyncView/NetworkVariable로 처리하는 것이 적절합니다.

4. **LuaBehaviour 없이 SyncView 사용**
   - `VivenCustomSyncView`는 `VivenLuaBehaviour`가 있어야 동작합니다. LuaBehaviour가 없으면 컴포넌트가 제거됩니다.

5. **직렬화 가능 타입만 동기화**
   - MessagePack으로 직렬화 가능한 타입만 NetworkVariable/SyncView에 사용할 수 있습니다. Lua Table은 배열 등으로 변환해야 합니다.

### 권장 사항

- 동기화가 필요한 오브젝트에는 반드시 `SDKNetworkObject`를 부착하고, 필요한 View(RPC, Transform, Rigidbody, Custom 등)를 추가합니다.
- 대역폭을 고려해 `VivenSDKSyncType`을 상황에 맞게 선택합니다.
- 상세 API와 Lua 예제는 [네트워크 및 동기화](../../03-scripting/05-networking-and-synchronization/)를 참고합니다.

## 관련 문서

- [Host/Client 서버 모델](./01-host-client-server-model.md)
- [네트워크 소유권](./03-network-ownership.md)
- [네트워크 및 동기화](../../03-scripting/05-networking-and-synchronization/) — RPC, NetworkVariable, SyncView, Room Property 상세
