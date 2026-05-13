# Viven API

## 개요

Viven API는 Lua 스크립트를 통해 Viven 플랫폼의 핵심 기능(플레이어 제어, 방 정보 관리, 시스템 UI 조작 등)을 제어할 수 있게 해주는 인터페이스입니다.

## 언제 사용하나요?

- 플레이어의 위치를 강제로 이동시키거나 속도를 조절하고 싶을 때
- 현재 방의 설정 정보(Room Property)를 읽거나 수정하고 싶을 때
- 시스템 알림(Toast)을 띄우거나 화면 페이드 효과를 주고 싶을 때
- 채팅 메시지를 전송하거나 음성 채팅 설정을 변경하고 싶을 때
- 맵의 환경(시간, 안개 등)을 동적으로 변경하고 싶을 때

## 주요 API 모듈

Viven API는 기능별로 여러 모듈로 나뉘어 있습니다. 모든 API는 별도의 임포트 없이 Lua 스크립트에서 즉시 사용할 수 있습니다.

### 1. Player (플레이어 제어)

로컬 플레이어(`Player.Mine`)와 다른 플레이어(`Player.Other`)의 정보를 관리합니다.

#### Player.Mine (나의 정보 및 제어)

| API | 설명 |
| :--- | :--- |
| `Player.Mine.Nickname` | 현재 플레이어의 닉네임을 가져옵니다. |
| `Player.Mine.UserID` | 현재 플레이어의 고유 ID(UUID)를 가져옵니다. |
| `Player.Mine.TeleportPlayer(pos, rot)` | 플레이어를 특정 위치와 회전값으로 순간이동 시킵니다. |
| `Player.Mine.CharacterMoveLock` | (Property) 플레이어의 이동 및 회전을 잠그거나 해제합니다. |
| `Player.Mine.CharacterAnimator` | (Property) 캐릭터의 Animator 컴포넌트에 접근합니다. |
| `Player.Mine.ChangeAvatar(avatarId)` | 플레이어의 아바타를 변경합니다 (사용자 승인 팝업 노출). |

#### Player.Other (타인 정보 및 제어)

| API | 설명 |
| :--- | :--- |
| `Player.Other.GetPlayerID(nickname)` | 닉네임으로 플레이어의 고유 ID를 조회합니다. |
| `Player.Other.GetPlayerData(playerId)` | 플레이어 ID로 닉네임, 태그 등을 포함한 데이터를 가져옵니다. |
| `Player.Other.TeleportOtherPlayer(id, pos, rot)` | 특정 플레이어를 순간이동 시킵니다 (방장 권한 필요). |

### 2. Room (방 및 네트워크 관리)

현재 접속 중인 방의 정보와 네트워크 동기화 데이터를 관리합니다.

#### Room (기본 방 관리)

| API | 설명 |
| :--- | :--- |
| `Room.GetRoomProp(propId)` | 방의 특정 프로퍼티 값을 가져옵니다. |
| `Room.SetRoomProp(propId, propVal)` | 방의 특정 프로퍼티 값을 설정합니다. |
| `Room.RegisterRoomPropChanged(id, callback)` | 프로퍼티 값이 변경될 때 실행될 콜백을 등록합니다. |
| `Room.LeaveRoom()` | 현재 방을 나가고 마이룸으로 이동합니다. |
| `Room.Player.KickPlayer(playerId)` | 특정 플레이어를 강제 퇴장시킵니다 (방장 권한 필요). |

#### Room.Map (맵 및 환경 설정)

| API | 설명 |
| :--- | :--- |
| `Room.Map.GetMapProp(propId)` | 맵 고유의 프로퍼티 값을 가져옵니다. |
| `Room.Map.Setting.SkyDomeTime` | (Property) 맵의 시간을 설정합니다 (0 ~ 24). |
| `Room.Map.Setting.SetSkyDomeFog(fog)` | 맵의 안개(Fog) 농도를 설정합니다. |
| `Room.Map.Setting.SetActiveSkyDome(active)` | SkyDome 시스템의 활성화 여부를 설정합니다. |

