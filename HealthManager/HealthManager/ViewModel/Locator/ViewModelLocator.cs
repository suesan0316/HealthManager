using CommonServiceLocator;
namespace HealthManager.ViewModel.Locator
{
    public class ViewModelLocator
    {
        public InputBasicDataViewModel InputBasicDataViewModel =>
            ServiceLocator.Current.GetInstance<InputBasicDataViewModel>();
    }
}