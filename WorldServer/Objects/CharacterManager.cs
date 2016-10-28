using Sean.PathFinding;
using Sean.Shared;
using Sean.WorldGenerator;
using System;
using System.Collections.Generic;

namespace Sean.WorldServer
{
    public static class CharacterManager
    {
        static CharacterManager ()
        {
            _characters = new Dictionary<int, Character>();
        }

        public static void AddRandomCharacters(Position centre)
        {
            int x, y, z, x1,y1,z1;
            Character character;

            x = centre.X + Settings.Random.Next(-15, 15);
            z = centre.Z + Settings.Random.Next(-15, 15);
            y = MainClass.WorldInstance.GetHeightMapLevel(x, z);
            x1 = centre.X + Settings.Random.Next(-15, 15);
            z1 = centre.Z + Settings.Random.Next(-15, 15);
            y1 = MainClass.WorldInstance.GetHeightMapLevel(x1, z1);
            character = new Character() { Id = 1, Name = "Chr1", Location = new Position(x, y, z), Destination = new Position(x1,y1,z1) };
            _characters.Add(character.Id, character);
            MessageProcessor.SendCharacterUpdate(character);

            x = centre.X + Settings.Random.Next(-15, 15);
            z = centre.Z + Settings.Random.Next(-15, 15);
            y = MainClass.WorldInstance.GetHeightMapLevel(x, z);
            x1 = centre.X + Settings.Random.Next(-15, 15);
            z1 = centre.Z + Settings.Random.Next(-15, 15);
            y1 = MainClass.WorldInstance.GetHeightMapLevel(x1, z1);
            character = new Character() { Id = 2, Name = "Chr2", Location = new Position(x, y, z), Destination = new Position(x1,y1,z1) };
            _characters.Add(character.Id, character);
            MessageProcessor.SendCharacterUpdate(character);

            x = centre.X + Settings.Random.Next(-15, 15);
            z = centre.Z + Settings.Random.Next(-15, 15);
            y = MainClass.WorldInstance.GetHeightMapLevel(x, z);
            x1 = centre.X + Settings.Random.Next(-15, 15);
            z1 = centre.Z + Settings.Random.Next(-15, 15);
            y1 = MainClass.WorldInstance.GetHeightMapLevel(x1, z1);
            character = new Character() { Id = 3, Name = "Chr3", Location = new Position(x, y, z), Destination = new Position(x1,y1,z1) };
            _characters.Add(character.Id, character);
            MessageProcessor.SendCharacterUpdate(character);

            x = centre.X + Settings.Random.Next(-15, 15);
            z = centre.Z + Settings.Random.Next(-15, 15);
            y = MainClass.WorldInstance.GetHeightMapLevel(x, z);
            x1 = centre.X + Settings.Random.Next(-15, 15);
            z1 = centre.Z + Settings.Random.Next(-15, 15);
            y1 = MainClass.WorldInstance.GetHeightMapLevel(x1, z1);
            character = new Character() { Id = 4, Name = "Chr4", Location = new Position(x, y, z), Destination = new Position(x1,y1,z1) };
            _characters.Add(character.Id, character);
            MessageProcessor.SendCharacterUpdate(character);
        }

        public static void UpdateJobs()
        {
            foreach (var character in _characters.Values) {
                if (character.Location != character.Destination && character.WalkPath.Count == 0)
                {
                    var pathFinder = new PathFinder (MainClass.WorldInstance);
                    character.WalkPath = pathFinder.FindPath (character.Location, character.Destination);
                }
            }
        }
        public static void MoveCharacters()
        {
            foreach (var character in _characters.Values)
            {
                if (character.WalkPath.Count != 0) {
                    Position newPosition = character.WalkPath.Dequeue();
                    character.Location.X = newPosition.X;
                    character.Location.Z = newPosition.Z;
                    character.Location.Y = newPosition.Y;
                    MessageProcessor.SendCharacterUpdate (character);
                }
            }
        }

        private static Dictionary<int, Character> _characters;
        private static Random rnd = new Random();
    }
}

