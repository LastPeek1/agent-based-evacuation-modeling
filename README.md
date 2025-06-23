# Crowd Evacuation Simulation in Unity using ML-Agents and Reinforcement Learning

[![Unity Version](https://img.shields.io/badge/Unity-2023.2-blue.svg)](https://unity.com/)
[![Python Version](https://img.shields.io/badge/Python-3.10-green.svg)](https://python.org)
[![ML-Agents](https://img.shields.io/badge/ML--Agents-2.0+-lightgrey.svg)](https://unity-technologies.github.io/ml-agents/) 

**This project implements an agent-based model for human evacuation from buildings using Reinforcement Learning (RL).** Agents make decisions solely based on local observations through RayPerceptionSensor3D components, simulating limited human vision in a dynamically changing environment.

**Key Feature:** Agents have no prior knowledge of the building layout and learn to find exits through a reward system, adapting to random obstacles and environmental changes.

![Simulation Example](https://github.com/LastPeek1/agent-based-evacuation-simulation/blob/bd715a5c2b317690ba1cea88290c318e56fcacc0/40%20-%20frame.jpg)

## 🧠 Core Concepts

*   **Local Perception (Raycast Vision):** Agents use 2 layers of RayPerceptionSensor3D (upper and lower) to scan space in a 180° arc (90° left/right) parallel to the floor. Rays detect:
    *   Walls
    *   Other agents
    *   Exits
    *   Closed doors
    *   Dangerous zones (not implemented)
*   **Reward System:**
    *   ✅ **Positive Reward:** Calculated by formula: `reward = baseReward * (1 - (elapsedTime / maxEpisodeTime))`.
    *   ❌ **Penalties:**
        *   For collisions: with walls, other agents, closed doors
        *   For exceeding evacuation time
*   **Dynamic Stochastic Environment:**
    *   Random agent placement at episode start
    *   Changing obstacles (door opening/closing)
    *   **Low Barriers:** Key feature! Impede movement but **do not block vision** (rays pass over them)
*   **No Global Information:** Agents don't know the building map. Decisions are made ONLY based on Raycast sensor data (observation vector).

![Raycast](https://github.com/LastPeek1/agent-based-evacuation-simulation/blob/8bfd65b9a04f0eff96a798d0d586b2e5434f94f5/image.png)

## ⚙️ Technology Stack and Implementation

| Component             | Purpose                                                                 |
|-----------------------|-------------------------------------------------------------------------|
| **Unity 2023.2**      | 3D environment, physics engine, rendering                              |
| **ML-Agents (v2.0+)** | Reinforcement learning integration                                      |
| **C#**                | Agent logic, environment interaction, reward system implementation     |
| **Python 3.10**       | Training configuration, training execution, metrics analysis           |
| **TensorBoard**       | Training metrics visualization (graphs, statistics)                    |

### 🧩 Code Architecture
*   **`Agent_Logic.cs`:** Inherits from `Agent`. Responsible for:
    *   Processing AI actions (`MoveAgent()`: forward/backward, rotations, door interaction)
    *   Collecting observations (via child `RayPerceptionSensor3D` components)
    *   Handling collisions (`OnCollisionEnter`, `OnCollisionStay` - penalties for walls/agents/doors, reward for reaching exit)
    *   Heuristic control (`Heuristic()`) for manual testing
*   **`LevelManager.cs`:** Environment "brain". Controls:
    *   Agent registration (`AgentInfo[]`)
    *   Episode reset: random agent placement, door reset, timers
    *   Agent group management (`SimpleMultiAgentGroup`)
    *   Reward calculation (`CalculateReward()`)
    *   Episode termination (all agents succeeded or `maxEnvironmentSteps` timeout)
*   **`AgentSettings.cs`:** Parameter storage (ScriptableObject). Contains speed, force, penalty, reward, ray length, episode time settings. Enables easy configuration and data protection.
*   **`Door.cs`:** Manages door state (open/closed), handles agent interaction for reward system.

### Windows Version
1. Download the latest build:  
   [📦 EvacuationSimulation_Windows_Build.zip](https://github.com/LastPeek1/agent-based-evacuation-simulation/releases/tag/EvacuationSimulation_Windows_Build)
2. Extract the archive
3. Run `MLAgent2.exe`
