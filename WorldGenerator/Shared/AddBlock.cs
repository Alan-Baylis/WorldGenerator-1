using System;
using Sean.Shared;

namespace Sean.WorldGenerator
{
    /*
	public class AddBlock : GameAction
	{
		public AddBlock()
		{
			DataLength = Position.SIZE + sizeof(ushort); //coords + block type
		}

		public AddBlock(Position position, Block.BlockType blockType) : this()
		{
			if (blockType == Block.BlockType.Air) throw new Exception("You can't place air, use RemoveBlock");
			Position = position;
			BlockType = blockType;
		}

		public override string ToString()
		{
			return String.Format("AddBlock {0} {1}", BlockType, Position);
		}

		public override ActionType ActionType { get { return ActionType.AddBlock; } }
		public Position Position;
		public Block.BlockType BlockType;

		protected override void Queue()
		{
			base.Queue();
			Write(Position);
			Write((ushort)BlockType);
		}

		public override void Receive()
		{
			lock (TcpClient)
			{
				base.Receive();
				var bytes = ReadStream(DataLength);
				Position = new Position(bytes, 0);
				BlockType = (Block.BlockType)BitConverter.ToUInt16(bytes, Position.SIZE);
			}

			World.PlaceBlock(Position, BlockType);
			
			//bm: this has to wait until the server can manage who's in creative mode
			//if (ConnectedPlayer.Inventory[(int)BlockType] <= 0) return;
			//ConnectedPlayer.Inventory[(int)BlockType] -= 1;

            // TODO - send to all players
			//foreach (var player in Server.Controller.Players.Values)
			//{
			//	new AddBlock(ref Position, BlockType) { ConnectedPlayer = player }.Send();
			//}
		}
	}
*/
}