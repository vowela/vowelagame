using System;
using System.Collections.Generic;
using VowelAServer.Shared.Data;
using VowelAServer.Shared.Interfaces;

namespace VowelAServer.Gameplay.Controllers
{
    public class WorldSimulation : ITickable
    {
        public static WorldSimulation Instance;

        public List<ContainerData> Scene;

        public WorldSimulation()
        {
            Instance = this;
            InitScene();
        }

        public void Tick()
        {

        }

        private void InitScene()
        {
            Scene = CreateTestScene();
        }

        private List<ContainerData> CreateTestScene()
        {
            var child0 = Guid.NewGuid().ToString();
            var child1 = Guid.NewGuid().ToString();
            var child2 = Guid.NewGuid().ToString();
            var child3 = Guid.NewGuid().ToString();
            return new List<ContainerData>()
            {
                new ContainerData
                {
                    ContainerName = "Test Name 1",
                    LuaCode = @"--This is test lua code
                    function Update() return 'Ya siel deda' end",
                    Id = child0
                },
                new ContainerData() {
                    ContainerName = "Test Name 2",
                    LuaCode = "--This is test lua code",
                    Id = child1
                },
                new ContainerData() {
                    ContainerName = "Test Name 3",
                    LuaCode = "--This is test lua code",
                    Id = child2,
                    ParentId = child0
                },
                new ContainerData() {
                    ContainerName = "Test Name 4",
                    LuaCode = "--This is test lua code",
                    Id = child3,
                    ParentId = child2
                }
            };
        }
    }
}
