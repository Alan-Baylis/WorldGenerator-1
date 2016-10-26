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
            var rnd = new Random();
            int x, y, z;
            Character character;

            x = centre.X + (rnd.Next() * 32 - 10);
            z = centre.Z + (rnd.Next() * 32 - 10);
            y = World.GetHeightMapLevel(x, z);
            character = new Character() { Id = 1, Name = "Chr1", Location = new Position(x, y, z) };
            _characters.Add(character.Id, character);
            MessageProcessor.SendCharacterUpdate(character);

            x = centre.X + (rnd.Next() * 32 - 10);
            z = centre.Z + (rnd.Next() * 32 - 10);
            y = World.GetHeightMapLevel(x, z);
            character = new Character() { Id = 2, Name = "Chr2", Location = new Position(x, y, z) };
            _characters.Add(character.Id, character);
            MessageProcessor.SendCharacterUpdate(character);

            x = centre.X + (rnd.Next() * 32 - 10);
            z = centre.Z + (rnd.Next() * 32 - 10);
            y = World.GetHeightMapLevel(x, z);
            character = new Character() { Id = 3, Name = "Chr3", Location = new Position(x, y, z) };
            _characters.Add(character.Id, character);
            MessageProcessor.SendCharacterUpdate(character);

            x = centre.X + (rnd.Next() * 32 - 10);
            z = centre.Z + (rnd.Next() * 32 - 10);
            y = World.GetHeightMapLevel(x, z);
            character = new Character() { Id = 4, Name = "Chr4", Location = new Position(x, y, z) };
            _characters.Add(character.Id, character);
            MessageProcessor.SendCharacterUpdate(character);
        }

        private static Dictionary<int, Character> _characters;
    }
}

