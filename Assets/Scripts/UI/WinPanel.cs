namespace DefaultNamespace.UI
{
    public class WinPanel : BasePanel
    {
        protected override void ClickButton()
        {
            GameManager.Instance.StartGame();
        }
    }
}