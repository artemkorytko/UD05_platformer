namespace DefaultNamespace.UI
{
    public class FailPanel : BasePanel
    {
        protected override void ClickButton()
        {
            GameManager.Instance.StartGame();
        }
    }
}