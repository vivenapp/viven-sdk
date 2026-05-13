# 스크립팅 (Scripting) 개요

## 개요

VivenScript는 Lua 기반 스크립트로, Viven 콘텐츠의 게임 로직과 상호작용을 구현하는 핵심 수단입니다. `VivenLuaBehaviour` 컴포넌트를 통해 Unity의 라이프사이클과 Viven 전용 이벤트를 Lua로 제어할 수 있으며, C# 컴파일 없이도 실시간으로 로직을 수정하고 테스트할 수 있습니다.

## 주요 특징

- **xLua 기반 런타임**: Unity C# API를 Lua에서 직접 호출할 수 있으며, 높은 성능과 유연성을 제공합니다.
- **Viven 전용 API**: 플레이어 제어, 방 정보 관리, 시스템 UI 조작 등 Viven 플랫폼 특화 기능을 API로 제공합니다.
- **네트워크 동기화 통합**: RPC와 네트워크 변수(`SyncVar`)를 통해 멀티플레이어 환경에서의 데이터 동기화를 손쉽게 구현합니다.
- **비동기 프로그래밍 지원**: 코루틴과 C# Task 연동을 통해 시간 지연이나 비동기 작업을 효율적으로 처리합니다.

## 학습 순서

1. **[VivenLuaBehaviour 활용](./01-viven-lua-behaviour.md)**: `VivenLuaBehaviour` 컴포넌트 사용법과 기본 스크립트 작성법을 익힙니다.
2. **[Viven 전용 서비스 및 API](./03-viven-services-and-api/00-overview.md)**: Unity 라이프사이클 콜백과 Viven 전용 API 모듈들의 사용법을 배웁니다.
3. **[플레이어 상호작용 모듈](./04-player-interaction-modules/00-overview.md)**: `Grabbable`, `Sittable` 등 상호작용 컴포넌트와 Lua 이벤트를 연결합니다.
4. **[네트워크 및 동기화](./05-networking-and-synchronization/00-overview.md)**: RPC, `SyncVar`, `SyncView`를 활용한 멀티플레이어 로직을 구현합니다.
5. **[비동기 프로그래밍](./06-asynchronous-programming/00-overview.md)**: 코루틴과 C# Task(`async/await`) 연동 패턴을 학습합니다.
6. **[Lua 레퍼런스](./08-lua-reference/00-overview.md)**: Lua 언어의 기본 문법과 xLua 특화 기능들을 상세히 확인합니다.

## 준비사항

- Viven SDK가 설치된 Unity 프로젝트
- Lua 스크립트 작성을 위한 에디터 (VSCode 권장)
- 기본적인 Lua 문법에 대한 이해

## 관련 문서

- [컨텐츠 제작 개요](../00-overview.md)
- [코드 재사용 및 모듈화](./02-code-reuse-and-modularity.md)
- [보안 및 검증 (Security)](./07-security/00-overview.md)
