**한국어** | [English](README.en.md)

# DropCatBlaster

세로 스크롤 방식의 펀치 액션 디펜스 게임입니다. Android 베타 버전으로 출시했던 프로젝트이며, 현재는 배포가 중단되어 있습니다.

## 프로젝트 배경

같은 연구실 구성원의 캡스톤 프로젝트로 시작했으며, 새로운 방식의 디펜스 게임을 만드는 것을 목표로 했습니다. 공모전 출품으로 이어지지는 않았지만 Unity 게임 개발의 기초를 쌓은 프로젝트입니다.

## 팀

TeamGooCat — 이영성, 박정배

## 개발 환경

| 항목 | 값 |
| --- | --- |
| Unity Editor | **2021.3.25f1** |
| Unity 프로젝트 폴더 | `DropCat/` |
| 기본 브랜치 | `main` |

Unity 버전은 [ProjectVersion.txt](DropCat/ProjectSettings/ProjectVersion.txt)에 기록된 값을 기준으로 합니다.

- Android 기기: 모바일 조작 및 빌드 확인용
- Git: Spine 및 BetterStreamingAssets의 Git 패키지 의존성 복원용
- Android 빌드 시 해당 Unity 버전의 Android Build Support 모듈

## 시작하기

```bash
git clone --branch main https://github.com/ReYoungSung/DropCatBlaster.git
```

1. Unity Hub에 위 버전의 Editor를 설치합니다.
2. 저장소 안의 `DropCat` 폴더를 프로젝트로 추가합니다. 저장소 최상위 폴더와 Unity 프로젝트 폴더는 서로 다릅니다.
3. 최초 임포트와 Package Manager의 패키지 복원을 완료합니다.
4. `Assets/01_Scenes/2023-New version 0.6.0/InitLogo.unity`을 열고 Play를 실행합니다.

빌드 설정에는 로고 → 타이틀 메뉴 → 튜토리얼 → 스테이지 씬이 등록되어 있습니다. 초기화가 필요한 게임이므로 개별 스테이지 대신 로고 씬부터 실행합니다.

빌드 구성은 [EditorBuildSettings.asset](DropCat/ProjectSettings/EditorBuildSettings.asset), 패키지 목록은 [manifest.json](DropCat/Packages/manifest.json)에서 확인할 수 있습니다. 아래 `Assets/` 경로는 Unity 프로젝트 폴더 기준입니다.

## 주요 폴더

| 경로 | 내용 |
| --- | --- |
| [Assets/01_Scenes/](DropCat/Assets/01_Scenes/) | 로고·메뉴·튜토리얼·스테이지 |
| [Assets/02_Scripts/](DropCat/Assets/02_Scripts/) | 입력·전투·레벨·저장·UI 로직 |

## 패키지

`Packages/manifest.json`에 Spine 4.1, BetterStreamingAssets, Input System, URP 등의 의존성이 정의되어 있습니다. 패키지 복원이 실패하면 Unity Console과 Package Manager에서 Git 실행 환경 및 다운로드 오류부터 확인합니다.

## 사용법

### 플레이 순서

1. 로고와 타이틀 화면을 지나 게임 시작 버튼을 선택합니다. 저장된 클리어 기록에 따라 새 게임 또는 이어하기 형태로 표시됩니다.
2. 첫 플레이는 튜토리얼부터 진행하고 화면의 이동·점프·공격 안내를 확인합니다.
3. 화면 조이스틱을 좌우로 움직여 위치를 조절합니다. 공격할 적이 있는 방향으로 조이스틱을 스와이프하면 해당 방향의 적을 감지해 펀치 공격을 시도합니다.
4. 점프 조작으로 발판과 적 사이를 이동합니다. 점프에는 횟수 제한이 있으며 지면에 착지하면 횟수를 회복합니다.
5. 스테이지 완료 또는 게임 오버 화면에서 결과를 확인합니다. 플레이 도중에는 일시정지 메뉴의 재개·재시작·메인 메뉴 복귀 기능을 이용합니다.

| 조작 | 동작 |
| --- | --- |
| 가상 조이스틱 좌우 이동 | 캐릭터의 수평 이동 |
| 조이스틱을 적 방향으로 스와이프 | 감지된 적을 향해 펀치; 기절 중에는 실행되지 않음 |
| 점프 입력 | 남은 점프 횟수 내에서 점프 |
| 일시정지 입력 | 게임을 멈추고 메뉴 표시 |

점프와 일시정지의 기기별 버튼은 프로젝트 Input Actions의 `PlayerJump`, `PauseGame` 바인딩을 따릅니다. 키보드의 특정 키를 모든 환경에서 동일하게 사용하는 것으로 가정하지 않습니다. 공격이 나가지 않으면 적이 감지 범위에 있는지, 캐릭터가 기절 상태인지 확인합니다.

동작 기준: [PlayerManager.cs](DropCat/Assets/02_Scripts/%5BCharacters%5D/%5BPlayer%5D/Manager/PlayerManager.cs) · [PauseManager.cs](DropCat/Assets/02_Scripts/UIs/PauseManager.cs)
