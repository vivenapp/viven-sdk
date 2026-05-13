# FMOD 사운드 재생 (VivenAudioEventInstance)

Viven 플랫폼에서 FMOD(Studio)의 사운드 이벤트를 재생하고 제어하는 방법을 설명합니다. `VivenAudioEventInstance` 컴포넌트를 사용하면 오디오 클립이나 FMOD 이벤트를 간편하게 재생하고, 볼륨이나 피치 등을 실시간으로 조절할 수 있습니다.

## 개요

`VivenAudioEventInstance`는 FMOD의 `EventInstance`를 Viven 시스템에 맞춰 래핑한 컴포넌트입니다. Unity 인스펙터에서 사운드 파일을 직접 할당하거나, FMOD 프로젝트의 `EventReference`를 연결하여 사용할 수 있습니다.

## 주요 설정

컴포넌트를 게임 오브젝트에 추가한 후, 인스펙터에서 다음 항목을 설정합니다.

- **Audio Clip**: 재생할 Unity `AudioClip`을 할당합니다. (FMOD 이벤트를 직접 사용하지 않을 경우)
- **Event Reference**: FMOD Studio에서 생성한 이벤트를 직접 참조할 때 사용합니다. (Audio Clip보다 우선순위가 높습니다)
- **Group Type**: 사운드가 속할 그룹을 지정합니다. 그룹별로 최대 재생 길이가 제한될 수 있습니다.
  - `Default`: 기본 효과음 (최대 2분 30초)
  - `Sfx`: 효과음 (최대 15초)
  - `Environment`: 환경음 (최대 15초)
  - `Bgm`: 배경음 (최대 15초)
- **Auto Play On Start**: 오브젝트가 활성화될 때 자동으로 사운드를 재생할지 여부입니다.
- **Is Looping On Start**: 자동 재생 시 반복 재생 여부를 설정합니다.

## Lua 스크립팅 가이드

Lua 스크립트에서 `VivenAudioEventInstance`를 참조하여 사운드를 제어할 수 있습니다.

### 기본 재생 및 정지

```lua
-- VivenAudioEventInstance 컴포넌트 참조 (인스펙터에서 연결되었다고 가정)
local audioInstance = self:GetComponent("VivenAudioEventInstance")

-- 사운드 재생 (반복 여부 선택 가능, 기본값 false)
audioInstance:Play(true) -- 반복 재생

-- 사운드 1회 재생 (PlayOneShot)
-- 루핑 설정이 무시되며, 재생 후 인스턴스가 자동으로 관리됩니다.
audioInstance:PlayOneShot()

-- 사운드 정지
audioInstance:Stop()
```

### 볼륨 및 피치 조절 (테스트 중)

재생 중인 사운드의 속성을 실시간으로 변경할 수 있습니다. (현재 해당 기능은 테스트 중입니다.)

```lua
-- 볼륨 설정 (0.0 ~ 1.0)
audioInstance:SetVolume(0.5)

-- 피치 설정 (기본값 1.0)
audioInstance:SetPitch(1.2)
```

## 네트워크 동기화 (RPC)

멀티플레이어 환경에서 모든 플레이어에게 동시에 사운드를 들려주려면 RPC(Remote Procedure Call)를 사용해야 합니다.

다음은 효과음을 재생할 때 서버를 거쳐 모든 클라이언트에서 실행되도록 하는 예시입니다.

```lua
-- SOUND_TYPE은 사전에 정의된 열거형이나 테이블이라고 가정합니다.
-- 예: SOUND_TYPE = { CLICK = 1, EXPLOSION = 2 }

--- 효과음 재생 요청 (모든 클라이언트 대상)
--- @param sfxType number
function PlayOneShotSFX(sfxType)
    -- "PlayOneShotSFX_Client"라는 이름의 RPC 함수를 모든 클라이언트에서 실행
    RPC_Client("PlayOneShotSFX_Client", sfxType)
end

--- 실제 클라이언트에서 실행되는 사운드 재생 로직
--- @param sfxType number
function PlayOneShotSFX_Client(sfxType)
    -- sfxAudio 테이블에 VivenAudioEventInstance들이 미리 매핑되어 있어야 합니다.
    local targetAudio = sfxAudio[sfxType]
    
    if targetAudio == nil then
        -- print("SoundManager: PlayOneShotSFX_Client - Invalid sfxType: " .. tostring(sfxType))
        return
    end

    -- 1회성 효과음 재생
    targetAudio:PlayOneShot()
end
```

## 주의 사항

- **최대 길이 제한**: `Group Type`에 따라 사운드의 최대 길이가 제한됩니다. 긴 배경음악은 반드시 `Default` 또는 `Bgm` 그룹을 사용하고, 효과음은 `Sfx` 그룹을 권장합니다.
- **리소스 해제**: `VivenAudioEventInstance`는 오브젝트가 파괴될 때(`OnDestroy`) 자동으로 FMOD 인스턴스를 정지하고 해제(`release`)합니다.
- **3D 사운드**: `PlayOneShot` 호출 시, 해당 오브젝트의 현재 위치(`transform.position`)가 3D 속성으로 자동 반영됩니다.
