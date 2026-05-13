# 플레이어 데이터 및 상태 관리

## 개요
VivenScript를 사용하여 현재 접속 중인 플레이어(나 또는 타인)의 닉네임, 고유 ID, 프로필 정보 등을 확인하고 관리할 수 있습니다.

## 언제 사용하나요?
- 플레이어의 닉네임을 UI에 표시해야 할 때
- 특정 유저 ID를 기반으로 점수나 상태를 저장하고 불러올 때
- 플레이어가 현재 어떤 모드(PC, XR, Mobile)로 접속 중인지 확인하여 연출을 다르게 하고 싶을 때
- 다른 플레이어의 위치를 특정 지점으로 이동(Teleport)시켜야 할 때

## 준비사항
- `VivenLuaBehaviour`가 부착된 게임 오브젝트
- `Player` API (VivenScript 기본 제공)

## 진행 순서

### 1. 내 정보 확인하기
`Player.Mine`을 통해 자신의 정보를 즉시 가져올 수 있습니다.

```lua
-- 내 닉네임과 ID 가져오기
local myNickname = Player.Mine.Nickname
local myUserId = Player.Mine.UserID

-- 내 상세 데이터 테이블 가져오기
local myData = Player.Mine.GetPlayerData()
-- myData.nickname : 닉네임
-- myData.userId : 고유 ID
-- myData.userTag : 로그인 ID (태그)

-- 현재 접속 모드 확인 (PC, XR, Mobile)
local playMode = Player.Mine.PlayMode
```

### 2. 다른 플레이어 정보 확인하기
`Player.Other`를 사용하여 방에 있는 다른 플레이어의 정보를 조회합니다.

```lua
-- 닉네임으로 유저 ID 찾기
local targetId = Player.Other.GetPlayerID("상대방닉네임")

if targetId ~= nil then
    -- 유저 ID로 상세 데이터 가져오기
    local otherData = Player.Other.GetPlayerData(targetId)
    if otherData ~= nil then
        Debug.Log("찾은 유저: " .. otherData.nickname)
    end
end
```

### 3. 유저 정보 조회 예시 (SpectatorManager 활용)
특정 유저 ID가 내 ID인지 확인하거나, 타인인 경우 정보를 안전하게 가져오는 방법입니다.

```lua
function GetUserNickname(userId)
    -- 내 ID와 비교
    if userId == Player.Mine.UserID then
        return Player.Mine.Nickname
    else
        -- 타인인 경우 상세 데이터에서 닉네임 추출
        local player_data = Player.Other.GetPlayerData(userId)
        if player_data ~= nil then
            return player_data.nickname
        else
            return "Unknown"
        end
    end
end
```

## 확인 방법
- `Debug.Log`를 사용하여 가져온 닉네임이나 ID가 올바른지 콘솔 창에서 확인합니다.
- `Player.Mine.PlayMode`를 출력하여 현재 기기 환경에 맞는 문자열이 반환되는지 확인합니다.

## 자주 일어나는 실수
- **존재하지 않는 유저 조회**: `Player.Other.GetPlayerData`는 해당 유저가 방에 없거나 정보를 찾을 수 없을 때 `nil`을 반환합니다. 반드시 결과값이 `nil`인지 체크해야 합니다.
- **내 정보 조회 시점**: 스크립트의 `awake` 시점에는 네트워크 정보가 아직 완전히 동기화되지 않았을 수 있습니다. 가급적 `start` 이후나 이벤트 콜백 내에서 정보를 조회하는 것이 안전합니다.
- **대소문자 구분**: API의 속성 이름(예: `Nickname`, `UserID`)은 대소문자를 정확히 지켜야 합니다.

## 관련 문서
- [아바타 시스템](../08-characters/01-viven-avatar-system.md)
- [월드 이동 및 텔레포트](./02-teleport-and-world-travel.md)
