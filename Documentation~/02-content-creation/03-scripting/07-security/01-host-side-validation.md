# Host-side 검증

Viven 플랫폼은 여러 플레이어가 동시에 접속하는 환경이므로, 악의적인 클라이언트의 데이터 조작이나 비정상적인 요청을 방지하기 위해 **Host-side 검증**이 필수적입니다. Viven의 네트워크 모델에서 'Host'는 방을 생성했거나 현재 오브젝트의 소유권(Ownership)을 가진 클라이언트를 의미합니다.

## Host와 Client의 권한 분리

Viven의 보안 모델은 **"Client는 요청하고, Host는 결정한다"**는 원칙을 따릅니다.

| 구분 | Client (일반 플레이어) | Host (오너/방장) |
| :--- | :--- | :--- |
| **역할** | 자신의 입력 처리, 시각적 피드백 출력 | 월드 상태의 최종 결정, 데이터 유효성 검사 |
| **권한** | 자신의 아바타 제어, 상호작용 요청 | 오브젝트 소유권 승인, 플레이어 강퇴, 게임 로직 판정 |
| **신뢰도** | 낮음 (변조 가능성 있음) | 높음 (해당 세션의 기준점) |

## Host-side 검증 패턴

중요한 게임 로직(예: 점수 추가, 아이템 획득, 문 열기 등)은 반드시 아래와 같은 패턴으로 작성해야 합니다.

### 1. IsMine을 통한 권한 확인
모든 상태 변경 로직은 현재 클라이언트가 해당 오브젝트의 주인(`IsMine`)인지 확인하는 것에서 시작합니다.

```lua
-- 아이템 획득 로직 예시
function OnItemTouch(player)
    -- 1. 내가 Host(오너)인지 확인
    if not SyncView.IsMine then
        return 
    end
    
    -- 2. Host 권한으로 아이템 획득 처리 및 동기화
    print(player.name .. "님이 아이템을 획득했습니다.")
    -- NetworkVariable 등을 통해 상태 전파
end
```

### 2. RPC를 통한 검증 요청
일반 클라이언트가 무언가를 수행하고 싶을 때는 Host에게 RPC를 보내 "허가"를 요청해야 합니다.

```lua
-- [Client] 상호작용 시도
function RequestOpenDoor()
    -- Host에게 문을 열어달라고 요청 (SendOption.Target 사용 권장)
    SyncView:SendRPC("ValidateOpenDoor", CS.TwentyOz.VivenSDK.Scripts.Core.VivenComponents.VivenFields.SDKRPCSendOption.All, { Player.LocalPlayer.id })
end

-- [Host] 요청 검증 및 실행
function ValidateOpenDoor(requesterId)
    if not SyncView.IsMine then return end
    
    -- 거리가 충분히 가까운지, 열쇠가 있는지 등 검증
    if IsValidRequest(requesterId) then
        -- 문 열림 상태 동기화
        SetDoorState(true)
    end
end
```

## 관리자 권한 활용

방장(Creator)은 `Room.Player.KickPlayer`와 같은 강력한 API를 사용할 수 있습니다. 이러한 기능은 오직 방장 권한을 가진 클라이언트에서만 실행되도록 보호해야 합니다.

```lua
function TryKick(targetId)
    -- 현재 클라이언트가 방 생성자인지 확인
    local creatorId = Room.GetCreatorUserID()
    if Player.LocalPlayer.id == creatorId then
        Room.Player.KickPlayer(targetId)
    else
        print("권한이 없습니다.")
    end
end
```

## 주의사항

- **신뢰할 수 없는 데이터**: 클라이언트가 RPC 인자로 보내는 데이터(예: "나 지금 100점 얻었어")는 절대 그대로 믿지 마십시오. Host에서 다시 계산하거나 유효 범위를 체크해야 합니다.
- **소유권 이전**: `RequestOwnership()`을 통해 소유권이 바뀔 수 있으므로, 중요한 로직 실행 직전에 항상 `IsMine`을 재확인하십시오.
- **시각적 허용**: 문이 열리는 애니메이션 등 시각적 요소는 클라이언트에서 즉시 실행하여 반응성을 높이되, 실제 통과 가능 여부는 Host의 데이터에 기반해야 합니다.
