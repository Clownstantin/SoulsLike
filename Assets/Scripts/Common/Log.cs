using UnityEngine;

namespace SoulsLike
{
	public sealed class Log
	{
		public static void Send(string message, MessageType type = MessageType.Default)
		{
			switch(type)
			{
				case MessageType.Default: Debug.Log(message); break;
				case MessageType.Warning: Debug.LogWarning(message); break;
				case MessageType.Error: Debug.LogError(message); break;
			}
		}

		public enum MessageType { Default, Warning, Error }
	}
}
