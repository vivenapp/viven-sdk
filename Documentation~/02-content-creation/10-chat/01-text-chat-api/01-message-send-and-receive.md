# 메시지 송수신

## 개요
VivenScript를 사용하여 월드 내 다른 플레이어와 텍스트 메시지를 주고받거나, 특정 플레이어에게 귓속말을 보낼 수 있습니다. 또한 채팅창 UI의 활성화 여부를 제어하여 특정 연출이나 상황에서 채팅 입력을 제한할 수 있습니다.

## 언제 사용하나요?
- 스크립트 이벤트 발생 시(예: 퀘스트 완료, 아이템 획득) 자동으로 채팅창에 메시지를 출력하고 싶을 때
- 특정 플레이어에게만 비밀 메시지(귓속말)를 보내야 할 때
- 미니게임 진행 중이나 컷신 연출 시 채팅창이 열리지 않도록 잠궈야 할 때

## 준비사항
- `VivenLuaBehaviour`가 부착된 게임 오브젝트
- `TextChat` API (VivenScript 기본 제공)
- `Player` API (상대방 ID 확인용)

## 진행 순서

### 1. 전체 채팅 보내기
`TextChat.SendChannelTextMessage`를 사용하여 현재 접속 중인 채널의 모든 플레이어에게 메시지를 보냅니다.

```lua
-- 전체 채팅 메시지 보내기
TextChat.SendChannelTextMessage("안녕하세요! 반가워요.")
```

### 2. 귓속말 보내기
`TextChat.SendDirectTextMessage`를 사용하여 특정 플레이어에게만 메시지를 보냅니다. 이때 상대방의 고유 유저 ID(`UserID`)가 필요합니다.

```lua
-- 상대방의 닉네임으로 UserID 찾기
local targetNickname = "상대방닉네임"
local targetId = Player.Other.GetPlayerID(targetNickname)

if targetId ~= nil then
    -- 찾은 ID로 귓속말 보내기
    TextChat.SendDirectTextMessage("비밀 메시지입니다.", targetId)
else
    Debug.Log("해당 닉네임을 가진 플레이어를 찾을 수 없습니다.")
end
```

### 3. 채팅창 UI 제어
특정 상황에서 플레이어가 채팅창을 열지 못하도록 잠그거나, 다시 열 수 있도록 설정할 수 있습니다.

```lua
-- 채팅창 UI 열기 잠금 (플레이어가 엔터 키 등을 눌러도 채팅창이 열리지 않음)
TextChat.LockTextChatUiOpen()

-- 채팅창 UI 잠금 해제
TextChat.UnlockTextChatUiOpen()
```

## 확인 방법
- `SendChannelTextMessage` 호출 후 실제 채팅창에 메시지가 정상적으로 출력되는지 확인합니다.
- `LockTextChatUiOpen` 호출 후 채팅창 단축키(기본 Enter)를 눌렀을 때 UI가 나타나지 않는지 확인합니다.

## 자주 일어나는 실수
- **잘못된 유저 ID**: 귓속말 전송 시 `targetId`가 `nil`인지 반드시 확인해야 합니다. 존재하지 않는 ID로 메시지를 보내면 전송되지 않습니다.
- **UI 잠금 상태 유지**: `LockTextChatUiOpen`을 호출한 뒤 적절한 시점에 `UnlockTextChatUiOpen`을 호출하지 않으면 플레이어가 계속해서 채팅을 사용할 수 없게 됩니다.

## 관련 문서
- [플레이어 데이터 및 상태 관리](../../09-players/01-player-data-and-state-management.md)
- [Viven UI 컴포넌트](../../06-ui/03-viven-ui-components.md)
