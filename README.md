# Crowd Evacuation Simulation in Unity using ML-Agents and Reinforcement Learning

[![Unity Version](https://img.shields.io/badge/Unity-2023.2.0f1-blue.svg)](https://unity.com/)
[![Python Version](https://img.shields.io/badge/Python-3.10.12-green.svg)](https://python.org)
[![ML-Agents](https://img.shields.io/badge/ML--Agents-1.1.0-lightgrey.svg)](https://unity-technologies.github.io/ml-agents/) 

**An agent-based model for human evacuation from buildings using Reinforcement Learning (RL).** Agents make decisions based solely on local observations through RayPerceptionSensor3D components, simulating limited human vision in dynamic environments.

**Key Innovation:** Agents have no prior knowledge of building layouts and learn to find exits through an adaptive reward system, handling random obstacles and environmental changes in real-time.

![Simulation Preview](https://github.com/LastPeek1/agent-based-evacuation-modeling/blob/9f562f1eefdb7568e719a7fb405df590e355858e/40%20-%20frame%20%E2%80%94%20%D0%BA%D0%BE%D0%BF%D0%B8%D1%8F.jpg)

## 🧠 Core Concepts

### Local Perception System
Agents use dual-layer RayPerceptionSensor3D (upper/lower) scanning 180° arc parallel to floor:
- **Detects:** Walls, Agents, Exits, Doors
- **Key Feature:** Low barriers impede movement but **don't block vision** (rays pass over them)

### Adaptive Reward System
| Reward Type       | Condition                     |
|-------------------|-------------------------------|
| ✅ Exit Reward    | Reaching exit                 |
| ❌ Wall Collision | Colliding with wall           | 
| ❌ Agent Collision| Colliding with other agent    |
| ❌ Time Penalty   | Exceeding evacuation time     |

### Dynamic Environment
- Random agent placement at episode start
- Dynamic obstacles (door opening/closing)
- Realistic agent-agent interactions
- No global map knowledge - decisions based solely on sensor data

![Ray Perception Visualization](https://github.com/LastPeek1/agent-based-evacuation-modeling/blob/9f562f1eefdb7568e719a7fb405df590e355858e/image.png)

## ⚙️ Technology Stack

| Component             | Purpose                                                                 |
|-----------------------|-------------------------------------------------------------------------|
| **Unity 2023.2.0f1**  | 3D environment, physics, rendering                                     |
| **ML-Agents 1.1.0**   | Reinforcement learning framework                                        |
| **PyTorch 2.2.1**     | Neural network backend                                                  |
| **C#**                | Agent logic, environment interaction                                   |
| **Python 3.10.12**    | Training configuration & execution                                     |
| **TensorBoard**       | Training metrics visualization                                         |

## 🧩 Architecture

### Core Scripts
| Script               | Responsibilities                                                                 |
|----------------------|----------------------------------------------------------------------------------|
| **`Agent_Logic.cs`** | - Action processing (movement, rotations)<br>- Observation collection<br>- Collision handling<br>- Heuristic control |
| **`LevelManager.cs`**| - Agent registration<br>- Episode reset logic<br>- Group management<br>- Reward calculation |
| **`AgentSettings.cs`** | Centralized parameters (ScriptableObject):<br>- Movement speeds<br>- Penalty values<br>- Ray configurations |
| **`Door.cs`**        | Door state management and interaction handling                                  |

## 🚀 Getting Started

### Prerequisites
**Essential reading before starting:**
- [📚 ML-Agents Official Documentation](https://github.com/Unity-Technologies/ml-agents/tree/develop)
- [📚 Installation Guide](https://github.com/Unity-Technologies/ml-agents/blob/develop/docs/Installation.md)

**Verified Software Versions:**
| Component       | Version     | Notes                                  |
|-----------------|-------------|----------------------------------------|
| Unity           | 2023.2.0f1  | Requires exact version                 |
| ML-Agents       | 1.1.0       | Python package                         |
| Python          | 3.10.12     | Must be 3.10.1-3.10.12                 |
| PyTorch (CUDA)  | 2.2.1       | GPU acceleration                       |

### Installation Guide

Clone repository
```
git clone https://github.com/LastPeek1/agent-based-evacuation-modeling.git
cd agent-based-evacuation-modeling
