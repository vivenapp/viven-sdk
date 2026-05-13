# 네트워크 변수 (Network Variables)

## 개요

네트워크 변수(`SyncVar`)는 특정 변수의 값이 변경될 때마다 자동으로 모든 클라이언트에 동기화되는 기능입니다. `VivenCustomSyncView`를 통해 Lua 스크립트에서 동적으로 생성하고 관리할 수 있습니다.

## 언제 사용하나요?

- 점수, 체력, 게임 상태 등 지속적으로 변화하는 데이터를 동기화해야 할 때
- 값이 변경되었을 때 모든 유저에게 특정 콜백 함수를 실행해야 할 때
- 나중에 접속한 유저(Late-joiner)도 현재의 최신 값을 받아야 할 때

## 준비사항

- **VObject**: 네트워크 식별을 위해 필요합니다.
- **VivenCustomSyncView**: `SyncVar`를 생성하고 관리하는 주체입니다.

## 진행 순서

1. **변수 생성**: `onSyncViewInitialized` 콜백 시점에 `SyncView:CreateLuaSyncVar`를 호출하여 변수를 등록합니다.
   ```lua
   local myScore = nil

   function onSyncViewInitialized(syncTable, fixedSyncTable)
       -- "Score"라는 ID로 초기값 0인 네트워크 변수 생성
       myScore = SyncView:CreateLuaSyncVar("Score", 0, true, OnScoreChanged)
   end

   -- 값이 변경될 때 실행될 콜백 함수
   function OnScoreChanged(oldValue, newValue)
       print("Score changed from " .. tostring(oldValue) .. " to " .. tostring(newValue))
   end
   ```

2. **값 수정 (오너 전용)**: 해당 오브젝트의 소유권을 가진 클라이언트에서만 값을 수정할 수 있습니다.
   ```lua
   function AddScore()
       if SyncView.IsMine then
           local current = myScore:Get()
           myScore:Set(current + 10)
       end
   end
   ```

3. **값 읽기**: `Get()` 메서드를 사용하여 현재 동기화된 값을 가져옵니다.
   ```lua
   local currentScore = myScore:Get()
   ```

## 확인 방법

- `OnScoreChanged` 콜백 함수 내에 로그를 작성하여 값이 변경될 때마다 정상적으로 호출되는지 확인합니다.
- 다른 클라이언트에서 해당 오브젝트의 값이 동일하게 유지되는지 확인합니다.

## 자주 일어나는 실수

- **오너십 확인 누락**: `Set()`을 통해 값을 변경하는 로직은 반드시 `SyncView.IsMine` 확인 후에 실행해야 합니다. 오너가 아닌 클라이언트에서의 수정은 무시됩니다.
- **지원되지 않는 타입**: `SyncVar`는 숫자(int, float), 문자열(string), Vector3, Quaternion 타입만 지원합니다. 테이블(table)은 직접 동기화할 수 없습니다.
- **초기화 시점**: `CreateLuaSyncVar`는 반드시 `onSyncViewInitialized` 이벤트가 발생한 후에 호출하는 것이 안전합니다.

## 관련 문서

- [원격 프로시저 호출 (RPC)](01-remote-procedure-calls.md)
- [동기화 뷰 (Sync View)](03-sync-view.md)
- [방 프로퍼티 (Room Property)](04-room-property.md)
