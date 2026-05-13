# 유니티 라이프사이클 및 실행 시점

## 개요

Lua 스크립트에서 Unity의 표준 이벤트 함수(Start, Update 등)와 Viven 플랫폼 고유의 이벤트를 처리하는 방법, 그리고 각 초기화 단계별 특징을 설명합니다. `VivenLuaBehaviour` 컴포넌트가 부착된 오브젝트는 스크립트 내에 특정 이름의 함수를 정의하는 것만으로 해당 시점에 로직을 실행할 수 있습니다.

## 언제 사용하나요?

- 오브젝트가 생성되거나 활성화될 때 초기화 로직을 수행할 때 (`awake`, `start`, `onEnable`)
- 매 프레임마다 상태를 업데이트하거나 입력을 확인할 때 (`update`)
- 물리 연산이나 일정 시간 간격의 로직이 필요할 때 (`fixedUpdate`)
- 오브젝트가 파괴되거나 비활성화될 때 자원을 정리할 때 (`onDestroy`, `onDisable`)
- 네트워크 변수(`SyncVar`)를 생성하고 초기값을 설정할 때 (`onSyncViewInitialized`)
- 룸 입장/퇴장 등 네트워크 이벤트에 대응할 때 (`onRoomJoined`, `onRoomLeave` 등)

## 준비사항

- `VivenLuaBehaviour` 컴포넌트가 부착된 Unity 오브젝트
- 해당 컴포넌트에 연결된 Lua 스크립트 파일 (`VivenScript`)
- 네트워크 동기화가 필요한 경우 `VivenCustomSyncView` 컴포넌트

## 초기화 및 실행 시점 가이드

안정적인 스크립트 작성을 위해 각 단계에서 보장되는 상태를 파악하는 것이 중요합니다.

### 1. 스크립트 최상단 (함수 바깥 영역)

스크립트 파일이 로드되는 즉시 실행되는 영역입니다.

- **가능한 작업**: 로컬 변수 선언, `checkInject`를 통한 주입 변수 확인, 기본 유틸리티 함수 정의.
- **주의사항**: 이 시점에서는 **다른 컴포넌트나 오브젝트의 초기화 상태를 절대 보장할 수 없습니다.** `GetLuaBehaviour` 등을 통해 다른 스크립트의 테이블을 가져오려고 하면 실패할 가능성이 매우 높습니다.

### 2. awake() 콜백

Unity의 `Awake` 시점에 호출됩니다.

- **특징**: 해당 오브젝트의 `VivenLuaBehaviour`는 초기화된 상태입니다.
- **주의사항**: 다른 오브젝트의 `Awake`가 아직 호출되지 않았을 수 있으므로, 외부 오브젝트의 컴포넌트를 참조하거나 테이블을 반환받는 것은 위험합니다.

### 3. onEnable() / onDisable() 콜백

오브젝트가 활성화/비활성화될 때마다 호출됩니다.

- **특징**: `awake` 이후, `start` 이전에 처음 호출됩니다.

### 4. start() 콜백

첫 번째 프레임 업데이트 직전에 호출됩니다.

- **특징**: 대부분의 일반적인 컴포넌트 초기화가 완료된 시점입니다.

### 5. onSyncViewInitialized(syncTable, fixedSyncTable)

네트워크 동기화 뷰(`SyncView`)가 완전히 준비되었을 때 호출되는 Viven 고유 콜백입니다.

- **특징**: **네트워크 변수(`SyncVar`) 및 `SyncView` 관련 데이터의 초기화가 보장되는 시점**입니다.
- **권장사항**: `CreateLuaSyncVar`를 통한 네트워크 변수 생성은 반드시 이 콜백 내에서 수행하십시오.

## 주요 콜백 목록

### Unity 표준 콜백

| 함수명 | 호출 시점 |
|:---|:---|
| `awake()` | 스크립트 인스턴스가 로딩될 때 (가장 먼저 호출) |
| `start()` | 첫 번째 프레임 업데이트 직전에 호출 |
| `onEnable()` | 오브젝트가 활성화될 때 호출 |
| `onDisable()` | 오브젝트가 비활성화될 때 호출 |
| `update()` | 매 프레임마다 호출 |
| `fixedUpdate()` | 설정된 물리 프레임마다 호출 |
| `onDestroy()` | 오브젝트가 파괴될 때 호출 |

### Viven 네트워크 콜백 (DTS)

| 함수명 | 호출 시점 |
|:---|:---|
| `onRoomJoined(roomData)` | 현재 방에 입장 완료했을 때 호출 |
| `onRoomUserJoined(userData)` | 다른 사용자가 방에 입장했을 때 호출 |
| `onUserLeaveRoom(userData)` | 다른 사용자가 방에서 나갔을 때 호출 |
| `onRoomLeave()` | 자신이 방에서 나가기 직전에 호출 |

### 트리거 및 충돌 콜백

| 함수명 | 호출 시점 |
|:---|:---|
| `onTriggerEnter(collider)` | 다른 트리거 콜라이더와 접촉했을 때 |
| `onCollisionEnter(collision)` | 다른 콜라이더와 충돌했을 때 |
| `onPlayerEnter(userId)` | Viven 플레이어가 트리거 영역에 들어왔을 때 |

## 확인 방법

1. Lua 스크립트에 `Debug.Log`를 추가하여 각 단계의 호출 순서와 변수 할당 여부를 확인합니다.
2. Unity Editor의 Console 창에서 로그가 정상적으로 출력되는지 확인합니다.

```lua
-- 예시 코드
function awake()
    Debug.Log("Lua Awake 호출됨")
end

function start()
    Debug.Log("Lua Start 호출됨")
end

function onSyncViewInitialized(syncTable, fixedSyncTable)
    Debug.Log("네트워크 동기화 준비 완료")
    -- 여기서 SyncVar 생성 권장
end

function onRoomJoined(roomData)
    Debug.Log("방 입장 완료: " .. roomData.roomName)
end
```

## 자주 일어나는 실수

- **대소문자 구분**: Lua는 대소문자를 구분합니다. `Start()`가 아닌 `start()`와 같이 소문자로 정의해야 정확히 매핑됩니다.
- **최상단 영역에서의 복잡한 로직**: 함수 바깥 영역에서 다른 오브젝트를 찾거나 복잡한 계산을 수행하면 초기화 순서 문제로 인해 에러가 발생하기 쉽습니다.
- **awake 시점의 테이블 참조**: `awake`에서 다른 스크립트의 데이터를 가져오려 할 때, 대상 스크립트의 `awake`가 아직 실행되지 않아 빈 테이블을 반환받거나 에러가 발생할 수 있습니다.
- **네트워크 변수 조기 생성**: `onSyncViewInitialized` 이전에 네트워크 변수를 생성하면 동기화가 누락될 수 있습니다.

## 관련 문서

- [Viven API 개요](02-viven-api.md)
- [네트워크 변수 활용](../05-networking-and-synchronization/02-network-variables.md)
- [상호작용 이벤트 처리](../04-player-interaction-modules/03-interaction-event-handling.md)
