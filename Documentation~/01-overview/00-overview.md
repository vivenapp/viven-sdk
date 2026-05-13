# 개요

VIVEN은 트웬티온스의 자체 메타버스 플랫폼입니다.  
VR 미디어 기반의 다중 접속 엔터테인먼트, 관광, 체험, 교육 콘텐츠를 제작하고 운영할 수 있으며, 플레이 중 `XR`과 `PC`를 실시간으로 전환할 수 있습니다.

이 가이드는 VIVEN 플랫폼 자체를 개발하는 사람을 위한 문서가 아니라, `Viven` 위에서 월드와 상호작용 콘텐츠를 만드는 제작자를 위한 사용자 가이드입니다.

## VIVEN에서 할 수 있는 일

VIVEN에서는 Unity 기반 프로젝트 안에서 다양한 형태의 콘텐츠를 만들 수 있습니다.

### 월드와 공간 만들기

3D 에셋, 라이팅, 카메라, 물리 요소를 조합해 사용자가 탐색하고 상호작용할 수 있는 공간을 구성할 수 있습니다.

자세한 내용은 다음 문서를 참고하세요.

- [컨텐츠 제작 개요](../02-content-creation/00-overview.md)
- [3D 월드 구성 및 에셋 개요](../02-content-creation/02-3d-world-building-and-assets/00-overview.md)
- [환경 개요](../02-content-creation/07-environment/00-overview.md)

### 스크립트로 상호작용 만들기

`VivenLuaBehaviour`와 Viven 전용 API를 사용해 오브젝트 동작, 플레이어 상호작용, 네트워크 동기화 로직을 구성할 수 있습니다.

자세한 내용은 다음 문서를 참고하세요.

- [스크립팅 개요](../02-content-creation/03-scripting/00-overview.md)
- [VivenLuaBehaviour](../02-content-creation/03-scripting/01-viven-lua-behaviour.md)

### 입력, 오디오, UI 연결하기

플랫폼별 입력 처리, 월드 안의 UI, 오디오 재생과 제어를 조합해 플레이 경험을 구성할 수 있습니다.

자세한 내용은 다음 문서를 참고하세요.

- [입력 개요](../02-content-creation/04-input/00-overview.md)
- [오디오 개요](../02-content-creation/05-audio/00-overview.md)
- [UI 개요](../02-content-creation/06-ui/00-overview.md)

### 캐릭터와 플레이 경험 구성하기

아바타, 이동, 텔레포트, 채팅 같은 요소를 조합해 사용자 경험을 완성할 수 있습니다.

자세한 내용은 다음 문서를 참고하세요.

- [캐릭터 개요](../02-content-creation/08-characters/00-overview.md)
- [플레이어 개요](../02-content-creation/09-players/00-overview.md)
- [채팅 개요](../02-content-creation/10-chat/00-overview.md)

## VIVEN 플랫폼의 특징

### 실시간 XR, PC 전환

VIVEN은 사용자가 플레이 중에도 상황에 따라 `XR`과 `PC` 환경을 오가며 콘텐츠를 이용할 수 있도록 설계되어 있습니다.  
따라서 콘텐츠를 만들 때는 입력 방식, UI 배치, 이동 방식이 플랫폼에 따라 어떻게 달라지는지 함께 고려해야 합니다.

### 다양한 목적의 콘텐츠 제작

VIVEN은 단순한 게임 제작 도구가 아니라, 엔터테인먼트, 관광, 체험, 교육 등 여러 목적의 콘텐츠를 만들 수 있는 플랫폼입니다.  
즉, 시각적 완성도뿐 아니라 사용자 안내, 상호작용 흐름, 멀티유저 환경에서의 경험 설계도 중요합니다.

### 제작자 중심의 확장 가능한 구조

VIVEN은 Unity 기반 제작 흐름 위에서 월드 구성, 스크립팅, 입력, UI, 오디오, 캐릭터 시스템을 조합해 콘텐츠를 확장할 수 있습니다.  
이 문서 세트는 이러한 기능을 한 번에 모두 설명하기보다, 필요한 주제를 하나씩 따라갈 수 있도록 구성되어 있습니다.

## 문서 읽는 순서

VIVEN이 처음이라면 아래 순서로 읽는 것을 권장합니다.

1. [SDK 설치 및 설정](01-sdk-installation-and-setup.md)
2. [개발 환경 설정](02-development-environment/00-overview.md) (선택)
3. [컨텐츠 제작 개요](../02-content-creation/00-overview.md)

특정 기능만 빠르게 찾고 싶다면 `컨텐츠 제작` 아래의 각 개요 문서부터 시작하세요.
