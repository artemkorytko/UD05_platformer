namespace DefaultNamespace.UI
{
    public class MenuPanel : BasePanel
    {
        protected override void OnClickRestarButton()
        {
            GameManager.Instance.StartGame();
        }
    }
}