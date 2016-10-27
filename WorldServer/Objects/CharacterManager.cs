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
            int x, y, z;
            Character character;

            x = centre.X + Settings.Random.Next(-15, 15);
            z = centre.Z + Settings.Random.Next(-15, 15);
            y = World.GetHeightMapLevel(x, z);
            character = new Character() { Id = 1, Name = "Chr1", Location = new Position(x, y, z) };
            _characters.Add(character.Id, character);
            MessageProcessor.SendCharacterUpdate(character);

            x = centre.X + Settings.Random.Next(-15, 15);
            z = centre.Z + Settings.Random.Next(-15, 15);
            y = World.GetHeightMapLevel(x, z);
            character = new Character() { Id = 2, Name = "Chr2", Location = new Position(x, y, z) };
            _characters.Add(character.Id, character);
            MessageProcessor.SendCharacterUpdate(character);

            x = centre.X + Settings.Random.Next(-15, 15);
            z = centre.Z + Settings.Random.Next(-15, 15);
            y = World.GetHeightMapLevel(x, z);
            character = new Character() { Id = 3, Name = "Chr3", Location = new Position(x, y, z) };
            _characters.Add(character.Id, character);
            MessageProcessor.SendCharacterUpdate(character);

            x = centre.X + Settings.Random.Next(-15, 15);
            z = centre.Z + Settings.Random.Next(-15, 15);
            y = World.GetHeightMapLevel(x, z);
            character = new Character() { Id = 4, Name = "Chr4", Location = new Position(x, y, z) };
            _characters.Add(character.Id, character);
            MessageProcessor.SendCharacterUpdate(character);
        }

        public static void MoveCharacters()
        {
            foreach (var character in _characters.Values)
            {
                character.Location.X += Settings.Random.Next(-1, 1);
                character.Location.Z += Settings.Random.Next(-1, 1);
                character.Location.Y = World.GetHeightMapLevel(character.Location.X, character.Location.Z);
                MessageProcessor.SendCharacterUpdate(character);
            }
        }

        private static Dictionary<int, Character> _characters;
        private static Random rnd = new Random();
    }
}

