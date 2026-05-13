# 오디오 (Audio) 개요

## 개요

Viven 플랫폼에서 FMOD(Studio)를 기반으로 입체적이고 풍부한 사운드 환경을 구축하는 방법을 설명합니다. `VivenAudioEventInstance` 컴포넌트를 통해 사운드 이벤트를 재생하고, 네트워크 동기화를 통해 모든 플레이어가 동일한 오디오 경험을 공유할 수 있습니다.

## 주요 특징

- **FMOD Studio 연동**: Unity 오디오 클립뿐만 아니라 FMOD의 강력한 사운드 이벤트를 직접 사용할 수 있습니다.
- **오디오 그룹 관리**: `Default`, `Sfx`, `Environment`, `Bgm` 등 그룹별로 최대 재생 길이와 볼륨을 관리합니다.
- **네트워크 동기화 재생**: RPC를 통해 모든 클라이언트에서 동시에 사운드를 재생하고 제어할 수 있습니다.
- **3D 사운드 지원**: 오브젝트의 위치에 따라 소리의 방향과 거리를 자동으로 반영합니다.

## 학습 순서

1. **[FMOD 사운드 재생 (VivenAudioEventInstance)](./01-viven-audio-event-instance-fmod.md)**: `VivenAudioEventInstance` 컴포넌트 설정과 Lua를 통한 재생/정지 제어 방법을 배웁니다.
2. **오디오 재생 및 제어** *(준비 중)*: `PlayOneShot`, 볼륨 및 피치 조절 등 상세한 오디오 제어 기법을 익힙니다.
3. **오디오 그룹 및 볼륨 관리** *(준비 중)*: 그룹별 재생 제한과 전체 볼륨 설정을 관리하는 방법을 학습합니다.

## 준비사항

- Viven SDK가 설치된 Unity 프로젝트
- 재생할 오디오 클립 또는 FMOD 이벤트 에셋
- `VivenLuaBehaviour`를 통한 Lua 스크립트 연결 환경

## 관련 문서

- [컨텐츠 제작 개요](../00-overview.md)
- [원격 프로시저 호출 (RPC)](../03-scripting/05-networking-and-synchronization/01-remote-procedure-calls.md)
- [Viven API 레퍼런스 (UI/System)](../03-scripting/03-viven-services-and-api/02-viven-api.md)
