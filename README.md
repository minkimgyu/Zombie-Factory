# 🧟 Zombie Factory

Unity를 사용하여 개발한 FPS 게임입니다. 💥

맵을 돌아다니며 좀비를 사냥하고 최종 목적지까지 이동하는 것이 목표입니다. 🗺️

<img src="https://github.com/user-attachments/assets/7b9e3912-ab78-40cb-9a88-ec509db603bf" alt="Zombie Factory Screenshot" width="85%" height="85%" />

## 📆 개발 기간
2024년 9월 ~ 2024년 12월

## 🧑‍🤝‍🧑 팀 구성
- 총 1명 (1인 개발) 🧑‍💻

## 🛠️ 개발 도구
- Unity (C#) 🎮

## 👨‍💻 담당 역할 및 기여도 (기여도 100%)

- ✅ **Finite State Machine을 활용한 Player 기능 구현** 🏃
- ✅ **Finite State Machine, Behavior Tree를 활용한 AI 구현** 🧠
- ✅ **Strategy Pattern을 활용한 Weapon 시스템 구현** 🔫
- ✅ **UI Toolkit을 사용하여 반동 커스텀 에디터 개발** 🎨
- ✅ **Multithreading을 활용한 길찾기 노드 계산 최적화** ⚡
- ✅ **3차원 Grid 기반 길찾기 알고리즘 (A\*) 개발 및 최적화** 🗺️
- ✅ **Factory Pattern을 사용한 생성 시스템 개발** 🏭
- ✅ **Object Pool을 사용하여 생성 시스템 최적화** ♻️

---

## 🏃 Finite State Machine (FSM)을 활용한 Player 구현

플레이어의 기능을 구현하기 위해 `ActionController`와 `WeaponController`를 구현했습니다. 🎮
기능의 복잡성을 줄이기 위해 각각의 기능을 독립시켜 **Concurrent State Machine**을 적용했습니다. 🔄
향후 확장을 위해 **Hierarchical Finite State Machine** 방식을 통해 `Movement FSM`을 확장했습니다.

### Player FSM 다이어그램 📊

<img src="https://github.com/user-attachments/assets/ba0d7523-bcaa-42de-b12b-07372b229cbc" alt="Zombie Factory Screenshot" width="65%" height="65%" />

*좌: ActionController FSM, 우: WeaponController FSM*

---

## 🧠 Finite State Machine, Behavior Tree를 활용한 AI 구현

AI 구현 시 FSM은 상태 수가 많아질수록 유지보수성이 저하되는 문제가 있습니다. 이를 보완하고자 핵심 행동 로직은 **Behavior Tree**로 구현했습니다. 🌳

* **FSM의 역할:** AI의 큰 틀의 상태 관리를 담당합니다.
* **Behavior Tree의 역할:** 각 상태 내에서 구체적인 행동 로직을 처리합니다.
* 이러한 이분화를 통해 기반 기술을 재사용하고 조합할 수 있는 확장성을 확보했습니다. ✅

### AI FSM & Behavior Tree 다이어그램 🤖

<img src="https://github.com/user-attachments/assets/3dca05ea-feac-480e-b067-e931d72e57e6" alt="Zombie Factory Screenshot" width="85%" height="85%" />

*좌: Swat Movement FSM & Battle FSM, 우: Zombie FSM*

---

## 🔫 Strategy Pattern을 활용한 Weapon 시스템 구현

발사 방식, 반동 처리 등 다양한 총기 작동 기능을 각각의 전략 클래스로 모듈화하여 유연한 기능 교체와 손쉬운 확장이 가능한 구조를 구현했습니다. 🛠️


### Weapon 시스템 구조 📜

<img src="https://github.com/user-attachments/assets/943bd546-34db-4e0e-9a5d-76d03ee028e3" alt="Zombie Factory Screenshot" width="85%" height="85%" />

---

## 🎨 UI Toolkit을 사용하여 반동 커스텀 에디터 개발
총기 반동 데이터의 효율적인 입력 작업을 위해 UI Toolkit Package를 사용하여 반동 에디터를 개발했습니다. 📈
이를 통해 작업의 효율성을 향상시켰습니다. 💡

### 반동 스프레이 에디터 🖥️

<img src="https://github.com/user-attachments/assets/4dc510e4-5a47-44c7-9dcd-4e63ec85d3f9" alt="Zombie Factory Screenshot" width="85%" height="85%" />

*좌: Spray Editor UI, 우: 게임 내 반동 시각화*

---

## ⚡ Multithreading을 활용한 길찾기 노드 계산 최적화

<img src="https://github.com/user-attachments/assets/e74b5644-d3be-4f95-b891-897bd38b7f48" alt="Zombie Factory Screenshot" width="85%" height="85%" />

3차원 Grid 기반 A* 알고리즘 적용을 위해 Nodes를 계산하는 과정에서 기존 Singlethreading 순차 처리 방식으로는 약 8.52초의 병목 ⏳이 발생했습니다.

<img src="https://github.com/user-attachments/assets/1d58e88b-4718-4a9a-b3a3-f70717bf272f" alt="Zombie Factory Screenshot" width="85%" height="85%" />

* 해결책: Multithreading 기법을 도입하여 해당 계산 작업을 병렬로 수행하도록 최적화했습니다.
* 결과: 수행 시간을 3.04초로 🚀 단축하여 게임 성능을 크게 개선했습니다.

### Multithreading 도입 전후 성능 비교 📈

<img src="https://github.com/user-attachments/assets/ccad0375-a887-4823-8484-9fcbef5a3d46" alt="Zombie Factory Screenshot" width="85%" height="85%" />

*좌: 게임 화면 및 디버그 로그 (8.52초), 우: Multithreading 방식 (3.04초) 다이어그램*

---

## 🗺️ 3차원 Grid 기반 길찾기 알고리즘 개발 및 최적화
A* 기반 길찾기 알고리즘의 성능을 향상시키기 위한 최적화 작업을 수행했습니다. ✨

<img src="https://github.com/user-attachments/assets/d543692f-41c0-483a-a1b2-9ea09bbfff71" alt="Zombie Factory Screenshot" width="85%" height="85%" />

### Heap 자료구조 적용 📦
* A* 탐색 과정에서 Openlist 내 비효율 노드를 줄여 연산 시간 복잡도를 줄이기 위해 Openlist를 Heap 자료 구조로 변경했습니다.
* 이를 통해 해당 복잡도를 기존 O(N^2)에서 O(N log N)으로 개선했습니다.
* 프로파일링 결과: 1.52ms에서 0.87ms로 단축 (약 42% 향상)

<img src="https://github.com/user-attachments/assets/e190886e-f03d-4c1c-a9ef-0aa71f2f46f8" alt="Zombie Factory Screenshot" width="85%" height="85%" />

### Weighted A*를 활용한 최적화 ⚖️
* 장애물이 많은 맵에서 탐색 효율을 극대화하고자 휴리스틱(h) 값에 가중치(w)를 적용하는 Weighted A*를 도입했습니다.
* 평가 함수 f(n) = g(n) + h(n) * w를 활용하여 휴리스틱의 영향을 키워 목표 지점에 더 큰 값으로 향하도록 탐색할 수 있도록 하여 복잡한 지형에서의 길찾기 수행 시간을 최적화했습니다.
* 프로파일링 결과: 0.87ms에서 0.2ms로 단축 (약 77% 향상)

<img src="https://github.com/user-attachments/assets/bbb1e380-21ed-4fba-a10a-e7428a43fe23" alt="Zombie Factory Screenshot" width="85%" height="85%" />

---

## 🏭 Factory Pattern을 사용한 생성 시스템 개발
객체 생성 로직을 클라이언트에서 분리하여 관리 효율을 높이고자 Factory 패턴을 적용했습니다. 🏗️
이를 통해 새로운 객체 타입 추가 시 기존 코드 수정 없이 확장 가능하도록 구현했습니다.

### Factory 패턴 예시 코드 📜

<img src="https://github.com/user-attachments/assets/3a7fae87-efcd-4795-b377-cb4e6e5dd8dc" alt="Zombie Factory Screenshot" width="85%" height="85%" />

---

## ♻️ Object Pool을 사용하여 생성 시스템 최적화
Factory 패턴과 Object Pool을 결합하여 객체 생성 및 재활용 시스템을 구축했습니다. 🔄
이를 통해 잦은 이펙트 및 오브젝트의 빈번한 생성/소멸에 따른 Garbage Collection 부하를 줄였습니다. 🗑️

### Object Pool 예시 코드 📝

<img src="https://github.com/user-attachments/assets/30b9dfdd-02f4-411a-8c73-b79d5a8c06b6" alt="Zombie Factory Screenshot" width="85%" height="85%" />
