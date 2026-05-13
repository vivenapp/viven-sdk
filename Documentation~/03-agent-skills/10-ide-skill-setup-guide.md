# IDE별 Agent Skill 폴더 구성 가이드

## 개요

Viven 프로젝트의 Agent Skill은 `.cursor/skills/`에 원본이 관리됩니다. Cursor 외의 AI 코딩 도구에서도 동일한 Skill을 사용하려면, 심볼릭 링크(Symbolic Link)를 통해 각 IDE의 Skill 디렉터리를 원본에 연결합니다.

## 왜 심볼릭 링크인가?

- **단일 소스**: Skill 파일을 한 곳(`.cursor/skills/`)에서만 관리
- **자동 동기화**: 원본 수정 시 모든 IDE에 즉시 반영
- **중복 방지**: 동일 파일을 여러 폴더에 복사하지 않아 불일치 위험 제거

## IDE별 Skill/Rules 경로 총정리

### Skills 디렉터리 호환 IDE (심볼릭 링크 대상)

YAML frontmatter + Markdown 형식의 SKILL.md를 직접 읽을 수 있는 IDE입니다.

| IDE | 프로젝트 Skill 경로 | 전역 Skill 경로 | 호환성 |
|-----|---------------------|-----------------|--------|
| Cursor (원본) | `.cursor/skills/` | `~/.cursor/skills/` | 원본 |
| Claude Code | `.claude/skills/` | `~/.claude/skills/` | `.cursor/skills/`도 자동 인식 |
| Codex (OpenAI) | `.agents/skills/` | `~/.codex/skills/` | SKILL.md 형식 동일 |
| Antigravity (Google) | `.agent/skills/` | `~/.gemini/antigravity/skills/` | description 기반 시맨틱 매칭 |
| Windsurf (Codeium) | `.windsurf/rules/` | IDE 설정 내 global_rules.md | YAML frontmatter 구조 유사 |
| Continue | `.continue/rules/` | `~/.continue/rules/` | alwaysApply/globs 구조 동일 |
| Cline | `.clinerules/` | `Documents/Cline/Rules/` | 선택적 frontmatter 지원 |
| Augment Code | `.augment/rules/` | `~/.augment/rules/` | 타 도구 rules 자동 임포트 지원 |

### Rules 파일만 지원 IDE (별도 구성)

SKILL.md 형식을 직접 읽지 못하고, 자체 Rules/Guidelines 형식만 지원합니다.
이들 IDE는 `AGENTS.md` 또는 자체 rules 디렉터리를 사용합니다.

| IDE | 프로젝트 설정 경로 | 특징 |
|-----|---------------------|------|
| GitHub Copilot | `.github/copilot-instructions.md` | `applyTo` glob, `AGENTS.md` 지원 |
| Amazon Q Developer | `.amazonq/rules/*.md` | 순수 Markdown, 자동 스캔 |
| JetBrains Junie | `.junie/guidelines.md` 또는 `.junie/guidelines/` | `AGENTS.md` 우선 탐색 |
| Roo Code | `.roo/rules/*.md` | 모드별 분리 (`.roo/rules-code/`), `AGENTS.md` 지원 |
| Trae (ByteDance) | `.trae/rules/project_rules.md` | 순수 Markdown, `#rulename` 호출 |
| Aider | `CONVENTIONS.md` | `.aider.conf.yml`에서 read 설정 |

> [!TIP]
> **AGENTS.md**는 OpenAI Codex가 주도하고 GitHub Copilot, Junie, Roo Code 등이 채택한 범용 표준입니다. Viven 프로젝트에서도 `AGENTS.md`를 생성하면 SKILL.md 비호환 IDE에서도 기본 지침을 자동으로 로드할 수 있습니다.

## 자동 설정 (권장)

프로젝트 루트에서 설정 스크립트를 실행하면 Interactive CLI로 심볼릭 링크를 구성합니다.

### Windows (PowerShell)

```powershell
# 관리자 권한 PowerShell에서 실행
.\scripts\setup-ide-skills.ps1
```

### macOS / Linux (Bash)

```bash
chmod +x scripts/setup-ide-skills.sh
./scripts/setup-ide-skills.sh
```

> [!CAUTION]
> **Windows에서 심볼릭 링크를 만들려면 관리자 권한이 필요합니다.** PowerShell을 "관리자 권한으로 실행"한 후 스크립트를 실행하세요. 또는 Windows 10 이상에서 개발자 모드를 활성화하면 일반 권한으로도 생성 가능합니다.

## 수동 설정

자동 스크립트를 사용할 수 없는 경우, 아래 명령어로 직접 심볼릭 링크를 생성합니다.

