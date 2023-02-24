namespace DefaultNamespace.UI
{
    public class WinPanel : BasePanel
    {
        protected override void OnClickRestarButton()
        {
            GameManager.Instance.StartGame();
        }
    }
}