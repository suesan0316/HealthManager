using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using Xamarin.Forms;

namespace HealthManager.Common.Extention
{
    internal class BindableStackLayout : StackLayout
    {
        public static readonly BindableProperty ItemsProperty =
            BindableProperty.Create(nameof(Items), typeof(ObservableCollection<Xamarin.Forms.View>), typeof(BindableStackLayout), null,
                propertyChanged: (b, o, n) =>
                {
                   ((ObservableCollection<Xamarin.Forms.View>) n).CollectionChanged += (coll, arg) =>
                    {
                        switch (arg.Action)
                        {
                            case NotifyCollectionChangedAction.Add:
                                foreach (var v in arg.NewItems)
                                    ((BindableStackLayout) b).Children.Add((Xamarin.Forms.View) v);
                                Debug.WriteLine("aaa");
                                break;
                            case NotifyCollectionChangedAction.Remove:
                                foreach (var v in arg.NewItems)
                                    ((BindableStackLayout) b).Children.Remove((Xamarin.Forms.View) v);
                                break;
                            case NotifyCollectionChangedAction.Reset:
                                ((BindableStackLayout) b).Children.Clear();
                                break;
                            case NotifyCollectionChangedAction.Move:
                                //Do your stuff
                                break;
                            case NotifyCollectionChangedAction.Replace:
                                //Do your stuff
                                break;
                        }
                    };
                });

        public ObservableCollection<Xamarin.Forms.View> Items
        {
            get => (ObservableCollection<Xamarin.Forms.View>) GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }
    }
}