# 입력 (Input) 개요

## 개요

Viven 플랫폼에서 PC(마우스/키보드), VR 컨트롤러, 핸드 트래킹 등 다양한 입력 장치를 통해 사용자와 상호작용하는 방법을 설명합니다. Unity의 새로운 Input System을 기반으로 하여 플랫폼 간의 일관된 입력 처리를 지원합니다.

## 주요 특징

- **Unity Input System 활용**: 플랫폼에 독립적인 입력 액션을 정의하고 처리할 수 있습니다.
- **플랫폼별 입력 처리**: PC의 마우스/키보드 입력과 VR 컨트롤러의 특수 입력을 구분하여 대응합니다.
- **핸드 트래킹(Hand Tracking) 지원**: 실험적 기능으로 제공되는 핸드 트래킹 API를 통해 컨트롤러 없이 맨손으로 상호작용할 수 있습니다.
- **입력 모드 전환**: PC와 VR 환경 간의 입력 모드 전환을 자동으로 처리합니다.

## 학습 순서

1. **[Unity Input System 활용](./01-unity-input-system-guide.md)**: 입력 액션을 정의하고 Lua에서 이벤트를 구독하는 기본 방법을 배웁니다.
2. **[플랫폼별 입력 처리](./02-platform-specific-input/00-overview.md)**: PC와 VR 환경의 입력 방식 차이와 대응 방법을 익힙니다.
3. **[핸드 트래킹(Hand Tracking) API](./02-platform-specific-input/03-hand-tracking-api.md)**: 손 포즈와 제스처를 감지하여 상호작용하는 실험적 기능을 학습합니다.

## 준비사항

- Viven SDK가 설치된 Unity 프로젝트
- VR 테스트를 위한 VR 기기 (선택 사항)
- Unity Input System 패키지에 대한 기본적인 이해

## 관련 문서

- [컨텐츠 제작 개요](../00-overview.md)
- [플레이어 상호작용 모듈](../03-scripting/04-player-interaction-modules/00-overview.md)
- [PC/VR 플랫폼 개발 가이드](../01-project-management/05-pc-vr-platform-development.md)
