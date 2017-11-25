using System.Collections.ObjectModel;
using Xamarin.Forms;
using XView = Xamarin.Forms.View;
namespace HealthManager.Common.Extention
{
    internal class BindableStackLayout : StackLayout
    {
        public static readonly BindableProperty ItemsProperty =
            BindableProperty.Create(nameof(Items), typeof(ObservableCollection<XView>),
                typeof(BindableStackLayout), null,
                propertyChanged: (b, o, n) =>
                {
                    // TODO アクションごとの処理を追加
                  foreach (var v in ((BindableStackLayout) b).Items)
                        ((BindableStackLayout) b).Children.Add(v);
                });

        public ObservableCollection<XView> Items
        {
            get => (ObservableCollection<XView>) GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }
    }
}