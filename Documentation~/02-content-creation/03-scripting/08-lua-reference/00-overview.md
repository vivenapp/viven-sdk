# Lua 레퍼런스 (Lua Reference) 개요

## 개요

VivenScript에서 사용하는 Lua 언어의 기본 문법과 xLua를 통해 Unity C# API를 호출할 때의 특수한 기능들을 상세히 설명합니다. Lua의 유연성과 C#의 강력한 기능을 결합하여 효율적인 스크립팅 환경을 구축할 수 있습니다.

## 주요 특징

- **Lua 5.5 표준 지원**: 최신 Lua 문법과 연산자, 제어 구조를 활용할 수 있습니다.
- **C# 타입 매핑**: Lua의 기본 타입과 C# 데이터 타입 간의 자동 변환 및 매핑 규칙을 제공합니다.
- **xLua 특화 기능**: 메서드 오버로딩, 연산자 오버로딩, 확장 메서드 등 C#의 고급 기능을 Lua에서 그대로 사용할 수 있습니다.
- **변수 범위 관리**: `local` 키워드를 통한 Private 변수와 전역 선언을 통한 Public 변수(Inspector 노출)를 구분합니다.

## 학습 순서

1. **[형식 (Types)](./01-types.md)**: Lua의 기본 타입과 C# 데이터 타입 간의 매핑 관계를 이해합니다.
2. **[데이터 구조 (Data Structures)](./02-data-structures.md)**: Lua의 핵심 데이터 구조인 테이블(Table)을 활용하여 배열과 딕셔너리를 구현합니다.
3. **[기능 (Features)](./03-features.md)**: C# 클래스 접근, 메서드 오버로딩, 연산자 오버로딩 등 xLua의 강력한 기능들을 배웁니다.
4. **[변수 및 범위 (Variables and Scope)](./04-variables-and-scope.md)**: 변수 선언 방식에 따른 노출 범위와 `Global` 테이블을 통한 전역 데이터 공유를 익힙니다.
5. **[연산자 및 제어 구조 (Operators and Control Flow)](./05-operators-and-control-flow.md)**: Lua의 표준 연산자와 조건문, 반복문 사용법을 학습합니다.
6. **[함수 및 이벤트 (Functions and Events)](./06-functions-and-events.md)**: Lua 함수 정의와 C# 델리게이트, 이벤트 구독 방법을 배웁니다.

## 준비사항

- Lua 스크립트 작성을 위한 에디터 환경
- xLua 기반의 Viven SDK 프로젝트 환경
- 기본적인 프로그래밍 개념에 대한 이해

## 관련 문서

- [스크립팅 개요](../00-overview.md)
- [VivenLuaBehaviour 활용](../01-viven-lua-behaviour.md)
- [Viven API 레퍼런스](../03-viven-services-and-api/02-viven-api.md)
