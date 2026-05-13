# 캐릭터 (Characters) 개요

## 개요

Viven 플랫폼에서 사용자의 분신이 되는 아바타 시스템과 이를 제어하는 캐릭터 컨트롤러에 대해 설명합니다. 아바타의 외형 커스터마이징, 감정 표현, 그리고 PC와 VR 환경 모두에 최적화된 이동 및 조작 시스템을 구축할 수 있습니다.

## 주요 특징

- **Viven 아바타 시스템**: 다양한 의상과 액세서리를 교체할 수 있는 유연한 아바타 구조를 제공합니다.
- **감정 및 얼굴 표현**: 애니메이션과 블렌드 쉐입을 활용하여 풍부한 감정 표현과 립싱크를 지원합니다.
- **커스텀 애니메이션**: Unity의 Animator를 활용하여 아바타 고유의 동작을 추가하고 제어할 수 있습니다.
- **플랫폼 최적화 컨트롤러**: PC의 WASD 이동과 VR의 텔레포트/로코모션 시스템을 동시에 지원합니다.

## 학습 순서

1. **[Viven 아바타 시스템](./01-viven-avatar-system.md)**: 아바타의 기본 구조와 SDKOutfitComponent를 통한 외형 관리 방법을 배웁니다.
2. **[아바타 외형 및 커스터마이징](./02-avatar-customization.md)**: 실시간으로 아바타의 의상과 구성을 변경하는 방법을 익힙니다.
3. **[감정 및 얼굴 표현](./03-emotes.md)**: 이모트(Emotes)와 표정(Facial Expressions)을 재생하고 제어하는 방법을 학습합니다.
4. **[커스텀 애니메이션 시스템](./05-custom-animation-system-animator.md)**: 사용자 정의 애니메이션을 아바타에 적용하고 Lua로 제어하는 기법을 배웁니다.
5. **[Viven 전용 캐릭터 컨트롤러](./06-viven-character-controller/00-overview.md)**: PC와 VR 환경에서의 이동 및 조작 가이드를 확인합니다.

## 준비사항

- Viven SDK가 설치된 Unity 프로젝트
- 아바타로 사용할 3D 모델 및 애니메이션 에셋
- `VivenLuaBehaviour`를 통한 Lua 스크립트 연결 환경

## 관련 문서

- [컨텐츠 제작 개요](../00-overview.md)
- [플레이어 데이터 및 상태 관리](../09-players/01-player-data-and-state-management.md)
- [Viven API 레퍼런스 (Player)](../03-scripting/03-viven-services-and-api/02-viven-api.md)
