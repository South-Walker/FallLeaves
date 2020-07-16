# 落叶模拟
刷知乎看到一篇从粒子模拟出发实现落叶物理效果的文章<sup>[1]</sup>，感觉很有意思，只是原文注重对原论文的讲解，没有看到实际的工程代码，很可惜，只好自己试着做一下。
## 基本思路
将落叶视为布料，基于PDB思想模拟其运动
## 粒子模拟
使用Flex框架，求解距离约束，等距弯曲约束来模拟布料<sup>[2,3]</sup>
![](/Imgs/ClothTest.gif)<br>使用Flex框架进行布料模拟<br>
## 落叶网格
落叶网格使用函数生成
## 效果示意图



## 引用
[1] [游戏物理落叶效果实现](https://zhuanlan.zhihu.com/p/143687250)<br>
[2] Macklin M , Mueller M , Chentanez N , et al. Unified particle physics for real-time applications[J]. Acm Transactions on Graphics, 2014, 33(4CD):1-12.<br>
[3] Bender J , Matthias Müller, Macklin M . Position-Based Simulation Methods in Computer Graphics[C]// Tutorial Proceedings of Eurographics. 2015.<br>
