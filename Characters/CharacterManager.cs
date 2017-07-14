using Sean.PathFinding;
using Sean.Shared;
using System;
using System.Collections.Generic;

namespace Sean.Characters
{
    public static class CharacterManager
    {
        static CharacterManager()
        {
            _characters = new Dictionary<int, Character>();
        }

        static Random Random = new Random(DateTime.Now.Second);
        static Dictionary<int, Character> _characters;

        public delegate void CharacterUpdateEventHandler(CharacterUpdateEventArgs e);
        public static event CharacterUpdateEventHandler CharacterUpdateEvent;

        public class CharacterUpdateEventArgs
        {
            public Character Character { get; set; }
        }

        private static void FireCharacterUpdateEvent(Character character)
        {
            if (CharacterUpdateEvent != null)
                CharacterUpdateEvent.Invoke(new CharacterUpdateEventArgs() { Character = character });
        }



        public static void AddRandomCharacters(Position centre)
        {
            int x, y, z;
            Character character;

            try
            {
                x = centre.X + Random.Next(-15, 15);
                z = centre.Z + Random.Next(-15, 15);
                y = _world.GetHeightMapLevel(x, z);
                character = new Character() { Id = 1, Name = "Chr1", Location = new Position(x, y, z) };
                _characters.Add(character.Id, character);
                FireCharacterUpdateEvent(character);
                //MessageProcessor.SendCharacterUpdate(character);
            }
            catch (Exception) // TODO GetHeightMapLevel used in this method sometimes throws
            { }
            try
            {
                x = centre.X + Random.Next(-15, 15);
                z = centre.Z + Random.Next(-15, 15);
                y = _world.GetHeightMapLevel(x, z);
                character = new Character() { Id = 2, Name = "Chr2", Location = new Position(x, y, z) };
                _characters.Add(character.Id, character);
                FireCharacterUpdateEvent(character);
                //MessageProcessor.SendCharacterUpdate(character);
            }
            catch (Exception)
            { }
            try
            {
                x = centre.X + Random.Next(-15, 15);
                z = centre.Z + Random.Next(-15, 15);
                y = _world.GetHeightMapLevel(x, z);
                character = new Character() { Id = 3, Name = "Chr3", Location = new Position(x, y, z) };
                _characters.Add(character.Id, character);
                FireCharacterUpdateEvent(character);
                //MessageProcessor.SendCharacterUpdate(character);
            }
            catch (Exception)
            { }
            try
            {
                x = centre.X + Random.Next(-15, 15);
                z = centre.Z + Random.Next(-15, 15);
                y = _world.GetHeightMapLevel(x, z);
                character = new Character() { Id = 4, Name = "Chr4", Location = new Position(x, y, z) };
                _characters.Add(character.Id, character);
                FireCharacterUpdateEvent(character);
                //MessageProcessor.SendCharacterUpdate(character);
            }
            catch (Exception)
            { }
        }

    }
}

