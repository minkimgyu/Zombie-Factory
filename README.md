# Zombie Factory

![image](https://github.com/user-attachments/assets/e5485387-4ee3-4d36-a0c1-b91566f2c27f)
참고 영상: https://www.youtube.com/watch?v=3kd6LchrqfI

## 프로젝트 소개
Unity를 사용하여 개발한 FPS 게임

## 개발 기간
23. 09 ~ 24. 05

## 인원
1인 개발

## 개발 환경
Unity (C#)

## 기능 설명

### A* 알고리즘을 활용한 길 찾기 시스템 개발 및 최적화

#### 문제점

<div align="center">
   <img src="https://github.com/minkimgyu/GraduationProject/assets/48249824/e324928c-16b9-46f8-9805-d6c5afac6281" width="85%" height="85%"/>
   <img src="https://github.com/minkimgyu/GraduationProject/assets/48249824/c6eee27a-78bf-47c5-91d8-e9770fe2ddc9" width="85%" height="85%"/>
</div>

<div>
   유닛을 맵의 끝에서 끝으로 가는 경로를 길찾기 알고리즘을 적용하여 찾았을 때 약 1초 가까이 걸리는 것을 확인할 수 있습니다. 이는 너무 느리기 때문에 실제 게임에 적용하기 힘듭니다. 
</div>

#### 해결 과정

<div>
   A* 알고리즘은 가중치가 가장 작은 노드를 선택해 길찾기를 하는 특성이 있습니다. 기존에는 List를 통해 순회하면서 가중치가 가장 작은 값을 찾았기 때문에 시간 복잡도가 O(n)이었습니다.
   </br>
   </br>
   그러나 Min Heap을 사용한다면 삽입의 경우 시간복잡도가 O(logn)이지만 알고리즘의 특성 상 가장 가중치가 작은 노드를 언제나 트리의 최상단에 위치되므로 가중치가 가장 작은 노드에 시간 복잡도가 O(1)안에 접근할 수 있습니다.
   </br>
   </br>
   <div align="center">
   이러한 특성을 사용하여 최적화를 진행했습니다.
   </div>
</div>

</br>
<div align="center">
   Min Heap 적용
   </br>
  <img src="https://github.com/minkimgyu/GraduationProject/assets/48249824/70af2e2b-e7c6-4f7b-a04f-9afe66244cfb" width="85%" height="85%"/>
</div>

</br>
<div align="center">
   거의 1초에 가깝던 수행 시간이 1/10 가까이 감소한 것을 확인할 수 있습니다.
</div>


### Object Pool를 사용한 Effect 생성 시스템 개발 및 최적화
#### 문제점

<div align="center">
   평상시
   </br>
   <img src="https://github.com/minkimgyu/GraduationProject/assets/48249824/beb36af6-d609-49ba-b960-8dd3ae12a001" width="55%" height="55%"/>

   사격 시
   </br>
  <img src="https://github.com/minkimgyu/GraduationProject/assets/48249824/58d2bba1-a9ea-41a9-ab22-cf30c07f69d6" width="55%" height="55%"/>
  <img src="https://github.com/minkimgyu/GraduationProject/assets/48249824/4d1294fb-baac-4da4-8cfd-ed6043fb720a" width="55%" height="55%"/>
</div>

<div>
   총기를 발사할 때 Effect와 Noise가 계속 생성되고 파괴되기 때문에 GC Spike가 많이 일어나며 프레임이 249에서 145까지 떨어지는 것을 확인할 수 있습니다.
</div>

#### 해결 과정

</br>

<div align="center">
   Object Pool을 이용하여 Effect와 Noise를 관리해줍나다.
</div>
</br>
<div align="center">
   Object Pool 적용
   </br>
  <img src="https://github.com/minkimgyu/GraduationProject/assets/48249824/cfbf2c71-292b-4e66-a131-ee587399fdca" width="55%" height="55%"/>
  <img src="https://github.com/minkimgyu/GraduationProject/assets/48249824/a4ad69ad-eeb0-4497-af08-da65a363aef9" width="55%" height="55%"/>
</div>

</br>
<div align="center">
   GC Spike가 눈에 띄게 줄어든 것을 볼 수 있으며 프레임도 220정도를 유지하는 것을 확인할 수 있습니다.
</div>

### UI Toolkit를 사용하여 반동 커스텀 에디터 개발

<div align="center">
   <img src="https://github.com/minkimgyu/GraduationProject/assets/48249824/5f8058ae-5c9e-4d3f-9e66-1ec668fc17be" width="100%" height="100%"/>

   <img src="https://github.com/minkimgyu/GraduationProject/assets/48249824/55fd198c-b728-4e39-9c37-4b859d71a024" width="100%" height="100%"/>
</div>
</br>

<div align="center">
   총기 밸런싱 작업을 효율적으로 하기 위해 UI ToolKit Package를 사용하여 반동 에디터를 개발해봤습니다.
</div>

### Rig Builder Package를 사용하여 IK 장전 애니메이션 적용

<div align="center">
   <img src="https://github.com/minkimgyu/GraduationProject/assets/48249824/c0eb37c8-92bd-4fa6-94df-e712123e9554" width="50%" height="50%"/>
   </br>
   적용 전
   </br>
   </br>
   <img src="https://github.com/minkimgyu/GraduationProject/assets/48249824/c8d42e5e-06e9-440b-8514-9e3f929dba57" width="50%" height="50%"/>
   </br>
   적용 후
</div>
</br>

<div align="center">
   Rig Builder Package를 활용해서 원본 애니메이션에 Inverse Kinematics를 적용해 수정하는 작업을 해봤습니다.
</div>

## 회고
졸업 프로젝트를 진행하면서 많은 부분을 새롭게 배우고 적용해봤습니다. 지금까지 했던 프로젝트 중에서 가장 많은 것을 얻어갈 수 있던 시간이었습니다.
