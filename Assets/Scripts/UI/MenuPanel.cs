namespace DefaultNamespace.UI
{
    public class MenuPanel : BasePanel
    {
        protected override void ClickButton()
        {
            GameManager.Instance.StartGame();
        }
    }
}