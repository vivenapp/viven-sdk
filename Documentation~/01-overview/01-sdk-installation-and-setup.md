# SDK 설치 및 설정

## 개요

이 문서는 VIVEN 콘텐츠 제작을 시작하기 위해 `Unity`, 필수 패키지, `Viven SDK`를 설치하고 기본 설정을 적용하는 방법을 설명합니다.

## 언제 사용하나요?

- 처음으로 `Viven` 기반 프로젝트를 만들 때
- 새 PC에 제작 환경을 다시 구성할 때
- `Viven SDK`를 새 프로젝트에 Import할 때

## 준비사항

### 필수 준비물

- `Unity Hub`
- `VIVEN` 계정
- 안정적인 인터넷 연결

### 지원 Unity 버전

현재 프로젝트 기준 버전은 `6000.0.68f1`입니다.

### 권장 Unity 모듈

VIVEN 콘텐츠는 여러 플랫폼 배포를 고려하는 경우가 많으므로, 아래 모듈을 함께 설치해 두는 것이 좋습니다.

- **`Windows Build Support (IL2CPP)`** (필수)
- `Mac Build Support (Mono)`
- `Web Build Support`
- `Android Build Support` (추후 지원 예정)
- `iOS Build Support` (추후 지원 예정)

### 선택 사항

`Viven Script` 편집을 더 편하게 하려면 아래 IDE 중 하나를 사용하는 것을 권장합니다.

- `Visual Studio 2019` 이상
- `JetBrains Rider`
- `Visual Studio Code`

`EmmyLua`, `Lua Language Server` 등 `Lua` 플러그인을 함께 설치하면 스크립트 작성이 편리해집니다.

## 진행 순서

### 1. Unity Editor 설치

1. `Unity Hub`를 엽니다.
2. `Installs`에서 `6000.0.68f1`을 설치합니다.
3. 설치 과정에서 필요한 플랫폼 모듈을 함께 선택합니다.

여러 플랫폼에 배포할 계획이 있다면 모듈을 나중에 다시 추가하기보다 처음부터 함께 설치하는 편이 안전합니다.

### 2. 새 URP 프로젝트 만들기

1. `Unity Hub`에서 새 프로젝트를 생성합니다.
2. 템플릿으로 `Universal 3D`를 선택합니다.
3. 원하는 프로젝트 이름과 위치를 정한 뒤 프로젝트를 생성합니다.

VIVEN은 `URP`(Universal Render Pipeline)를 기준으로 동작합니다.  
따라서 `Built-in` 템플릿보다 `Universal 3D` 템플릿으로 시작하는 것이 좋습니다.

### 3. Viven SDK 설치

1. 아래 `VIVEN SDK` Git URL을 복사합니다.

   > `https://gitlab.twentyoz.kr:8443/viven-public/viven-sdk.git`

2. Unity 프로젝트에서 `Window > Package Manager` 창을 엽니다.
3. 왼쪽 상단 `+` 버튼을 클릭하고 `Install package from git URL…`을 선택합니다.
4. 복사한 Git URL을 입력하고 `Install` 버튼을 클릭합니다.

필수 의존 패키지(`Addressables`, `OpenXR Plugin` 등)는 SDK 설치 시 자동으로 함께 설치됩니다.

### 4. Viven SDK Settings 적용

설치가 완료되면 환경설정 UI가 자동으로 표시됩니다.

- 아직 설정되지 않은 항목은 **회색**, 설정이 완료된 항목은 **초록색** 버튼으로 표시됩니다.
- 위에서부터 순차적으로 버튼을 눌러 환경설정을 완료합니다.

### 5. 로그인 및 도메인 확인

설정이 끝나면 Unity Editor 우측 상단에 로그인 버튼이 표시되는지 확인합니다.

1. 우측 상단의 `Login` 버튼을 누릅니다.
2. 자신의 `VIVEN` 계정으로 로그인합니다.
3. 도메인 선택이 보이면 `Public`을 선택합니다.