#### Room.VoiceChat (음성 채팅)

| API | 설명 |
| :--- | :--- |
| `Room.VoiceChat.MicMute(isMute)` | 내 마이크를 음소거하거나 해제합니다. |
| `Room.VoiceChat.SpeakerMute(isMute)` | 전체 음성 출력을 음소거하거나 해제합니다. |
| `Room.VoiceChat.SetSpeakerVolume(volume)` | 음성 채팅 출력 볼륨을 설정합니다 (0 ~ 1). |
| `Room.VoiceChat.MutePlayer(userId, isMute)` | 특정 플레이어의 음성만 음소거합니다. |

### 3. TextChat (텍스트 채팅)

채팅 메시지 전송 및 채팅 UI 제어를 담당합니다.

| API | 설명 |
| :--- | :--- |
| `TextChat.SendChannelTextMessage(msg)` | 현재 채널에 전체 메시지를 보냅니다. |
| `TextChat.SendDirectTextMessage(msg, targetId)` | 특정 유저에게 귓속말을 보냅니다. |
| `TextChat.LockTextChatUiOpen()` | 채팅창 UI가 열리지 않도록 잠급니다. |
| `TextChat.UnlockTextChatUiOpen()` | 채팅창 UI 잠금을 해제합니다. |

### 4. UI (시스템 UI 및 효과)

Viven 시스템 UI를 조작하거나 사용자에게 메시지를 전달합니다.

| API | 설명 |
| :--- | :--- |
| `UI.ToastMessage(message, duration)` | 화면 하단에 일반 안내 메시지를 띄웁니다. |
| `UI.ToastWarningMessage(message, duration)` | 화면 하단에 경고 메시지를 띄웁니다. |
| `UI.FadeIn(time, callback)` | 화면을 서서히 밝게 만듭니다 (Fade In). |
| `UI.FadeOut(time, callback)` | 화면을 서서히 어둡게 만듭니다 (Fade Out). |
| `UI.OpenSettingWindow()` | 시스템 설정 창을 엽니다. |
| `UI.CloseAllWindow()` | 열려 있는 모든 시스템 창을 닫습니다. |

### 5. System (시스템 설정)

언어 설정 및 마우스 커서 등 시스템 레벨의 기능을 제어합니다.

| API | 설명 |
| :--- | :--- |
| `Locale.GetLocale()` | 현재 설정된 언어를 가져옵니다 ("Korean", "English"). |
| `Locale.SetLocale(locale)` | 시스템 언어를 변경합니다. |
| `VivenSystem.Mouse.ShowCursor()` | 마우스 커서를 화면에 표시합니다. |
| `VivenSystem.Mouse.HideCursor()` | 마우스 커서를 숨깁니다. |

## 사용 예시

### 환경 제어 및 알림 (시간 변경)

```lua
-- 맵의 시간을 정오로 변경하고 알림 표시
Room.Map.Setting.SkyDomeTime = 12.0
UI.ToastMessage("현재 시간이 정오로 설정되었습니다.", 3)
```

### 귓속말 전송 예시

```lua
-- 특정 닉네임을 가진 플레이어에게 귓속말 보내기
local targetNickname = "VivenUser"
local targetId = Player.Other.GetPlayerID(targetNickname)

if targetId ~= nil then
    TextChat.SendDirectTextMessage("안녕하세요!", targetId)
else
    UI.ToastWarningMessage("플레이어를 찾을 수 없습니다.")
end
```

## 자주 일어나는 실수


- **대소문자 구분**: Lua는 대소문자를 엄격히 구분하므로 `Player.Mine`과 `player.mine`은 다르게 처리됩니다.
- **콜백 해제**: `RegisterRoomPropChanged`로 등록한 콜백은 `UnRegisterRoomPropChanged`를 통해 반드시 해제해야 메모리 누수를 방지할 수 있습니다.

## 관련 문서

- [Unity 생명주기 콜백](01-unity-lifecycle-callbacks.md)
- [원격 프로시저 호출 (RPC)](../05-networking-and-synchronization/01-remote-procedure-calls.md)
