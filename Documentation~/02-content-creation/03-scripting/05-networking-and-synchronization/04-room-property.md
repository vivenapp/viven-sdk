# 방 프로퍼티 (Room Property)

## 개요

방 프로퍼티(`Room Property`)는 현재 접속 중인 방(Room) 단위로 공유되는 데이터 테이블입니다. 특정 오브젝트에 귀속되지 않고 방 전체의 상태를 저장하며, 서버에 저장되어 방이 유지되는 동안 데이터가 보존됩니다.

## 언제 사용하나요?

- 게임의 전체 점수판(Leaderboard) 관리
- 현재 게임의 진행 단계(준비, 시작, 종료 등) 공유
- 방의 설정 값(맵 테마, 제한 시간 등) 저장
- 플레이어가 방에 없어도 유지되어야 하는 공통 데이터 관리

## 준비사항

- **Room API**: Lua 스크립트에서 `Room.SetRoomProp`, `Room.GetRoomProp` 메서드에 접근할 수 있어야 합니다.

## 진행 순서

1. **프로퍼티 설정**: `Room.SetRoomProp`을 사용하여 데이터를 저장합니다.
   ```lua
   -- "GameState" 프로퍼티를 "Playing"으로 설정
   Room.SetRoomProp("GameState", "Playing")
   ```

2. **프로퍼티 읽기**: `Room.GetRoomProp`을 호출하여 현재 설정된 값을 즉시 가져옵니다.
   ```lua
   -- "GameState" 값 읽기
   local state = Room.GetRoomProp("GameState")
   print("Current Game State: " .. tostring(state))
   ```

3. **변경 이벤트 구독**: 특정 프로퍼티 값이 변경될 때 실행될 콜백을 등록할 수 있습니다.
   ```lua
   function onStart()
       -- "GameState" 변경 시 OnStateChanged 함수 호출 등록
       Room.RegisterRoomPropChanged("GameState", OnStateChanged)
   end

   function OnStateChanged(newValue)
       print("Game State Updated: " .. tostring(newValue))
   end

   function onDestroy()
       -- 등록된 콜백 해제
       Room.UnRegisterRoomPropChanged("GameState", OnStateChanged)
   end
   ```

## 확인 방법

- 서로 다른 클라이언트에서 동일한 프로퍼티를 설정하고 읽었을 때 값이 일치하는지 확인합니다.
- 방을 나갔다가 다시 들어왔을 때(방이 유지되고 있다면) 이전 값이 그대로 남아있는지 확인합니다.

## 자주 일어나는 실수

- **데이터 덮어쓰기**: 여러 클라이언트가 동시에 같은 프로퍼티를 수정하면 마지막에 도달한 요청이 이전 값을 덮어씁니다.
- **지연 시간**: 방 프로퍼티는 서버를 거쳐 동기화되므로 즉각적인 반응이 필요한 물리 동기화 등에는 적합하지 않습니다.
- **무결성 보장 미흡**: 네트워크 상태에 따라 데이터 전송이 지연되거나 순서가 바뀔 수 있으므로 중요한 로직은 이를 고려해야 합니다.

## 관련 문서

- [원격 프로시저 호출 (RPC)](01-remote-procedure-calls.md)
- [네트워크 변수 (Network Variables)](02-network-variables.md)
- [동기화 뷰 (Sync View)](03-sync-view.md)
