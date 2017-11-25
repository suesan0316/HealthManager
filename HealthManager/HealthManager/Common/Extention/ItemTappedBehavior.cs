using System.Windows.Input;
using Xamarin.Forms;

namespace HealthManager.Common.Extention
{
    public class ItemTappedBehavior : Behavior<ListView>
    {
        public static BindableProperty CommandProperty = BindableProperty.Create(
            "Command", typeof(ICommand), typeof(ItemTappedBehavior), null
        );

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);

            bindable.BindingContextChanged += (sender, e) => {
                this.BindingContext = ((ListView)sender).BindingContext;
            };

            bindable.ItemTapped += (sender, e) => {
                this.Command.Execute(e.Item);
            };
        }
    }
}
