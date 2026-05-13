# 원격 프로시저 호출 (RPC)

## 개요

RPC(Remote Procedure Call)는 같은 방에 있는 다른 유저의 네트워크 객체 내 메서드를 원격으로 실행할 수 있는 기능입니다. 연속적인 데이터 동기화가 아닌, 특정 시점에 발생하는 일회성 이벤트를 전달할 때 사용합니다.

## 언제 사용하나요?

- 특정 유저가 버튼을 눌렀을 때 모든 유저에게 효과음을 재생해야 할 때
- 게임 시작, 종료와 같은 상태 변화를 알릴 때
- 특정 유저에게만 메시지를 보내거나 UI를 표시해야 할 때

## 준비사항

- **VObject**: RPC를 송수신할 게임 오브젝트에 부착되어 있어야 합니다.
- **VivenCustomSyncView**: Lua 스크립트에서 RPC를 사용하기 위해 필요합니다.
- **VivenLuaBehaviour**: RPC로 실행될 함수가 정의된 Lua 스크립트가 연결되어 있어야 합니다.

## 진행 순서

1. **RPC 함수 정의**: Lua 스크립트에 다른 유저가 호출할 함수를 작성합니다.
   ```lua
   -- 호출될 함수 정의
   function PlayEffect(effectId)
       print("Effect played: " .. effectId)
       -- 효과음 재생 로직
   end
   ```

2. **RPC 전송**: `SyncView:SendRPC` 또는 `SyncView:SendTargetRPC`를 사용하여 함수를 호출합니다.
   ```lua
   -- 모든 유저(나 포함)에게 RPC 전송
   local AllOption = RPCSendOption.All
   SyncView:SendRPC("PlayEffect", AllOption, "Explosion_01")

   -- 나를 제외한 다른 유저들에게만 전송
   local OthersOption = RPCSendOption.Others
   SyncView:SendRPC("PlayEffect", OthersOption, "Explosion_01")
   ```

3. **특정 대상에게 전송**: 특정 유저에게만 RPC를 보낼 수 있습니다.
   ```lua
   -- SyncView의 소유권을 가진 사람에게 RPC를 전송
   local targetId = SyncView.ControlUserId
   if targetId ~= nil then
       local players = { targetId }
       SyncView:SendTargetRPC("PlayEffect", players, "SecretMessage")
   end
   ```

## 확인 방법

- 로그 창(`Debug.Log`)을 통해 RPC가 정상적으로 송수신되는지 확인합니다.
- 멀티플레이 환경에서 다른 클라이언트의 화면이나 소리가 의도대로 동작하는지 확인합니다.

## 자주 일어나는 실수

- **함수 이름 오타**: RPC로 호출할 함수 이름이 Lua 스크립트에 정의된 이름과 정확히 일치해야 합니다.
- **매개변수 타입 제한 및 직렬화**: RPC 매개변수로는 Lua의 기본 자료형(숫자, 문자열, 불리언)만 직접 전달할 수 있습니다. 
- **테이블 전달 시 직렬화 필요**: 테이블 구조를 RPC로 보내려면 `Sync View`와 마찬가지로 데이터를 문자열로 변환(직렬화)하여 전달해야 합니다. 받는 쪽에서는 이를 다시 테이블로 복구(역직렬화)하는 과정이 필요합니다.
- **매개변수 개수 제한**: Viven RPC는 최대 10개의 매개변수까지 지원합니다.
- **함수 오버로딩 불가**: Lua는 함수 오버로딩을 지원하지 않으므로, 같은 이름의 함수를 여러 개 정의하지 마세요.
- **VObject 부재**: RPC는 `VObject`를 기반으로 통신하므로, 해당 컴포넌트가 없는 오브젝트에서는 동작하지 않습니다.

## 관련 문서

- [네트워크 변수 (Network Variables)](02-network-variables.md)
- [동기화 뷰 (Sync View)](03-sync-view.md)
- [방 프로퍼티 (Room Property)](04-room-property.md)
