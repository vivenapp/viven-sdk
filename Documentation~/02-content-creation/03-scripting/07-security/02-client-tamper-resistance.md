# 클라이언트 변조 방지

클라이언트 변조 방지는 악의적인 사용자가 로컬 실행 환경(메모리, 스크립트 등)을 조작하여 부당한 이득을 취하거나 다른 플레이어의 경험을 방해하는 것을 최소화하는 전략입니다. Viven은 구조적으로 클라이언트의 영향력을 제한하여 보안을 유지합니다.

## 클라이언트의 책임과 한계

클라이언트는 오직 **자신에게 허용된 범위** 내에서만 데이터를 생성하고 전송할 책임이 있습니다.

- **책임**: 자신의 위치 동기화, 로컬 입력 캡처, 서버로부터 받은 상태 렌더링.
- **한계**: 다른 플레이어의 상태 강제 변경 불가, Host 권한 없이 월드 프로퍼티 수정 불가.

## 주요 방어 전략

### 1. NetworkVariable의 단방향 동기화
`NetworkVariable`은 오직 소유자(`IsMine == true`)만이 값을 수정할 수 있습니다. 일반 클라이언트가 메모리 변조를 통해 로컬 값을 바꾸더라도, 네트워크를 통해 전파되지 않으며 다음 동기화 시점에 Host의 값으로 덮어씌워집니다.

```lua
-- 클라이언트가 로컬에서 값을 바꿔도, 오너가 아니면 전파되지 않음
function Update()
    if not SyncView.IsMine then
        -- 여기서 값을 수정하려고 시도해도 네트워크 상에서는 무시됨
        myNetVar:Set(9999) 
    end
end
```

### 2. 소유권(Ownership) 기반 제어
Viven의 모든 동기화 오브젝트는 소유권이 명확히 분리되어 있습니다.

- **내 아바타**: 내가 소유자이므로 이동 데이터를 보낼 수 있음.
- **공용 오브젝트**: 누군가 `RequestOwnership()`을 통해 권한을 획득하기 전까지는 Host가 제어함.

클라이언트가 소유권이 없는 오브젝트의 위치를 강제로 옮기려 해도, 다른 플레이어들에게는 Host가 관리하는 원래 위치로 보이게 됩니다.

### 3. RPC 인자 검증 (Sanitization)
클라이언트가 Host에게 보내는 RPC 인자는 항상 조작될 수 있다고 가정해야 합니다.

```lua
-- [안전하지 않은 예시] 클라이언트가 보낸 점수를 그대로 더함
function UnsafeAddScore(score)
    if not SyncView.IsMine then return end
    CurrentScore = CurrentScore + score
end

-- [안전한 예시] 요청만 받고, 점수 계산은 Host가 직접 수행
function SafeAddScore()
    if not SyncView.IsMine then return end
    -- 클라이언트의 인자 없이 Host가 상황을 판단하여 점수 부여
    CurrentScore = CurrentScore + 10 
end
```

### 4. 속도 및 거리 체크 (Sanity Check)
클라이언트가 보내는 위치 데이터가 비정상적으로 빠르거나 멀리 떨어져 있는지 Host에서 체크할 수 있습니다.

```lua
local lastPos = nil

function OnPlayerMove(player, newPos)
    if not SyncView.IsMine then return end
    
    if lastPos then
        local distance = Vector3.Distance(lastPos, newPos)
        if distance > MaxAllowedSpeed then
            -- 핵 사용 의심: 위치 강제 되돌리기 또는 경고
            print("비정상적인 이동 감지: " .. player.name)
        end
    end
    lastPos = newPos
end
```

## 요약: 보안 체크리스트

1. [ ] **상태 변경 전 IsMine 확인**: 내가 이 데이터를 바꿀 권한이 있는가?
2. [ ] **중요 로직의 Host 집중**: 판정 로직이 일반 클라이언트에 분산되어 있지 않은가?
3. [ ] **RPC 인자 최소화**: 클라이언트에게 너무 많은 정보를 요구하거나 믿고 있지 않은가?
4. [ ] **RoomProperty 활용**: 월드 전체에 영향을 주는 설정은 반드시 방장 권한으로 `Room.SetRoomProp`을 통해 관리하고 있는가?

Viven 플랫폼은 클라이언트의 자율성을 존중하면서도, Host 중심의 검증 체계를 통해 월드의 무결성을 유지합니다.
