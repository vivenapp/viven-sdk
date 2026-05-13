# 사용자 인터페이스 (UI) 개요

## 개요

Viven 컨텐츠 내에서 사용자에게 정보를 제공하고 상호작용하기 위한 UI(User Interface) 제작의 전반적인 개념을 설명합니다. Viven은 PC와 VR 환경을 모두 지원하므로, 두 환경 모두에서 원활하게 작동하는 UI를 설계하고 구현하는 것이 핵심입니다.

## 주요 특징

> [!CAUTION]
> **UI Toolkit은 Viven SDK에서 지원하지 않습니다.** 모든 UI는 반드시 **UGUI**로 제작해야 합니다.
> 텍스트 표시에는 Unity 기본 `Text` 대신 **TextMeshPro** (`TMP_Text`, `TextMeshProUGUI`) 컴포넌트를 사용하세요.

- **Unity UGUI 기반**: 친숙한 Unity의 UGUI 시스템을 그대로 활용할 수 있습니다.
- **Lua 스크립팅**: UI의 로직(버튼 클릭, 텍스트 업데이트 등)은 Lua 스크립트를 통해 동적으로 제어합니다.
- **PC/VR 통합 지원**: `VivenGraphicRaycaster`를 통해 마우스와 VR 레이캐스트 입력을 동시에 처리합니다.
- **World Space 권장**: VR 환경과의 호환성을 위해 모든 UI는 `World Space` Canvas로 제작하는 것을 권장합니다.
- **TextMeshPro 권장**: 텍스트 UI에는 `TextMeshProUGUI` 컴포넌트를 사용하세요. Unity 기본 `Text` 대비 해상도 독립적이고 스타일링 옵션이 풍부합니다.

## 학습 순서

1. **[Unity UGUI 활용 가이드](./01-unity-ugui-guide.md)**: 기본적인 UI 컴포넌트 사용법과 Lua 스크립트 연결 방법을 배웁니다.
2. **[체험 내 UI (World Space UI)](./02-world-space-ui.md)**: VR 환경을 고려한 World Space Canvas 설정과 PC/VR 대응 방법을 배웁니다.
3. **[Viven 전용 UI 컴포넌트](./03-viven-ui-components.md)**: `VivenGraphicRaycaster`, `UIModeChanger` 등 Viven에서 제공하는 특수 컴포넌트의 상세 기능을 확인합니다.

## 준비사항

- Unity의 UGUI 시스템에 대한 기본적인 이해
- Viven SDK가 설치된 Unity 프로젝트
- UI 로직을 작성할 Lua 스크립트 환경

## 관련 문서

- [스크립팅 (Scripting) 개요](../03-scripting/00-overview.md)
- [PC/VR 플랫폼 개발](../01-project-management/05-pc-vr-platform-development.md)
