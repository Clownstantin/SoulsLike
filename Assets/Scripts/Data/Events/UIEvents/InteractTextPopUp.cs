namespace SoulsLike
{
	public struct InteractTextPopUp : IGameEvent
	{
		public readonly string interactableText;
		public readonly bool isActive;

		public InteractTextPopUp(string interactableText, bool isActive)
		{
			this.interactableText = interactableText;
			this.isActive = isActive;
		}
	}
}