﻿using HealthManager.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HealthManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BodyImageView : ContentPage
    {
        public BodyImageView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new BodyImageViewModel(ImageStack.Children);
        }
    }
}