### 6. Sample Scene 다운로드 (선택)

`Window > Package Manager`에서 `Viven SDK`를 선택하면 `Sample Scene`을 추가로 다운받을 수 있습니다.
샘플을 통해 SDK의 기본 기능과 사용법을 빠르게 확인할 수 있습니다.

### 7. Addressables 오류가 있으면 재확인

`Addressable Settings` 자동 적용 후에도 일부 설정을 수동으로 다시 맞춰야 할 수 있습니다.
빌드나 Import 과정에서 Addressables 관련 오류가 나면 아래 순서로 다시 확인하세요.

1. `VIVEN SDK > Settings`에서 관련 설정 버튼을 다시 실행합니다.
2. `Window > Package Manager`에서 `Addressables` 패키지가 정상 설치되었는지 확인합니다.
3. `Window > Asset Management > Addressables`에서 설정 자산이 생성되었는지 확인합니다.
4. `Addressables` 설정에서 `Catalog.json` 사용 옵션이 활성화되어 있는지 확인합니다.
5. 오류가 계속되면 팀에서 사용하는 기준 프로젝트 또는 샘플 프로젝트의 Addressables 설정과 비교합니다.

## 확인 방법

아래 항목이 모두 충족되면 기본 설치와 설정이 끝난 것입니다.

- `Unity 6000.0.68f1`로 프로젝트가 정상적으로 열립니다.
- 프로젝트가 `Universal 3D` 기반으로 생성되어 `URP`를 사용합니다.
- `Package Manager`에서 `Viven SDK`와 의존 패키지(`Addressables`, `OpenXR Plugin`)가 설치된 것을 확인할 수 있습니다.
- 환경설정 UI에서 모든 버튼이 초록색으로 표시됩니다.
- Unity Editor 우측 상단에 `Login` 버튼이 표시되고 `VIVEN` 계정으로 로그인할 수 있습니다.
- `Console`에 SDK 설치 실패나 필수 패키지 누락 오류가 남지 않습니다.

## 자주 일어나는 실수

### Unity 버전이 다름

`6000.0.68f1`이 아닌 다른 버전을 사용하면 패키지 호환성이나 SDK 설정 항목이 다르게 보일 수 있습니다.

### URP가 아닌 프로젝트로 시작함

`Built-in` 템플릿으로 프로젝트를 만들면 이후 렌더링 설정을 다시 맞춰야 할 수 있습니다.  
새 프로젝트는 `Universal 3D`로 시작하는 편이 안전합니다.

### 기준 프로젝트와 다른 패키지 버전 사용

패키지 충돌이 보이면 현재 기준 프로젝트의 `manifest.json` 값을 먼저 확인하세요.

### 환경설정 버튼을 건너뜀

설치 후 표시되는 환경설정 UI에서 일부 버튼을 건너뛰면 런타임 동작에 필요한 설정이 빠질 수 있습니다.
모든 버튼이 초록색이 될 때까지 순차적으로 실행하세요.

### 플랫폼 모듈을 나중에 추가하려고 미룸

초기에는 Windows만 사용하더라도, 이후 `Android`, `iOS`, `Web` 테스트가 필요해질 수 있습니다.  
가능하면 처음 설치할 때 필요한 Build Support를 함께 넣어 두세요.

## 다음 단계

SDK 설치가 끝났으면 선택적으로 개발 도구를 추가 설정할 수 있습니다.

- [개발 환경 설정](02-development-environment/00-overview.md) — AI Toolkit, Lua Language Server, MCP Server

바로 콘텐츠 제작을 시작하려면 다음 문서로 이동하세요.

- [컨텐츠 제작 개요](../02-content-creation/00-overview.md)

## 관련 문서

- [개요](00-overview.md)
- [컨텐츠 제작 개요](../02-content-creation/00-overview.md)
