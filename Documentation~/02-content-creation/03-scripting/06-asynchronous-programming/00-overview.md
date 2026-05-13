# 비동기 프로그래밍 (Asynchronous Programming) 개요

## 개요

Viven 플랫폼에서 시간 지연, 네트워크 요청, 리소스 로딩 등 즉각적인 완료를 보장할 수 없는 작업을 효율적으로 처리하는 방법을 설명합니다. Lua의 코루틴과 C#의 `Task`(`async/await`) 연동을 통해 게임의 프레임 저하 없이 비동기 로직을 구현할 수 있습니다.

## 주요 특징

- **코루틴(Coroutine) 활용**: 실행을 일시 중단하고 특정 조건(시간 경과 등)이 충족되면 다시 실행하여 순차적인 비동기 로직을 작성합니다.
- **C# Task 연동**: `Player.Mine.TryGrab` 등 `Task`를 반환하는 Viven API를 Lua에서 안전하게 대기하고 결과를 받아옵니다.
- **Awaiter 패턴**: Lua에서 C#의 `await`와 유사한 동작을 구현하기 위해 `GetAwaiter()`와 `IsCompleted`를 활용합니다.
- **프레임 양보(Yielding)**: `WaitForSeconds`, `WaitForEndOfFrame` 등을 통해 Unity 메인 스레드와 조화롭게 비동기 작업을 수행합니다.

## 학습 순서

1. **[코루틴 (Coroutine)](./01-unity-coroutines.md)**: Lua에서 `StartCoroutine`과 `util.cs_generator`를 사용하여 시간 지연 로직을 구현하는 방법을 배웁니다.
2. **[C# Task와 async/await](./02-csharp-task-and-async-await.md)**: `Task`를 반환하는 Viven API를 호출하고, `Awaiter`를 통해 비동기 작업의 완료를 대기하고 결과를 가져오는 패턴을 익힙니다.

## 준비사항

- `VivenLuaBehaviour`를 사용하는 Lua 스크립트
- `xlua.util` 모듈 (XLua 기본 제공)
- 기본적인 Lua 코루틴 개념에 대한 이해

## 관련 문서

- [스크립팅 개요](../00-overview.md)
- [Viven API 레퍼런스](../03-viven-services-and-api/02-viven-api.md)
- [Unity 라이프사이클 및 실행 시점](../03-viven-services-and-api/01-unity-lifecycle-callbacks.md)