### Windows (PowerShell, 관리자)

```powershell
# 프로젝트 루트에서 실행 — Skills 호환 IDE
New-Item -ItemType SymbolicLink -Path ".claude\skills" -Target ".cursor\skills"
New-Item -ItemType SymbolicLink -Path ".agents\skills" -Target ".cursor\skills"
New-Item -ItemType SymbolicLink -Path ".agent\skills" -Target ".cursor\skills"

# Rules 호환 IDE (디렉토리 구조가 다르므로 skills 내용을 rules로 링크)
New-Item -ItemType SymbolicLink -Path ".windsurf\rules" -Target ".cursor\skills"
New-Item -ItemType SymbolicLink -Path ".continue\rules" -Target ".cursor\skills"
New-Item -ItemType SymbolicLink -Path ".clinerules" -Target ".cursor\skills"
New-Item -ItemType SymbolicLink -Path ".augment\rules" -Target ".cursor\skills"
```

### macOS / Linux

```bash
# 프로젝트 루트에서 실행 — Skills 호환 IDE
ln -s .cursor/skills .claude/skills
ln -s .cursor/skills .agents/skills
ln -s .cursor/skills .agent/skills

# Rules 호환 IDE
ln -s .cursor/skills .windsurf/rules
ln -s .cursor/skills .continue/rules
ln -s .cursor/skills .clinerules
ln -s .cursor/skills .augment/rules
```

## 설정 확인

심볼릭 링크가 올바르게 설정되었는지 확인합니다.

### Windows

```powershell
Get-ChildItem -Path "." -Filter "*skills*" -Recurse -Depth 2 |
  Where-Object { $_.Attributes -band [System.IO.FileAttributes]::ReparsePoint } |
  Select-Object FullName, Target
```

### macOS / Linux

```bash
find . -maxdepth 3 -type l -name "skills" -o -name "rules" -o -name ".clinerules" 2>/dev/null | xargs ls -la
```

## IDE별 호환성 상세

### YAML Frontmatter 호환 매트릭스

| 필드 | Cursor | Windsurf | Continue | Cline | Augment | Copilot |
|------|:------:|:--------:|:--------:|:-----:|:-------:|:-------:|
| `name` | ✅ | ✅ | ✅ | ✅ | ✅ | ❌ |
| `description` | ✅ | ✅ | ✅ | ✅ | ✅ | ❌ |
| `globs` | ✅ | ✅ | ✅ | ✅ | ❌ | `applyTo` |
| `alwaysApply` | ✅ | ✅ | ✅ | ❌ | `always_apply` | ❌ |

### Skill 자동 로드 방식 비교

| IDE | 자동 매칭 | `/skill-name` 호출 | `@` 컨텍스트 첨부 |
|-----|:---------:|:------------------:|:-----------------:|
| Cursor | ✅ description 매칭 | ✅ | ✅ |
| Claude Code | ✅ description 매칭 | ✅ | ❌ |
| Codex | ✅ implicit matching | ✅ `/skills` | ❌ |
| Antigravity | ✅ 시맨틱 매칭 | ✅ | ❌ |
| Windsurf | ✅ Agent 판단 | ❌ | ❌ |
| Continue | ✅ Agent 판단 | ❌ | ❌ |
| Cline | ✅ globs/항상 활성 | ❌ | ❌ |
| Augment | ✅ agent_requested | ✅ `@` 멘션 | ✅ |

## 주의사항

- `.gitignore`에 심볼릭 링크 대상 디렉터리가 이미 무시 처리되어 있는지 확인하세요.
- Claude Code는 `.cursor/skills/`도 자동으로 인식하므로, 심볼릭 링크 없이도 Skill이 로드될 수 있습니다.
- Augment Code는 타 도구의 rules 폴더를 자동 임포트하는 기능이 있어, 심볼릭 링크 없이도 동작할 수 있습니다.
- Windsurf는 워크스페이스 룰 파일당 **12,000자** 제한이 있습니다. 큰 SKILL.md는 분할이 필요할 수 있습니다.
- 이미 대상 경로에 파일이 있다면, 심볼릭 링크 생성 전에 백업 후 삭제하세요.
- SKILL.md 비호환 IDE(Copilot, Amazon Q, Junie, Roo Code, Trae, Aider)에는 `AGENTS.md`를 프로젝트 루트에 생성하여 기본 지침을 제공하는 것을 권장합니다.

## 관련 문서

- [Agent Skill 사용 가이드](08-agent-skill-user-guide.md)
- [Agent Skill 개발 가이드](09-agent-skill-dev-guide.md)
- [Skill 분류](02-skill-taxonomy.md)
