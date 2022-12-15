namespace Maui.App.Framework.MVVM
{
	public partial class BaseViewModel : ObservableObject
	{
		[ObservableProperty]
		private string title = string.Empty;

		[ObservableProperty]
		public bool isBusy = false;

		[ObservableProperty]
		private string loadingText = string.Empty;

		[ObservableProperty]
		private bool dataLoaded = false;

		[ObservableProperty]
		private bool isErrorState = false;

		[ObservableProperty]
		private string errorMessage = string.Empty;	

		[ObservableProperty]
		private string errorImage = string.Empty;

		public BaseViewModel() =>
			IsErrorState = false;

		//Called on Page Appearing
		public virtual async void OnNavigatedTo(object parameters) =>
			await Task.CompletedTask;

		//Set Loading Indicators for Page
		protected void SetDataLoadingIndicators(bool isStaring = true)
		{
			if (isStaring)
			{
				isBusy = true;
				DataLoaded = false;
				IsErrorState = false;
				ErrorMessage = "";
				ErrorImage = "";

			}
			else
			{
				LoadingText = "";
				IsBusy = false;

			}
		}
	}
}
