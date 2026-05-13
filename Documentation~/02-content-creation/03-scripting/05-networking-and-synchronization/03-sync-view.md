# 동기화 뷰 (Sync View)

## 개요

동기화 뷰(`Sync View`)는 네트워크 객체의 상태(위치, 회전, 물리 등)를 클라이언트 간에 자동으로 동기화하는 컴포넌트입니다. Viven은 기본적으로 `TransformView`와 `RigidbodyView`를 제공하며, Lua를 통해 커스텀 동기화 로직을 작성할 수 있는 `VivenCustomSyncView`를 지원합니다.

## 언제 사용하나요?

- 오브젝트의 위치(`Transform`)나 물리 상태(`Rigidbody`)를 실시간으로 동기화해야 할 때
- Lua 스크립트에서 특정 테이블 데이터를 매 틱(Tick)마다 동기화하고 싶을 때 (`CustomSyncView`)
- 오브젝트의 소유권(Ownership) 변화에 따른 처리가 필요할 때

## 준비사항

- **VObject**: 모든 동기화 뷰는 `VObject` 컴포넌트와 함께 사용되어야 합니다.
- **동기화 컴포넌트**: `SDKTransformView`, `SDKRigidbody`, `VivenCustomSyncView` 중 필요한 것을 부착합니다.

## 진행 순서

1. **컴포넌트 설정**: 유니티 에디터에서 `VObject`가 부착된 오브젝트에 `SDKTransformView` 등을 추가합니다.
2. **커스텀 데이터 동기화 (선택)**: `VivenCustomSyncView`를 사용하면 Lua 테이블 기반의 데이터 동기화가 가능합니다. `onSyncViewInitialized` 콜백에서 제공되는 `syncTable`과 `fixedSyncTable`을 활용합니다.
   ```lua
   -- 동기화할 데이터를 테이블에 담아 보냄
   function sendSyncUpdate()
       local data = {}
       data[1] = self.transform.position.x
       data[2] = MyCustomValue
       return data
   end

   -- 다른 클라이언트에서 동기화된 데이터를 받음
   function receiveSyncUpdate(syncTable)
       if syncTable then
           local x = syncTable[1]
           local customVal = syncTable[2]
           -- 받은 데이터로 로직 처리
       end
   end
   ```
3. **소유권 확인**: Lua 스크립트에서 `SyncView.IsMine`을 통해 현재 내가 이 오브젝트를 제어할 권한이 있는지 확인합니다.
   ```lua
   function onUpdate()
       if SyncView.IsMine then
           -- 내가 소유자일 때의 로직 (예: 키보드 입력으로 이동)
       else
           -- 소유자가 아닐 때의 로직 (자동으로 위치가 동기화됨)
       end
   end
   ```
3. **소유권 요청**: 필요한 경우 `SyncView:RequestOwnership()`을 호출하여 소유권을 가져올 수 있습니다. (상호작용 시 자동으로 처리되기도 합니다.)

## 확인 방법

- 두 개 이상의 클라이언트를 실행하여 한쪽에서 움직인 오브젝트가 다른 쪽에서도 부드럽게 따라오는지 확인합니다.
- `onOwnershipChanged` 콜백을 등록하여 소유권이 변경되는 시점을 로그로 확인합니다.

## 자주 일어나는 실수

- **커스텀 데이터 타입 제한**: `VivenCustomSyncView`를 통한 데이터 동기화 시, Lua의 기본 자료형(숫자, 문자열, 불리언)만 직접 전달할 수 있습니다.
- **테이블 동기화 시 직렬화 필요**: Lua 테이블을 직접 동기화 데이터로 넘길 수 없습니다. 테이블 구조를 동기화하려면 데이터를 문자열로 변환(직렬화)하여 전달하고, 받는 쪽에서 다시 테이블로 복구(역직렬화)해야 합니다.
  - Viven SDK에는 기본 JSON 라이브러리가 포함되어 있지 않으므로, 복잡한 테이블 구조를 동기화하려면 오픈소스 Lua JSON 라이브러리를 프로젝트에 포함하여 사용하는 것을 권장합니다.
  - 간단한 구조의 경우 아래와 같이 직접 직렬화 코드를 작성하여 사용할 수 있습니다.
  ```lua
  -- [직렬화 예시] 테이블의 값을 특정 구분자(;)를 사용해 문자열로 변환
  function SerializeTable(t)
      local str = ""
      for k, v in pairs(t) do
          str = str .. k .. ":" .. tostring(v) .. ";"
      end
      return str
  end

  -- [역직렬화 예시] 구분자로 나뉜 문자열을 다시 테이블로 변환
  function DeserializeTable(str)
      local t = {}
      for k, v in string.gmatch(str, "([^:]+):([^;]+);") do
          t[k] = v
      end
      return t
  end

  -- [보내는 쪽]
  function sendSyncUpdate()
      local myTable = { id = "1", status = "active" }
      local data = {}
      data[1] = SerializeTable(myTable) -- "id:1;status:active;" 문자열로 변환
      return data
  end

  -- [받는 쪽]
  function receiveSyncUpdate(syncTable)
      if syncTable and syncTable[1] then
          local myTable = DeserializeTable(syncTable[1]) -- 테이블로 복구
          print(myTable.status)
      end
  end
  ```
- **오너의 receiveSyncUpdate 미호출**: 현재 시스템에서 오브젝트의 소유자(IsMine == true)는 `receiveSyncUpdate` 함수가 자동으로 호출되지 않습니다. 오너 클라이언트에서도 동일한 로직이 실행되어야 한다면, `sendSyncUpdate` 시점에 수동으로 호출하거나 별도의 처리가 필요합니다. (이 동작은 추후 업데이트로 변경될 수 있습니다.)
- **중복 동기화**: 한 오브젝트에 `TransformView`와 `RigidbodyView`를 동시에 사용하면 물리 연산과 위치 보간이 충돌하여 떨림 현상이 발생할 수 있습니다.
- **동기화 타입 설정**: `Continuous`(지속적), `Manual`(수동), `OnChanged`(변경 시) 중 목적에 맞는 타입을 선택해야 합니다.
- **네트워크 부하**: 너무 많은 데이터를 실시간으로 동기화하면 네트워크 지연(Lag)의 원인이 됩니다.

## 관련 문서

- [원격 프로시저 호출 (RPC)](01-remote-procedure-calls.md)
- [네트워크 변수 (Network Variables)](02-network-variables.md)
- [방 프로퍼티 (Room Property)](04-room-property.md)
