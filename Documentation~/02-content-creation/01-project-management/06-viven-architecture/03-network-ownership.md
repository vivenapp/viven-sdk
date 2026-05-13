# 네트워크 소유권

## 개요

Viven의 Host/Client 구조에서 **네트워크 소유권(Network Ownership)**은 각 VObject가 **누가 상태를 송신할 권한을 가지는지**를 결정합니다. 소유자(Owner)만 자신의 오브젝트 상태를 DTS 서버로 송신하고, 비소유자는 서버에서 받은 데이터를 로컬에 적용합니다. Grab, Sit 등 상호작용을 수행하면 소유권이 변경되며, 소유한 Client가 방을 나갈 경우에도 DTS 서버가 소유권을 재분배합니다.

## 언제 사용하나요?

- Grabbable, Sittable 등 상호작용 오브젝트를 여러 사용자가 사용할 때
- 오브젝트의 Transform, Rigidbody, NetworkVariable 등이 다른 플레이어와 동기화되어야 할 때
- Lua 스크립트에서 `onOwnershipChanged` 콜백으로 소유권 변경을 감지해야 할 때

## Host/Client 구조와 소유권

Viven은 [Host/Client 서버 모델](./01-host-client-server-model.md)을 사용합니다. Host와 Client 모두 **소유한 VObject에 한해** 동기화를 송신합니다.

| 역할 | 소유권 관련 동작 |
|------|------------------|
| **Host** | 방에 처음 들어온 사람. 기본적으로 새로 생성되는 오브젝트의 소유권을 가짐 |
| **Client** | Grab, Sit 등 상호작용 시 소유권을 요청해 받을 수 있음 |

소유권은 **DTS 서버에서 관리**되며, `ControlUserId`로 식별됩니다. 클라이언트는 `SetRoomObjControlUser` 요청을 통해 소유권을 요청하고, 서버가 `OnSetRoomObjControlUser`로 결과를 브로드캐스트합니다.

## 네트워크 상태 동기화와 소유권

[동기화 시스템](./02-synchronization-system.md)에서 설명한 대로, 동기화는 **소유권 기반**으로 동작합니다.

1. **소유자만 송신**: 각 VObject는 한 명의 소유자를 가집니다. 소유자만 자신의 오브젝트 상태(Transform, Rigidbody, NetworkVariable 등)를 DTS로 송신합니다.
2. **서버 중계**: DTS 서버가 송신된 데이터를 수신해, 같은 Room의 다른 클라이언트들에게 전달합니다.
3. **비소유자 수신**: 비소유자는 서버에서 받은 데이터를 로컬 오브젝트에 적용해, 모든 플레이어가 동일한 상태를 보도록 합니다.

소유권이 없으면 오브젝트의 상태 변경이 다른 플레이어에게 전달되지 않습니다. 상호작용(그랩, 앉기 등)으로 소유권을 얻은 뒤 동기화가 이루어집니다.

## 상호작용으로 인한 소유권 변경

### Grab (그랩)

- Grabbable 오브젝트를 잡을 때 연결된 Attach된 오브젝트들까지 포함해 소유권 요청을 보냅니다.
- DTS 서버가 요청을 받아 소유권을 해당 플레이어에게 할당하고, `OnSetRoomObjControlUser`로 모든 클라이언트에 알립니다.
- 그랩을 놓아도(EndInteraction) 소유권은 그대로 유지됩니다. (다른 플레이어가 소유권을 요청하기 전까지)

### Sit (앉기)

- Sittable 오브젝트에 앉는 동안 해당 오브젝트의 소유권이 앉은 플레이어에게 할당됩니다.
- Sittable 오브젝트에서 일어나도, 다른 플레이어가 앉기 전까지 소유권은 유지됩니다.

## 소유자가 방을 나갈 때

오브젝트를 소유한 Client(또는 Host)가 방을 나가면, **DTS 서버가 해당 오브젝트들의 소유권을 자동으로 재분배**합니다.

- Host가 방에서 나갈 경우: 오브젝트들의 소유권은 **랜덤하게 남은 Client들에게 분배**되고, 새로운 Host가 선출됩니다.
- Client가 방에서 나갈 경우: 해당 Client가 소유하던 오브젝트의 소유권도 마찬가지로 DTS 서버에 의해 남은 플레이어들에게 재분배됩니다.

클라이언트는 `OnSetRoomObjControlUser` 이벤트를 통해 새 소유권 정보를 받고, `IsMine` 값이 변경됩니다.

## 확인 방법

- **Lua**: `VivenCustomSyncView`의 `onOwnershipChanged` 콜백을 등록해 소유권 변경 시점을 감지할 수 있습니다.
- **C#**: `VivenDtsObject.IsMine`, `ControlUserId` 프로퍼티로 현재 소유자 여부와 ID를 확인할 수 있습니다.

## 자주 일어나는 실수

1. **소유권 없이 송신 기대**
   - 오브젝트를 소유하지 않은 클라이언트에서 상태 변경을 해도 다른 플레이어에게 전달되지 않습니다.
   - 상호작용(그랩, 앉기 등)으로 소유권을 얻은 뒤 동기화가 이루어집니다.

. **소유자가 나갈 때의 처리**

- DTS 서버가 자동으로 소유권을 재분배하므로, 별도 클라이언트 로직이 필요하지 않습니다.
- 다만 `onOwnershipChanged` 등으로 소유권 변경을 감지해 UI나 로직을 조정할 수 있습니다.

## 관련 문서

- [Host/Client 서버 모델](./01-host-client-server-model.md)
- [동기화 시스템](./02-synchronization-system.md)
- [네트워크 및 동기화](../../03-scripting/05-networking-and-synchronization/) — RPC, NetworkVariable, SyncView, Room Property 상세
