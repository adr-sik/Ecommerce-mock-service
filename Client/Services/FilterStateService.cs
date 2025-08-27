namespace Client.Services
{
    public class FilterStateService
    {
        public string SelectedCategory { get; private set; } = "";
        public bool Show { get; private set; } = false;

        public event Action OnChange;

        public void SetCategory(string category)
        {
            SelectedCategory = category;
            if (category != "")
            {
                Show = true;
            }
            else
            {
                Show = false;
            }
            NotifyStateChanged();
        }

        public void NotifyStateChanged() => OnChange?.Invoke();
    }
}
