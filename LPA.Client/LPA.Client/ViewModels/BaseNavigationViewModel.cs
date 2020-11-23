using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LPA.Client.ViewModels
{
	/// <summary>
	/// Implements the INavigation interface on top of BaseViewModel.
	/// </summary>
	public abstract class BaseNavigationViewModel : INotifyPropertyChanged, INavigation
	{
		private bool isNotBusy;
		INavigation Navigation => Application.Current?.MainPage?.Navigation;

		public bool IsNotBusy
		{
			get { return isNotBusy; }
			protected set
			{
				isNotBusy = value;
				OnPropertyChanged("IsNotBusy");
			}
		}

		public async Task GetBusyWith(Task task)
		{
			IsNotBusy = false;
			await task;
			IsNotBusy = true;
		}

		#region INavigation implementation

		public void RemovePage(Page page)
		{
			Navigation?.RemovePage(page);
		}

		public void InsertPageBefore(Page page, Page before)
		{
			Navigation?.InsertPageBefore(page, before);
		}

		public async Task PushAsync(Page page)
		{
			var task = Navigation?.PushAsync(page);
			if (task != null)
				await task;
		}

		public async Task<Page> PopAsync()
		{
			var task = Navigation?.PopAsync();
			return task != null ? await task : await Task.FromResult(null as Page);
		}

		public async Task PopToRootAsync()
		{
			var task = Navigation?.PopToRootAsync();
			if (task != null)
				await task;
		}

		public async Task PushModalAsync(Page page)
		{
			var task = Navigation?.PushModalAsync(page);
			if (task != null)
				await task;
		}

		public async Task<Page> PopModalAsync()
		{
			var task = Navigation?.PopModalAsync();
			return task != null ? await task : await Task.FromResult(null as Page);
		}

		public async Task PushAsync(Page page, bool animated)
		{
			var task = Navigation?.PushAsync(page, animated);
			if (task != null)
				await task;
		}

		public async Task<Page> PopAsync(bool animated)
		{
			var task = Navigation?.PopAsync(animated);
			return task != null ? await task : await Task.FromResult(null as Page);
		}

		public async Task PopToRootAsync(bool animated)
		{
			var task = Navigation?.PopToRootAsync(animated);
			if (task != null)
				await task;
		}

		public async Task PushModalAsync(Page page, bool animated)
		{
			var task = Navigation?.PushModalAsync(page, animated);
			if (task != null)
				await task;
		}

		public async Task<Page> PopModalAsync(bool animated)
		{
			var task = Navigation?.PopModalAsync(animated);
			return task != null ? await task : await Task.FromResult(null as Page);
		}

		public IReadOnlyList<Page> NavigationStack => Navigation?.NavigationStack;

		public IReadOnlyList<Page> ModalStack => Navigation?.ModalStack;

		#endregion

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberNameAttribute]string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}